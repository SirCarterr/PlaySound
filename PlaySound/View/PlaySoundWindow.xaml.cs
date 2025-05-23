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
using System.Windows.Shapes;
using PlaySound.ViewModel;

namespace PlaySound.View
{
    /// <summary>
    /// Interaction logic for PlaySoundWindow.xaml
    /// </summary>
    public partial class PlaySoundWindow : Window
    {
        public PlaySoundWindow(PlaySoundVM viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
