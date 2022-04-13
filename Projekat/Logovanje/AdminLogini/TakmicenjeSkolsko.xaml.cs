using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Projekat.Logovanje.AdminLogini
{
    /// <summary>
    /// Interaction logic for TakmicenjeSkolsko.xaml
    /// </summary>
    public partial class TakmicenjeSkolsko : Page
    {
        public TakmicenjeSkolsko()
        {
            DataContext = this;
            InitializeComponent();
        }
            
        public BAZA3 B1 = new BAZA3();

        public ObservableCollection<Mec> Obs { get; set; } = new ObservableCollection<Mec>();
        public void Refresh()
        {
            Obs.Clear();
            string pomocna = Cb1.SelectedItem as string;
            foreach (var a in B1.Mecs.Where(a=>a.NazivSkole==pomocna))
                Obs.Add(a as Mec);
  
       

        }
        public ObservableCollection<Statistika> Statistike { get; set; } = new ObservableCollection<Statistika>();

        public ObservableCollection<string> NaziviSkole { get; set; } = new ObservableCollection<string>();
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {           //Zasad dok nema skoli
            string pomocni = Cb1.SelectedItem as string;
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += Dt_Tick;
            dt1.Interval = TimeSpan.FromSeconds(1);
            dt1.Tick += Odmor;
  
            var Mecevi = (from a in B1.Mecs where  a.NazivSkole==pomocni select a);
            foreach (var a in Mecevi)
                Obs.Add(a as Mec);
            var skolice = from a in B1.Skolas select a.Naziv;
            foreach (var a in skolice)
                NaziviSkole.Add(a as string);
        }

        //Observable kolekcije koje ce cuvati podatke timova za listview popupova
        public ObservableCollection<Ucenik> Tim1 { get; set; } = new ObservableCollection<Ucenik>();
        public ObservableCollection<Ucenik> Tim2 { get; set; } = new ObservableCollection<Ucenik>();
        public DispatcherTimer dt = new DispatcherTimer();
        public DispatcherTimer dt1 = new DispatcherTimer();


        public List<int> PomocnaLista = new List<int>();
        private void Play(object sender, RoutedEventArgs e)
        {

            Statistike.Clear();
            Tim1.Clear(); //Ocistimo svaki tim pre igre
            Tim2.Clear();
            var  Odabrani = Lw.SelectedItem as Mec;
            if (Odabrani != null)
            {
                if (Odabrani.Odigrano == "Nije") //AKO NIJE MEC ODIGRAN MOZE DA SE IGRA
                {
                    Statistike.Clear();
                    dt.Start();
                    Skor.Text = $"{Score1}:{Score2}";
                    myPopup.IsOpen = true;
                    //Za tim 1 izdvoijimo listu idija ucenika
                    var PrviTim = (from a in B1.Tims
                                   where Odabrani.Razred1 == a.Razred &&
                    Odabrani.NazivSkole == a.Naziv_Skole
                                   select a.id_ucenika);
                    var DrugiTim = (from a in B1.Tims
                                    where Odabrani.Razred2 == a.Razred &&
                                    Odabrani.NazivSkole == a.Naziv_Skole
                                    select a.id_ucenika);
                    foreach(var a in PrviTim)  //Dodamo u idijeve za statistiku.
                        {
                        Statistika S = new Statistika();
                        S.ID_UCENIKA = a;
                        S.BrPoena = 0;
                       
                        Statistike.Add(S);
                            }
                    foreach (var a in DrugiTim)
                    {
                        Statistika S = new Statistika();
                        S.ID_UCENIKA = a;
                        S.BrPoena = 0;
                        Statistike.Add(S);

                    }
                    foreach (var a in PrviTim)
                    {
                        var b = B1.Uceniks.Where(Student => Student.Id == a);
                        Tim1.Add(b.Single());
                    }
                    foreach (var a in DrugiTim)
                    {
                        var b = B1.Uceniks.Where(Student => Student.Id == a);
                        Tim2.Add(b.Single());
                    }
                }
                else { MessageBox.Show("MEC JE ODIGRAN"); }
            }
            else MessageBox.Show("MORATE SELEKTOVATI MEC ZA IGRANJE");
        }
        
        //Za tajmer inkrementeri i brojac cetvrtina
        private int increment=0;
        private int pauza = 0;
        private int cetvrtina=1;
        //Tajmer cetvrtina
        private void Dt_Tick(object sender, EventArgs e)
        {
            Dugme.IsEnabled = true;
            increment++; //svake sekunde uveca se
            Vreme.Text = increment.ToString();
            if (cetvrtina < 5) //ako nije kraj
            {
                if (increment == 8)
                {
                    cetvrtina++;
                    dt.Stop();
                    dt1.Start();
                    Dugme.IsEnabled = false;
                }
            }
            else{  //ako je zadnja cetvrtina
                cetvrtina = 1;
                var Odabrani = Lw.SelectedItem as Mec;
                Vreme.Text = "KRAJ UTAKMICE";
             
                dt.Stop(); //Pogasimo Tajmere
                dt1.Stop();
                pauza = 0;
                increment = 0; if (Score1 == Score2)
                {
                    Random ra = new Random();
                    int broj = ra.Next(0, 2);
                    if (broj == 1) Score1++;
                    else Score2++;
                }
                //Proverimo ko je pobedio
                if (Score1>Score2)
                    Pobeda1(Odabrani as Mec);
                if (Score1 < Score2)
                    Pobeda2(Odabrani as Mec);
               
                //Ubacimo  rezultate u bazi za odigrane meceve
                var b = B1.Mecs.Where(Tim => Tim.Id == Odabrani.Id).FirstOrDefault();
                b.Poen1 = Score1;
                b.Poen2 = Score2;
                b.Odigrano = "DA";
                B1.SaveChanges();

                //Resetujemo skorove tako da bude 0-0 kada drugi timovi dodju
                Score1 = 0;

                Score2 = 0;
                L.Clear(); //Mora i tabela da se doradi.
                tabela();
                myPopup.IsOpen = false;
                MVP_DODELA();
                PobednikTakmicenja();
                Refresh();
                Dugme.IsEnabled = false;


            }

        }


        //Funkcija za pauzu.
        private void Odmor(object sender, EventArgs e)
        {
            pauza++;
            Vreme.Text = $"KRAJ {cetvrtina-1} CETVRTINE: " + pauza.ToString();
            if(pauza==5)
            {
                pauza = 0;
                dt1.Stop();
                increment = 0;
                dt.Start(); 
            }
            
        }
        //funkcija za Pobede
        #region Pobede
        public void Pobeda1(Mec M)
        {
            
            var Tim = from a in B1.Tims
                      where 
            M.Razred1 == a.Razred && a.Naziv_Skole == M.NazivSkole
                      select a.Id;
            int id = Tim.First();
            Pobede b = new Pobede()
            {
                Id_tima = id,
                NazivSkole = M.NazivSkole,
                };
            B1.Pobedes.Add(b);
            B1.SaveChanges();
        }
        public void Pobeda2(Mec M)
        {
            var Tim = from a in B1.Tims
                      where
            M.Razred2 == a.Razred && a.Naziv_Skole == M.NazivSkole
                      select a.Id;
            int id = Tim.First();
            Pobede b = new Pobede()
            {
                Id_tima = id,
                NazivSkole = M.NazivSkole
            };
            B1.Pobedes.Add(b);
            B1.SaveChanges();
        }
        #endregion

        //Fuja za Radio Buttone
        public int  Radio(){
            if (R1.IsChecked == true)
                return 1;
            else if (R2.IsChecked == true)
                return 2;
            else if (R3.IsChecked == true)
                return 3;
            else
                return 0;
        }


        private int Score1=0;//Skor za prvi tim
        private int Score2=0;//Skor za drugi tim
        //Akp su oba igraca selektovana gasi ih
        public void Selektovane(){
            if (LTima.SelectedItem != null && LT2.SelectedItem != null)
            {
                LT2.UnselectAll();
                LTima.UnselectAll();
            }
        }
        private void Poen(object sender, RoutedEventArgs e)
        {
            //Imamo dva tima Ltima i LT2
            // U zavisnosti koji igrac je selektovan treba da se azurira statistika
            // i broj poena koji je postigao tim1/tim2
            //Trebao bih u bazu podataka da dodam i brojac pobeda.

            Selektovane(); //AKO SE OBE SELEKTUJU OBRISI LIJEPO :)
            var Selektovani = LTima.SelectedItem as Ucenik;
            var Selektovani2 = LT2.SelectedItem as Ucenik;
            if (Selektovani != null)
            {  //Dodamo poene lepo uceniku
                foreach (var a in Statistike)
                    if (a.ID_UCENIKA == Selektovani.Id)
                        a.BrPoena += Radio();
                
      
                Score1 += Radio();
                Skor.Text = $"{Score1}:{Score2}";
                LTima.UnselectAll();
            }
            if(Selektovani2!=null)
            {
                foreach (var a in Statistike)
                    if (a.ID_UCENIKA == Selektovani2.Id)
                        a.BrPoena += Radio();
                Score2 += Radio();
                Skor.Text = $"{Score1}:{Score2}";
                LT2.UnselectAll();
            }
        }
        //Fuja za dodelu MVpija
        public void MVP_DODELA(){
            MVP Mv; 
            
            int i, j;
            Statistika Smax = new Statistika();//Nadjemo max statisitku sa max poena.
            Smax = Statistike[0];// MORA
            for (i = 0; i < Statistike.Count() - 1; i++)
                for (j = 0; j < Statistike.Count(); j++)
                    if (Smax.BrPoena.Value < Statistike[j].BrPoena.Value)
                        Smax = Statistike[j];
            Mv = new MVP()
            {
                Id_uceniak = Smax.ID_UCENIKA,
                NazivSkole=Cb1.SelectedItem as string
            };
            B1.MVPs.Add(Mv);
            B1.SaveChanges();

            var ime = from a in B1.Uceniks
                      where a.Id == Smax.ID_UCENIKA select a.Ime;
            MessageBox.Show($"MVP JE: {ime.FirstOrDefault()} ");
            //Nakon zavrsetka meca sve poene resetujemo na 0;
            foreach (var a in Statistike)
                a.BrPoena = 0;
                
        }
        //Funkcija koja proverava da li se dati idi vec nalazi u takmicenju.
        public bool Proveri(int most){
                //Proverimo da li ima neko u takmicenju sa argumentom.
            var Pronadji = from a in B1.PobednikSkolskogTakmcienjas
                           where a.Id_Tima == most 
                           select a;
            bool dali = false;
            if (Pronadji.Count()>0)  //Ako rezultat kverija nije nula
                dali = true;

            return dali;
        } 
        public void PobednikTakmicenja(){
            //Proverimo da li je takmicenje gotovo.
            string pomocna = Cb1.SelectedItem as string;
            int brojacMeceva = 1;
            PomocnaLista.Clear();
            foreach (var a in Obs)
            {
                if (a.Odigrano == "Nije")
                    brojacMeceva = 0;

            }
            if (brojacMeceva == 1)
            {
                //Takmicenje je gotovo
                List<MVP> mVPs = new List<MVP>();
                var winner = (from a in B1.Pobedes where a.NazivSkole== pomocna select a.Id_tima);
                foreach (var a in winner)
                    PomocnaLista.Add(a.Value);

                var Pom = from a in B1.MVPs where a.NazivSkole == pomocna select a;
                foreach(var a in Pom) //Smestimo u listu meceva
                    mVPs.Add(a);
                    
                
                var most = (from i in PomocnaLista
                            group i by i into grp
                            orderby grp.Count() descending
                            select grp.Key).First();
                //Sortiramo i uzmemo najveci count idija  ucenika
                var mvp = (from a in  mVPs 
                           group a by a.Id_uceniak into grp
                           orderby grp.Count()  descending
                           select grp.Key).First();

                var uc = from a in B1.Uceniks where a.Id == mvp  && a.Naziv_Skole==pomocna  select a.Ime;
                MessageBox.Show($"TAKMICENJE JE zavrseno POBEDNIK JE SA IDIJEM {most}");
                MessageBox.Show($"MVP TAKMICENJA JE: {uc.FirstOrDefault()}");
             
                if (Proveri(most) == false)
                {
                    PobednikSkolskogTakmcienja P1 = new PobednikSkolskogTakmcienja()
                    {
                        NazivSkole = pomocna,
                        Id_Tima = most
                    };
                    MVPSkolskogTakmicenja M = new MVPSkolskogTakmicenja()
                    {
                        Id_ucenika = mvp,
                        NazivSkole = pomocna
                    };
                    B1.PobednikSkolskogTakmcienjas.Add(P1);
                    B1.MVPSkolskogTakmicenjas.Add(M);
                    B1.SaveChanges();
                    TbPobednik();

                }

            }
        }
        private void myPopup_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = e.GetPosition(this.myPopup);
                this.myPopup.HorizontalOffset = mousePosition.X;
                this.myPopup.VerticalOffset = mousePosition.Y;
            }
        }
        public ObservableCollection<LeaderBoard> L { get; set; } = new ObservableCollection<LeaderBoard>();
        public void tabela(){
            string pomocna = Cb1.SelectedItem as string;
            var Takmicenje = from a in B1.Takmicenjes where a.NazivSkole == pomocna select a;
            foreach(var a in Takmicenje){
                var idijevi = from b in B1.Tims where b.Naziv_Skole == pomocna && b.Razred == a.Razred select b.Id;
                var idi = idijevi.FirstOrDefault();
                var pob = from c in B1.Pobedes where c.Id_tima == idi select c.Id_tima;
                LeaderBoard L1 = new LeaderBoard()
                {
                    Naziv = pomocna,
                    Razred = a.Razred.Value,
                    Pobede = pob.Count(),
                    Id = idijevi.First(),
                    
                };
                L.Add(L1);
                
            }                   
        }

        public DispatcherTimer d2 = new DispatcherTimer();

            
        

    public void TbPobednik(){
            string pomocni = Cb1.SelectedItem as string;
            var mvp = from a in B1.MVPSkolskogTakmicenjas where a.NazivSkole == pomocni select a.Id_ucenika;
            var imeucenika = from a in B1.Uceniks where a.Id == mvp.FirstOrDefault() select a.Ime;
            var takmicenje = from a in B1.PobednikSkolskogTakmcienjas where a.NazivSkole == pomocni select a;
            if (takmicenje.Count() > 0)
            {
                var a = takmicenje.FirstOrDefault();
                var be = from b in B1.Tims where b.Id == a.Id_Tima select b.Razred;
                Tblock.Visibility = Visibility.Visible;
                Tblock.Text = $"Takmicenje je vec odigrano a pobednik je tim skole: {pomocni} razred: {be.FirstOrDefault()}  \n" +
$"              Dok je MVP OVOG TAKMICENJA {imeucenika.FirstOrDefault()}";
            }
        }      
        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tblock.Visibility = Visibility.Hidden;
            string pomocni = Cb1.SelectedItem as string;
            L.Clear();
            tabela();
            Obs.Clear();
            var Mecevi = (from a in B1.Mecs where a.NazivSkole == pomocni select a);
            foreach (var a in Mecevi)
                Obs.Add(a as Mec);
            TbPobednik();
      
        }










    }
}


//Treba da napravim resenje za slucajeve kada je broj pobeda izmedju vise itmova jednak dosta nereseivo
// Problem sa statistikom. Tr mozda resivo
//Ako se mec zavrsi nereseno sta onda?







