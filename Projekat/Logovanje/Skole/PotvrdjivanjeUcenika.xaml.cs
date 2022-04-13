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

namespace Projekat.Logovanje.Skole
{
    /// <summary>
    /// Interaction logic for PotvrdjivanjeUcenika.xaml
    /// </summary>
    public partial class PotvrdjivanjeUcenika : Page
    {

        public BAZA3 B1 = new BAZA3();
        public List<NepotvrdjeniUcenici> Lista = new List<NepotvrdjeniUcenici>();
        public ObservableCollection<NepotvrdjeniUcenici> Lista2 { get; set; } = new ObservableCollection<NepotvrdjeniUcenici>();  

        public PotvrdjivanjeUcenika()
        {            
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {   //Nadjemo Koja je trenutno ulogovana skola   
            int i;
            string NazivS;
            IEnumerable<int> TrenutniID = (from a in B1.TrenutnoUlogovanis
                                           select a.Id);
            i = TrenutniID.First(); // Id tr ulogovane skole

            //Nadjemo ime skole koja se podudara sa idijem
            IEnumerable<String> NazivSkole = (from a in B1.Skolas
                                              where a.Id == i
                                              select a.Naziv);
            NazivS = NazivSkole.First(); //ime tr skole

            var Nepotrvrdjeni = (from a in B1.NepotvrdjeniUcenicis
                           where a.Naziv_Skole == NazivS
                           select a);

            Lista = Nepotrvrdjeni.ToList();
            foreach (var a in Lista)
                Lista2.Add(a);

        }

        private void Potvrdi(object sender, RoutedEventArgs e)
        {
            NepotvrdjeniUcenici b = new NepotvrdjeniUcenici();
            b = (NepotvrdjeniUcenici)Gridara.SelectedItem;

            Ucenik a = new Ucenik
            {
                Ime = b.Ime,
                Prezime = b.Prezime,
                Sifra = b.Sifra,
                Email = b.Email,
                Godine = b.Godine,
                Mobile = b.Mobile,
                Naziv_Skole = b.Naziv_Skole,
                Visina = b.Visina,
                pol = b.pol,
                JMBG = b.JMBG,
                Razred = b.Razred


            };
            int idi = (Gridara.SelectedItem as NepotvrdjeniUcenici).Id;
            var ZaBrisanje = B1.NepotvrdjeniUcenicis.Where(m => m.Id == idi).Single();
            Lista2.Remove((Gridara.SelectedItem as NepotvrdjeniUcenici));

            B1.Uceniks.Add(a);
            B1.NepotvrdjeniUcenicis.Remove(ZaBrisanje);

            B1.SaveChanges();

        }

        private void Odbi(object sender, RoutedEventArgs e)
        {
            int idi = (Gridara.SelectedItem as NepotvrdjeniUcenici).Id;
            var ZaBrisanje = B1.NepotvrdjeniUcenicis.Where(m => m.Id == idi).Single();
            Lista2.Remove((Gridara.SelectedItem as NepotvrdjeniUcenici));
            B1.NepotvrdjeniUcenicis.Remove(ZaBrisanje);
            B1.SaveChanges();

        }
    }
}
