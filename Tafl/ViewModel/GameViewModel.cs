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

        private Model.GameModel gameModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Model.GameModel GameModel
        {
            get
            {
                return gameModel;
            }
            set
            {
                GameModel = value;
                RaisePropertyChanged("GameModel");
            }

        }
        
        
        public GameViewModel(BoardViewModel a_board)
        {
            //Attach commands to Relays
            CreateNewBoardCommand();
            this.Board = a_board;
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
