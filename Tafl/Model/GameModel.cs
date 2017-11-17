using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tafl.Model
{
    public class GameModel
    {

        public TurnState currentTurnState;

        public bool attackerIsAI;

        public bool defenderIsAI;

        public enum TurnState
        {
            Attacker, Defender, VictoryDefender, VictoryAttacker
        };


        
    }
}
