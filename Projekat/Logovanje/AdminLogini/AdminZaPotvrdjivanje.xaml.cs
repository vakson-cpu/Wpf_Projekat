using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projekat.Logovanje
{
    /// <summary>
    /// Interaction logic for AdminZaPotvrdjivanje.xaml
    /// </summary>
    public partial class AdminZaPotvrdjivanje : Page
    {
       

        public AdminZaPotvrdjivanje()
        {

            DataContext = this;
            InitializeComponent();
        }
        public BAZA3 B1 = new BAZA3();


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Gridara.ItemsSource = B1.NepotvrdjeniUcenicis.ToList();
            }
                catch(Exception ex)
                    {
                MessageBox.Show("Greska" + ex);
            }
        }
      
        private void Potvrdi(object sender, RoutedEventArgs e)
        {
            BAZA3 B1 = new BAZA3();
            NepotvrdjeniUcenici b = new NepotvrdjeniUcenici();
            b = (NepotvrdjeniUcenici)Gridara.SelectedItem;
            //  int maxAge = B1.Uceniks.Max(p => p.Id);

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
                Razred=b.Razred


            };
            int idi = (Gridara.SelectedItem as NepotvrdjeniUcenici).Id;
            var ZaBrisanje = B1.NepotvrdjeniUcenicis.Where(m => m.Id == idi).Single();
            B1.Uceniks.Add(a);
            B1.NepotvrdjeniUcenicis.Remove(ZaBrisanje);

            B1.SaveChanges();
            Gridara.ItemsSource = B1.NepotvrdjeniUcenicis.ToList();

            MessageBox.Show("Ucenik Je uspesno Potvrdjen");


        }



        private void Odbi(object sender, RoutedEventArgs e)
        {
            int idi = (Gridara.SelectedItem as NepotvrdjeniUcenici).Id;
            var ZaBrisanje = B1.NepotvrdjeniUcenicis.Where(m => m.Id == idi).Single();
            B1.NepotvrdjeniUcenicis.Remove(ZaBrisanje);
            B1.SaveChanges();
            Gridara.ItemsSource = B1.NepotvrdjeniUcenicis.ToList();
        }
    }
}
