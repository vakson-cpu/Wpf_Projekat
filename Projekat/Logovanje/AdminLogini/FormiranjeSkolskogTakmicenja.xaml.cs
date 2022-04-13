using Projekat.Logovanje.Skole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projekat.Logovanje.AdminLogini
{
    /// <summary>
    /// Interaction logic for FormiranjeSkolskogTakmicenja.xaml
    /// </summary>
    public partial class FormiranjeSkolskogTakmicenja : Page
    {

        public FormiranjeSkolskogTakmicenja()
        {
            DataContext = this;
            InitializeComponent();
        }
        public BAZA3 B1 = new BAZA3();
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var Skolice = (from a in B1.Skolas
                           select a.Naziv);
            c1.ItemsSource = Skolice.ToList();
        }
        //Za Selection Changed,treba da updejtuem listu timova koji mogu da se prijave za takmicenje

        public ObservableCollection<Tim> T { get; set; } = new ObservableCollection<Tim>();
        //Ob colekcija za prvi data grid
        public ObservableCollection<Teams> ObT { get; set; } = new ObservableCollection<Teams>();
        // oob za drugi
        public ObservableCollection<Teams> ObT2 { get; set; } = new ObservableCollection<Teams>();



        //Pomocne kolekcije za skladistenje ucenika prve druge trece i cetvrte godine
        public ObservableCollection<Teams> ObPrvi { get; set; } = new ObservableCollection<Teams>();
        public ObservableCollection<Teams> ObDrugi { get; set; } = new ObservableCollection<Teams>();
        public ObservableCollection<Teams> ObTreci { get; set; } = new ObservableCollection<Teams>();
        public ObservableCollection<Teams> ObCetvrti { get; set; } = new ObservableCollection<Teams>();

        //Kopira elemnte iz jedne observabalke u  drugu
        public void Copy(ObservableCollection<Teams> T, ObservableCollection<Teams> B)
        {
            if (B.Count() > 0)
            {
                foreach (var a in B)
                    T.Add(a);
            }
        }
        private void c1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Ocistimo nakon promene selekcije  Datagrid
            ObT.Clear();

            //       ComboBoxItem C = (ComboBoxItem)c1.SelectedItem;
            string pomocni = c1.SelectedItem as string;
            // Kupimo Timove sa Nazivom trenutno selektovane Skole
            //U Timovi se nalaze rezultati querrija za svaki razred
            Teams T1 = new Teams();
        

            var Timovi1 = (from a in B1.Tims
                           where a.Naziv_Skole == pomocni && a.Razred == 1
                           select a);
            var Timovi2 = (from a in B1.Tims
                           where a.Naziv_Skole == pomocni && a.Razred == 2
                           select a);
            var Timovi3 = (from a in B1.Tims
                           where a.Naziv_Skole == pomocni && a.Razred == 3
                           select a);
            var Timovi4 = (from a in B1.Tims
                           where a.Naziv_Skole == pomocni && a.Razred == 4
                           select a);
            if (Timovi1.Count() == 5)
            {
                T1 = new Teams();
                T1.Naziv = pomocni;
                T1.Razred = 1;
                ObPrvi.Clear(); //Ocistimo prvu zato sto se ovo ponavlja na svaku selection changed,pa kao bi vratili imali bi dupliakt
                ObPrvi.Add(T1); //Dodamo u listu validnih timova za prvi razred
                Copy(ObT, ObPrvi);   //Kopiramo u glavnu observable te timove i tako za svaki razred
            }
            if (Timovi2.Count() == 5)
            {
                T1 = new Teams();
                //  T2.Naziv = pomocni;
                // T2.Razred = 2;
                T1.Naziv = pomocni;
                T1.Razred = 2;
                ObDrugi.Clear();
                ObDrugi.Add(T1);
                Copy(ObT, ObDrugi);
            }
            if (Timovi3.Count() == 5)
            {
                T1 = new Teams();
                //  T2.Naziv = pomocni;
                // T2.Razred = 2;
                T1.Naziv = pomocni;
                T1.Razred = 3;
                // T3.Naziv = pomocni;
                // T3.Razred = 3;
                ObTreci.Clear();
                ObTreci.Add(T1);
                Copy(ObT, ObTreci);
            }
            if (Timovi4.Count() == 5)
            {
                T1 = new Teams();
         
                T1.Naziv = pomocni;
                T1.Razred = 4;
                //  T4.Naziv = pomocni;
                // T4.Razred = 4;
                ObCetvrti.Clear();
                ObCetvrti.Add(T1);
                Copy(ObT, ObCetvrti);
            }

        }

        //FUNKCIJA ZA DODAVANJE TIMOVA U TAKMICENJE


        //Trebace mi liste koje ce da cuvaju unete timove kako 
        // kada bi se selekcija menjala nebi se menjao kontent
        private void DODAJ(object sender, RoutedEventArgs e)
        {
            
            //1) Treba da znamo koja je trenutna skola.
            //2) U zavisnosti od toga dodajemo sve timove koji su spremni(5 clanova).
            //U ta gridaru imamo naziv skole tima i njegov razred,sve nam to treba for competition
            Teams T1 = Gridara1.SelectedItem as Teams;
            string pomocni = c1.SelectedItem as string; //Naziv tr skole.
            bool dali=false;
            //Proverimo da li se tim vec nalazi u listi.
            if(T1!=null)
            {
                foreach (var a in ObT2)
                {
                    if (a.Razred == T1.Razred && a.Naziv == T1.Naziv)
                    {
                        MessageBox.Show("TIM JE VEC UNET U TAKMICENJE");
                        dali = true;
                    }
                }
                //Ako se ne nalazi dodamo ga
                if (dali == false && T1.Naziv == pomocni)
                {
                    ObT2.Add(T1);
                    ObT.Remove(T1);
                    c1.IsEnabled = false;

                }


            }
            else{ MessageBox.Show("Morate selektovati neki tim"); }

        }
            //_____-----------------------___________
            //______________--------------------______

        public void Randomizacija(ObservableCollection<Teams> o){
            int i, j;
            string pomocni = c1.SelectedItem as string; //Naziv tr skole.
            Mec M1;
            for (i=0;i<o.Count();i++)
                for(j=i+1;j<o.Count();j++)
                {
                    M1 = new Mec();  //formiramo matchup
                    M1.Razred1 = o[i].Razred;
                    M1.Razred2 = o[j].Razred;
                    M1.NazivSkole = pomocni;
                    M1.Odigrano = "Nije";
                    B1.Mecs.Add(M1);
                    B1.SaveChanges();
                
                }
        
        }
        private void FORMIRAJ(object sender, RoutedEventArgs e)
        {
            //Formiramo i matchUpove
            //Sve elemente iz trenutne liste prebaci u takmicenje
            //Treba da proverimo da li se trenutni tim koji dodajemo u listu nalazi vec u takmicenju.
            var query = (from a in B1.Takmicenjes select a);// uzmemo podatke takmicenja/ imamo listu querya
            int brojac = 0; //pomocni brojac
            Takmicenje Tak;
            foreach (var a in ObT2)
                foreach( var b in query)
                    if ( a.Naziv == b.NazivSkole)
                        brojac = 1; //stavlja se brojac na 1 ako vec postoji skolsko takmicenje za tu skolu
            if (brojac != 1)
            {
                foreach (var a in ObT2)
                {
                    Tak = new Takmicenje();
                    Tak.Razred = a.Razred;
                    Tak.NazivSkole = a.Naziv;
                    B1.Takmicenjes.Add(Tak);
                    B1.SaveChanges();

                }
                Randomizacija(ObT2);
                //Svakkao se brisu podaci liste i omogucuje se menjanje skole
                ObT2.Clear();
                c1.IsEnabled = true;
            }
            else{ MessageBox.Show("TIMOVI SU VEC U TAKMICENJU!");
                ObT2.Clear();
                c1.IsEnabled = true;
            }
        }

        private void DELETE(object sender, RoutedEventArgs e)
        {
            var A = Gridara2.SelectedItem as Teams;
            ObT2.Remove(A);
            ObT.Add(A);
            if (ObT2.Count() == 0)
                c1.IsEnabled = true;

        }
    }
}
