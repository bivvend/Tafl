﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tafl.AI;

namespace Tafl.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public BoardViewModel boardViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Model.GameModel gameModel;
        public Model.GameModel Game
        {
            get
            {
                return gameModel;
            }
            set
            {
                gameModel = value;
            }

        }

        private Model.BoardModel boardModel;
        public Model.BoardModel Board
        {
            get
            {
                return boardModel;
            }
            set
            {
                boardModel = value;

            }

        }

        private ObservableCollection<MoveViewModel> moveList;
        public ObservableCollection<MoveViewModel> MoveList
        {
            get
            {
                return moveList;
            }
            set
            {
                moveList = value;
                RaisePropertyChanged("MoveList");

            }

        }

        private ObservableCollection<MoveViewModel> moveHistory;
        public ObservableCollection<MoveViewModel> MoveHistory
        {
            get
            {
                return moveHistory;
            }
            set
            {
                moveHistory = value;
                RaisePropertyChanged("MoveHistory");

            }

        }

        private double runTime;
        public double RunTime
        {
            get
            {
                return runTime;
            }
            set
            {
                runTime = value;
                RaisePropertyChanged("RunTime");
            }
        }

        private bool pauseAfterAITurn;
        public bool PauseAfterAITurn
        {
             get
            {
                return Game.PauseAfterAITurn;
            }
            set
            {

                Game.PauseAfterAITurn = value;
                pauseAfterAITurn = value;
                RaisePropertyChanged("PauseAfterAITurn");
            }
        }

        private Model.GameModel.TurnState currentTurnState;
        public Model.GameModel.TurnState CurrentTurnState
        {
            get
            {
                return Game.currentTurnState;
            }
            set
            {
                
                Game.currentTurnState = value;
                currentTurnState = value;
                RaisePropertyChanged("CurrentTurnState");
            }
        }

        private bool attackerIsAI;
        public bool AttackerIsAI
        {
            get
            {
                return Game.attackerIsAI;
            }
            set
            {
                Game.attackerIsAI = value;
                attackerIsAI = value;
                RaisePropertyChanged("AttackerIsAI");
            }
        }

        private bool defenderIsAI;
        public bool DefenderIsAI
        {
            get
            {
                return Game.defenderIsAI;
            }
            set
            {
                defenderIsAI = value;
                Game.defenderIsAI = value;
                RaisePropertyChanged("DefenderIsAI");
            }
        }

        private bool thinking;
        public bool Thinking
        {
            get
            {
                return thinking;
            }
            set
            {
                thinking = value;
                RaisePropertyChanged("Thinking");
            }
        }

        public ICommand NewBoardCommand
        {
            get;
            internal set;
        }

        public ICommand AIDefenderSetChanged
        {
            get;
            internal set;
        }



        public ICommand AIAttackerSetChanged
        {
            get;
            internal set;
        }




        public GameViewModel(Model.BoardModel boardModel, Model.GameModel gameModel)
        {

            Board = boardModel;
            Game = gameModel;
            MoveList = Game.MoveViewModelList;
            MoveHistory = new ObservableCollection<MoveViewModel>();
            //Attach commands to relays
            NewBoardCommand = new RelayCommand(NewBoardExecute, param => true);
            AIDefenderSetChanged = new RelayCommand(AIDefenderSetChangedExecute, param => true);
            AIAttackerSetChanged = new RelayCommand(AIAttackerSetChangedExecute, param => true);
        }        




        public void NewBoardExecute(object obj)
        {
            Board.CreateBoard();
            MoveHistory.Clear();
            CurrentTurnState = Model.GameModel.TurnState.Attacker;
        }

        public void AIDefenderSetChangedExecute(object obj)
        {
            if (DefenderIsAI && CurrentTurnState == Model.GameModel.TurnState.Defender)
            {
                //Start AI
                StartAI();
            }
        }

        public void AIAttackerSetChangedExecute(object obj)
        {
            if (AttackerIsAI && CurrentTurnState == Model.GameModel.TurnState.Attacker)
            {
                //Start AI
                StartAI();
            }
        }



        public async void StartAI()
        {
            Thinking = true;
            Move AIMove = new Move();
            await Task.Run(async () =>
            {
                //AIMove = await Game.RunAITurn(Board.GetSimpleBoard());
                if(boardViewModel.lowMem)
                    AIMove = await Game.RunAITurnLowerMem(Board.GetSimpleBoard());
                else
                    AIMove = await Game.RunAITurn(Board.GetSimpleBoard());
                await boardViewModel.ApplyAIMove(AIMove);
                
                RunTime = AIMove.runTime;
            });
            MoveHistory.Add(new MoveViewModel(AIMove));
            Thinking = false;
            

            //Start next turn

            if(currentTurnState == Model.GameModel.TurnState.Attacker && AttackerIsAI)
            {
                StartAI();
            }

            if (currentTurnState == Model.GameModel.TurnState.Defender && DefenderIsAI)
            {
                StartAI();
            }


        }
    }

    
}
