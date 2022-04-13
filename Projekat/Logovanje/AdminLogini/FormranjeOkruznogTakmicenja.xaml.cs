using Projekat.Logovanje.Skole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Projekat.Logovanje.AdminLogini
{
    /// <summary>
    /// Interaction logic for FormranjeOkruznogTakmicenja.xaml
    /// </summary>
    public partial class FormranjeOkruznogTakmicenja : Page
    {
        //U prvi listview treba da dodam par timova.        
        //TI timovi moraju biti pobednici Skolskih takmicenja.
        //Trebace mi baza za takmicenje okruzno :}

        public ObservableCollection<Teams> Obs { get; set; } = new ObservableCollection<Teams>();
        public ObservableCollection<Teams> Obs1 { get; set; } = new ObservableCollection<Teams>();
        public FormranjeOkruznogTakmicenja() 
            {
                DataContext = this;
                InitializeComponent();
            }
        public BAZA3 B1 = new BAZA3();
            private void Page_Loaded(object sender, RoutedEventArgs e)
            {
            List<int> idijevi = new List<int>();
                //treba da izdvojimo idijeve
                foreach(var a in B1.PobednikSkolskogTakmcienjas)
                {
                var Idi = from b in B1.Tims where b.Id == a.Id_Tima select b.Razred;
                foreach(var c in Idi ){ idijevi.Add(c.Value); }//Dodamo u listicu
                
                    }
                int i = 0;
                foreach(var a in B1.PobednikSkolskogTakmcienjas){

                Teams T1 = new Teams()
                {
                    Naziv = a.NazivSkole,
                    Razred = idijevi[i],

                };
                 i++;
                Obs.Add(T1);
                }
            }

        private void Dodaj(object sender, RoutedEventArgs e)
        {
            Teams T1 = Gridara1.SelectedItem as Teams;
            if (T1 != null)
            {
                Obs1.Add(T1);
                Obs.Remove(T1);
            }
            else MessageBox.Show("MORATE SELEKTOVATI TAKMICENJE");
        }
        //funkcija za formiranje susreta
        public void Randomizacija(ObservableCollection<Teams> o)
        {
            int i, j;
            OkruzniMec M1;
            for (i = 0; i < o.Count(); i++)
                for (j = i + 1; j < o.Count(); j++)
                {
                    M1 = new OkruzniMec();  //formiramo matchup
                    M1.Razred1 = o[i].Razred;
                    M1.Razred2 = o[j].Razred;
                    M1.NazivSkole1 = o[i].Naziv;
                    M1.NazivSkole2 = o[j].Naziv;
                    M1.Odigrano = "Nije";
                    B1.OkruzniMecs.Add(M1);
                    B1.SaveChanges();

                }

        }
        //Formiranje Takmicenja+susreta
        private void Formiraj(object sender, RoutedEventArgs e)
        {
            var query = (from a in B1.OkruznoTakmicenjes select a);// uzmemo podatke takmicenja/ imamo listu querya
            int brojac = 0; //pomocni brojac
            OkruznoTakmicenje T;
            foreach (var b in query)  //Moguce je imati samo jedno takmicenje aktivno za skole
                if (query.Count()>0)
                        brojac = 1; //stavlja se brojac na 1 ako elemnt postoji u bazi.
            if (brojac != 1)
            {
                foreach (var a in Obs1)
                {
                    T = new OkruznoTakmicenje();
                    T.NazivSkole = a.Naziv;
                    T.Razred = a.Razred;
                    B1.OkruznoTakmicenjes.Add(T);
                    B1.SaveChanges();

                }
                Randomizacija(Obs1);
                //Svakkao se brisu podaci liste i omogucuje se menjanje skole
                Obs1.Clear();
            }
            else
            {
                MessageBox.Show("TIMOVI SU VEC U TAKMICENJU!");
                Obs1.Clear();
            }
        }

        private void Obrisi(object sender, RoutedEventArgs e)
        {
            var A = Gridara2.SelectedItem as Teams;
            Obs1.Remove(A);
            Obs.Add(A);
         
        }
    }
}
