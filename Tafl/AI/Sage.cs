using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tafl.Model.GameModel;

namespace Tafl.AI
{
    /// <summary>
    /// Evaluator used to compare the scores of all the other evaluators
    /// </summary>
    public class Sage
    {
        General general { get; set; }
        Assassin assassin { get; set; }
        KingsCouncil kingsCouncil { get; set; }

        public double weightGeneral { get; set; } = 0.0d;
        public double weightAssassin { get; set; } = 0.0d;
        public double weightKingsCouncil { get; set; } = 0.0d;

        public List<List<Move>>  inputMoveList { get; set; }
        public List<Move> bestList { get; set; }
        public Move suggestedMove { get; set; }

        public TurnState currentTurnState { get; set; } = TurnState.Attacker;

        public Sage()
        {
            general = new General();
            assassin = new Assassin();
            kingsCouncil = new KingsCouncil();

            weightAssassin = 1.0;
            weightGeneral = 1.0;
            weightKingsCouncil = 1.0;
        }

        public void ProcessMoves(List<List<Move>> input, TurnState turnState)
        {
            this.inputMoveList = input;
            this.currentTurnState = turnState;

            bestList = new List<Move>();

            //Ask general to process moves
            bestList.AddRange(general.Evaluate(inputMoveList, turnState));

            if (turnState == TurnState.Attacker)
                bestList.AddRange(assassin.Evaluate(inputMoveList, turnState));

            if (turnState == TurnState.Defender)
                bestList.AddRange(kingsCouncil.Evaluate(inputMoveList, turnState));

            bestList.ForEach((item) =>
            {

                item.scoreSage = item.scoreGeneral * weightGeneral + item.scoreKingsCouncil * weightKingsCouncil + item.scoreAssassin * weightAssassin;

            });

            //pick best to return
            suggestedMove = bestList.MaxObject((item) => item.scoreSage);
        }

        


    }
}
