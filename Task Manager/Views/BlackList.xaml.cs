using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Task_Manager.Views
{
    /// <summary>
    /// Interaction logic for BlackList.xaml
    /// </summary>
    public partial class BlackList : Window
    {
        public ObservableCollection<ProcessData> GetBlackList { get; } = new ObservableCollection<ProcessData>();

        public BlackList()
        {
            InitializeComponent();
            ListViewManager.ItemsSource = GetBlackList;
        }

    }
}
