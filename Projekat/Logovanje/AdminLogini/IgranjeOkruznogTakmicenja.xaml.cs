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
    /// Interaction logic for IgranjeOkruznogTakmicenja.xaml
    /// </summary>
    public partial class IgranjeOkruznogTakmicenja : Page
    {
        public IgranjeOkruznogTakmicenja()
        {
            DataContext = this;
            InitializeComponent();
        }
        public BAZA3 B1 = new BAZA3();
        public ObservableCollection<OkruzniMec> Mecevi { get; set; } = new ObservableCollection<OkruzniMec>();



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tabela();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += Dt_Tick;
            dt1.Interval = TimeSpan.FromSeconds(1);
            dt1.Tick += Odmor;
            Mecevi.Clear();
            foreach (var a in B1.OkruzniMecs)
                Mecevi.Add(a);
        }
        //Observable kolekcije koje ce cuvati podatke timova za listview popupova
        public ObservableCollection<Ucenik> Tim1 { get; set; } = new ObservableCollection<Ucenik>();
        public ObservableCollection<Ucenik> Tim2 { get; set; } = new ObservableCollection<Ucenik>();
        public DispatcherTimer dt = new DispatcherTimer();
        public DispatcherTimer dt1 = new DispatcherTimer();

        //Skorovi
        public int Score1=0;
        public int Score2 = 0;

        //Statistika za ucenika
        public ObservableCollection<Statistika> Statistike { get; set; } = new ObservableCollection<Statistika>();
        private void Play(object sender, RoutedEventArgs e)
        {
            Tim1.Clear(); //Ocistimo svaki tim pre igre
            Tim2.Clear();
            var Odabrani = Lw.SelectedItem as OkruzniMec;
            if (Odabrani != null)
            {
                if (Odabrani.Odigrano == "Nije") //AKO NIJE MEC ODIGRAN MOZE DA SE IGRA
                {
                    dt.Start();
                    Skor.Text = $"{Score1}:{Score2}";
                    myPopup.IsOpen = true;
                    //Za tim 1 izdvoijimo listu idija ucenika
                    var PrviTim = (from a in B1.Tims
                                   where Odabrani.Razred1 == a.Razred &&
                    Odabrani.NazivSkole1 == a.Naziv_Skole
                                   select a.id_ucenika);
                    var DrugiTim = (from a in B1.Tims
                                    where Odabrani.Razred2 == a.Razred &&
                                    Odabrani.NazivSkole2 == a.Naziv_Skole
                                    select a.id_ucenika);
                    foreach (var a in PrviTim)  //Dodamo u idijeve za statistiku.
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

        public void Refresh()
        {
            Mecevi.Clear();
            foreach (var a in B1.OkruzniMecs)
                Mecevi.Add(a as OkruzniMec);



        }


        //Prvi TIk
        //Za tajmer inkrementeri i brojac cetvrtina
        private int increment = 0;
        private int pauza = 0;
        private int cetvrtina = 1;

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
            else
            {  //ako je zadnja cetvrtina
                cetvrtina = 1;
                var Odabrani = Lw.SelectedItem as OkruzniMec;
                Vreme.Text = "KRAJ UTAKMICE";

                dt.Stop(); //Pogasimo Tajmere
                dt1.Stop();
                pauza = 0;
                increment = 0;
                //Proverimo ko je pobedio
                if(Score1==Score2)
                {
                    Random ra = new Random();
                    int broj = ra.Next(0, 1);
                    if (broj == 1) Score1++;
                    else Score2++;
                }
                if (Score1 > Score2)
                    Pobeda1(Odabrani as OkruzniMec);
                if (Score1 < Score2)
                    Pobeda2(Odabrani as OkruzniMec);

                //Ubacimo  rezultate u bazi za odigrane meceve
                var b = B1.OkruzniMecs.Where(Tim => Tim.Id == Odabrani.Id).FirstOrDefault();
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
                TbPobednik();
                Refresh();
                Dugme.IsEnabled = false;
                KRAJ(); //Obrisemo sve podatke vezane za takmicenje


            }

        }

        //Na zavrsetku ocistimo sve lijepo...
        public void KRAJ(){
            var Tak = from a in B1.OkruznoTakmicenjes select a;
            var Matchup = from a in B1.OkruzniMecs select a;
            var pob = from a in B1.Pobede2 select a;
            var okrug = from a in B1.PobednikOkruznogTakmicenjas select a;
            var mvp = from a in B1.MVP1 select a;
            var mvp2 = from a in B1.MVPOkruznogTakmicenjas select a;
            var skolsko = from a in B1.PobednikSkolskogTakmcienjas select a;
            var pobskolskog = from a in B1.Pobedes select a;
            var mecevskol = from a in B1.Mecs select a;
            var tak1 = from a in B1.Takmicenjes select a;
            var mvp3 = from a in B1.MVPs select a;
            var mvp4 = from a in B1.MVPSkolskogTakmicenjas select a;
            var rs = from a in B1.RsSkolskogs select a;
            B1.OkruznoTakmicenjes.RemoveRange(Tak);
            B1.OkruzniMecs.RemoveRange(Matchup);
            B1.Pobede2.RemoveRange(pob);
            B1.PobednikOkruznogTakmicenjas.RemoveRange(okrug);
            B1.MVP1.RemoveRange(mvp);
            B1.MVPOkruznogTakmicenjas.RemoveRange(mvp2);
            B1.PobednikSkolskogTakmcienjas.RemoveRange(skolsko);
            B1.Pobedes.RemoveRange(pobskolskog);
            B1.Mecs.RemoveRange(mecevskol);
            B1.Takmicenjes.RemoveRange(tak1);
            B1.MVPs.RemoveRange(mvp3);
            B1.MVPSkolskogTakmicenjas.RemoveRange(mvp4);
            B1.RsSkolskogs.RemoveRange(rs);
            B1.SaveChanges();
        
            
        }

        //Funkcija za pauzu.
        private void Odmor(object sender, EventArgs e)
        {
            pauza++;
            Vreme.Text = $"KRAJ {cetvrtina - 1} CETVRTINE: " + pauza.ToString();
            if (pauza == 3)
            {
                pauza = 0;
                dt1.Stop();
                increment = 0;
                dt.Start();
            }

        }
        #region Pobede
        public void Pobeda1(OkruzniMec M)
        {

            var Tim = from a in B1.Tims
                      where
            M.Razred1 == a.Razred && a.Naziv_Skole == M.NazivSkole1
                      select a.Id;
            int id = Tim.First();
            Pobede2 b = new Pobede2()
            {
                Id_TIma = id,
                NazivSkole = M.NazivSkole1,
            };
            B1.Pobede2.Add(b);
            B1.SaveChanges();
        }


        public void Pobeda2(OkruzniMec M)
        {

            var Tim = from a in B1.Tims
                      where
            M.Razred2 == a.Razred && a.Naziv_Skole == M.NazivSkole2
                      select a.Id;
            int id = Tim.First();
            Pobede2 b = new Pobede2()
            {
                Id_TIma = id,
                NazivSkole = M.NazivSkole1,
            };
            B1.Pobede2.Add(b);
            B1.SaveChanges();
        }
        #endregion
        //Za poene
        public int Radio()
        {
            if (R1.IsChecked == true)
                return 1;
            else if (R2.IsChecked == true)
                return 2;
            else if (R3.IsChecked == true)
                return 3;
            else
                return 0;
        }
        //Za selekciju
        public void Selektovane()
        {
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
            {
                foreach (var a in Statistike)
                    if (a.ID_UCENIKA == Selektovani.Id)
                        a.BrPoena += Radio();

                Score1 += Radio();
                Skor.Text = $"{Score1}:{Score2}";
                LTima.UnselectAll();
            }
            if (Selektovani2 != null)
            {
                foreach (var a in Statistike)
                    if (a.ID_UCENIKA == Selektovani2.Id) //Dodamo  poene 
                        a.BrPoena += Radio();
                Score2 += Radio();
                Skor.Text = $"{Score1}:{Score2}";
                LT2.UnselectAll();
            }
        }
        //Fuja za dodelu MVpija
        public void MVP_DODELA()
        {
            MVP1 Mv;

            int i, j;
            Statistika Smax = new Statistika();//Nadjemo max statisitku sa max poena.
            Smax = Statistike[0];// MORA
            for (i = 0; i < Statistike.Count() - 1; i++)
                for (j = 0; j < Statistike.Count(); j++)
                    if (Smax.BrPoena.Value < Statistike[j].BrPoena.Value)
                        Smax = Statistike[j];
            Mv = new MVP1()
            {
                Id_Ucenika = Smax.ID_UCENIKA,
            };
            B1.MVP1.Add(Mv);
            B1.SaveChanges();

            var ime = from a in B1.Uceniks
                      where a.Id == Smax.ID_UCENIKA
                      select a.Ime;
            MessageBox.Show($"MVP utakmice  JE: {ime.FirstOrDefault()} ");
            //Nakon zavrsetka meca sve poene resetujemo na 0;
            foreach (var a in Statistike)
                a.BrPoena = 0;

        }



        public List<int> PomocnaLista = new List<int>();

        //Funkcija za pobednika takmicenja
        public void PobednikTakmicenja()
        {
            //Proverimo da li je takmicenje gotovo.
            int brojacMeceva = 1;
            PomocnaLista.Clear();
            foreach (var a in Mecevi)
            {
                if (a.Odigrano == "Nije")
                    brojacMeceva = 0;

            }
            if (brojacMeceva == 1)
            {
                List<MVP1> mVPs = new List<MVP1>();

                //Takmicenje je gotovo
                Plej.IsEnabled = false;
                var winner = from a in B1.Pobede2 select a.Id_TIma;
                foreach (var a in winner)
                    PomocnaLista.Add(a.Value);
                var Pom = from a in B1.MVP1  select a;
                foreach (var a in Pom)
                {
                    mVPs.Add(a);
                }

                var most = (from i in PomocnaLista
                            group i by i into grp
                            orderby grp.Count() descending
                            select grp.Key).First();
                //Sortiramo i uzmemo najveci count idija  ucenika
                var mvp = (from a in mVPs
                           group a by a.Id_Ucenika into grp
                           orderby grp.Count() descending
                           select grp.Key).First();

                var uc = from a in B1.Uceniks where a.Id == mvp  select a.Ime;
                MessageBox.Show($"TAKMICENJE JE zavrseno POBEDNIK JE SA IDIJEM {most}");
                MessageBox.Show($"MVP TAKMICENJA JE: {uc.FirstOrDefault()}");
                var naziv = from a in B1.Tims where a.Id == most select a.Naziv_Skole; //uzemomo nazif
                if (Proveri(most) == false)
                {
                    PobednikOkruznogTakmicenja P1 = new PobednikOkruznogTakmicenja()
                    {
                        //Ovo mi nije ni trebalo.
                        NazivSkole = naziv.FirstOrDefault(),
                        Id_tima = most
                    };
                    B1.PobednikOkruznogTakmicenjas.Add(P1);
                    B1.SaveChanges();
                    if (MessageBox.Show("Da li zelite da Arhivirate Takmicenje?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {  //Ako  je kraj :( arhiviramoooooo
                        DateTime date = new DateTime();
                        date = DateTime.Now;
                        Arhiva A1 = new Arhiva()
                        {
                            Datum = date,
                            Id_mvpija=mvp,
                            Id_Takmicara=most
                        };
                        B1.Arhivas.Add(A1);
                        B1.SaveChanges();
                    }

                }


            }

        }
        //Funkcija koja proverava da li se dati idi vec nalazi u takmicenju.
        public bool Proveri(int most)
        {
            //Proverimo da li ima neko u takmicenju sa argumentom.
            var Pronadji = from a in B1.PobednikOkruznogTakmicenjas
                           where a.Id_tima == most
                           select a;
            bool dali = false;
            if (Pronadji.Count() > 0)  //Ako rezultat kverija nije nula
                dali = true;

            return dali;
        }
        //Fuja za pomeranje popupova
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
        public void tabela()
        {
            var Takmicenje = from a in B1.OkruznoTakmicenjes  select a;
            foreach (var a in Takmicenje)
            {
                var idijevi = from b in B1.Tims where b.Naziv_Skole == a.NazivSkole && b.Razred == a.Razred select b.Id;
                var idi = idijevi.FirstOrDefault();
                var pob = from c in B1.Pobede2 where c.Id_TIma == idi select c.Id_TIma;
                LeaderBoard L1 = new LeaderBoard()
                {
                    Naziv = a.NazivSkole,
                    Razred = a.Razred.Value,
                    Pobede = pob.Count(),
                    Id = idijevi.First(),

                };
                L.Add(L1);

            }
        }

        public void TbPobednik()
        {
            var mvp = from a in B1.MVPOkruznogTakmicenjas  select a.Id_Ucenika;
            var imeucenika = from a in B1.Uceniks where a.Id == mvp.FirstOrDefault() select a.Ime;
            var takmicenje = from a in B1.PobednikOkruznogTakmicenjas  select a;
            if (takmicenje.Count() > 0)
            {
                var a = takmicenje.FirstOrDefault(); //Pobednik Okruznog
                var be = from b in B1.Tims where b.Id == a.Id_tima select b.Razred;
                var be1 = from b in B1.Tims where b.Id == a.Id_tima select b.Naziv_Skole;

                Tblock.Visibility = Visibility.Visible;
                Tblock.Text = $"Takmicenje je vec odigrano a pobednik je tim skole: {be1.FirstOrDefault()} razred: {be.FirstOrDefault()}  \n" +
                $"Dok je MVP OVOG TAKMICENJA {imeucenika.FirstOrDefault()}";
            }
        }







    }
}
