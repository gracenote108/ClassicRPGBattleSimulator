using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator
{
    public class Fighter : IParticipant
    {
        private int _speed;

        private string _name;

        private string _attName;

        public string Name
        {
            get { return _name; }
        }

        public int Speed
        {
            get { return _speed; }
        }

        public string AttributeName
        {
            get { return _attName; }
        }

        public Fighter(string name, int speedNum, string attName)
        {
            _name = name;
            _speed = speedNum;
            _attName = attName;
        }

        public string Attack()
        {
            return String.Format("{0} performs an attack at {1} speed.", _name, _speed);
        }

        public string Special()
        {
            return String.Format("{0} performs an limit break.", _name);
        }
    }
}
