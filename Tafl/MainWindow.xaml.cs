﻿using System;
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
        private ViewModel.BoardViewModel MainBoardViewModel = new ViewModel.BoardViewModel();
        private ViewModel.GameViewModel MainGameViewModel;

        public MainWindow()
        {
            InitializeComponent();
            //Associate game and board.
            this.MainGameViewModel = new ViewModel.GameViewModel(MainBoardViewModel);
            this.MainBoardView.DataContext = MainBoardViewModel;
            this.MainGameView.DataContext = MainGameViewModel;
        }
    }
}
