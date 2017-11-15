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

        public Model.BoardModel MainBoardModel;
        public Model.GameModel MainGameModel;

        public MainWindow()
        {
            InitializeComponent();

            MainBoardModel = new Model.BoardModel();           

            MainGameModel = new Model.GameModel();

            MainGameViewModel = new ViewModel.GameViewModel(MainBoardModel, MainGameModel);
            MainBoardViewModel = new ViewModel.BoardViewModel(MainBoardModel, MainGameViewModel);
            
            MainBoardView.DataContext = MainBoardViewModel;
            MainGameView.DataContext = MainGameViewModel;

            MainBoardViewModel.PieceInfo = this.MainPieceInfoView;
            MainBoardViewModel.PieceInfo.DataContext = MainBoardViewModel;

            MainBoardModel.CreateBoard();
        }
    }
}
