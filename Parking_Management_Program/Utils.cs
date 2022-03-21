using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Parking_Management_Program
{
    [Serializable]
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

        public void ListToFile(ICollection list, string fileName)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, list);
            }
        }

        #endregion

    }
}
