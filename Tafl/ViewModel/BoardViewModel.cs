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
            CreateTestCommand();
            
        }

        public void CreateBoard()
        {
            for(int j = 0; j < BoardSetup.SizeY;  j++)
            {
                for(int i = 0; i < BoardSetup.SizeY; i++)
                {
                    Model.Square a_square = new Model.Square(i, j, Model.Square.occupation_type.Empty, Model.Square.square_type.Normal);
                    Board.Add(a_square);
                }
            }
            RaisePropertyChanged("Board");

        }

        public ICommand TestCommand
        {
            get;
            internal set;
        }

        private bool CanExecuteTestCommand()
        {
            return true;
        }

        private void CreateTestCommand()
        {
            TestCommand = new RelayCommand(TestExecute, param => CanExecuteTestCommand());
        }

        public void TestExecute(object obj)
        {
            Board[0].SquareType = Model.Square.square_type.AttackerStart;
        }

    }
}
