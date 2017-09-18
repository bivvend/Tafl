using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Tafl.ViewModel
{
    public class GameViewModel:INotifyPropertyChanged
    {
        MainWindow parent;

        //Associated board
        private BoardViewModel board;
        public BoardViewModel Board
        {
            get
            {
                return this.board;
            }
            set
            {
                this.board = value;
                RaisePropertyChanged("Board");
            }
            
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Model.GameModel gameModel;
        public Model.GameModel GameModel
        {
            get
            {
                return gameModel;
            }
            set
            {
                gameModel = value;
                RaisePropertyChanged("GameModel");
            }

        }

        private Model.GameModel.TurnState currentTurnState;

        public Model.GameModel.TurnState CurrentTurnState
        {
            get
            {
                return currentTurnState;
            }
            set
            {
                currentTurnState = value;
                RaisePropertyChanged("CurrentTurnState");
            }
        }

        private bool attackerIsAI;
        public bool AttackerIsAI
        {
            get
            {
                return attackerIsAI;
            }
            set
            {
                attackerIsAI = value;
                RaisePropertyChanged("AttackerIsAI");
            }
        }

        private bool defenderIsAI;
        public bool DefenderIsAI
        {
            get
            {
                return defenderIsAI;
            }
            set
            {
                defenderIsAI = value;
                RaisePropertyChanged("DefenderIsAI");
            }
        }


        public GameViewModel(BoardViewModel a_board, MainWindow window)
        {
            this.parent = window;
            //Attach commands to Relays
            CreateNewBoardCommand();
            this.Board = a_board;
            this.GameModel = new Model.GameModel();
            this.CurrentTurnState = Model.GameModel.TurnState.Attacker;
            this.AttackerIsAI = true;
            this.DefenderIsAI = true;            
        }        


        public ICommand NewBoardCommand
        {
            get;
            internal set;
        }

        private bool CanExecuteNewBoardCommand()
        {
            return true;
        }

        private void CreateNewBoardCommand()
        {
            NewBoardCommand = new RelayCommand(NewBoardExecute, param => CanExecuteNewBoardCommand());
        }

        public void NewBoardExecute(object obj)
        {
            Board.CreateBoard();
        }
    }

    
}
