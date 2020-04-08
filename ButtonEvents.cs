using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KcptunManager
{
    public partial class MainWindow : Window
    {
        private void LaunchButton_OnClick(object sender, RoutedEventArgs e)
        {
            ConfigListBox.SelectedItem = null;
            var selectedItems = ConfigListBox.GetSelectedItems();

            if (selectedItems.Count < 1)
            {
                MessageBox.Show("请至少选择一个配置后重试", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var selectedItem in selectedItems)
            {
                ConfigListBox.Items.IsLiveSorting = true;
                selectedItem.Background = new SolidColorBrush(Colors.GreenYellow);
                selectedItem.IsEnabled = false;
                selectedItem.UnChecked();

                var config = selectedItem.DataContext as Config;

                var textBox = new TextBox();

                var layout = new Grid();
                layout.Children.Add(textBox);

                var nameHeader = new TextBlock { Text = config?.Name };
                var addrHeader = new TextBlock { Text = config?.LocalAddress };

                var tabHeaderPanel = new StackPanel();
                tabHeaderPanel.Children.Add(nameHeader);
                tabHeaderPanel.Children.Add(addrHeader);

                TabItem tabItem = new TabItem
                {
                    Header = tabHeaderPanel,
                    Content = layout
                };

                tabItem.Background = new SolidColorBrush(Colors.Lime);
                tabItem.MouseDoubleClick += (o, args) =>
                {
                    InstanceTab.Items.Remove(tabItem);
                    if (_instanceMaps.Any(i => i.TabItem == tabItem))
                    {
                        var instanceMaps = _instanceMaps
                            .Where(i => i.TabItem == tabItem)
                            .ToList();

                        instanceMaps.Where(i => i.Process.HasExited == false).ToList().ForEach(i => i.Process.Kill());

                        instanceMaps.ForEach(i => i.ListBoxItem.IsEnabled = true);
                        instanceMaps.ForEach(i => i.ListBoxItem.Background = null);
                        instanceMaps.ForEach(i => _instanceMaps.Remove(i));
                    }
                };

                InstanceTab.Items.Add(tabItem);

                var instance = Kcptun.GetInstance(config);
                instance.ErrorDataReceived += (o, args) =>
                {
                    if (textBox.Dispatcher.CheckAccess())
                    {
                        textBox.Text += $"{args.Data} \r\n";
                    }
                    else
                    {
                        textBox.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            textBox.Text += $"{args.Data} \r\n";
                        }));
                    }
                };

                instance.Exited += (o, args) =>
                {
                    if (tabItem.Dispatcher.CheckAccess())
                    {
                        tabItem.Background = new SolidColorBrush(Color.FromArgb(203, 255, 0, 0));
                    }
                    else
                    {
                        tabItem.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            tabItem.Background = new SolidColorBrush(Color.FromArgb(203, 255, 0, 0));
                        }));
                    }
                };
                instance.Start();
                instance.BeginErrorReadLine();

                _instanceMaps.Add(new InstanceMap
                {
                    ListBoxItem = selectedItem,
                    TabItem = tabItem,
                    Process = instance
                });
            }
        }

        private void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var config = new Config { Name = "未命名" };
            _configs.Add(config);

            ConfigHelper.WriteConfig(_configs.ToList());
        }

        private void ModifyButton_OnClick(object sender, RoutedEventArgs e)
        {
            ConfigPanel.IsEnabled = true;
        }

        private void DuplicateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = ConfigListBox.SelectedItems;

            if (selectedItems.Count < 1)
            {
                MessageBox.Show("请至少选择一个配置后重试", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var config = ConfigListBox.SelectedItem as Config;
            if (config?.Clone() is Config copiedConfig)
            {
                copiedConfig.Name = $"{copiedConfig.Name}-副本";
                _configs.Add(copiedConfig);
                ConfigHelper.WriteConfig(_configs.ToList());
            }
        }

        private void EliminateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = ConfigListBox.GetSelectedItems();

            if (selectedItems.Count < 1)
            {
                MessageBox.Show("请至少选择一个配置后重试", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var configs = selectedItems
                .Select(i => i.DataContext)
                .Cast<Config>()
                .ToList();

            configs.ForEach(i => _configs.Remove(i));

            ConfigHelper.WriteConfig(_configs.ToList());
        }

        private void LockButton_OnClick(object sender, RoutedEventArgs e)
        {
            ConfigPanel.IsEnabled = false;
        }
    }
}
