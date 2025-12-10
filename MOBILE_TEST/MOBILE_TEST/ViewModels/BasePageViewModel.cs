using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MOBILE_TEST.ViewModels
{

        public class BasePageViewModel : INotifyPropertyChanged
        {
            public BasePageViewModel()
            { }

            #region INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            protected void SetProperty<TData>(ref TData storage, TData value, [CallerMemberName] string propertyName = "")
                {
                    if (storage?.Equals(value) == true)
                        return;

                    storage = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }

                #endregion
        }
    }

