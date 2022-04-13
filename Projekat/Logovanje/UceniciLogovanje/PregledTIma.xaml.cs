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

namespace Projekat.Logovanje.UceniciLogovanje
{
    /// <summary>
    /// Interaction logic for PregledTIma.xaml
    /// </summary>
    public partial class PregledTIma : Page
    {
        public BAZA3 B1 = new BAZA3();
        public ObservableCollection<Ucenik> Obs { get; set; } = new ObservableCollection<Ucenik>();
        public PregledTIma()
        {
            DataContext = this;
            InitializeComponent();
        }

        public int TrenutniId()
        {
            int i;
            IEnumerable<int> TrenutniID = (from a in B1.TrenutnoUlogovanis
                                           select a.Id);
            i = TrenutniID.First(); // Id tr ulogovane skole
            return i;
        }
        public void ucitaj()
        {
            Obs.Clear();
            int idi = TrenutniId();
            var uc = from a in B1.Uceniks where a.Id == idi select a.Naziv_Skole;
            var uc1 = from a in B1.Uceniks where a.Id == idi select a.Razred;
            string NazivSkole = uc.FirstOrDefault();

            //Imamo naziv skole i razred tog ucenika.
            //sad treba da potrzimo nejgove suigrace
            var suigrac = from a in B1.Tims
                          where a.Naziv_Skole == NazivSkole && a.Razred == uc1.FirstOrDefault()
                          select a.id_ucenika;
            foreach (var b in suigrac)
            {
                var lista = from a in B1.Uceniks
                            where a.Id == b
                            select a;
                Obs.Add(lista.FirstOrDefault());
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Obs.Clear();

            //  var Timov= from a in B1.Tims where  
            //Treba da pookupimo idijeve ucenika
            int idi = TrenutniId();
            bool dali = false;
            foreach (var a in B1.Tims)
            {
                if (idi == a.id_ucenika)
                    dali = true;
            }


            if (dali == true)
                ucitaj();
                
            
        }

        private void Odjavi(object sender, RoutedEventArgs e)
        {
            bool dali = false;
            int idi = TrenutniId();
            var od = from a in B1.Tims where a.id_ucenika == idi select a;
            if (od != null)
            {
                foreach (var a in od)
                    foreach (var b in B1.Takmicenjes) //Ako ima razred i skola tu
                        if (a.Naziv_Skole == b.NazivSkole && a.Razred == b.Razred)
                            dali = true;
                //Ako se tim ne nalazi u takmicenju mozemo ucenika iskljuciti iz tima
                if (dali != true)
                {
                    B1.Tims.Remove(od.FirstOrDefault());
                    B1.SaveChanges();
                    MessageBox.Show("Napustili ste tim.");
                    Obs.Clear();        
                }
                //ako je tim u takmicenju ne moze;
                else
                {
                    MessageBox.Show("Nije uredu napustati tim u sred takmicenja!");
                }
            }



        }
    }
}