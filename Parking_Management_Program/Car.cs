using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //private Utils utils;
        #endregion

        #region property
        public string CarNum { get { return carNum; } set { carNum = value; } }
        public string CarType { get { return carType; } set { carType = value; } }
        public DateTime EnterTime { get { return enterTime; } set { enterTime = value; } }
        public DateTime ExitTime { get { return exitTime; } set { exitTime = value; } }
        public long Fee { get { return fee; } set { fee = value; } }
        #endregion

        public Car(string catNum, string carType, DateTime enterTime) // delete exitTime
        {
            this.carNum = catNum;
            this.carType = carType;
            this.enterTime = enterTime;
        }
        //요것은 무엇인가요?
        //public void SearchParkedCar()
        //{
        //    this.exitTime = exitTime;
        //    this.utils = new Utils();
        //}
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
}
