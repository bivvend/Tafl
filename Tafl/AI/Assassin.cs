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
        public double desireForWinDepth0 = 1000000000.0;  // Always win.
        public double desireForWinDepth2 = 10000.0;

        public double desireNotToLoseDepth1 = 1000000000.0;  // Always move to block a loss 

        public List<Move> Evaluate(List<List<Move>> inputMoveList, TurnState currentTurnState)
        {
            List<Move> suggestedMoves = new List<Move>();
            
            //Look to have the outer pieces on the maximum number of rows

            int sizeX = inputMoveList[0][0].board.OccupationArray.GetLength(0);
            int sizeY = inputMoveList[0][0].board.OccupationArray.GetLength(1);

            int numberMovesDepth0 = 0;

            int numberOfLosesDepth1 = 0;

            inputMoveList[0].ForEach(item =>
            {
                if(item.CheckForAttackerVictory())
                {
                    item.scoreAssassin = desireForWinDepth0;
                    numberMovesDepth0++;
                }
            });

            //check to see if a move blocks a depth 1 loss
            inputMoveList[1].ForEach(item =>
            {
                if(item.FindTheKing(item.board).SquareType == Model.Square.square_type.Corner)
                {
                    numberOfLosesDepth1++;
                }                
            });

            //If a loss is possible all moves in which the loss can happen should be penalised.
            if(numberOfLosesDepth1>0)
            {
                inputMoveList[1].ForEach(item =>
                {
                    if (item.FindTheKing(item.board).SquareType == Model.Square.square_type.Corner)
                    {
                        item.parent.scoreAssassin -= desireNotToLoseDepth1;
                    }
                });

            }


            if (numberMovesDepth0 < 1)
            {
                inputMoveList[2].ForEach(item =>
                {
                    if (item.CheckForAttackerVictory())
                    {
                        item.parent.parent.scoreAssassin += desireForWinDepth2 / (double)inputMoveList[1].Count;
                    }
                    
                });
            }

            suggestedMoves.Add(inputMoveList[0].MaxObject(item => item.scoreAssassin));

            return suggestedMoves;

        }

        private double GetFractionOfRowsAndColumnsControlled(SimpleBoard board)
        {
            double fraction = 0.0d;
            return fraction;
        }

    }
}
