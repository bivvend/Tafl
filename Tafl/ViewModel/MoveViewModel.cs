using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tafl.AI;

namespace Tafl.ViewModel
{
    public class MoveViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string details;
        public string Details
        {
            get
            {
                return details;
            }
            set
            {
                details = value;
                RaisePropertyChanged("Name");
            }
        }

        private double sageScore;
        public double SageScore
        {
            get
            {
                return sageScore;
            }
            set
            {
                sageScore = value;
                RaisePropertyChanged("SageScore");
            }
        }

        private double assassinScore;
        public double AssassinScore
        {
            get
            {
                return assassinScore;
            }
            set
            {
                assassinScore = value;
                RaisePropertyChanged("AssassinScore");
            }
        }

        private double generalScore;
        public double GeneralScore
        {
            get
            {
                return generalScore;
            }
            set
            {
                generalScore = value;
                RaisePropertyChanged("GeneralScore");
            }
        }

        private double kingsCouncilScore;
        public double KingsCouncilScore
        {
            get
            {
                return kingsCouncilScore;
            }
            set
            {
                kingsCouncilScore = value;
                RaisePropertyChanged("KingsCouncilScore");
            }
        }

        public MoveViewModel(Move move)
        {

            Details = move.ToString();
        }


    }
}
