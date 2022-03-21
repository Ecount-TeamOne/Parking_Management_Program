using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Parking_Management_Program
{
    [Serializable]
    class Manager
    {
        private Car[,] parkingStatus;
        private List<Car> recordList;
        private Dictionary<string, User> userList;
        private Dictionary<string, DateTime> longNonExitList;
        private const string ADMIN_ID = "root";
        private const string ADMIN_PW = "rootpw";
        private Utils utils;

        public Manager()
        {
            parkingStatus = new Car[10, 10];
            recordList = new List<Car>();
            userList = new Dictionary<string, User>();
            longNonExitList = new Dictionary<string, DateTime>();
            utils = new Utils();
        }
        public void Run()
        {
            LoadData();

            int key = 0;
            
            while ((key = SelectMenu()) != 0)
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
                        Enter();
                        break;
                    case 4:
                        Exit();
                        break;
                    default:
                        Console.WriteLine("잘 못  선 택 하 였 습 니 다.");
                        break;
                }
            }
            SaveData();
            Console.WriteLine("종료합니다...");
        }

        private int SelectMenu()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                            주 차 장 관 리  프 로 그 램                              │");
            Console.WriteLine("│                                  1. 로 그 인                                        │");
            Console.WriteLine("│                                  2. 회 원 가 입                                     │");
            Console.WriteLine("│                                  3. 주 차 하 기                                     │");
            Console.WriteLine("│                                  4. 출 차 하 기                                     │");
            Console.WriteLine("│                                  0. 종 료                                           │");
            Console.WriteLine("└─────────────────────────────────────────────────────────────────────────────────────┘");
            Console.Write("[메 뉴 를  선 택 해 주 세 요 : ]  ");
            int key = int.TryParse(Console.ReadLine(),out key)?key:999;
            return key;
        }
        private int ManagerMenu()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                                    관 리 자 메 뉴                                   │");
            Console.WriteLine("│                                  1. 정 산 목 록  조 회                              │");
            Console.WriteLine("│                                  2. 주 차 차 량  검 색                              │");
            Console.WriteLine("│                                  3. 주 차 차 량  현 황                              │");
            Console.WriteLine("│                                  4. 장기미출차목록 조회                             │");
            Console.WriteLine("│                                  5. 회 원 목 록 조 회                               │");
            Console.WriteLine("│                                  6. 차 량 목 록 조 회                               │");
            Console.WriteLine("│                                  0. 이  전  메  뉴                                  │");
            Console.WriteLine("└─────────────────────────────────────────────────────────────────────────────────────┘");
            Console.Write("[메 뉴 를  선 택 해 주 세 요 : ]  ");
            int key = int.TryParse(Console.ReadLine(), out key) ? key : 999;
            return key;
        }
        private int UserMenu()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                                   회 원 메 뉴                                       │");
            Console.WriteLine("│                                  1. 적 립 금 조 회                                  │");
            Console.WriteLine("│                                  2. 적 립 금 충 전                                  │");
            Console.WriteLine("│                                  0. 이 전 메 뉴                                     │");
            Console.WriteLine("└─────────────────────────────────────────────────────────────────────────────────────┘");
            Console.Write("[메 뉴 를  선 택 해 주 세 요 : ]  ");
            int key = int.TryParse(Console.ReadLine(), out key) ? key : 999;
            return key;
        }
        public void UserSelect(string carNum)
        {
            int key;
            while ((key = UserMenu()) != 0)
            {
                switch (key)
                {
                    case 1:
                        ShowUserMoney(carNum);
                        break;
                    case 2:
                        ChargeUserMoney(carNum);
                        break;
                    default:
                        Console.WriteLine("[잘 못  선 택 하 였 습 니 다 . ]");
                        break;
                }
            }
        }
        public void ManagerSelect()
        {
            int key = 0;
            while ((key = ManagerMenu()) != 0)
            {
                switch (key)
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
                    case 4:
                        ShowLongNonExitList();
                        break;
                    case 5:
                        ShowUserList();
                        break;
                    case 6:
                        ShowCarList();
                        break;
                    case 7:
                        //Test 용
                        Test();
                        break;

                    default:
                        Console.WriteLine("[잘 못  선 택 하 였 습 니 다 . ]");
                        break;
                }
            }
        }

        public void AddRecord(Car car)
        {
            this.recordList.Add(car);
        }

        public void ShowRecordList()
        {
            if (recordList.Count == 0)
            {
                Console.WriteLine("[ 표 시 할  기 록 이  없 습 니 다 . ]");
            }
            else
            {
                foreach (var record in recordList)
                {
                    Console.WriteLine(record);  //tostring 출력
                }
            }
        }

        private void UpdateLongNonExitList() // 장기미출차(7일 초과) 리스트 갱신
        {
            // 나간 차량은 리스트에서 제거
            Dictionary<string, DateTime> longNonExitListCopy = new Dictionary<string, DateTime>();
            foreach (var car in longNonExitList)
            {
                longNonExitListCopy.Add(car.Key, car.Value);
            }
            foreach (var car in longNonExitListCopy)
            {
                if (GetParkedCarLoc(car.Key) == null)
                {
                    this.longNonExitList.Remove(car.Key);
                }
            }

            // 장기 미출차 차량을 리스트에 추가
            for (int i = 0; i < this.parkingStatus.GetLength(0); i++)
            {
                for (int j = 0; j < this.parkingStatus.GetLength(1); j++)
                {
                    if (this.parkingStatus[i, j] != null)
                    {
                        Car car = this.parkingStatus[i, j];
                        TimeSpan parkingTime = DateTime.Now.Subtract(car.EnterTime);
                        if (parkingTime.Days > 7 && !this.longNonExitList.ContainsKey(car.CarNum))
                        {
                            this.longNonExitList.Add(car.CarNum, car.EnterTime);
                        }
                    }
                }
            }

            utils.SaveFile(this.longNonExitList, "LongNonExitList.txt");
        }

        public void ShowLongNonExitList()
        {
            UpdateLongNonExitList();

            if (File.Exists("LongNonExitList.txt"))
            {
                using (Stream stream = new FileStream("LongNonExitList.txt", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    this.longNonExitList = (Dictionary<string, DateTime>)formatter.Deserialize(stream);
                }
            }

            if (longNonExitList.Count > 0)
            {
                foreach (var car in longNonExitList)
                {
                    Console.WriteLine($"차량번호 : {car.Key} | 입차시간 : {car.Value}");
                }
            }
            else
            {
                Console.WriteLine("장 기 미 출 차 차 량 이  없 습 니 다 .");
            }
        }

        // 장기미출차 테스트용
        public void Test()
        {
            string carNum = utils.InputCarNum();
            var loc = GetParkedCarLoc(carNum);
            parkingStatus[loc.Item1, loc.Item2].EnterTime = new DateTime(2022, 03, 11, 00, 00, 00);
        }

        private void SaveData()
        {
            utils.SaveFile(this.parkingStatus, "parkingLot.txt");
            utils.SaveFile(this.recordList, "records.txt");
            utils.SaveFile(this.userList, "users.txt");
        }

        private void LoadData()
        {
            if (File.Exists("parkingLot.txt"))
            {
                using (Stream stream = new FileStream("parkingLot.txt", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    this.parkingStatus = (Car[,])formatter.Deserialize(stream);    //serialize 해제 후 지정
                }
            }
            if (File.Exists("records.txt"))
            {
                using (Stream stream = new FileStream("records.txt", FileMode.Open, FileAccess.ReadWrite))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    this.recordList = (List<Car>)formatter.Deserialize(stream);    //serialize 해제 후 지정
                }
            }
            if (File.Exists("users.txt"))
            {
                using (Stream stream = new FileStream("users.txt", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    this.userList = (Dictionary<string, User>)formatter.Deserialize(stream);    //serialize 해제 후 지정
                }
            }
        }

        public void ChargeUserMoney(string carNum)
        {
            long addUserMoney;
            if (this.userList.ContainsKey(carNum))
            {

                Console.Write("[충 전 하 실  금 액 을  입 력 해 주 세 요. ]");
                addUserMoney = long.Parse(Console.ReadLine());
                this.userList[carNum].UserMoney += addUserMoney;
                Console.WriteLine($"[{addUserMoney} 원 이  정 상 적 으 로  충 전 되 었 습 니 다. ]");
            }
            else
            {
                Console.WriteLine("[회 원 이  아 닙 니 다 . ]");
                Console.WriteLine("[회 원 가 입 후,  사 용 해 주 세 요 . ]");
            }
        }

        public void ShowUserMoney(string carNum)
        {
            if (this.userList.ContainsKey(carNum))
            {
                Console.WriteLine($"[회 원 님 의   잔 액 은 . . . {this.userList[carNum].UserMoney}  원  입 니 다 . ]");
            }
            else
            {
                Console.WriteLine("[회 원 이  아 닙 니 다 .]");
                Console.WriteLine("[회 원 가 입 후,  사 용 해 주 세 요 . ]");
            }
        }

        public void Login()
        {
            string id;
            string pw;

            Console.Write("[I D   입 력 (차 량 번 호  입 력) : ]");
            id = Console.ReadLine();
            Console.Write("[비 밀 번 호  입 력 (핸 드 폰 번 호  입 력) : ]");
            pw = Console.ReadLine();
            if (id == ADMIN_ID && pw == ADMIN_PW)
            {
                Console.WriteLine("[관 리 자 님  환 영 합 니 다.]\n");
                ManagerSelect();
            }
            else if (userList.ContainsKey(id) && pw == userList[id].PhoneNum) // 수정 
            {
                Console.WriteLine($"[{userList[id].UserName} 님  환 영 합 니 다.]\n");
                UserSelect(id);
            }
            else
            {
                Console.WriteLine("[정 보 가  잘 못 되 었 습 니 다 . ]\n");
            }

        }

        public void SignUp()
        {
            string userName;
            string carNum;
            string phoneNum;
            string userNum;
            while (true)
            {
                Console.WriteLine("[회 원 가 입]");

                Console.Write("[이 름 을  입 력 해 주 세 요 : ]");
                userName = Console.ReadLine();

                Console.Write("[차 량 번 호 를  입 력 해 주 세 요 : ]");
                carNum = Console.ReadLine();

                if (!utils.CheckCarNum(carNum) || userList.ContainsKey(carNum))
                {
                    Console.WriteLine("[>> 잘못된 차량번호입니다. ]\n");
                    continue;
                }
                Console.Write("[핸 드 폰 번 호 를  입 력 해 주 세 요 : ]");
                phoneNum = Console.ReadLine();
                if (!utils.CheckPhoneNum(phoneNum))
                {
                    Console.WriteLine("[>> 잘못된 핸드폰번호 입니다. ]\n");
                    continue;
                }

                userNum = DateTime.Now.ToString("yyyyMMddHHmmss");
                userList.Add(carNum, new User(carNum, userName, 0, userNum, phoneNum));
                break;
            }

        }

        public void ShowUserList()
        {
            if (userList.Count == 0)
            {
                Console.WriteLine("[ 표 시 할  회 원 정 보 가  없 습 니 다 . ]");

            }
            else
            {
                Console.WriteLine("=========================================== 회원 목록 ===========================================");
                foreach(var user in userList)
                {
                    Console.WriteLine(user);
                }
                Console.WriteLine("=================================================================================================");
            }

        }

        public void ShowCarList()
        {
            if (ParkingCarCount() == 0)
            {
                Console.WriteLine("주차장에 차량이 없습니다.");
            }
            else
            {
                Console.WriteLine("======================================= 현재 주차 차량 목록 =======================================");

                foreach (var car in parkingStatus)
                {
                    if (car != null)
                    {
                        Console.WriteLine($"차량번호 : {car.CarNum} | 입차시간 : {car.EnterTime}");
                    }
                    else
                    {
                        continue;
                    }
                }
                Console.WriteLine("===================================================================================================");
            }

        }


        public void PrintParkingStatus()
        {
            string result;
            Console.WriteLine($"********** 총 주차된 차량 수 : {ParkingCarCount()} **********");
            for (int i = 0; i < this.parkingStatus.GetLength(0); i++)
            {
                for (int j = 0; j < this.parkingStatus.GetLength(1); j++)
                {
                    if (this.parkingStatus[i, j] != null)
                    {
                        
                        result = this.parkingStatus[i, j].CarNum.Length == 7 ? $"[ {this.parkingStatus[i, j].CarNum}] " : $"[{this.parkingStatus[i, j].CarNum}] ";
                        Console.Write(result);
                    }
                    else
                    {
                        Console.Write($"[  {(char)(i + 65)} - {j + 1}  ] ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"************************************************************");
        }

        public void SearchParkedCar()
        {
            string carNum;
            Console.Write("[차 량 번 호 를  입 력 하 세 요 >> ]");
            carNum = Console.ReadLine();
            for (int i = 0; i < parkingStatus.GetLength(0); i++)
            {
                for (int j = 0; j < parkingStatus.GetLength(1); j++)
                {
                    if (parkingStatus[i, j] == null)
                    {
                        continue;
                    }
                    else if (parkingStatus[i, j].CarNum == carNum)
                    {
                        Console.WriteLine($"[해 당 차 량 은  {(char)(i + 65)}-{j + 1}  에  주 차 되 어 있 습 니 다 . ]");
                        return;
                    }

                }
            }
            Console.WriteLine("[현재 주차장에 입력한 차량번호가 존재하지 않습니다.]");
        }

        public void PrintReceipt(Car car)
        {
            Console.WriteLine("================= 영수증 =================");
            Console.WriteLine($"차량번호\t: {car.CarNum}");
            Console.WriteLine($"입차시간\t: {car.EnterTime}");
            Console.WriteLine($"출차시간\t: {car.ExitTime}");
            Console.WriteLine($"총 주차시간\t: {car.GetParkingTime()}");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"요금\t\t: {car.GetFee()}원");
            Console.WriteLine("==========================================");
        }

        public void Enter()
        {
            string inputLoc;
            int i, j;
            string carType, carNum;
            DateTime enterTime;
            // 만차 여부 확인
            if (IsParkinglotFull())
            {
                Console.WriteLine("[>> 현재 만차로 주차가 불가능합니다. 초기 메뉴로 이동합니다.]");
                return;
            }
            // 요금 및 주차 현황 출력
            PrintFeeTable();
            PrintParkingStatus();

            // 주차 구역 입력
            while (true)
            {
                Console.WriteLine("\n[원하는 주차 자리를 입력해주세요! ex) A-2]");
                inputLoc = Console.ReadLine();
                // 입력 예외
                if (!utils.CheckParkingLocation(inputLoc))
                {
                    Console.WriteLine("[>> 잘못된 입력입니다. 구역-번호 양식으로 입력하세요.]");
                    continue;
                }
                i = char.Parse(inputLoc.Split('-')[0]) - 'A';
                j = int.Parse(inputLoc.Split('-')[1]) - 1;
                // 주차 가능 여부 확인
                if (parkingStatus[i, j] != null)
                {
                    Console.WriteLine("[>> 주차가 불가한 구역입니다.]");
                    continue;
                }
                Console.Write($"[>> 선택한 {(char)(i + 65)}-{j + 1}는 주차 가능한 자리입니다.]");
                break;
            }

            // 차량 번호 입력
            while (true)
            {
                Console.Write($"\n[차량번호를 입력하세요 :]");
                carNum = Console.ReadLine();
                // 입력 예외
                if (!utils.CheckCarNum(carNum))
                {
                    Console.WriteLine("[>> 잘못된 차량번호입니다.]\n");
                    continue;
                }
                // 기주차 차량 여부 확인
                if (GetParkedCarLoc(carNum) != null)
                {
                    Console.WriteLine("[>> 해당 차량은 이미 주차되어 있습니다. 초기 메뉴로 이동합니다.]\n");
                    return;
                }
                break;
            }

            do
            {
                Console.Write("\n[차종을 입력해주세요 [ 소형 | 중형 | 대형 ]: ]");
                carType = Console.ReadLine();
            } while (!utils.CheckCarType(carType));

            enterTime = DateTime.Now;
            Car enterCar;
            if(carType == "소형")
            {
                enterCar = new CompactCar(carNum, enterTime);
            }
            else if(carType == "중형")
            {
                enterCar = new MidsizedCar(carNum, enterTime);
            }
            else
            {
                enterCar = new FullsizedCar(carNum, enterTime);
            }

            parkingStatus[i, j] = enterCar;
            Console.WriteLine("[>> 입차 처리가 완료되었습니다.]");
            PrintEnterInfo(enterCar, i, j);
        }

        public void Exit()
        {
            string carNum = utils.InputCarNum();
            Tuple<int, int> carSpace = GetParkedCarLoc(carNum);

            if (carSpace == null)
            {
                Console.WriteLine("[존재하지 않는 차량번호입니다.]");
                return;
            }
            Car car = this.parkingStatus[carSpace.Item1, carSpace.Item2];
            car.ExitTime = DateTime.Now;
            car.Fee = car.GetFee();
            Pay(car);
            AddRecord(car);
            this.parkingStatus[carSpace.Item1, carSpace.Item2] = null;
            Console.WriteLine("[출차가 완료되었습니다.]");
        }

        public bool IsParkinglotFull()
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


        public void PrintFeeTable()
        {
            Console.WriteLine("========= 요 금 표 =========");
            Console.WriteLine("    1시간  :  2 0 0 0 원");
            Console.WriteLine("    (중형 : 10% 추가금액)");
            Console.WriteLine("    (대형 : 20% 추가금액)");
            Console.WriteLine("===========================");
        }

        public void PrintEnterInfo(Car car, int i, int j)
        {
            Console.WriteLine("============주차 정보=============");
            Console.WriteLine($"차량 번호 :\t{car.CarNum}");
            Console.WriteLine($"주차 구역 :\t{(char)(i + 65)}-{j + 1}");
            Console.WriteLine($"입차 시간 :\t{car.EnterTime.ToString("yyyy/MM/dd HH:mm:ss")}");
            Console.WriteLine("==================================");
        }

        public Tuple<int, int> GetParkedCarLoc(string carNum)
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
        public int ParkingCarCount()    //주차된 차량 수 탐색
        {
            int count = 0;
            for (int i = 0; i < this.parkingStatus.GetLength(0); i++)
            {
                for (int j = 0; j < this.parkingStatus.GetLength(1); j++)
                {
                    if (this.parkingStatus[i, j] != null)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void Pay(Car car)
        {
            string carNum = car.CarNum;
            long fee = car.GetFee();

            if (this.userList.ContainsKey(carNum))
            {
                Console.WriteLine("********* 회 원 입 니 다 *********");
                User user = userList[carNum];

                if (user.UserMoney >= fee)
                {
                    user.UserMoney -= fee;
                    Console.WriteLine($"[차 감  적 립 금 : ]{fee}");
                    Console.WriteLine($"[남 은  적 립 금 : ]{user.UserMoney}");
                }
                else
                {
                    long diffMoney = fee - user.UserMoney;
                    user.UserMoney = 0;
                    Console.WriteLine($"[적 립 금 이  {diffMoney} 원  부 족 합 니 다]");
                    Console.Write("[차 액 을  넣 어 주 세 요 : ]");
                    utils.InsertMoney(diffMoney);
                }
            }
            else
            {
                Console.WriteLine("********* 비 회 원 입 니 다 *********");
                Console.WriteLine($"[요 금 은  {fee} 원  입 니 다]");
                Console.Write("[돈 을  넣 어 주 세 요 : ]");
                utils.InsertMoney(fee);
            }

            PrintReceipt(car);
        }

    }
}
