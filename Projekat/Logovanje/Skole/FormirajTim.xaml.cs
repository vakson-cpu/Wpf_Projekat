using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Projekat.Logovanje
{
    /// <summary>
    /// Interaction logic for FormirajTim.xaml
    /// </summary>
    public partial class FormirajTim : Page
    {
        public FormirajTim()
        {
            DataContext = this;

            InitializeComponent();
        }


        public BAZA3 B1 = new BAZA3();
        public List<Ucenik> Lista = new List<Ucenik>();
        public ObservableCollection<Ucenik> Lista2 { get; set; } = new ObservableCollection<Ucenik>();
        
            //Vraca tr idi 
        public int TrenutniId(){
            int i;
            IEnumerable<int> TrenutniID = (from a in B1.TrenutnoUlogovanis
                                           select a.Id);
            i = TrenutniID.First(); // Id tr ulogovane skole
            return i;
        }
        //Vraca tr naziv skole na osnovu tr idija
        public string TrenutnaSkola(int i)
            {
            string NazivS;
            //Nadjemo ime skole koja se podudara sa idijem
            IEnumerable<String> NazivSkole = (from a in B1.Skolas
                                              where a.Id == i
                                              select a.Naziv);
            NazivS = NazivSkole.First(); //ime tr skole
            return NazivS;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Nadjemo Koja je trenutno ulogovana skola
            int i;
            string NazivS;
            i = TrenutniId();
            NazivS = TrenutnaSkola(i);

           
            var uceniks = (from a in B1.Uceniks
                           where a.Naziv_Skole == NazivS
                           select a);

            Lista = uceniks.ToList();
            foreach (var a in Lista)
            {
                
                Lista2.Add(a);
            }

            //  Gridara.ItemsSource = uceniks.ToList();

        }
        //Proverava da li se ucenik  vec nalazi u datom timu
        public bool Proveri(int raz,int idi){
            var pom = (from a in B1.Tims
                       where a.id_ucenika == idi && a.Razred == raz
                       select "T");
            //Vrati T iz tima kao se u njega nalazi ucenik sa odgovarajucim raredom i idijem
            bool dali = false;
            string tacno = pom.FirstOrDefault();
            if (tacno == "T")
                dali = true;
            else dali = false;

            return dali;        
        
        }

  
        //Dodajemo Clanove u tim
        private void Dodaj(object sender, RoutedEventArgs e)
        {
            Igrac I1 = new Igrac();
            Ucenik U = new Ucenik();
            U = (Gridara.SelectedItem as Ucenik);
            // nije mi htelo cb.selected item pa sam ovako 
            ComboBoxItem c1 = (ComboBoxItem)Cb.SelectedItem;
            if (c1 != null)
            {
                string pomocni = c1.Content.ToString();
                int razred = int.Parse(pomocni);
                //Trazimo Broj Clanova tima da bi znali da li je tim punacak
                var Broj = (from a in B1.Tims where a.Naziv_Skole == U.Naziv_Skole && a.Razred == razred select a);
                int number = Broj.Count();
                if (Proveri(razred, U.Id) == false) // PRoveri da li se ucenik vec nalazi u timu
                {
                    if (U.Razred == razred && U.Visina>170 && U.pol!="Z")
                    {
                        if (number < 5)
                        {
                            Tim T1 = new Tim
                            {
                                id_ucenika = (int)U.Id,
                                Naziv_Skole = (string)U.Naziv_Skole,
                                Razred = razred
                            };

                            Obse.Add(U);
                            Pb.Value = Obse.Count() * 20;
                            B1.Tims.Add(T1);
                            B1.SaveChanges();
                        }
                        else { MessageBox.Show("TIM JE PUN//NIJE MOGUCE DODAVATI VISE UCENJAKA"); }
                    }
                    else
                    { MessageBox.Show("Ucenik mora biti u isti razred kao i tim za koji se formira razred \n i mora biti veci od 170"); }
                }
                else
                {
                    MessageBox.Show("Ucenik je vec deo tima.");
                }

            }
            else MessageBox.Show("MORATE SPECIFIRATI RAZRED ZA KOJI FORMIRATE TIM");
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = true;
        }
 
        public string Radio()
        {
            if (Radio1.IsChecked == true)
                return "M";
            else
                return "Z";
        }
       public bool Check(){
            if (Radio1.IsChecked == true || Radio2.IsChecked==true)
                return true;
            else
                return false;
        }
        
        
        private void Sort(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Ucenik> L2 = new ObservableCollection<Ucenik>();
            IEnumerable<Ucenik> L=Lista2;
            if (!String.IsNullOrWhiteSpace(Ime1.Text))
                L = L.Where(Student => Student.Ime == Ime1.Text);
            if (!String.IsNullOrWhiteSpace(Prez1.Text))
                L = L.Where(Student => Student.Prezime == Prez1.Text);
            if (Check() == true)
            {
                string a = Radio();
                L = L.Where(Student => Student.pol == a);
            }
            if(Cb2.SelectedItem!=null){
                ComboBoxItem c2 = (ComboBoxItem)Cb2.SelectedItem;
                string pomocni = c2.Content.ToString();
                int razred = int.Parse(pomocni);
                L = L.Where(Student => Student.Razred == razred);

            }
            foreach (var a in L.ToList())
                L2.Add(a);
                
             Gridara.ItemsSource = L2.ToList();

            //foreach (var a in Lista2)
            //   if (a.Ime == Ime1.Text)
            //       L.Add(a);

            myPopup.IsOpen = false;
        }




        //Za List view sa leve strane
        public ObservableCollection<Ucenik> Obse { get; set; } = new ObservableCollection<Ucenik>();
        public ObservableCollection<Tim> T { get; set; } = new ObservableCollection<Tim>();
        public List<Tim> Timovi = new List<Tim>();

      


        private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Obse.Clear();
            

            ObservableCollection<Ucenik> Pomocna = new ObservableCollection<Ucenik>();         
                //Lista idija ucenika koji se nalaze u timu
            List<int> idi = new List<int>();
            ComboBoxItem c1 = (ComboBoxItem)Cb.SelectedItem;
            string pomocni = c1.Content.ToString();
            int razred = int.Parse(pomocni);
            int i;
            string NazivS;
            i = TrenutniId();
            NazivS = TrenutnaSkola(i);
            //Kupimo ucenike timova
            var Timovi1 = (from a in B1.Tims
                        where a.Razred == razred && a.Naziv_Skole==NazivS
                        select a);
            Timovi = Timovi1.ToList();
            foreach (var a in Timovi)
            {
                idi.Add(a.id_ucenika);
                T.Add(a);

            }
            //Dodajemo svakog ucenika u pomocnu listu
            foreach(var a in idi){
                var b = B1.Uceniks.Where(Student => Student.Id == a);
                Pomocna.Add(b.Single());
            }
            foreach (var a in Pomocna)
                Obse.Add(a);
            Pb.Value = Obse.Count()*20;
            
        }

    }
}