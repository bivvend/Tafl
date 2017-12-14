using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tafl.AI;
using Tafl.ViewModel;

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

        private bool pauseAfterAITurn;
        public bool PauseAfterAITurn
        {
            get
            {
                return pauseAfterAITurn;
            }
            set
            {

                pauseAfterAITurn = value;
            }
        }

        public enum TurnState
        {
            Attacker, Defender, VictoryDefender, VictoryAttacker, Resetting
        };

        public ObservableCollection<MoveViewModel> MoveViewModelList;

        public GameModel()
        {
            MoveViewModelList = new ObservableCollection<MoveViewModel>();
        }

        private async void InvokeAction (Action a)
        {
            await Application.Current.Dispatcher.BeginInvoke(a);
        }

        public async Task<Move> RunAITurn(SimpleBoard startBoard)
        {
            this.BaseBoard = startBoard;
            Move suggetedMove = new Move();

            List<List<Move>> moveList = new List<List<Move>>();
            InvokeAction(new Action(() => MoveViewModelList.Clear()));
            

            
            //Fill list with all possible moves of depth 0
            
            moveList.Add(BaseBoard.GetPossibleMoves(this.currentTurnState, null, 0));
            //Create Depth 1 moves
            moveList.Add(new List<Move>());
            moveList[0].ForEach((m) =>
            {
                //Make the moves
                m.MakeMove(m, BaseBoard);
                //Look at all the depth 1 moves for the opposing side
                
                if (currentTurnState == TurnState.Defender)
                {

                    moveList[1].AddRange(m.board.GetPossibleMoves(TurnState.Attacker, m, 1));
                }
                if (currentTurnState == TurnState.Attacker)
                {
                    moveList[1].AddRange(m.board.GetPossibleMoves(TurnState.Defender, m, 1));

                }

            });

            //Create Depth 2 moves
            moveList.Add(new List<Move>());
            moveList[1].ForEach((m2) =>
            {
                //Make the moves
                m2.MakeMove(m2, m2.parent.board);
                //Look at all the depth 1 moves for the opposing side

                if (currentTurnState == TurnState.Defender)
                {
                    moveList[2].AddRange(m2.parent.board.GetPossibleMoves(TurnState.Defender, m2, 2));
                }
                if (currentTurnState == TurnState.Attacker)
                {
                    moveList[2].AddRange(m2.parent.board.GetPossibleMoves(TurnState.Attacker, m2, 2));

                }

            });
            //Make depth 2 moves
            moveList[2].ForEach((m3) =>
            {
                //Make the moves
                m3.MakeMove(m3, m3.parent.board);

            });


            //Propagate scores
            for (int i = moveList.Count-1; i>=0; i--)
            {


                moveList[i].ForEach((item) =>                
                {
                    if (i != 0)
                    {
                        //Total number of takes in the pipeline downwards
                        if (item.numberTakesAttacker > 0 || item.numberTakesDefender > 0)
                        {
                            item.parent.numberTakesAttackerAtDepth[i] += item.numberTakesAttacker;
                            item.parent.numberTakesDefenderAtDepth[i] += item.numberTakesDefender;
                        }
                    }
                    else
                    {
                        item.numberTakesAttackerAtDepth[0] = item.numberTakesAttacker;
                        item.numberTakesDefenderAtDepth[0] = item.numberTakesDefender;
                    }

                    
                    
                });               

            }


            

            //Pick the best
            if (currentTurnState == TurnState.Defender)
            {
                suggetedMove = moveList[0].MaxObject((item) => item.numberTakesDefenderAtDepth[0] * 10.0 - item.numberTakesAttackerAtDepth[1]);
                InvokeAction( new Action(() =>MoveViewModelList.Add(new MoveViewModel(suggetedMove))));
                
            }
            else if(currentTurnState == TurnState.Attacker)
            {
                suggetedMove = moveList[0].MaxObject((item) => item.numberTakesAttackerAtDepth[0] * 10.0 - item.numberTakesDefenderAtDepth[1]);
                InvokeAction(new Action(() => MoveViewModelList.Add(new MoveViewModel(suggetedMove))));
            }

            return suggetedMove;
            
        }

        

        
        
    }

    static class EnumerableExtensions   // Used to efficiently find the item in a list with a maximum value
    {
        public static T MaxObject<T, U>(this IEnumerable<T> source, Func<T, U> selector)
          where U : IComparable<U>
        {
            if (source == null) throw new ArgumentNullException("source");
            bool first = true;
            T maxObj = default(T);
            U maxKey = default(U);
            foreach (var item in source)
            {
                if (first)
                {
                    maxObj = item;
                    maxKey = selector(maxObj);
                    first = false;
                }
                else
                {
                    U currentKey = selector(item);
                    if (currentKey.CompareTo(maxKey) > 0)
                    {
                        maxKey = currentKey;
                        maxObj = item;
                    }
                }
            }
            if (first) throw new InvalidOperationException("Sequence is empty.");
            return maxObj;
        }
    }
}
