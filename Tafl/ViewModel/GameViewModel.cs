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

        

        public Model.GameModel.TurnState CurrentTurnState
        {
            get
            {
                return Game.currentTurnState;
            }
            set
            {
                Game.currentTurnState = value;
                RaisePropertyChanged("CurrentTurnState");
            }
        }

        
        public bool AttackerIsAI
        {
            get
            {
                return Game.attackerIsAI;
            }
            set
            {
                Game.attackerIsAI = value;
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

        public ICommand NewBoardCommand
        {
            get;
            internal set;
        }

        public GameViewModel(Model.BoardModel boardModel, Model.GameModel gameModel)
        {

            Board = boardModel;
            Game = gameModel;
            //Attach commands to relays
            NewBoardCommand = new RelayCommand(NewBoardExecute, param => true);
        }        




        public void NewBoardExecute(object obj)
        {
            Board.CreateBoard();
        }
    }

    
}
