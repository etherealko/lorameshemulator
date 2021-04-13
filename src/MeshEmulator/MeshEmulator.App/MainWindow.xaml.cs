using MeshEmulator.App.ViewModels;
using MeshEmulator.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace MeshEmulator.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Context _context;
        private Emulator<Context> _emulator;

        private DispatcherTimer _timer = new DispatcherTimer();

        public List<TransmitterNodeViewModel> Nodes { get; private set; }

        public MainWindow()
        {
            var config = JsonConvert.DeserializeObject<ConfigJson>(File.ReadAllText("config.json"));

            var parameters = new TransmitterParameters { SnrCutoff = config.SnrCutoff, NoiseCutoff = config.NoiseCutoff };

            _context = new Context { Nodes = new List<TransmitterNode>() };

            foreach (var node in config.Nodes)
                _context.Nodes.Add(new TransmitterNode(parameters) { Position = new TransmitterPosition(node.X, node.Y) });

            _emulator = new Emulator<Context>(_context);

            _timer.Interval = TimeSpan.FromMilliseconds(config.TimerIntervalMs);
            _timer.Tick += (o, e) =>
            {
                if (_context.Nodes.All(n => n.TransmitQueueLength == 0))
                    _timer.Stop();
                else
                    Button_Click(null, null);
            };

            Nodes = _context.Nodes.Select(n => new TransmitterNodeViewModel(n, _context)).ToList();

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _emulator.ProcessNextTick();
            foreach (var node in Nodes)
                node.UpdateProperties();

            CurrentTickTextBlock.Text = _context.CurrentTick.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }
    }

    public class ConfigJson
    {
        public List<NodeJson> Nodes { get; set; }
        public double SnrCutoff { get; set; }
        public double NoiseCutoff { get; set; }
        public double TimerIntervalMs { get; set; }
    }

    public class NodeJson
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
