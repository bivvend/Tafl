using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Tafl.ViewModel 
{
    public class BoardViewModel: INotifyPropertyChanged
    {
        public MainWindow parent;
        public Model.BoardModel BoardSetup;
        public Model.GameModel Game;

        private ObservableCollection<Model.Square> board;
        public ObservableCollection<Model.Square> Board
        {
            get
            {
                return BoardSetup.board;
            }
            set
            {
                BoardSetup.board = value;
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

        public BoardViewModel(Model.BoardModel boardModel, Model.GameModel gameModel)
        {
            BoardSetup = boardModel;
            Game = gameModel;
            BoardSetup.SizeX = 11;
            BoardSetup.SizeY = 11;
            this.BoardSetup = boardModel;
            this.Game = gameModel;            
            CreateSquareClickCommand();
            
        }

        

        public ICommand SquareClickCommand
        {
            get;
            internal set;
        }

        private bool CanExecuteSquareClickCommand()
        {
            return true;
        }

        private void CreateSquareClickCommand()
        {
            SquareClickCommand = new RelayCommand(SquareClickExecute, param => CanExecuteSquareClickCommand());
        }

        public void SquareClickExecute(object obj)
        {
            int[] Coords = (int[]) obj;
            foreach(Model.Square square in Board)
            {
                if(square.Row == Coords[1] && square.Column== Coords[0])
                {

                    if (square.AttackerPresent && Game.attackerIsAI==false && Game.currentTurnState == Model.GameModel.TurnState.Attacker)
                    {
                        square.Occupation = Model.Square.occupation_type.King;
                    }
                    if (square.DefenderPresent && Game.defenderIsAI == false && Game.currentTurnState == Model.GameModel.TurnState.Defender)
                    {
                        square.Occupation = Model.Square.occupation_type.King;
                    }
                }
            }

        }

    }
}
