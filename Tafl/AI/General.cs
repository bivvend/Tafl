using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tafl.AI
{
    /// <summary>
    /// Class look to weigh up piece advantages by likely hood of takes
    /// </summary>
    public class General
    {

        public void Evaluate(Move move)
        {

            move.scoreGeneral = 0.0;
        }
    }
}
