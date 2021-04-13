using MeshEmulator.App.ViewModels;
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
    /// Interaction logic for NodesControl.xaml
    /// </summary>
    public partial class NodesControl : UserControl
    {
        public static readonly DependencyProperty NodesProperty = DependencyProperty.Register("Nodes", typeof(List<TransmitterNodeViewModel>), typeof(NodesControl));
        public List<TransmitterNodeViewModel> Nodes { get => (List<TransmitterNodeViewModel>)GetValue(NodesProperty); set => SetValue(NodesProperty, value); }

        public NodesControl()
        {
            InitializeComponent();
        }

        private void ItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Nodes == null)
                return;

            var xMax = Nodes.Select(n => n.Position.X).Max();
            var xMin = Nodes.Select(n => n.Position.X).Min();
            var yMax = Nodes.Select(n => n.Position.Y).Max();
            var yMin = Nodes.Select(n => n.Position.Y).Min();
            
            var scale = Math.Min(e.NewSize.Width / (xMax - xMin), e.NewSize.Height / (yMax - yMin));
            scale = Math.Min(scale, 10000);

            var leftOffst = (e.NewSize.Width - (xMax - xMin) * scale) / 2;
            var topOffset = (e.NewSize.Height - (yMax - yMin) * scale) / 2;

            foreach (var node in Nodes)
            {
                node.CanvasLeft = (node.Position.X - xMin) * scale + leftOffst - node.ActualWidth / 2;
                node.CanvasTop = (node.Position.Y - yMin) * scale + topOffset - node.ActualHeight / 2;
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var node = (TransmitterNodeViewModel)((Grid)sender).DataContext;

            node.ActualWidth = e.NewSize.Width;
            node.ActualHeight = e.NewSize.Height;

            var xOffset = e.NewSize.Width - e.PreviousSize.Width;
            var yOffset = e.NewSize.Height - e.PreviousSize.Height;

            node.CanvasLeft -= xOffset / 2;
            node.CanvasTop -= yOffset / 2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var node = (TransmitterNodeViewModel)((FrameworkElement)sender).DataContext;

            node.RequestTransmit();
        }
    }
}
