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

namespace XmlEditorTool
{
    /// <summary>
    /// Interaction logic for SettingsPageView.xaml
    /// </summary>
    public partial class SettingsPageView : Page
    {
        public SettingsPageView()
        {
            InitializeComponent();
            // TODO -- add a viewmodel and model to improve loading of the current settings upon page generation
        }
    }
}
