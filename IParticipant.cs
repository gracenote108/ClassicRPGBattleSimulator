using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator
{
    public interface IParticipant
    {
        string Name { get; }

        string AttributeName { get; }
        int Speed { get; }

        string Attack();

        string Special();

    }
}
