using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Logovanje.AdminLogini
{
public class LeaderBoard:INotifyPropertyChanged
    {
        public LeaderBoard(){

        }

    
        private int pobede;

        public int Pobede
        {
            get { return pobede; }
            set { pobede = value; OnPropertyChanged(); }
        }
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
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }


        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
