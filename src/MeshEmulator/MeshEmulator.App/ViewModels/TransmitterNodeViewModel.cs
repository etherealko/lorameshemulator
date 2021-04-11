using MeshEmulator.Logic;
using System.ComponentModel;

namespace MeshEmulator.App.ViewModels
{
    public class TransmitterNodeViewModel : ViewModel
    {
        public string Name { get; set; }
        public TransmitterPosition Position { get; set; }

        public bool IsTransmitting { get; set; }
        public bool IsReceiving { get; set; }
    }
}
