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
        public Model.BoardModel BoardSetup = new Model.BoardModel();

        private ObservableCollection<Model.Square> board;
        public ObservableCollection<Model.Square> Board
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
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

        public BoardViewModel()
        {
            BoardSetup.SizeX = 11;
            BoardSetup.SizeY = 11;

            this.Board = new ObservableCollection<Model.Square>();
            CreateBoard();
            //Attach commands to Relays
            CreateNewBoardCommand();
            
        }

        public void CreateBoard()
        {
            for(int j = 0; j < BoardSetup.SizeY;  j++)
            {
                for(int i = 0; i < BoardSetup.SizeY; i++)
                {
                    Model.Square a_square = new Model.Square(i, j, Model.Square.occupation_type.Empty, Model.Square.square_type.Normal);
                    if(a_square.BareTileType == Model.Square.bare_tile_type.tile1)
                        a_square.ImageName = "/Tafl;component/Resources/tile1.bmp";
                    if (a_square.BareTileType == Model.Square.bare_tile_type.tile2)
                        a_square.ImageName = "/Tafl;component/Resources/tile2.bmp";
                    if (a_square.BareTileType == Model.Square.bare_tile_type.tile3)
                        a_square.ImageName = "/Tafl;component/Resources/tile3.bmp";
                    if (a_square.BareTileType == Model.Square.bare_tile_type.tile4)
                        a_square.ImageName = "/Tafl;component/Resources/tile4.bmp";
                    Board.Add(a_square);
                }
            }
            //Make corner squares

            //Make central zone

            //Make throne

            //Make edge zones for attackers

            RaisePropertyChanged("Board");
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
            Board[0].Occupation = Model.Square.occupation_type.King;
        }

    }
}
