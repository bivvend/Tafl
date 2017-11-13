using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Tafl.Model;

namespace Tafl.ViewModel 
{
    public class BoardViewModel: INotifyPropertyChanged
    {
        public MainWindow parent;
        public BoardModel BoardSetup;
        public GameModel Game;
        public Views.PieceView PieceInfo;

        public Square SelectedSquare { get; set; }

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
        public ICommand EmptySquareClickCommand { get; private set; }

        public BoardViewModel(Model.BoardModel boardModel, GameModel gameModel)
        {
            BoardSetup = boardModel;
            Game = gameModel;
            BoardSetup.SizeX = 11;
            BoardSetup.SizeY = 11;
            this.Game = gameModel;

            //SquareClickCommand = new RelayCommand(SquareClickExecute, param => true);
            SquareClickCommand = new RelayCommand(SquareClickExecute, param => true);
            EmptySquareClickCommand = new RelayCommand(EmptySquareClickExecute, param => true);
        }

        public void EmptySquareClickExecute(object obj)
        {
            int[] Coords = (int[])obj;
            foreach (Model.Square square in Board)
            {
                if (square.Row == Coords[1] && square.Column == Coords[0])
                {
                    if (square.Highlighted && SelectedSquare != null)
                    {
                        if (BoardSetup.MovePiece(SelectedSquare.Row, SelectedSquare.Column, square.Row, square.Column))
                        {
                            SelectedSquare.Selected = false;
                            SelectedSquare = null;
                            Board.ToList().ForEach((item) => item.Highlighted = false);
                            if (Game.currentTurnState == GameModel.TurnState.Attacker)
                            {
                                Game.currentTurnState = GameModel.TurnState.Defender;
                            }
                            else if(Game.currentTurnState == GameModel.TurnState.Defender)
                            {
                                Game.currentTurnState = GameModel.TurnState.Attacker;
                            }

                        }
                    }
                    else
                    {

                    }
                }
            }
        }



        public void SquareClickExecute(object obj)
        {
            int[] Coords = (int[]) obj;
            foreach(Model.Square square in Board)
            {
                if(square.Row == Coords[1] && square.Column== Coords[0])
                {
                    
                    if (square.AttackerPresent && Game.attackerIsAI == false && Game.currentTurnState == Model.GameModel.TurnState.Attacker)
                    {
                        ApplySelection(square);

                    }
                    if ((square.DefenderPresent || square.KingPresent) && Game.defenderIsAI == false && Game.currentTurnState == Model.GameModel.TurnState.Defender)
                    {
                        ApplySelection(square);
                    }

                }
            }
        }



        private void ApplySelection(Square square)
        {
            SelectSquare(square);
            if (square.Selected)
            {
                HighlightPossibleMoves(square);
                SelectedSquare = square;
            }
            else
            {
                Board.ToList().ForEach((item) => item.Highlighted = false);
                SelectedSquare = null;
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
                //Clear All Current Highlighting

            }

            squareToSelect.Selected = !squareToSelect.Selected;
        }

        private void HighlightPossibleMoves(Square squareSelected)
        {
            int startColumn = squareSelected.Column;
            int startRow = squareSelected.Row;
            //Clear All Current Highlighting
            Board.ToList().ForEach((item) => item.Highlighted = false);

            //decrease column until zero. Stop when find occupied or zero
            Square aSquare = null;

            for (int N = startColumn-1; N>=0; N--)
            {
                aSquare = GetSquare(startRow, N);
                if ( aSquare != null)
                {
                    if(aSquare.Occupation == Square.occupation_type.Empty)
                    {
                        aSquare.Highlighted = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //increase column. Stop when find occupied or Full size

            for (int N = startColumn + 1; N < BoardSetup.SizeX; N++)
            {
                aSquare = GetSquare(startRow, N);
                if (aSquare != null)
                {
                    if (aSquare.Occupation == Square.occupation_type.Empty)
                    {
                        aSquare.Highlighted = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //increase row until zero. Stop when find occupied or Full size

            for (int N = startRow + 1; N < BoardSetup.SizeY; N++)
            {
                aSquare = GetSquare(N, startColumn);
                if (aSquare != null)
                {
                    if (aSquare.Occupation == Square.occupation_type.Empty)
                    {
                        aSquare.Highlighted = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //decrease row until zero. Stop when find occupied or zero
            for (int N = startRow - 1; N >= 0; N--)
            {
                aSquare = GetSquare(N, startColumn);
                if (aSquare != null)
                {
                    if (aSquare.Occupation == Square.occupation_type.Empty)
                    {
                        aSquare.Highlighted = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private Square GetSquare(int Row, int Column)
        {
            Square squareFound = null;

            List<Square> squaresFound = Board.Where((item) => item.Column == Column && item.Row == Row).ToList();
            if(squaresFound.Count>0)
            {
                squareFound = squaresFound[0];
            }
            return squareFound;
        }
    }
}
