using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MeshEmulator.App.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))            
                return false;
            
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
