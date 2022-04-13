using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projekat.Logovanje
{
    /// <summary>
    /// Interaction logic for PotvrdaSkoli.xaml
    /// </summary>
    public partial class PotvrdaSkoli : Page
    {
        public PotvrdaSkoli()
        {
            InitializeComponent();
        }

        private void Potvrda(object sender, RoutedEventArgs e)
        {
            BAZA3 B1 = new BAZA3();
            PomocnaSkola b = new PomocnaSkola();
            b = (PomocnaSkola)Gridara.SelectedItem;
            //  int maxAge = B1.Uceniks.Max(p => p.Id);

            Skola a = new Skola
            {
                Naziv = b.Naziv,
                Sifra = b.Sifra,
                Grad=b.Grad

            };
            string idi = (Gridara.SelectedItem as PomocnaSkola).Naziv;
            var ZaBrisanje = B1.PomocnaSkolas.Where(m => m.Naziv == idi).Single();
            B1.Skolas.Add(a);
            B1.PomocnaSkolas.Remove(ZaBrisanje);

            B1.SaveChanges();
            Gridara.ItemsSource = B1.PomocnaSkolas.ToList();

        }

        private void Odbi(object sender, RoutedEventArgs e)
        {
            BAZA3 B1 = new BAZA3();
            string idi = (Gridara.SelectedItem as PomocnaSkola).Naziv;
            var ZaBrisanje = B1.PomocnaSkolas.Where(m => m.Naziv == idi).Single();
            B1.PomocnaSkolas.Remove(ZaBrisanje);

            B1.SaveChanges();
            Gridara.ItemsSource = B1.PomocnaSkolas.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BAZA3 B1 = new BAZA3();

            Gridara.ItemsSource = B1.PomocnaSkolas.ToList();
        }
    }
}
