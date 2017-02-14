using BabbleMouth.PlayableClasses;
using System;
using System.Configuration;
using System.Windows;

namespace BabbleMouth.Windows
{
    /// <summary>
    /// Interaction logic for AddPlayer.xaml
    /// </summary>
    public partial class AddPlayer : Window
    {

        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        public AddPlayer()
        {
            try
            {
                InitializeComponent();
                ClassComboBox.ItemsSource = Enum.GetValues(typeof(PlayableClassEnum));
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!mainWindow.PlayableClassCollection.ContainsKey(PlayerNameInputTextBox.Text))
                {
                    if (ClassComboBox.SelectedItem != null && !string.IsNullOrEmpty(PlayerNameInputTextBox.Text))
                    {
                        PlayableClassEnum playableClassEnum = (PlayableClassEnum)Enum.Parse(typeof(PlayableClassEnum), ClassComboBox.SelectedItem.ToString());
                        switch (playableClassEnum)
                        {
                            case PlayableClassEnum.DEATH_KNIGHT:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new DeathKnight() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.DRUID:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Druid() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.HUNTER:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Hunter() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.MAGE:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Mage() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.MONK:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Monk() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.PALADIN:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Paladin() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.PRIEST:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Priest() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.ROGUE:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Rogue() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.SHAMAN:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Shaman() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.WARLOCK:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Warlock() { Name = PlayerNameInputTextBox.Text });
                                break;
                            case PlayableClassEnum.WARRIOR:
                                mainWindow.PlayableClassCollection.Add(PlayerNameInputTextBox.Text, new Warrior() { Name = PlayerNameInputTextBox.Text });
                                break;
                            default:
                                break;
                        }
                        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings.Add(PlayerNameInputTextBox.Text, ClassComboBox.SelectedItem.ToString());
                        config.Save(ConfigurationSaveMode.Modified);
                        mainWindow.StatusBarOutput.Background = System.Windows.Media.Brushes.DodgerBlue;
                        mainWindow.StatusBarTextBox.Text = "Added new player " + PlayerNameInputTextBox.Text + " - " + ClassComboBox.SelectedItem.ToString();

                        mainWindow.PlayerListBox.ItemsSource = null;
                        mainWindow.PlayerListBox.ItemsSource = mainWindow.PlayableClassCollection.Keys;
                        CloseButton_Click(null, null);
                    }
                    else MessageBox.Show("Please ensure all fields are completed!");
                }
                else MessageBox.Show("Player with same name already exists!");
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
