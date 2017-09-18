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
            
            
        }

        public void CreateBoard()
        {
            this.Board.Clear();
            for (int j = 0; j < BoardSetup.SizeY;  j++)
            {
                for(int i = 0; i < BoardSetup.SizeX; i++)
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


                    //Make corner squares
                    if((i==0 && j==0) || (i==0 && j==BoardSetup.SizeY-1) || (i==BoardSetup.SizeX-1 && j==0) || (i==BoardSetup.SizeX-1 && j==BoardSetup.SizeY-1))
                    {
                        a_square.SquareType = Model.Square.square_type.Corner;
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile1)
                            a_square.ImageName = "/Tafl;component/Resources/deftile1.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile2)
                            a_square.ImageName = "/Tafl;component/Resources/deftile2.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile3)
                            a_square.ImageName = "/Tafl;component/Resources/deftile3.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile4)
                            a_square.ImageName = "/Tafl;component/Resources/deftile4.bmp";

                    }
                    //Make central (Defender) zone
                    if( (i==3 && j ==5)  ||   (i==4 && j>3 && j<7) || (i==5 && j>2 && j<8)  || (i==6 && j>3 && j < 7) ||  (i==7 && j==5))
                    {
                        a_square.SquareType = Model.Square.square_type.DefenderStart;
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile1)
                            a_square.ImageName = "/Tafl;component/Resources/deftile1.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile2)
                            a_square.ImageName = "/Tafl;component/Resources/deftile2.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile3)
                            a_square.ImageName = "/Tafl;component/Resources/deftile3.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile4)
                            a_square.ImageName = "/Tafl;component/Resources/deftile4.bmp";
                        a_square.Occupation = Model.Square.occupation_type.Defender;
                    }
                    //Make throne
                    if( i== BoardSetup.SizeX/2 && j==BoardSetup.SizeY/2)
                    {
                        a_square.SquareType = Model.Square.square_type.Throne;
                        a_square.ImageName = "/Tafl;component/Resources/throne.bmp";
                        a_square.Occupation = Model.Square.occupation_type.King;
                    }
                    //Make edge zones for attackers
                    if( ((i==0 && j>2 && j <8) || (i== 1 && j==5))    || ((i == 10 && j > 2 && j < 8) || (i == 9 && j == 5))  || ((j == 0 && i > 2 && i < 8) || (j == 1 && i == 5))  || ((j == 10 && i > 2 && i < 8) || (j == 9 && i == 5)))
                    {
                        a_square.SquareType = Model.Square.square_type.AttackerStart;
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile1)
                            a_square.ImageName = "/Tafl;component/Resources/attacktile1.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile2)
                            a_square.ImageName = "/Tafl;component/Resources/attacktile2.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile3)
                            a_square.ImageName = "/Tafl;component/Resources/attacktile3.bmp";
                        if (a_square.BareTileType == Model.Square.bare_tile_type.tile4)
                            a_square.ImageName = "/Tafl;component/Resources/attacktile4.bmp";
                        a_square.Occupation = Model.Square.occupation_type.Attacker;
                    }
                    Board.Add(a_square);
                }
            }            

            RaisePropertyChanged("Board");
        }

        

    }
}
