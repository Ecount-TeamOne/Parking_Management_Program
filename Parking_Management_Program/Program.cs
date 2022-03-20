using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Parking_Management_Program
{
    public interface IParking
    {
        void PrintParkingStatus();
        void SearchParkedCar();
        void PrintReceipt(Car car);
    }

    [Serializable]
    public class Car : IParking
    {

        private string carNum;
        private string carType;
        private DateTime enterTime;
        private DateTime exitTime;

        public string CarNum { get => carNum; }
        public DateTime EnterTime { get => enterTime; }
        public DateTime ExitTime { 
            get => exitTime;
            set => exitTime = value; // add set property
        }
        
        public Car(string catNum, string carType, DateTime enterTime) // delete exitTime
        {
            this.carNum = catNum;
            this.carType = carType;
            this.enterTime = enterTime;
        }


        public string printCar()  // override string ToString()??
        {
            return string.Format($"차량번호 :{carNum}, 차종 : {carType}, 입차시간 : {enterTime},  입차시간 :  {exitTime}");
        }


        public void Exit()
        {

        }

        public void PrintParkingStatus() { 
            throw new NotImplementedException();
        }

        public void SearchParkedCar()
        {
            throw new NotImplementedException();
        }

        public void PrintReceipt(Car car)
        {
            throw new NotImplementedException();
        }
    }

    class Manager : IParking
    {
        private Car[,] parkingStatus;
        private Dictionary<string, Car> recordList;
        private Dictionary<string, User> userList;
        private readonly string ADMIN_ID;
        private readonly string ADMIN_PW;

        public Manager()
        {
            parkingStatus = new Car[0, 0];
            recordList = new Dictionary<string, Car>();
            userList = new Dictionary<string, User>();

        }
        public void Run()
        {
            LoadData();

            int key = 0;
            while ((key = selectMenu()) != 0)
            {
                switch (key)
                {
                    case 1:
                        Login();
                        break;
                    case 2:
                        SignUp();
                        break;
                    case 3:
                        AddRecord();
                        break;
                    case 4:
                        RemoveRecord();
                        break;
                    case 5:
                        ShowUserMoneyList();
                        break;
                    default:
                        Console.WriteLine("잘못 선택하였습니다.");
                        break;
                }
            }
            SaveData();
            Console.WriteLine("종료합니다...");
        }

        private int selectMenu()
        {

            //Console.WriteLine("1:추가 2:삭제 3:검색 4:차량 목록 5:Car 현황 0:종료");
            Console.WriteLine("1. 로 그 인");
            Console.WriteLine("2. 회 원 가 입");
            Console.WriteLine("3. 주 차 하 기");
            Console.WriteLine("4. 출 차 하 기");
            Console.WriteLine("5. 적 립 금 조 회");
            Console.WriteLine("0. 종 료");
            int key = int.Parse(Console.ReadLine());
            return key;
        }
        private int managerMenu()
        {
            Console.WriteLine("1. 정산목록 조회");
            Console.WriteLine("2. 주차차량 검색");
            Console.WriteLine("3. 주차차량 현황");
            int key = int.Parse(Console.ReadLine());
            return key;
        }

        public void ManagerSelect()
        {
            int key = 0;
            while((key = managerMenu()) != 0)
            {
                switch(key)
                {
                    case 1:
                        ShowRecordList();
                        break;
                    case 2:
                        SearchParkedCar();
                        break;
                    case 3:
                        PrintParkingStatus();
                        break;
                    default:
                        Console.WriteLine("잘못 선택하였습니다.");
                        break;
                }
            }
        }

        public void AddRecord()
        {

        }

        public void ShowRecordList()
        {

        }

        public void RemoveRecord()
        {

        }
        private void SaveData()
        {

            using (Stream stream = new FileStream("parkingLot.txt", FileMode.Create))   //주차 현 상태 저장
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this.parkingStatus);
            }
            
            using (Stream stream = new FileStream("records.txt", FileMode.Create))    
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this.recordList);   //serialize해서 저장
            }
            using (Stream stream = new FileStream("users.txt", FileMode.Create))  
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this.recordList);
            }
            //using (Stream stream = new FileStream("car.txt", FileMode.Append))    //있으면 추가모드로 저장
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(stream, this.recordList);   //serialize해서 저장
            //} //불러오기 안할 경우에만 append쓰는게 맞지않나...
        }
        private void LoadData()
        {
            if (File.Exists("parkingLot.txt")&& File.Exists("records.txt") && File.Exists("users.txt"))
            {
                using (Stream stream = new FileStream("parkingLot.txt", FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Car[,] parkingStatus = (Car[,])formatter.Deserialize(stream);    //serialize 해제 후 지정
                    //parkingStatus = (Car[,])formatter.Deserialize(stream);    //직접 지정은 안될까? 나중에 시험 >> 안됨

                }
                using (Stream stream = new FileStream("records.txt", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Dictionary<string, Car> recordList = (Dictionary<string, Car>)formatter.Deserialize(stream);    //serialize 해제 후 지정
                }
                using (Stream stream = new FileStream("users.txt", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Dictionary<string, User> userList = (Dictionary<string, User>)formatter.Deserialize(stream);    //serialize 해제 후 지정
                }
            }

        }

        public long GetFee(Car car)
        {
            long fee;
            fee = ((car.ExitTime.Subtract(car.EnterTime)).Hours) * 2000;
            return fee;
        }


        public void Pay()
        {

        }

        public void ChargeUserMoney()
        {

        }

        public void ShowUserMoneyList()
        {

        }

        public void Login()
        {
            string id;
            string pw;

            Console.Write("I D   입 력 : ");
            id = Console.ReadLine();
            Console.Write("비 밀 번 호  입 력 : ");
            pw = Console.ReadLine();
            if (id == ADMIN_ID && pw == ADMIN_PW)
            {
                Console.WriteLine("관 리 자 님  환 영 합 니 다.");
                ManagerSelect();
            }
            else // 수정 
            {
                Console.WriteLine($"{id} 님  환 영 합 니 다.");
            }

        }

        public void SignUp()
        {
            string userName;
            string carNum;
            string phoneNum;
            Console.WriteLine("회 원 가 입");
            Console.Write("이 름 을  입 력 해 주 세 요 : ");
            userName = Console.ReadLine();
            Console.Write("차 량 번 호 를  입 력 해 주 세 요 : ");
            carNum = Console.ReadLine();
            Console.Write("핸 드 폰 번 호 를  입 력 해 주 세 요 : ");
            phoneNum = Console.ReadLine();
            userList.Add(carNum, new User(userName, carNum, 0, "", phoneNum));
        }

        public void PrintParkingStatus()
        {

            for (int i = 0; i < parkingStatus.GetLength(0); i++)
            {
                for (int j = 0; j < parkingStatus.GetLength(1); j++)
                {
                    if (parkingStatus[i, j] == null)
                    {
                        Console.Write($"{(char)(i + 65)} - {j + 1}\t");
                    }
                    else
                    {
                        Console.Write($"{parkingStatus[i, j].CarNum}\t");
                    }

                }
                Console.WriteLine();
            }
        }

        public void SearchParkedCar()
        {
            string carNum;
            Console.Write("차 량 번 호 를  입 력 하 세 요 >> ");
            carNum = Console.ReadLine();
            for (int i = 0; i < parkingStatus.GetLength(0); i++)
            {
                for (int j = 0; j < parkingStatus.GetLength(1); j++)
                {
                    if (parkingStatus[i, j] == null)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"해 당 차 량 은  {(char)(i + 65)}-{j + 1}  에  주 차 되 어 있 습 니 다 . ");
                    }

                }
            }
            Console.WriteLine("현재 주차장에 입력한 차량번호가 존재하지 않습니다.");
        }

        public void PrintReceipt(Car car)
        {
            Console.WriteLine("============주차 정보=============");
            Console.WriteLine($"차량 번호 :\t{car.CarNum}");
            Console.WriteLine($"입차 시간 :\t{car.EnterTime.ToString("yyyy/MM/dd HH:mm:ss")}");
            Console.WriteLine($"출차 시간 :\t{car.ExitTime.ToString("yyyy/MM/dd HH:mm:ss")}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"주차 요금 :\t{GetFee(car)}");
            Console.WriteLine("==================================");
        }

        /////////////////////////////////
        public void Enter()
        {
            string[] inputSpot;
            int i, j;
            string carType, carNum;
            DateTime enterTime;
            if (!isParkinglotFull())
            {
                Console.WriteLine("현재 만차로 주차가 불가능합니다. 초기 메뉴로 이동합니다.");
                return;
            }
            Console.WriteLine("주차요금은 시간 당 2,000원입니다.");
            Console.WriteLine("현재 주차 현황입니다. 주차 가능한 구역만 출력됩니다.");
            Console.WriteLine("원하는 주차 자리를 구역-번호 양식으로 입력해주세요! ex) A-11");
            inputSpot = Console.ReadLine().Split('-');
            // 입력 정규식 표현으로 확인 및 예외 처리
            i = (int)((char)inputSpot[0][0]) - 65;
            j = int.Parse(inputSpot[1]);
            //주차 가능한 자리인지 확인
            Console.Write($"선택한 {(char)(i + 65)}-{j}는 주차 가능한 자리입니다. \n 차량번호를 입력하세요 :");
            carNum = Console.ReadLine();
            // 입력 정규식 표현으로 확인 및 예외 처리
            Console.Write("차종을 입력해주세요 : ");
            carType = Console.ReadLine();
            enterTime = DateTime.Now;
            Car enterCar = new Car(carNum, carType, enterTime);
            parkingStatus[i, j] = enterCar;

            Console.WriteLine("주차가 완료되었습니다.");
            Console.WriteLine("============주차 정보=============");
            Console.WriteLine($"차량 번호 :\t{carNum}");
            Console.WriteLine($"주차 구역 :\t{(char)(i + 65)}-{j}");
            Console.WriteLine($"입차 시간 :\t{enterTime.ToString("yyyy/MM/dd HH:mm:ss")}");
            Console.WriteLine("==================================");
        }

        public void Exit()
        {
            string carNum;
            Tuple<int, int> carSpace;
            Console.Write($"출차할 차량번호를 입력하세요 :");
            // 입력 정규식 표현으로 확인 및 예외 처리
            carNum = Console.ReadLine();
            carSpace = getParkedCarSpace(carNum);
            if (carSpace == null)
            {
                Console.WriteLine("초기 메뉴로 이동합니다.");
                return;
            }
            Car exitCar = parkingStatus[carSpace.Item1, carSpace.Item2];
            exitCar.ExitTime = DateTime.Now;
            PrintReceipt(exitCar);

        }
        public bool isParkinglotFull()
        {
            for (int i = 0; i < parkingStatus.GetLength(0); i++)
            {
                for (int j = 0; j < parkingStatus.GetLength(1); j++)
                {
                    if (parkingStatus[i, j] == null)
                    {
                        return false;
                    }

                }
            }
            return true;
        }

        public Tuple<int, int> getParkedCarSpace(string carNum)
        {
            for (int i = 0; i < parkingStatus.GetLength(0); i++)
            {
                for (int j = 0; j < parkingStatus.GetLength(1); j++)
                {
                    if (parkingStatus[i, j].CarNum == carNum)
                    {
                        Console.WriteLine($"해 당 차 량 은  {(char)(i + 65)}-{j + 1}  에  주 차 되 어 있 습 니 다 . ");
                        return new Tuple<int, int>(i, j);
                    }
                }
            }
            Console.WriteLine("현재 주차장에 입력한 차량번호가 존재하지 않습니다.");
            return null;
        }

        public void Pay(Car car)
        {

        }
    }

    class User
    {
        private string carNum;
        private string userName;
        private long userMoney;
        private string userNum;
        private string phoneNum;

        public User(string carNum, string userName, long userMoney, string userNum, string phoneNum)
        {
            this.carNum = carNum;
            this.userName = userName;
            this.userMoney = userMoney;
            this.userNum = userNum;
            this.phoneNum = phoneNum;

        }
    }

    class Utils
    {
        public bool checkCarNum()
        {
            return true;
        }

        public bool checkPhoneNum()
        {
            return true;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Manager bm = new Manager();
            bm.Run();
        }
    }
}
