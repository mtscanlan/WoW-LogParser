using BabbleMouth.PlayableClasses;
using BabbleMouth.Properties;
using BabbleMouth.Windows;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace BabbleMouth
{
    public partial class MainWindow : Window
    {

        public SortedDictionary<string, PlayableClass> PlayableClassCollection = new SortedDictionary<string, PlayableClass>();
        public List<string> SpellList = new List<string>();

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                LoadPlayers();
                StatusBarTextBox.Text = "Click File -> Import Combat Log... to begin.";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (new AboutBox()).Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (new AddPlayer()).ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void AddSpellInfoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (new AddSpell()).ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void ExportAllPlayerBBCodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string output = string.Empty;
                PlayableClassCollection.ForEach(player =>
                {
                    if (player.Value.Abilities.Count() != 0)
                    {
                        output += "[b][u]" + player.Key + "[/u][/b]";
                        output += "[spoiler]";
                        player.Value.Abilities.ForEach(ability =>
                        {
                            output += ability.Value + "\t" + ability.Key + "\n";
                        });
                        output += "[/spoiler]";
                    }
                });
                ExportBBCode exportBBCodeWindow = new ExportBBCode();
                exportBBCodeWindow.FillRichTextBox(output);
                StatusBarTextBox.Text = "Exporting to BBCode";
                exportBBCodeWindow.Closing += (s, ev) =>
                {
                    StatusBarTextBox.Text = string.Empty;
                };
                exportBBCodeWindow.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void ExportAllSpellBBCodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string output = string.Empty;
                SpellList.ForEach(spell =>
                {
                    int spellCounter = 0; // Counter to ensure that spell was cast (helps clean up BBCode).
                    string spellOutput = "[b][u]" + spell + "[/u][/b]";
                    spellOutput += "[spoiler]";
                    PlayableClassCollection.ForEach(player =>
                    {
                        if (player.Value.Abilities.Count() != 0)
                        {
                            player.Value.Abilities.ForEach(ability =>
                            {
                                if (spell == ability.Key)
                                {
                                    spellCounter++;
                                    spellOutput += ability.Value + "\t" + player.Key + "\n";
                                }
                            });
                        }
                    });
                    spellOutput += "[/spoiler]";
                    if (spellCounter != 0) output += spellOutput;
                });
                ExportBBCode exportBBCodeWindow = new ExportBBCode();
                exportBBCodeWindow.FillRichTextBox(output);
                StatusBarTextBox.Text = "Exporting to BBCode";
                exportBBCodeWindow.Closing += (s, ev) =>
                {
                    StatusBarTextBox.Text = string.Empty;
                };
                exportBBCodeWindow.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void ExportSelectedPlayerBBCodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string output = string.Empty;
                PlayableClassCollection.ForEach(player =>
                {
                    if (player.Value.Abilities.Count() != 0 && PlayerListBox.SelectedItems.Count > 0 && PlayerListBox.SelectedItems.Contains(player.Key))
                    {
                        output += "[b][u]" + player.Key + "[/u][/b]";
                        output += "[spoiler]";
                        player.Value.Abilities.ForEach(ability =>
                        {
                            output += ability.Value + "\t" + ability.Key + "\n";
                        });
                        output += "[/spoiler]";
                    }
                });
                ExportBBCode exportBBCodeWindow = new ExportBBCode();
                exportBBCodeWindow.FillRichTextBox(output);
                StatusBarTextBox.Text = "Exporting to BBCode";
                exportBBCodeWindow.Closing += (s, ev) =>
                {
                    StatusBarTextBox.Text = string.Empty;
                };
                exportBBCodeWindow.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void ExportSelectedSpellBBCodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string output = string.Empty;
                SpellList.ForEach(spell =>
                {
                    if (SpellsListBox.SelectedItems.Count > 0 && SpellsListBox.SelectedItems.Contains(spell))
                    {
                        int spellCounter = 0; // Counter to ensure that spell was cast (helps clean up BBCode).
                        string spellOutput = "[b][u]" + spell + "[/u][/b]";
                        spellOutput += "[spoiler]";
                        PlayableClassCollection.ForEach(player =>
                        {
                            if (player.Value.Abilities.Count() != 0)
                            {
                                player.Value.Abilities.ForEach(ability =>
                                {
                                    if (spell == ability.Key)
                                    {
                                        spellCounter++;
                                        spellOutput += ability.Value + "\t" + player.Key + "\n";
                                    }
                                });
                            }
                        });
                        spellOutput += "[/spoiler]";
                        if (spellCounter != 0) output += spellOutput;
                    }
                });
                ExportBBCode exportBBCodeWindow = new ExportBBCode();
                exportBBCodeWindow.FillRichTextBox(output);
                StatusBarTextBox.Text = "Exporting to BBCode";
                exportBBCodeWindow.Closing += (s, ev) =>
                {
                    StatusBarTextBox.Text = string.Empty;
                };
                exportBBCodeWindow.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void ImportCombatLogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";

                // Process open file dialog box results 
                if (dlg.ShowDialog() == true)
                {
                    StatusBarTextBox.Text = "Importing combat log...";
                    StatusBarOutput.Background = System.Windows.Media.Brushes.Chocolate;
                    PlayableClassCollection.Values.ForEach(x => x.PlayerLogs.Clear());
                    UInt64 lineCounter = 0;
                    List<Thread> threadPool = new List<Thread>();
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += (s, ev) =>
                    {
                        using (System.IO.StreamReader file = new System.IO.StreamReader(dlg.FileName))
                        {
                            string line = string.Empty;
                            while (!string.IsNullOrEmpty(line = file.ReadLine()))
                            {
                                ParseLine(line);
                                lineCounter++;
                            }
                        }
                    };
                    bw.ProgressChanged += (s, ev) =>
                    {
                        StatusBarTextBox.Text = ("Completed: " + ev.ProgressPercentage.ToString() + "%");
                    };
                    bw.RunWorkerCompleted += (s, ev) =>
                    {
                        PlayableClassCollection.Values.ForEach(player =>
                        {
                            Thread newThread = new Thread(() =>
                            {
                                player.ParseLines();
                            });
                            threadPool.Add(newThread);
                            newThread.Start();
                        });
                        threadPool.ForEach(thread => thread.Join());
                        PlayerListBox.MouseDoubleClick += PlayerListBox_MouseDoubleClick;
                        timer.Stop();
                        StatusBarTextBox.Text = "Import combat log successful! " + lineCounter + " lines in " + timer.Elapsed.TotalSeconds + "s";
                        StatusBarOutput.Background = System.Windows.Media.Brushes.DodgerBlue;
                    };
                    bw.RunWorkerAsync();
                }
                else StatusBarTextBox.Text = string.Empty;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void LoadPlayers()
        {
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                foreach (string key in appSettings)
                {
                    if (appSettings[key] == "SPELL")
                    {
                        SpellList.Add(key);
                    }
                    else
                    {
                        PlayableClassEnum playableClassEnum = (PlayableClassEnum)Enum.Parse(typeof(PlayableClassEnum), appSettings[key]);
                        switch (playableClassEnum)
                        {
                            case PlayableClassEnum.DEATH_KNIGHT:
                                PlayableClassCollection.Add(key, new DeathKnight() { Name = key });
                                break;
                            case PlayableClassEnum.DRUID:
                                PlayableClassCollection.Add(key, new Druid() { Name = key });
                                break;
                            case PlayableClassEnum.HUNTER:
                                PlayableClassCollection.Add(key, new Hunter() { Name = key });
                                break;
                            case PlayableClassEnum.MAGE:
                                PlayableClassCollection.Add(key, new Mage() { Name = key });
                                break;
                            case PlayableClassEnum.MONK:
                                PlayableClassCollection.Add(key, new Monk() { Name = key });
                                break;
                            case PlayableClassEnum.PALADIN:
                                PlayableClassCollection.Add(key, new Paladin() { Name = key });
                                break;
                            case PlayableClassEnum.PRIEST:
                                PlayableClassCollection.Add(key, new Priest() { Name = key });
                                break;
                            case PlayableClassEnum.ROGUE:
                                PlayableClassCollection.Add(key, new Rogue() { Name = key });
                                break;
                            case PlayableClassEnum.SHAMAN:
                                PlayableClassCollection.Add(key, new Shaman() { Name = key });
                                break;
                            case PlayableClassEnum.WARLOCK:
                                PlayableClassCollection.Add(key, new Warlock() { Name = key });
                                break;
                            case PlayableClassEnum.WARRIOR:
                                PlayableClassCollection.Add(key, new Warrior() { Name = key });
                                break;
                            default:
                                break;
                        }
                    }
                }
                PlayerListBox.ItemsSource = PlayableClassCollection.Keys;
                SpellList.Sort();
                SpellsListBox.ItemsSource = SpellList;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void ParseLine(string line)
        {
            try
            {
                // string split
                if (line != string.Empty)
                {
                    string[] logSplit = line.Split(new char[] { ' ' }, 3).Last().Split(',');
                    string name = logSplit[2].Replace('\"', ' ').Trim();
                    if (PlayableClassCollection.ContainsKey(name))
                    {
                        PlayableClassCollection[name].PlayerLogs.Add(logSplit);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void PlayerListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (PlayerListBox.SelectedItems.Count == 1 && PlayableClassCollection[PlayerListBox.SelectedItem.ToString()].Abilities.Count > 0)
                {
                    PlayerDetails playerDetailsWindow = new PlayerDetails();
                    playerDetailsWindow.InsertData(PlayableClassCollection[PlayerListBox.SelectedItem.ToString()].Abilities);
                    playerDetailsWindow.Title = "Player Summary - " + PlayerListBox.SelectedItem.ToString();
                    StatusBarTextBox.Text = playerDetailsWindow.Title;
                    playerDetailsWindow.Closing += (s, ev) =>
                    {
                        StatusBarTextBox.Text = String.Empty;
                    };
                    playerDetailsWindow.Show();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void RemovePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PlayerListBox.SelectedItems.Count > 0)
                {
                    foreach (string player in PlayerListBox.SelectedItems)
                    {
                        PlayableClassCollection.Remove(player);
                        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings.Remove(player);
                        config.Save(ConfigurationSaveMode.Modified);
                    }
                    if (PlayerListBox.SelectedItems.Count > 1)
                    {
                        StatusBarOutput.Background = System.Windows.Media.Brushes.DodgerBlue;
                        StatusBarTextBox.Text = "Removed multiple players";
                    }
                    else
                    {
                        StatusBarOutput.Background = System.Windows.Media.Brushes.DodgerBlue;
                        StatusBarTextBox.Text = "Removed " + PlayerListBox.SelectedItem;
                    }

                    PlayerListBox.SelectedIndex = -1;
                    PlayerListBox.ItemsSource = null;
                    PlayerListBox.ItemsSource = PlayableClassCollection.Keys;
                }
                else
                {
                    StatusBarOutput.Background = System.Windows.Media.Brushes.Chocolate;
                    StatusBarTextBox.Text = "No player selected!";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void RemoveSpellButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SpellsListBox.SelectedItems.Count > 0)
                {
                    foreach (string spell in SpellsListBox.SelectedItems)
                    {
                        SpellList.Remove(spell);
                        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings.Remove(spell);
                        config.Save(ConfigurationSaveMode.Modified);
                    }
                    if (SpellsListBox.SelectedItems.Count > 1)
                    {
                        StatusBarOutput.Background = System.Windows.Media.Brushes.DodgerBlue;
                        StatusBarTextBox.Text = "Removed multiple spells";
                    }
                    else
                    {
                        StatusBarOutput.Background = System.Windows.Media.Brushes.DodgerBlue;
                        StatusBarTextBox.Text = "Removed " + SpellsListBox.SelectedItem;
                    }

                    SpellsListBox.SelectedIndex = -1;
                    SpellList.Sort();
                    SpellsListBox.ItemsSource = null;
                    SpellsListBox.ItemsSource = SpellList;
                }
                else
                {
                    StatusBarOutput.Background = System.Windows.Media.Brushes.Chocolate;
                    StatusBarTextBox.Text = "No spell selected!";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

    }
}
