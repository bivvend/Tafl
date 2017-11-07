using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Tafl.Model
{
    public class BoardModel
    {

        public ObservableCollection<Model.Square> board;


        private int sizeX;
        public int SizeX
        {
            get
            {
                return sizeX;
            }
            set
            {
                if(value > 0)
                {
                    sizeX = value;
                    
                }
            }
        }

        private int sizeY;

        public int SizeY
        {
            get
            {
                return sizeY;
            }
            set
            {
                if (value > 0)
                {
                    sizeY = value;
                    
                }
            }
        }

        public BoardModel()
        {
            board = new ObservableCollection<Square>();
            CreateBoard();
        }

        public void CreateBoard()
        {
            this.board.Clear();
            for (int j = 0; j < SizeY; j++)
            {
                for (int i = 0; i < SizeX; i++)
                {
                    Model.Square a_square = new Model.Square(i, j, Model.Square.occupation_type.Empty, Model.Square.square_type.Normal);
                    if (a_square.BareTileType == Model.Square.bare_tile_type.tile1)
                        a_square.ImageName = "/Tafl;component/Resources/tile1.bmp";
                    if (a_square.BareTileType == Model.Square.bare_tile_type.tile2)
                        a_square.ImageName = "/Tafl;component/Resources/tile2.bmp";
                    if (a_square.BareTileType == Model.Square.bare_tile_type.tile3)
                        a_square.ImageName = "/Tafl;component/Resources/tile3.bmp";
                    if (a_square.BareTileType == Model.Square.bare_tile_type.tile4)
                        a_square.ImageName = "/Tafl;component/Resources/tile4.bmp";


                    //Make corner squares
                    if ((i == 0 && j == 0) || (i == 0 && j == SizeY - 1) || (i == SizeX - 1 && j == 0) || (i == SizeX - 1 && j == SizeY - 1))
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
                    if ((i == 3 && j == 5) || (i == 4 && j > 3 && j < 7) || (i == 5 && j > 2 && j < 8) || (i == 6 && j > 3 && j < 7) || (i == 7 && j == 5))
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
                    if (i == SizeX / 2 && j == SizeY / 2)
                    {
                        a_square.SquareType = Model.Square.square_type.Throne;
                        a_square.ImageName = "/Tafl;component/Resources/throne.bmp";
                        a_square.Occupation = Model.Square.occupation_type.King;
                    }
                    //Make edge zones for attackers
                    if (((i == 0 && j > 2 && j < 8) || (i == 1 && j == 5)) || ((i == 10 && j > 2 && j < 8) || (i == 9 && j == 5)) || ((j == 0 && i > 2 && i < 8) || (j == 1 && i == 5)) || ((j == 10 && i > 2 && i < 8) || (j == 9 && i == 5)))
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
                    board.Add(a_square);
                }
            }            
        }
    }
}
