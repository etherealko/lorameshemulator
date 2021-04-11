using MeshEmulator.Logic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeshEmulator.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var parameters = new TransmitterParameters();

            var context = new Context { Nodes = new List<TransmitterNode>() };

            context.Nodes.Add(new TransmitterNode(parameters) { Name = "1", Position = new TransmitterPosition(0, 0) });
            context.Nodes.Add(new TransmitterNode(parameters) { Name = "2", Position = new TransmitterPosition(1, 0) });
            context.Nodes.Add(new TransmitterNode(parameters) { Name = "3", Position = new TransmitterPosition(2, 0) });

            var emulator = new Emulator<Context>(context);

            context.Nodes[0].RequestTransmit(context);

            for (;;)
                emulator.ProcessNextTick();
        }
    }
}
