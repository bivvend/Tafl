using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tafl.Model.GameModel;

namespace Tafl.AI
{
    /// <summary>
    /// Class look to weigh up piece advantages by likely hood of takes
    /// </summary>
    public class General
    {

        public List<Move> Evaluate(List<List<Move>> inputMoveList, TurnState currentTurnState)
        {
            List<Move> suggestedMoves = new List<Move>();
            if(currentTurnState == TurnState.Defender)
            {
                Move TestMove = inputMoveList[0].MaxObject((item) => item.numberTakesAttackerAtDepth[1]);
            }

            //Pick the best
            if (currentTurnState == TurnState.Defender)
            {
                suggestedMoves.Add(inputMoveList[0].MaxObject((item) => (double)item.numberTakesDefenderAtDepth[0] * 10.0 - (double)item.numberTakesAttackerAtDepth[1] + (double)item.numberTakesDefenderAtDepth[2] * 0.001));
                suggestedMoves.ForEach((item) =>
                {
                    item.scoreGeneral = (double)item.numberTakesDefenderAtDepth[0] * 10.0 - (double)item.numberTakesAttackerAtDepth[1] + (double)item.numberTakesDefenderAtDepth[2] * 0.001;
                });

            }
            else if (currentTurnState == TurnState.Attacker)
            {
                suggestedMoves.Add(inputMoveList[0].MaxObject((item) => (double)item.numberTakesAttackerAtDepth[0] * 10.0 - (double)item.numberTakesDefenderAtDepth[1] + (double)item.numberTakesAttackerAtDepth[2] * 0.001));
                suggestedMoves.ForEach((item) =>
                {
                    item.scoreGeneral = (double)item.numberTakesAttackerAtDepth[0] * 10.0 - (double)item.numberTakesDefenderAtDepth[1] + (double)item.numberTakesAttackerAtDepth[2] * 0.001;

                });
            }

            return suggestedMoves;
            
        }
    }
}
