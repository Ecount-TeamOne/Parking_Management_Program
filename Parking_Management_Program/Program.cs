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
        void GetParkedCar();
        void PrintReceipt();
    }

    [Serializable]
    public class Car
    {

        private string carNum;
        private string carType;
        private DateTime enterTime;
        private DateTime exitTime;


        public Car(string catNum, string carType, DateTime enterTime, DateTime exitTime)
        {
            this.carNum = catNum;
            this.carType = carType;
            this.enterTime = enterTime;
            this.exitTime = exitTime;
        }


        public string printCar()  // override string ToString()??
        {
            return string.Format($"차량번호 :{carNum}, 차종 : {carType}, 입차시간 : {enterTime},  입차시간 :  {exitTime}");
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }
    }

    class Manager
    {
        Car[,] parkingStatus;
        Dictionary<string, Car> recordList;
        Dictionary<string, User> userList;
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
                    //parkingStatus = (Car[,])formatter.Deserialize(stream);    //직접 지정은 안될까? 나중에 시험

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

        public long GetFee()
        {
            return 0;
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
            Console.Write("비 밀 번 호  입 력");
            pw = Console.ReadLine();

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
