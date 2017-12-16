using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tafl.Model.GameModel;

namespace Tafl.AI
{
    //Evaluator look at the potential for a move to enhance the capture likelihood of the King.
    public class Assassin 
    {
        public List<Move> Evaluate(List<List<Move>> inputMoveList, TurnState currentTurnState)
        {
            List<Move> suggestedMoves = new List<Move>();

            return suggestedMoves;

        }

    }
}
