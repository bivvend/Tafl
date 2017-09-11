using System;
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
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public enum occupation_type
        {
            Attacker, Defender, King, Empty
        };

        public enum square_type
        {
            Normal, Throne, AttackerStart, Corner
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
                RaisePropertyChanged("Row");
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
                RaisePropertyChanged("Column");
            }
        }

        public Square()
        {

        }

        public Square( int _row, int _column, occupation_type _occupancy_type, square_type _square_type)
        {
            this.Row = _row;
            this.Column = _column;
            this.Occupation = _occupancy_type;
            this.SquareType = _square_type;
        }
    }
}
