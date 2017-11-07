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
        public Views.PieceView PieceInfo;

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

        private string pieceInfoString = "No piece selected";
        public string PieceInfoString
        {
            get => pieceInfoString;
            set
            {
                pieceInfoString = value;
                RaisePropertyChanged("PieceInfoString");
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

        public ICommand SquareClickCommand { get; private set; }


        public BoardViewModel(Model.BoardModel boardModel, Model.GameModel gameModel)
        {
            BoardSetup = boardModel;
            Game = gameModel;
            BoardSetup.SizeX = 11;
            BoardSetup.SizeY = 11;
            this.BoardSetup = boardModel;
            this.Game = gameModel;

            //SquareClickCommand = new RelayCommand(SquareClickExecute, param => true);
            SquareClickCommand = new RelayCommand(SquareClickExecute, param => true);
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
                        SelectSquare(square);
                        
                    }
                    if (square.DefenderPresent && Game.defenderIsAI == false && Game.currentTurnState == Model.GameModel.TurnState.Defender)
                    {
                        SelectSquare(square);
                    }
                }
            }
        }

        private void SelectSquare(Model.Square squareToSelect)
        {
            //Deselect all other squares
            foreach (Model.Square square in Board)
            {
                if (square.Row != squareToSelect.Row || square.Column != squareToSelect.Column)
                {
                    square.Selected = false;
                }
            }
            bool selectionMade = !squareToSelect.Selected;
            if(selectionMade)
            {
                PieceInfoString = String.Format("Row {0} Column {0}", squareToSelect.Row, squareToSelect.Column);
            }
            else
            {
                PieceInfoString = "No piece selected";
            }

            squareToSelect.Selected = !squareToSelect.Selected;
        }

        private void HighlightPossibleMoves(Model.Square squareSelected)
        {

        }
    }
}
