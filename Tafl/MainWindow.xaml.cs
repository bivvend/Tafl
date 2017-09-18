using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tafl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel.BoardViewModel MainBoardViewModel;
        public ViewModel.GameViewModel MainGameViewModel;

        public MainWindow()
        {
            InitializeComponent();
            MainBoardViewModel = new ViewModel.BoardViewModel(this);
            //Associate game and board.
            this.MainGameViewModel = new ViewModel.GameViewModel(MainBoardViewModel,this);
            //Associate board and game
            
            this.MainBoardView.DataContext = MainBoardViewModel;
            this.MainGameView.DataContext = MainGameViewModel;
        }
    }
}
