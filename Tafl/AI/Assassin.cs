using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tafl.Model.GameModel;

namespace Tafl.AI
{
    //Evaluator look at the potential for a move to enhance the capture likelihood of the King and to surround pieces
    public class Assassin 
    {
        public List<Move> Evaluate(List<List<Move>> inputMoveList, TurnState currentTurnState)
        {
            List<Move> suggestedMoves = new List<Move>();

            //Look to have the outer pieces on the maximum number of rows

            int sizeX = inputMoveList[0][0].board.OccupationArray.GetLength(0);
            int sizeY = inputMoveList[0][0].board.OccupationArray.GetLength(1);

            inputMoveList[0].ForEach(item =>
            {

            });

            return suggestedMoves;

        }

        private double GetFractionOfRowsAndColumnsControlled(SimpleBoard board)
        {
            double fraction = 0.0d;


            return fraction;
        }

    }
}
