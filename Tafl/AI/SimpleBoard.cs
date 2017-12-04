using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tafl.AI
{
    public class SimpleBoard  // Simple representation of a board used by AI calculations
    {
        public enum Occupation
        {
            King, Defender, Attacker, Empty
        };

        public enum SquareType
        {
            Normal, Throne, AttackerStart, Corner, DefenderStart
        };


        public Occupation[][] OccuptationArray { get; set; }

        public SquareType[][] SquareTypeArray { get; set; }    // Will be used sparingly,  as all boards will take the same type array

        public SimpleBoard()
        {
            
        }
    }
}
