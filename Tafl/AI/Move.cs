using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tafl.AI
{
    public class Move
    {
        public int startColumn { get; set; }
        public int startRow { get; set; }
        public int endColumn { get; set; }
        public int endRow { get; set; }

        public Move()
        {
            this.startColumn = 0;
            this.startRow = 0;
            this.endColumn = 0;
            this.endRow = 0;
        }

        public Move(int iStartColumn, int iStartRow, int iEndColumn, int iEndRow)
        {
            this.startColumn = iStartColumn;
            this.startRow = iStartRow;
            this.endColumn = iEndColumn;
            this.endRow = iEndRow;
        }

        public override string ToString()
        {
            return this.startColumn.ToString() + "," + this.startRow.ToString() + " To " + this.endColumn.ToString() + "," + this.endRow.ToString();
        }
    }
}
