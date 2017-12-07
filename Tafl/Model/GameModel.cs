﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tafl.AI;

namespace Tafl.Model
{
    public class GameModel
    {

        private SimpleBoard baseBoard;
        public SimpleBoard BaseBoard
        {
            get
            {
                return baseBoard;
            }
            set
            {
                baseBoard = value;
            }
        }

        private TurnState _currentTurnState;
        public TurnState currentTurnState
        {
            get
            {
                return _currentTurnState;
            } 
            set
            {
                _currentTurnState = value;              

            }

        }


        private bool _attackerIsAI;
        public bool attackerIsAI
        {
            get
            {
                return _attackerIsAI;
            }
            set
            {
                _attackerIsAI = value;
            }
        }

        private bool _defenderIsAI;
        public bool defenderIsAI
        {
            get
            {
                return _defenderIsAI;
            }
            set
            {
                _defenderIsAI = value;
            }
        }


        public enum TurnState
        {
            Attacker, Defender, VictoryDefender, VictoryAttacker, Resetting
        };


        public async Task<Move> RunAITurn(SimpleBoard startBoard)
        {
            this.BaseBoard = startBoard;
            Move suggetedMove = new Move();

            List<Move> moveList = new List<Move>();
            //Fill list with all possible moves of depth 0

            moveList = BaseBoard.GetPossibleMoves(this.currentTurnState);

            //Make the moves
            moveList.ForEach((m) =>
            {
                m.MakeMove(m, BaseBoard);
            });


            //Pick the best
            suggetedMove = moveList[0];

            return suggetedMove;
            
        }

        

        
        
    }
}
