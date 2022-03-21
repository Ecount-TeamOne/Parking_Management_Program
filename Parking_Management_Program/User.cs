using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_Management_Program
{
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

        public string CarNum { get => this.carNum; }
        public long UserMoney { get => this.userMoney; set => this.userMoney = value; }
        public string PhoneNum { get => this.phoneNum; }
        public string UserName { get => this.userName; }

        public override string ToString()
        {
            return $"차량번호 : {carNum} | 회원이름 : {userName} | 전화번호 : {phoneNum}원 | 적립금 : {userMoney}원";
        }
    }
}
