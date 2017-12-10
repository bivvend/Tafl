using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tafl.Model;

namespace Tafl.AI
{
    public class Move
    {
        public int startColumn { get; set; }
        public int startRow { get; set; }
        public int endColumn { get; set; }
        public int endRow { get; set; }

        public Move parent { get; set; }
        public int depth { get; set; }

        public double score { get; set; }

        public SimpleBoard board { get; set; }  //Board associated with the move.  Represents the state before and then after the move is made

        public enum direction
        {
            FromBelow, FromAbove, FromLeft, FromRight
        };        

        public Move()
        {
            this.startColumn = 0;
            this.startRow = 0;
            this.endColumn = 0;
            this.endRow = 0;
            parent = null;
            score = 0.0d;
            depth = 0;
        }

        public Move(int iStartColumn, int iStartRow, int iEndColumn, int iEndRow, Move parentMove, int iDepth)
        {
            this.startColumn = iStartColumn;
            this.startRow = iStartRow;
            this.endColumn = iEndColumn;
            this.endRow = iEndRow;
            this.parent = parentMove;
            this.depth = iDepth;            

        }

        //private void SetMoveDirection()
        //{
        //    if(this.endColumn > this.startColumn  && this.startRow == this.endRow)
        //    {
        //        this.MoveDirection = direction.FromLeft;
        //    }
        //    else if (this.endColumn < this.startColumn && this.startRow == this.endRow)
        //    {
        //        this.MoveDirection = direction.FromRight;
        //    }
        //    else if(this.endRow < this.startRow && this.endColumn == this.startColumn)
        //    {
        //        this.MoveDirection = direction.FromBelow;
        //    }
        //    else if (this.endRow > this.startRow && this.endColumn == this.startColumn)
        //    {
        //        this.MoveDirection = direction.FromAbove;
        //    }

        //}



        public override string ToString()
        {
            return this.startColumn.ToString() + "," + this.startRow.ToString() + " To " + this.endColumn.ToString() + "," + this.endRow.ToString() + "(" + this.score.ToString("0.0000") + ")";
        }

        /// <summary>
        /// Makes the move and process takes etc
        /// </summary>
        /// <param name="move"></param>
        public void  MakeMove(Move move, SimpleBoard initialBoard)
        {
            //Use copy contructor
            board = new SimpleBoard(initialBoard);
            //Determine who is making the move
            bool isDefender = false;
            bool isAttacker = false;
            bool isKing = false;

            if (board.OccupationArray[move.startColumn, move.startRow] == Square.occupation_type.Defender)
                isDefender = true;
            if (board.OccupationArray[move.startColumn, move.startRow] == Square.occupation_type.Attacker)
                isAttacker = true;
            if (board.OccupationArray[move.startColumn, move.startRow] == Square.occupation_type.King)
                isKing = true;

            //Make the move
            board.OccupationArray[move.endColumn, move.endRow] = board.OccupationArray[move.startColumn, move.startRow];
            board.OccupationArray[move.startColumn, move.startRow] = Square.occupation_type.Empty;

            //Process takes
            CheckAndProcessTake();


        }

        private SimpleSquare GetSquare(int row, int column)   //Need to be careful throughout as have a habit of swapping these.  Using row, column (y,x) here and in Check and process take
        {
            SimpleSquare retSquare = new SimpleSquare();
            
            return retSquare;

        }

        private void CheckAndProcessTake()
        {
            ////Check UP 1 ROW
            //SimpleSquare squareToCheck = GetSquare(endSquare.Row - 1, endSquare.Column);
            //if (squareToCheck != null) //Is a valid square
            //{
            //    SearchAroundForTake(squareToCheck, endSquare, direction.FromBelow);
            //}

            ////Check DOWN 1 ROW
            //squareToCheck = GetSquare(endSquare.Row + 1, endSquare.Column);
            //if (squareToCheck != null) //Is a valid square
            //{
            //    SearchAroundForTake(squareToCheck, endSquare, direction.FromAbove);
            //}

            ////Check LEFT 1 COLUMN
            //squareToCheck = GetSquare(endSquare.Row, endSquare.Column - 1);
            //if (squareToCheck != null) //Is a valid square
            //{
            //    SearchAroundForTake(squareToCheck, endSquare, direction.FromRight);
            //}

            ////Check RIGHT 1 COLUMN
            //squareToCheck = GetSquare(endSquare.Row, endSquare.Column + 1);
            //if (squareToCheck != null) //Is a valid square
            //{
            //    SearchAroundForTake(squareToCheck, endSquare, direction.FromLeft);
            //}

        }

        private void SearchAroundForTake(SimpleSquare squareToCheck, SimpleSquare endSquare, direction dir)
        {
            ////squareToCheck is the square with the possible piece to be taken,  endSquare is the square into which the possible taker moved, direction is the direction that the taker moved w.r.t takee.
            //Square squareTwoAway = null;
            //if (squareToCheck.Occupation != Square.occupation_type.Empty) //Something in the square
            //{
            //    if (squareToCheck.AttackerPresent && (endSquare.KingPresent || endSquare.DefenderPresent))
            //    {
            //        //Defender or King moved next to Attacker
            //        //Look 2 squares away in given direction for defender or King
            //        switch (dir)
            //        {
            //            case direction.FromAbove:
            //                squareTwoAway = GetSquare(endSquare.Row + 2, endSquare.Column);
            //                break;
            //            case direction.FromBelow:
            //                squareTwoAway = GetSquare(endSquare.Row - 2, endSquare.Column);
            //                break;
            //            case direction.FromLeft:
            //                squareTwoAway = GetSquare(endSquare.Row, endSquare.Column + 2);
            //                break;
            //            case direction.FromRight:
            //                squareTwoAway = GetSquare(endSquare.Row, endSquare.Column - 2);
            //                break;
            //        }
            //        if (squareTwoAway != null)
            //        {
            //            if (squareTwoAway.DefenderPresent || squareTwoAway.KingPresent || squareTwoAway.SquareType == Square.square_type.Corner)
            //            {
            //                squareToCheck.Occupation = Square.occupation_type.Empty;
            //            }
            //        }

            //    }
            //    if (squareToCheck.DefenderPresent && endSquare.AttackerPresent)
            //    {
            //        //Attacker moved next to defender
            //        switch (dir)
            //        {
            //            case direction.FromAbove:
            //                squareTwoAway = GetSquare(endSquare.Row + 2, endSquare.Column);
            //                break;
            //            case direction.FromBelow:
            //                squareTwoAway = GetSquare(endSquare.Row - 2, endSquare.Column);
            //                break;
            //            case direction.FromLeft:
            //                squareTwoAway = GetSquare(endSquare.Row, endSquare.Column + 2);
            //                break;
            //            case direction.FromRight:
            //                squareTwoAway = GetSquare(endSquare.Row, endSquare.Column - 2);
            //                break;
            //        }
            //        if (squareTwoAway != null)
            //        {
            //            if (squareTwoAway.AttackerPresent || squareTwoAway.SquareType == Square.square_type.Corner)
            //            {
            //                squareToCheck.Occupation = Square.occupation_type.Empty;
            //            }
            //        }
            //    }
            //}
        }


    }
}
