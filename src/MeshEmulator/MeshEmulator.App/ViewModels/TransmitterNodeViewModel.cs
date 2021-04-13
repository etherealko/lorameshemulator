using MeshEmulator.Logic;
using System.ComponentModel;

namespace MeshEmulator.App.ViewModels
{
    public class TransmitterNodeViewModel : ViewModel
    {
        private readonly TransmitterNode _node;
        private readonly Context _context;

        private double _canvasLeft;
        private double _canvasTop;
        private bool _isTransmitting;
        private bool _isReceiving;
        private int _transmitQueueLength;
        private int _seenMessagesCount;

        public string Name => _node.Name;
        public TransmitterPosition Position => _node.Position;
        
        public double CanvasLeft { get => _canvasLeft; set => SetProperty(ref _canvasLeft, value); }
        public double CanvasTop { get => _canvasTop; set => SetProperty(ref _canvasTop, value); }
        public double ActualWidth { get; set; }
        public double ActualHeight { get; set; }

        public bool IsTransmitting { get => _isTransmitting; set => SetProperty(ref _isTransmitting, value); }
        public bool IsReceiving { get => _isReceiving; set => SetProperty(ref _isReceiving, value); }

        public int TransmitQueueLength { get => _transmitQueueLength; set => SetProperty(ref _transmitQueueLength, value); }
        public int SeenMessagesCount { get => _seenMessagesCount; set => SetProperty(ref _seenMessagesCount, value); }

        public TransmitterNodeViewModel(TransmitterNode node, Context context)
        {
            _node = node;
            _context = context;
            UpdateProperties();
         }

        public void UpdateProperties()
        {
            IsTransmitting = _node.Status == TransmitterStatus.Transmitting;
            IsReceiving = _node.Status == TransmitterStatus.Receiving;
            TransmitQueueLength = _node.TransmitQueueLength;
            SeenMessagesCount = _node.SeenMessagesCount;
        }

        public void RequestTransmit()
        {
            _node.RequestTransmit(_context);
            UpdateProperties();
        }
    }
}
