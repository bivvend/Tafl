using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Tafl.Model;
using Tafl.AI;

namespace Tafl.ViewModel 
{
    public class BoardViewModel: INotifyPropertyChanged
    {
        public MainWindow parent;
        public BoardModel BoardSetup;
        public GameModel Game;
        public GameViewModel GameVModel;
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

        public BoardViewModel(Model.BoardModel boardModel, GameViewModel gameViewModel, GameModel game)
        {
            BoardSetup = boardModel;
            GameVModel = gameViewModel;            
            BoardSetup.SizeX = 11;
            BoardSetup.SizeY = 11;
            Game = game;

            //SquareClickCommand = new RelayCommand(SquareClickExecute, param => true);
            SquareClickCommand = new RelayCommand(SquareClickExecute, param => true);
            EmptySquareClickCommand = new RelayCommand(EmptySquareClickExecute, param => true);
        }

        public async void EmptySquareClickExecute(object obj)
        {
            int[] Coords = (int[])obj;
            Move AIMove = new Move();
            bool moveMade = false;
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
                            

                            //Check for Defender victory
                            if(square.SquareType == Square.square_type.Corner && square.KingPresent)
                            {
                                //Defended Won!
                                GameVModel.CurrentTurnState = GameModel.TurnState.VictoryDefender;
                            }

                            //Check for Attacker victory
                            if(GameVModel.CurrentTurnState == GameModel.TurnState.Attacker)
                            {
                                //Is king surrounded on 4 sides
                                Board.ToList().ForEach((item) =>
                                {
                                    if(item.KingPresent)
                                    {
                                        if(BoardSetup.CheckForAttackerVictory())
                                        {
                                            GameVModel.CurrentTurnState = GameModel.TurnState.VictoryAttacker;
                                        }
                                    }                                   

                                });

                                
                            }

                            //Next Turn
                            moveMade = true;
                            if (GameVModel.CurrentTurnState == GameModel.TurnState.Attacker)
                            {
                                GameVModel.CurrentTurnState = GameModel.TurnState.Defender;
                                if(GameVModel.DefenderIsAI)
                                {
                                    GameVModel.Thinking = true;
                                    await Task.Run(async () =>
                                    {
                                        AIMove = await GameVModel.Game.RunAITurn(BoardSetup.GetSimpleBoard());
                                        await ApplyAIMove(AIMove);
                                    });
                                    GameVModel.Thinking = false;

                                }
                            }
                            else if (GameVModel.CurrentTurnState == GameModel.TurnState.Defender)
                            {
                                GameVModel.CurrentTurnState = GameModel.TurnState.Attacker;
                                if(GameVModel.AttackerIsAI)
                                {
                                    
                                    GameVModel.Thinking = true;
                                    await Task.Run(async () =>
                                    {
                                        AIMove = await GameVModel.Game.RunAITurn(BoardSetup.GetSimpleBoard());
                                        await ApplyAIMove(AIMove);
                                    });
                                    GameVModel.Thinking = false;
                                }
                            }

                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        public async Task ApplyAIMove(Move aMove)
        {
            while(Game.PauseAfterAITurn)
            {
                Task.Delay(10);
            }

            //Overlay the simple board onto the real board
            int SizeX = aMove.board.OccupationArray.GetLength(0);
            int SizeY = aMove.board.OccupationArray.GetLength(1);
            Square aSquare = new Square();
            for(int i=0; i<SizeY; i++) //Rows
            {
                for(int j=0; j<SizeX;j++) //Columns
                {
                    aSquare = GetSquare(i, j);
                    if(aSquare!=null)
                    {
                        aSquare.Occupation = aMove.board.OccupationArray[j, i];
                    }
                }
            }


            if (GameVModel.CurrentTurnState == GameModel.TurnState.Defender)
            {  
                GameVModel.CurrentTurnState = GameModel.TurnState.Attacker;
            }
            else if (GameVModel.CurrentTurnState == GameModel.TurnState.Attacker)
            {

                GameVModel.CurrentTurnState = GameModel.TurnState.Defender;
            }

        }

        

        public void SquareClickExecute(object obj)
        {
            int[] Coords = (int[]) obj;
            foreach(Model.Square square in Board)
            {
                if(square.Row == Coords[1] && square.Column== Coords[0])
                {
                    
                    if (square.AttackerPresent && GameVModel.AttackerIsAI == false && GameVModel.CurrentTurnState == Model.GameModel.TurnState.Attacker)
                    {
                        ApplySelection(square);

                    }
                    if ((square.DefenderPresent || square.KingPresent) && GameVModel.DefenderIsAI == false && GameVModel.CurrentTurnState == Model.GameModel.TurnState.Defender)
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
                PieceInfoString = String.Format("Row {0} Column {1}", squareToSelect.Row, squareToSelect.Column);
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
                if (aSquare != null)
                {
                    if (aSquare.Occupation == Square.occupation_type.Empty && (aSquare.SquareType != Square.square_type.Corner || squareSelected.KingPresent))
                    {
                        if (aSquare.SquareType != Square.square_type.Throne)
                        {
                            aSquare.Highlighted = true;
                        }
                        else
                        {
                            if (squareSelected.KingPresent)
                            {
                                aSquare.Highlighted = true;
                            }
                        }
                    }
                    else
                    {
                        //Don't break for Throne unless it is occupied
                        if (aSquare.SquareType != Square.square_type.Throne || aSquare.Occupation != Square.occupation_type.Empty)
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
                    if (aSquare.Occupation == Square.occupation_type.Empty && (aSquare.SquareType != Square.square_type.Corner || squareSelected.KingPresent))
                    {
                        if (aSquare.SquareType != Square.square_type.Throne)
                        {
                            aSquare.Highlighted = true;
                        }
                        else
                        {
                            if (squareSelected.KingPresent)
                            {
                                aSquare.Highlighted = true;
                            }
                        }
                    }
                    else
                    {
                        //Don't break for Throne
                        if (aSquare.SquareType != Square.square_type.Throne || aSquare.Occupation != Square.occupation_type.Empty)
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
                    if (aSquare.Occupation == Square.occupation_type.Empty && (aSquare.SquareType != Square.square_type.Corner || squareSelected.KingPresent))
                    {
                        if (aSquare.SquareType != Square.square_type.Throne)
                        {
                            aSquare.Highlighted = true;
                        }
                        else
                        {
                            if(squareSelected.KingPresent)
                            {
                                aSquare.Highlighted = true;
                            }
                        }
                    }
                    else
                    {
                        //Don't break for Throne
                        if (aSquare.SquareType != Square.square_type.Throne || aSquare.Occupation != Square.occupation_type.Empty)
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
                    if (aSquare.Occupation == Square.occupation_type.Empty && (aSquare.SquareType != Square.square_type.Corner || squareSelected.KingPresent))
                    {
                        if (aSquare.SquareType != Square.square_type.Throne)
                        {
                            aSquare.Highlighted = true;
                        }
                        else
                        {
                            if (squareSelected.KingPresent)
                            {
                                aSquare.Highlighted = true;
                            }
                        }
                    }
                    else
                    {
                        //Don't break for Throne
                        if (aSquare.SquareType != Square.square_type.Throne || aSquare.Occupation != Square.occupation_type.Empty)
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
