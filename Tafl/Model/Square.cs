﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Tafl.Model
{
    public class Square: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
               PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        
        public enum occupation_type
        {
            Attacker, Defender, King, Empty
        };

        public enum square_type
        {
            Normal, Throne, AttackerStart, Corner, DefenderStart
        };

        private occupation_type occupation;
        public  occupation_type Occupation
        {
            get
            {
                return occupation;
            }
            set
            {
                occupation = value;
                if(value == occupation_type.Attacker)
                {
                    KingPresent = false;
                    AttackerPresent = true;
                    DefenderPresent = false;                    
                }
                if(value== occupation_type.Defender)
                {
                    KingPresent = false;
                    AttackerPresent = false;
                    DefenderPresent = true;
                }
                if(value == occupation_type.King)
                {
                    KingPresent = true;
                    AttackerPresent = false;
                    DefenderPresent = false;
                }
                if(value == occupation_type.Empty)
                {
                    KingPresent = false;
                    AttackerPresent = false;
                    DefenderPresent = false;
                }
                RaisePropertyChanged("Occupation");

            }
        }

        private square_type squareType;
        public square_type SquareType
        {
            get
            {
                return squareType;
            }
            set
            {
                squareType = value;
                RaisePropertyChanged("SquareType");
            }
        }

        public enum bare_tile_type
        {
            tile1, tile2, tile3, tile4
        };

        private bare_tile_type bareTileType;
        public bare_tile_type BareTileType
        {
            get
            {
                return bareTileType;
            }
            set
            {
                bareTileType = value;
                RaisePropertyChanged("BareTileType");
            }
        }

        private bool attackerPresent;
        public bool AttackerPresent
        {
            get
            {
                return attackerPresent;
            }
            set
            {
                attackerPresent = value;
                RaisePropertyChanged("AttackerPresent");
            }
        }

        private bool defenderPresent;
        public bool DefenderPresent
        {
            get
            {
                return defenderPresent;
            }
            set
            {
                defenderPresent = value;
                RaisePropertyChanged("DefenderPresent");
            }
        }
        private bool kingPresent;
        public bool KingPresent
        {
            get
            {
                return kingPresent;
            }
            set
            {
                kingPresent = value;
                RaisePropertyChanged("KingPresent");
            }
        }




        private string imageName;
        public string ImageName
        {
            get
            {
                return imageName;
            }
            set
            {
                imageName = value;
                RaisePropertyChanged("ImageName");
            }

        }


        private int row;
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
                Coords[0] = value;
                RaisePropertyChanged("Row");
                RaisePropertyChanged("Coords");
            }
        }

        private int column;
        

        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
                Coords[0] = value;
                RaisePropertyChanged("Column");
                RaisePropertyChanged("Coords");
            }
        }

        private int[] coords;
        public int[] Coords
        {
            get
            {
                return coords;
            }
            set
            {
                coords = value;
                RaisePropertyChanged("Coords");
            }
        }

        private bool selected;
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                RaisePropertyChanged("Selected");
            }
        }

        private bool highlighted;
        public bool Highlighted
        {
            get => highlighted;
            set
            {
                highlighted = value;
                RaisePropertyChanged("Highlighted");
            }
        }


        public Square()
        {
            Random random = new Random();
            double a_val = random.NextDouble();

            if (a_val >= 0.0 && a_val < 0.25)
                this.BareTileType = bare_tile_type.tile1;
            if (a_val >= 0.25 && a_val < 0.5)
                this.BareTileType = bare_tile_type.tile2;
            if (a_val >= 0.5 && a_val < 0.75)
                this.BareTileType = bare_tile_type.tile3;
            if (a_val >= 0.75)
                this.BareTileType = bare_tile_type.tile4;
            Highlighted = false;
            Selected = false;

            this.Coords = new int[] { 0, 0 };
        }

        public Square( int _column, int _row, occupation_type _occupancy_type, square_type _square_type)
        {
            this.Coords = new int[] { _column, _row };
            this.Row = _row;
            this.Column = _column;
            this.Occupation = _occupancy_type;
            this.SquareType = _square_type;
            Random random = new Random((_row+1)*(_column+1) + _row + _column);
            int seed = random.Next();
            random = new Random(seed + _row + _column);            
            double a_val = random.NextDouble();
            if (a_val >= 0.0 && a_val < 0.25)
                this.BareTileType = bare_tile_type.tile1;
            if (a_val >= 0.25 && a_val < 0.5)
                this.BareTileType = bare_tile_type.tile2;
            if (a_val >= 0.5 && a_val < 0.75)
                this.BareTileType = bare_tile_type.tile3;
            if (a_val >= 0.75)
                this.BareTileType = bare_tile_type.tile4;
            Highlighted = false;
            Selected = false;
        }
    }
}
