using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Projekat.Logovanje
{
    public partial class FormirajTim
    {
        public class Igrac:INotifyPropertyChanged{
         public   Igrac(){ }
            private string ime;

            public string Ime
            {
                get { return ime; }
                set { ime = value;OnPropertyChanged(); }
            }
            private string prezime;

            public event PropertyChangedEventHandler PropertyChanged;

            public string Prezime
            {
                get { return prezime; }
                set { prezime = value; OnPropertyChanged(); }
            }

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}