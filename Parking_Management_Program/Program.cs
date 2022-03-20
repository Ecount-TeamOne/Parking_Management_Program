using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parking_Management_Program
{
    [Serializable]
    public class Car 
    {

        private string carNum;
        private string carType;
        private DateTime enterTime;
        private DateTime exitTime;
        #region 추가
        private long fee;
        private Utils utils;
        #endregion

        public string CarNum { get => carNum; }
        public long Fee { set => fee = value; }
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

        public void SearchParkedCar()
        {
            this.exitTime = exitTime;
            this.utils = new Utils();
        }

        #region
        public override string ToString()
        {
            return $"차량번호 : {carNum} | 차종 : {carType} | 입차시간 : {enterTime} | 출차시간 : {exitTime} | 요금 : {fee}원";
        }
        public TimeSpan GetParkingTime()
        {
            TimeSpan time = exitTime.Subtract(enterTime);
            return time;
        }
        #endregion
    }
    [Serializable]
    class Manager 
    {
        private Car[,] parkingStatus;
        private List<Car> recordList;
        private Dictionary<string, User> userList;
        private const string ADMIN_ID = "root";
        private const string ADMIN_PW = "rootpw";
        private Utils utils;

        public Manager()
        {
            parkingStatus = new Car[10, 10];
            recordList = new List<Car>();
            userList = new Dictionary<string, User>();
            utils = new Utils();
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
            Console.WriteLine("1. 관 리 자 로 그 인");
            Console.WriteLine("2. 회 원 가 입");
            Console.WriteLine("3. 주 차 하 기");
            Console.WriteLine("4. 출 차 하 기");
            Console.WriteLine("5. 적 립 금 조 회");
            Console.WriteLine("6. 적 립 금 충 전");
            Console.WriteLine("0. 종 료");
            int key = int.Parse(Console.ReadLine());
            return key;
        }
        private int managerMenu()
        {
            Console.WriteLine("1. 정산목록 조회");
            Console.WriteLine("2. 주차차량 검색");
            Console.WriteLine("3. 주차차량 현황");
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
                formatter.Serialize(stream, this.userList);
            }
            
        }
        private void LoadData()
        {
            if (File.Exists("parkingLot.txt"))
            {
                using (Stream stream = new FileStream("parkingLot.txt", FileMode.Open, FileAccess.ReadWrite))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Car[,] parkingStatus = (Car[,])formatter.Deserialize(stream);    //serialize 해제 후 지정
                }
            }
            if (File.Exists("records.txt"))
            {
                using (Stream stream = new FileStream("records.txt", FileMode.Open, FileAccess.ReadWrite))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    List<Car> recordList = (List<Car>)formatter.Deserialize(stream);    //serialize 해제 후 지정
                }
            }
            if (File.Exists("users.txt"))
            {
                using (Stream stream = new FileStream("users.txt", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Dictionary<string, User> userList = (Dictionary<string, User>)formatter.Deserialize(stream);    //serialize 해제 후 지정
                }
                foreach(var user in userList)
                {
                    Console.WriteLine(user);
                }
            }


        }

        public long GetFee(Car car)
        {
            TimeSpan parkingTime = car.GetParkingTime();
            long fee = parkingTime.Hours * 2000;
            return fee;
        }

        public void Pay()
        {
            
        }

        public void ChargeUserMoney()
        {
            long addUserMoney;
            Console.WriteLine("차량번호를 입력해주세요");
            string carNum = Console.ReadLine();
            if (this.userList.ContainsKey(carNum))
            {
                
                Console.Write("충 전 하 실  금 액 을  입 력 해 주 세 요. ");
                addUserMoney = long.Parse(Console.ReadLine());
                this.userList[carNum].UserMoney += addUserMoney;
                Console.WriteLine($"{addUserMoney} 원 이  정 상 적 으 로  충 전 되 었 습 니 다. ");
            }
            else
            {
                Console.WriteLine("회 원 이  아 닙 니 다 .");
                Console.WriteLine("회 원 가 입 후,  사 용 해 주 세 요 . ");
            }
        }

        public void ShowUserMoneyList()
        {
            Console.WriteLine("차량번호를 입력해주세요");
            string carNum = Console.ReadLine();
            if (this.userList.ContainsKey(carNum))
            {
                Console.WriteLine($"회 원 님 의   잔 액 은 . . . {this.userList[carNum].UserMoney}  원  입 니 다 . ");

            }
            else
            {
                Console.WriteLine("회 원 이  아 닙 니 다 .");
                Console.WriteLine("회 원 가 입 후,  사 용 해 주 세 요 . ");
            }
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

        public void PrintFeeTable()
        {
            Console.WriteLine("========= 요 금 표 =========");
            Console.WriteLine("1 0 분  :  1 0 0 0 원");
            Console.WriteLine("============================");
        }

        public void PrintReceipt(Car car)
        {
            Console.WriteLine("============주차 정보=============");
            Console.WriteLine($"차량 번호 :\t{car.CarNum}");
            Console.WriteLine($"입차 시간 :\t{car.EnterTime.ToString("yyyy/MM/dd HH:mm:ss")}");
            Console.WriteLine($"출차 시간 :\t{car.ExitTime.ToString("yyyy/MM/dd HH:mm:ss")}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"주차 요금 :\t{GetFee(car)}원");
            Console.WriteLine("==================================");
        }

        /////////////////////////////////
        public void Enter()
        {
            string carNum;
            string carType;
            string location;

            PrintFeeTable();
            PrintParkingStatus();

            do
            {
                Console.Write("주 차 구 역  선 택 ( ex.  A-1 ) : ");
                location = Console.ReadLine();
            } while (!utils.CheckParkingLocation(location));

            string[] locationXy = location.Split('-');
            int locationX = char.Parse(locationXy[0]) - 'A';
            int locationY = int.Parse(locationXy[1]) - 1;

            if (this.parkingStatus[locationX, locationY] != null)
            {
                Console.WriteLine("이미 주차된 자리입니다.");
                return;
            }

            carNum = utils.InputCarNum();
            if (getParkedCarSpace(carNum) != null)
            {
                Console.WriteLine("이미 존재하는 차량입니다.");
                return;
            }
            do
            {
                Console.Write("차 종 을  입 력 해 주 세 요 : ");
                carType = Console.ReadLine();
            } while (!utils.CheckCarType(carType));

            Car car = new Car(carNum, carType, DateTime.Now);
            this.parkingStatus[locationX, locationY] = car;
            Console.WriteLine("주차가 완료되었습니다.");
        }

        public void Exit()
        {
            string carNum = utils.InputCarNum();
            Tuple<int, int> carSpace = getParkedCarSpace(carNum);
            if (carSpace == null)
            {
                Console.WriteLine("존재하지 않는 차량번호입니다.");
                return;
            }
            Car car = this.parkingStatus[carSpace.Item1, carSpace.Item2];
            car.ExitTime = DateTime.Now;
            car.Fee = GetFee(car);
            Pay(car);
            // AddRecord
            this.parkingStatus[carSpace.Item1, carSpace.Item2] = null;
            Console.WriteLine("출차가 완료되었습니다.");
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
                    Car car = this.parkingStatus[i, j];
                    if (car != null && car.CarNum == carNum)
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }
            return null;
        }

        public void Pay(Car car)
        {
            string carNum = car.CarNum;
            long fee = this.GetFee(car);

            if (this.userList.ContainsKey(carNum))
            {
                Console.WriteLine("********* 회 원 입 니 다 *********");
                User user = userList[carNum];

                if (user.UserMoney >= fee)
                {
                    user.UserMoney -= fee;
                    Console.WriteLine($"차 감  적 립 금 : {fee}");
                    Console.WriteLine($"남 은  적 립 금 : {user.UserMoney}");
                }
                else
                {
                    long diffMoney = fee - user.UserMoney;
                    user.UserMoney = 0;
                    Console.WriteLine($"적 립 금 이  {diffMoney} 원  부 족 합 니 다");
                    Console.Write("차 액 을  넣 어 주 세 요 : ");
                    utils.InsertMoney(diffMoney);
                }
            }
            else
            {
                Console.WriteLine("********* 비 회 원 입 니 다 *********");
                Console.WriteLine($"요 금 은  {fee} 원  입 니 다");
                Console.Write("돈 을  넣 어 주 세 요 : ");
                utils.InsertMoney(fee);
            }

            PrintReceipt(car);
        }

    }
  
    [Serializable]
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

        public string CarNum { get => carNum; }
        public long UserMoney { get => userMoney; set => userMoney = value; }
    }

    class Utils
    {
        public bool CheckCarNum(string carNum)
        {
            return Regex.IsMatch(carNum, @"^\d{2,3}[가-힣]\d{4}$");
        }

        public bool CheckPhoneNum(string phoneNum)
        {
            return Regex.IsMatch(phoneNum, @"^01([0-1|6-9])[ -]?(\d{3,4})[ -]?(\d{4})$");
        }

        #region 추가

        public bool CheckCarType(string carType)
        {
            List<string> carTypes = new List<string> { "소형", "중형", "대형" };
            return carTypes.Contains(carType);
        }

        public bool CheckParkingLocation(string location)
        {
            return Regex.IsMatch(location, @"^[A-J]-([1-9]|10)$");
        }

        public string InputCarNum()
        {
            string carNum;
            do
            {
                Console.Write("차 량 번 호 를  입 력 해 주 세 요 : ");
                carNum = Console.ReadLine();

            } while (!CheckCarNum(carNum));
            return carNum;
        }

        public void InsertMoney(long fee)
        {
            long money;

            while (true)
            {
                money = long.Parse(Console.ReadLine());

                if (money < fee)
                {
                    Console.Write("돈 이  부 족 합 니 다. 다 시  넣 어 주 세 요 : ");
                }
                else if (money >= fee)
                {
                    Console.WriteLine($"거 스 름 돈 : {money - fee} 원");
                    break;
                }
            }
            Console.WriteLine("감 사 합 니 다 .  안 녕 히  가 세 요 .");
        }


        #endregion

    }

    class Program
    {
        static void Main(string[] args)
        {
            Manager bm = new Manager();

            bm.Enter();
            bm.Enter();
            bm.PrintParkingStatus();
            bm.Exit();
            bm.Exit();
            bm.PrintParkingStatus();

        }
    }
}

