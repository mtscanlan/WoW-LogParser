using System;
using System.Configuration;
using System.Windows;

namespace BabbleMouth.Windows
{
    /// <summary>
    /// Interaction logic for AddSpell.xaml
    /// </summary>
    public partial class AddSpell : Window
    {

        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        public AddSpell()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!mainWindow.SpellList.Contains(SpellInputTextBox.Text))
                {
                    if (!string.IsNullOrEmpty(SpellInputTextBox.Text))
                    {
                        mainWindow.SpellList.Add(SpellInputTextBox.Text);
                        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings.Add(SpellInputTextBox.Text, "SPELL");
                        config.Save(ConfigurationSaveMode.Modified, true);
                        ConfigurationManager.RefreshSection("appSettings");
                        mainWindow.StatusBarOutput.Background = System.Windows.Media.Brushes.DodgerBlue;
                        mainWindow.StatusBarTextBox.Text = "Added new spell " + SpellInputTextBox.Text;

                        mainWindow.SpellList.Sort();
                        mainWindow.SpellsListBox.ItemsSource = null;
                        mainWindow.SpellsListBox.ItemsSource = mainWindow.SpellList;
                        CloseButton_Click(null, null);
                    }
                    else MessageBox.Show("Please ensure that all fields are completed!");
                }
                else MessageBox.Show("Spell with same name already exists!");

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Window_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    AddButton_Click(sender, e);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
    }
}
