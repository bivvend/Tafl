using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Tafl.Model
{
    public class BoardModel
    {
    }

    public class Board : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int sizeX;
        public int SizeX
        {
            get
            {
                return sizeX;
            }
            set
            {
                if(sizeX != value)
                {
                    sizeX = value;
                    RaisePropertyChanged("SizeX");
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
                if (sizeY != value)
                {
                    sizeY = value;
                    RaisePropertyChanged("SizeY");
                }
            }
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
