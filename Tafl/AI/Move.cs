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

        private enum direction
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

            board.OccupationArray[move.endColumn, move.endRow] = board.OccupationArray[move.startColumn, move.startRow];
            board.OccupationArray[move.startColumn, move.startRow] = Square.occupation_type.Empty;     
            
            //Determine the move direction

            //Process takes


        }

        
    }
}
