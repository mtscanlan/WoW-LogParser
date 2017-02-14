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
using System.Windows.Shapes;

namespace BabbleMouth.Windows
{
    /// <summary>
    /// Interaction logic for PlayerDetails.xaml
    /// </summary>
    public partial class PlayerDetails : Window
    {

        private Dictionary<string, int> playerDetails = new Dictionary<string, int>();

        public PlayerDetails()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        public void InsertData(Dictionary<string, int> dictionary)
        {
            try
            {
                playerDetails = dictionary;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PlayerDetailsDataGrid.ItemsSource = playerDetails;
                PlayerDetailsDataGrid.Columns.Last().Width = DataGridLength.Auto;
                PlayerDetailsDataGrid.Columns.First().Header = "Ability";
                PlayerDetailsDataGrid.Columns.Last().Header = "Count";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
    }
}
