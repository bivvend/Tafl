using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Tafl.Model
{
    public class BoardModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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
                if (value > 0)
                {
                    sizeY = value;
                    RaisePropertyChanged("SizeY");
                }
            }
        }

        public BoardModel()
        {

        }
    }
}
