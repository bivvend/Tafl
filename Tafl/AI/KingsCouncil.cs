using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tafl.Model.GameModel;

namespace Tafl.AI
{
    /// <summary>
    /// Evaluator plans best route for King to get to Exit
    /// </summary>
    /// 
    public class KingsCouncil
    {
        private List<List<Move>> moveList;

        public double desireToWin = 1000000000.0;
        public double desrieToWinDepth2 = 1000.0;
        public double desireForFreeKing = 0.1;
        public double desireForKingToBeClearToCorner = 10.0;

        public List<Move> Evaluate(List<List<Move>> inputMoveList, TurnState currentTurnState)
        {
            moveList = inputMoveList;

            List<Move> suggestedMoves = new List<Move>();

            if (currentTurnState == TurnState.Defender)
            {
                SimpleSquare evalSquare = new SimpleSquare();
                
                bool foundWin = false;
                //Evaluate the if the King's path to the thrones is clear.  If so one of the moves will have the King on the throne. 
                moveList[0].ForEach((item) =>
                {
                    evalSquare = item.GetSquare(item.endRow, item.endColumn);
                    if(evalSquare.SquareType == Model.Square.square_type.Corner)
                    {
                        item.scoreKingsCouncil = desireToWin;
                        suggestedMoves.Add(item);
                        foundWin = true;
                    }
                });
                if (foundWin)
                    return suggestedMoves;

                //Look at all depth 2 moves to see if any of these are wins
                bool foundWinsDepth2 = false;
                int numberDepth2Wins = 0;               

                moveList[2].ForEach((item) =>
                {

                    evalSquare = item.GetSquare(item.endRow, item.endColumn);
                    if (evalSquare.SquareType == Model.Square.square_type.Corner)
                    {
                        item.parent.parent.scoreKingsCouncil += desrieToWinDepth2;
                        foundWinsDepth2 = true;
                        numberDepth2Wins++;
                    }
                });

                //if number of depth 2 wins is zero allow more full analyis
                int numMovesForKing = 0;
                List<Move> tempMoveList = new List<Move>();
                List<Move> kingsMoveList = new List<Move>();
                SimpleSquare kingSquare = new SimpleSquare();

                if(numberDepth2Wins ==0)
                {
                    //Award moves that have the King more free to move 
                    moveList[0].ForEach((item) =>
                    {
                        //Find the King
                        kingSquare = item.FindTheKing(item.board);

                        tempMoveList = item.board.GetPossibleMoves(TurnState.Defender, null, 0);

                        //filter the list based on moves of the king                        
                        kingsMoveList = tempMoveList.Where(move => move.startRow == kingSquare.Row && move.startColumn == kingSquare.Column).ToList();
                        numMovesForKing = kingsMoveList.Count;
                        //Give a bonus for the king being freed up 
                        item.scoreKingsCouncil += (double)numMovesForKing * desireForFreeKing;                   
                        

                    });

                }

                

                       

                
                //return the best
                suggestedMoves.Add(moveList[0].MaxObject((item) => item.scoreKingsCouncil));
            }
            else
            {
                return suggestedMoves; //will be empty.
            }

            return suggestedMoves;

        }

        

    }
}
