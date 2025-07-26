namespace _250726_homework
{
    // 큐? 
    // 큐는 선입선출구조로 먼저들어온 데이터가 먼저 나간다는 특징이 있다
    // 왜 큐를 썼는가? => 비행기 흐름을 보자
    // 1. 먼저 줄을 선 사람이 먼저 탑승
    // 2. VIP 승객이 일반 승객보다 우선 탑승
    // 이걸 코드로 구현할때,
    // 줄 서는 행위 : Enqueue()  -> Enqueue는 큐에 데이터를 추가 하는 동작
    // 탑승하는 행위를 : Dequeue() 로 표현 -> Dequeue는 데이터를 빼는 동작
    
    abstract class Passenger // 추상클래스 만들기
    {
        public string Name { get; set; } // 이름 속성 정의
        public Passenger(string name)
        {
            Name = name;
        }
        public abstract string GetPassengerType();
    }
    interface IFlightQueue
    {
        void AddPassenger(Passenger passenger); // 승객 추가
        Passenger BoardPassenger(); // 가장 먼저 등록된 승객을 탑승시키고 리스트에서 제거
        int GetPassengerCount(); // 현재 대기 승객 수 반환
        
    }
    class GeneralPassenger : Passenger 
    {
        public GeneralPassenger(string name) : base(name) { }
        public override string GetPassengerType() // 필수 구현해야함
        {
            return "일반";
        }
    }
    class VipPassenger : Passenger 
    {
        public VipPassenger(string name) : base(name){ }
        public override string GetPassengerType()
        {
            return "VIP";
        }
    }
    class FlightQueue : IFlightQueue
    {
        private Queue<Passenger> vipQueue = new Queue<Passenger>();
        private Queue<Passenger> generalQueue = new Queue<Passenger>();

        public void AddPassenger(Passenger passenger)
        {
            if (passenger is VipPassenger)
            {
                vipQueue.Enqueue(passenger);
            }
            else
            {
                generalQueue.Enqueue(passenger);
            }
        }
        public Passenger BoardPassenger()
        {
            if (vipQueue.Count > 0)
            {
                return vipQueue.Dequeue();
            }
            else if (generalQueue.Count > 0)
            {
                return generalQueue.Dequeue();
            }
            else
            {
                return null;
            }
        }
        public int GetPassengerCount()
        {
            return vipQueue.Count + generalQueue.Count;
        }
        internal class Program
        {
            static void Main(string[] args)
            {
                IFlightQueue flightQueue = new FlightQueue();

                flightQueue.AddPassenger(new GeneralPassenger("박민수"));
                flightQueue.AddPassenger(new VipPassenger("김영희"));
                flightQueue.AddPassenger(new GeneralPassenger("정하나"));
                flightQueue.AddPassenger(new VipPassenger("이철수"));

                Console.WriteLine($"현재 대기 승객 수: {flightQueue.GetPassengerCount()}");

                while (flightQueue.GetPassengerCount() > 0)
                {
                    Passenger boardedPassenger = flightQueue.BoardPassenger();
                    Console.WriteLine($"탑승 완료: {boardedPassenger.GetPassengerType()} - {boardedPassenger.Name}");
                }

                Console.WriteLine($"현재 대기 승객 수: {flightQueue.GetPassengerCount()}");
            }
        }
    }
}
