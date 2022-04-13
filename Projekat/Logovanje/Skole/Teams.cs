using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Logovanje.Skole
{
    public class Teams : INotifyPropertyChanged
        {
            public Teams() { }
            private string naziv;

            public string Naziv
            {
                get { return naziv; }
                set { naziv = value; OnPropertyChanged(); }
            }
            private int razred;

            public event PropertyChangedEventHandler PropertyChanged;

            public int Razred
            {
                get { return razred; }
                set { razred = value; OnPropertyChanged(); }
            }

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }

