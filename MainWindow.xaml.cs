using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;

namespace KcptunManager
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Config> _configs;
        private ObservableCollection<InstanceMap> _instanceMaps = new ObservableCollection<InstanceMap>();

        public MainWindow()
        {
            InitializeComponent();

            _configs = new ObservableCollection<Config>(ConfigHelper.ReadConfig());

            NotifyIcon icon = new NotifyIcon();
            icon.Icon = Properties.Resources.Icon;
            icon.Visible = true;
            icon.DoubleClick += (sender, args) =>
            {
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
                Topmost = true;
                Topmost = false;
            };

            ConfigListBox.ItemsSource = _configs;

            ConfigPanel.IsEnabled = false;
            ConfigPanel.AddHandler(TextBox.LostFocusEvent, new RoutedEventHandler((sender, args) =>
            {
                ConfigHelper.WriteConfig(_configs.ToList());
            }));

            _instanceMaps.CollectionChanged += (sender, args) => { icon.Text = $@"{_instanceMaps.Count} instance(s) are running"; };
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            foreach (var instanceMap in _instanceMaps)
            {
                if (!instanceMap.Process.HasExited)
                    instanceMap.Process.Kill();
            }

            ConfigHelper.WriteConfig(_configs.ToList());
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var listBoxItems = ConfigListBox.GetItems();
            var items = listBoxItems
                .Where(i => ((Config)i.DataContext).AutoStart)
                .ToList();

            if (items.Count > 0)
            {
                items.ForEach(i => i.Checked());
                LaunchButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void MainWindow_OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                ShowInTaskbar = false;
        }
    }
}
