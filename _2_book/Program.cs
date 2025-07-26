namespace _2_book
{
    abstract class Book // Book 추상클래스 만들기
    {
        public string Title { get; set; }
        public Book(string title) { 
            Title = title;
        }
    }
    interface IBorrowable
    {
        void Borrow(); // 책을 대출
        void Return(); // 책을 반납
        bool IsBorrowed{ get; }
    }
    // 일반책은 리스트에서 제거하는 방식으로 반납, 전자책은 상태를 변경하는 방식을 반납
    public class PhysicalBook : Book, IBorrowable  //왜 오류 나지?
    {
        private bool isBorrowed = false;

        public PhysicalBook(string title) : base(title) { }

        public void Borrow()
        {
            if (!isBorrowed)
            {
                isBorrowed = true;
                Console.WriteLine($"[대출] 일반책 '{Title}' 대출됨.");
            }
            else
            {
                Console.WriteLine($"[경고] 일반책 '{Title}'는 이미 대출 중입니다.");
            }
        }

        public void Return()
        {
            if (isBorrowed)
            {
                isBorrowed = false;
                Console.WriteLine($"[반납] 일반책 '{Title}' 반납됨.");
            }
            else
            {
                Console.WriteLine($"[경고] 일반책 '{Title}'는 이미 반납된 상태입니다.");
            }
        }

        public bool IsBorrowed => isBorrowed;
    }
    // Ebook에서 Iborrowble을 구현하지 않았을때 오류나는 이유  __ from gpt
    // Library클래스에선 IBorrowable타입으로 EBook을 사용하고 있다.
    // public void BorrowBook(IBorrowable book) { ... }
    // 이 형식은 인터페이스가 요구하는 멤버(Borrow, Return, IsBorrowed를 가진다는 보장이 없기때문에 에러가 난다.
    class EBook : Book, IBorrowable // 상속
    {
        public bool isBorrowed = false;

        public EBook(string title) : base(title) { }

        public void Borrow()
        {
            if (!isBorrowed)
            {
                isBorrowed = true;
                Console.WriteLine($"[대출] 전자책 '{Title}' 대출됨.");
            }
            else
            {
                Console.WriteLine($"[경고] 전자책 '{Title}'는 이미 대출 중입니다.");
            }
        }

        public void Return()
        {
            isBorrowed = false;
            Console.WriteLine($"[반납] 전자책 '{Title}' 반납됨.");
        }

        public bool IsBorrowed => isBorrowed;
    }
    class Library
    {
        public string Name { get; set; }
        private List<IBorrowable> borrowedBooks = new List<IBorrowable>();

        public void BorrowBook(IBorrowable book)
        {
            book.Borrow();
            if (book is PhysicalBook || (book is EBook && book.IsBorrowed))
            {
                borrowedBooks.Add(book);
            }
        }

        public void ReturnBook(IBorrowable book)
        {
            book.Return();
            if (book is PhysicalBook)
            {
                borrowedBooks.Remove(book);
            }
        }
        public void ShowBorrowedBooks()
        {
            Console.WriteLine("현재 대출된 책 목록:");
            foreach (var book in borrowedBooks)
            {
                if (book is Book b && book.IsBorrowed)
                {
                    Console.WriteLine($"- {b.Title} ({(book is EBook ? "전자책" : "일반책")})");
                }
            }
            Console.WriteLine();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();

            PhysicalBook book1 = new PhysicalBook("해리포터");
            PhysicalBook book2 = new PhysicalBook("반지의 제왕");
            EBook book3 = new EBook("소설 토지");

            library.BorrowBook(book1);
            library.BorrowBook(book2);
            library.BorrowBook(book3);

            library.ShowBorrowedBooks();

            library.ReturnBook(book2);
            Console.WriteLine("'반지의 제왕' 반납 완료!\n");

            library.ShowBorrowedBooks();
        }
    }
}
