﻿using System;
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
        private Sage sage;
        public Sage Sage   //Sage evalutes the move tree
        {
            get
            {
                return sage;
            }
            set
            {
                sage = value;
            }
        }

        public string errorString = "";

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

        private List<Move> moveHistory;
        public List<Move> MoveHistory
        {
            get
            {
                return moveHistory;
            }
            set
            {
                moveHistory = value;
            }
        }

        public GameModel()
        {
            MoveViewModelList = new ObservableCollection<MoveViewModel>();
            Sage = new Sage();
            MoveHistory = new List<Move>();
        }

        private async void InvokeAction (Action a)
        {
            await Application.Current.Dispatcher.BeginInvoke(a);
        }

        public async Task<Move> RunAITurnLowerMem(SimpleBoard startBoard)
        {
            this.BaseBoard = startBoard;

            List<Move> moveList_0 = new List<Move>();

            InvokeAction(new Action(() => MoveViewModelList.Clear()));
            DateTime start = DateTime.Now;
            Sage.bestList = new List<Move>();
            Sage.longTermBestList = new List<Move>();
            
            //Fill list with all possible moves of depth 0

            moveList_0= BaseBoard.GetPossibleMoves(this.currentTurnState, null, 0);

            Object _lock = new object();

            //Create Depth 1 moves
            Parallel.ForEach(moveList_0, (m) =>
            {
                //Make the moves
                m.MakeMove(m, BaseBoard);

                //Look at all the depth 1 moves for the opposing side

                List<Move> moveList_1 = new List<Move>();
                List<Move> moveList_2 = new List<Move>();

                if (currentTurnState == TurnState.Defender)
                {

                    moveList_1 = m.board.GetPossibleMoves(TurnState.Attacker, m, 1);
                }
                if (currentTurnState == TurnState.Attacker)
                {
                    moveList_1 = m.board.GetPossibleMoves(TurnState.Defender, m, 1);

                }

                moveList_1.ForEach((m2) =>
                {
                    m2.MakeMove(m2, m2.parent.board);
                });

                moveList_1.ForEach((m2) =>
                {
                    //Look at all the depth 1 moves for the initial side                    

                    if (currentTurnState == TurnState.Defender)
                    {
                        moveList_2 = m2.board.GetPossibleMoves(TurnState.Defender, m2, 2);
                    }
                    if (currentTurnState == TurnState.Attacker)
                    {
                        moveList_2 = m2.board.GetPossibleMoves(TurnState.Attacker, m2, 2);

                    }

                    moveList_2.ForEach((m3) =>
                    {
                        //Make the moves
                        m3.MakeMove(m3, m3.parent.board);
                    });


                    //build List<List<Move>>
                    List<List<Move>> moveList = new List<List<Move>>();
                    moveList.Add(new List<Move>());
                    moveList[0].Add(m);
                    moveList.Add(moveList_1);
                    moveList.Add(moveList_2);

                    //Propagate scores
                    for (int i = moveList.Count - 1; i >= 0; i--)
                    {

                        //Push all the data to the depth 0 moves
                        moveList[i].ForEach((item) =>
                        {
                            if (i != 0)
                            {
                                //Total number of takes in the pipeline downwards
                                if (item.numberTakesAttacker > 0 || item.numberTakesDefender > 0)
                                {
                                    if (i == 1)
                                    {
                                        item.parent.numberTakesAttackerAtDepth[i] += item.numberTakesAttacker;
                                        item.parent.numberTakesDefenderAtDepth[i] += item.numberTakesDefender;
                                    }
                                    if (i == 2)
                                    {
                                        item.parent.parent.numberTakesAttackerAtDepth[i] += item.numberTakesAttacker;
                                        item.parent.parent.numberTakesDefenderAtDepth[i] += item.numberTakesDefender;
                                    }
                                }
                            }
                            else
                            {
                                item.numberTakesAttackerAtDepth[0] = item.numberTakesAttacker;
                                item.numberTakesDefenderAtDepth[0] = item.numberTakesDefender;
                            }


                        });

                    }

                    Sage.ProcessMovesLowerMem(moveList, currentTurnState);
                 

                });

            });   

            TimeSpan depth2duration = (DateTime.Now - start);
            double runtimetodepth2 = depth2duration.TotalSeconds;
            
            TimeSpan duration = (DateTime.Now - start);
            double runtime = duration.TotalSeconds;
            Move bestMove = Sage.PickBestLowerMem();


            InvokeAction(new Action(() =>
            {
                MoveViewModelList.Add(new MoveViewModel(bestMove));               

            }));

            bestMove.runTime = runtime;
            return bestMove;


        }

       
        public async Task<Move> RunAITurn(SimpleBoard startBoard)
        {
            this.BaseBoard = startBoard;

            List<List<Move>> moveList = new List<List<Move>>();
            InvokeAction(new Action(() => MoveViewModelList.Clear()));

            DateTime start = DateTime.Now;

            
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

            // moveList[1].ForEach((m2) =>
            //{
            //     //Make the moves
            //     m2.MakeMove(m2, m2.parent.board);
            //     //Look at all the depth 1 moves for the initial side

            //     if (currentTurnState == TurnState.Defender)
            //    {
            //        moveList[2].AddRange(m2.board.GetPossibleMoves(TurnState.Defender, m2, 2));
            //    }
            //    if (currentTurnState == TurnState.Attacker)
            //    {
            //        moveList[2].AddRange(m2.board.GetPossibleMoves(TurnState.Attacker, m2, 2));

            //    }

            //});


            Object _lock = new object();

            Parallel.ForEach(moveList[1], (m2) =>
            {
                //Look at all the depth 1 moves for the initial side
                m2.MakeMove(m2, m2.parent.board);
                lock (_lock)
                {
                    if (currentTurnState == TurnState.Defender)
                    {
                        moveList[2].AddRange(m2.board.GetPossibleMoves(TurnState.Defender, m2, 2));
                    }
                    if (currentTurnState == TurnState.Attacker)
                    {
                        moveList[2].AddRange(m2.board.GetPossibleMoves(TurnState.Attacker, m2, 2));

                    }
                }
            });



            //Make depth 2 moves
            //moveList[2].ForEach((m3) =>
            //{
            //    //Make the moves
            //     m3.MakeMove(m3, m3.parent.board);
            //});

            Parallel.ForEach(moveList[2], m3 =>
            {
                //Make the moves
                m3.MakeMove(m3, m3.parent.board);
            });

            TimeSpan depth2duration = (DateTime.Now - start);
            double runtimetodepth2 = depth2duration.TotalSeconds;

            //Propagate scores
            for (int i = moveList.Count-1; i>=0; i--)
            {

                //Push all the data to the depth 0 moves
                moveList[i].ForEach((item) =>                
                {
                    if (i != 0)
                    {
                        //Total number of takes in the pipeline downwards
                        if (item.numberTakesAttacker > 0 || item.numberTakesDefender > 0)
                        {
                            if (i == 1)
                            {
                                item.parent.numberTakesAttackerAtDepth[i] += item.numberTakesAttacker;
                                item.parent.numberTakesDefenderAtDepth[i] += item.numberTakesDefender;
                            }
                            if (i == 2)
                            {
                                item.parent.parent.numberTakesAttackerAtDepth[i] += item.numberTakesAttacker;
                                item.parent.parent.numberTakesDefenderAtDepth[i] += item.numberTakesDefender;
                            }
                        }
                    }
                    else
                    {
                        item.numberTakesAttackerAtDepth[0] = item.numberTakesAttacker;
                        item.numberTakesDefenderAtDepth[0] = item.numberTakesDefender;
                    }
                    
                    
                });               

            }

            Sage.ProcessMoves(moveList, currentTurnState, false);


            InvokeAction(new Action(() =>
            {
                Sage.bestList.ForEach((item) =>
                {
                    MoveViewModelList.Add(new MoveViewModel(item));
                });
                
            }));


            TimeSpan duration = (DateTime.Now - start);
            double runtime = duration.TotalSeconds;
            Sage.suggestedMove.runTime = runtime;
            return Sage.suggestedMove;
            
        }

        

        
        
    }

    
}
