using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HühnerRennen
{
    class Player
    {
        private double m_money = 300.0f;

        public string Name
        {
            get;
            set;
        }

        public double Money
        {
            get { return m_money; }
            set { _SetMoney(value); }
        }

        public double WonMoney
        {
            get;
            private set;
        }

        public double LostMoney
        {
            get;
            private set;
        }

        public Dictionary<Chicken, double> Chickens
        {
            get;
            private set;
        }

        public Player()
        {
            Chickens = new Dictionary<Chicken, double>();
        }

        private void _SetMoney(double money){
            if(money < m_money){
                LostMoney += m_money - money;
            }else{
                WonMoney += money - m_money;
            }

            m_money = money;
        }
    }
}
