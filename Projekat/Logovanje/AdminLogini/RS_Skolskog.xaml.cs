using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Projekat.Logovanje.AdminLogini
{
    /// <summary>
    /// Interaction logic for RS_Skolskog.xaml
    /// </summary>
    public partial class RS_Skolskog : Page
    {
        public BAZA3 B1 = new BAZA3();
        public RS_Skolskog()
        {
            DataContext = this;
            InitializeComponent();
        }
        public Button[,] Dugmici = new Button[4, 5]; //posle cemo promijenit mozda
        public ObservableCollection<Ucenik> Obs { get; set; } = new ObservableCollection<Ucenik>();

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Treba da pokupimo timove u takmicenju.
            // Da nadjemo njihove ucenike 
            //I da ih ubacimo u listview...
            Obs.Clear(); 
            ComboBoxItem c2 = (ComboBoxItem)C2.SelectedItem;


            if (C1.SelectedItem != null && c2!=null )
            {
                Obs.Clear();
                string pomocna = C1.SelectedItem as string;
                List<int> idijevi = new List<int>();
                string pom1 = c2.Content as string;
                int raz = int.Parse(pom1);
                var RazredTimova = from a in B1.Takmicenjes
                             where a.NazivSkole == pomocna && a.Razred == raz
                             select a.Razred; //Pokupimo sve timove u takmicenju.OD njih nam je bitan razred

                foreach (var b in RazredTimova)
                {
                    var Timovi = from a in B1.Tims where a.Razred == b  && a.Naziv_Skole==pomocna select a.id_ucenika;
                    idijevi = Timovi.ToList();
                }
                    foreach (var a in idijevi)
                    {
                        var ucenicki = from c in B1.Uceniks where c.Id == a  select c;
                        Obs.Add(ucenicki.FirstOrDefault());
                    }

                    
                //Deo sa sedistima.
                //Treba da resetujem celu tabelu sedista. 
                //Zatim da je popunim delovima gde postoje rezervisana mjesta.
                OcistiTribine();
                UpdejtujTribinu();
            }
        }
        public void OcistiTribine(){
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 5; j++)
                    Dugmici[i, j].Content = "_";
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            var Skolice = (from a in B1.Skolas
                           select a.Naziv);
            C1.ItemsSource = Skolice.ToList();
            NapraviTribinu();
        }
       public void UpdejtujTribinu(){
            ComboBoxItem c2 = (ComboBoxItem)C2.SelectedItem;
            string pomocna = C1.SelectedItem as string;
            string pom1 = c2.Content as string;
            int raz = int.Parse(pom1);
            //Skontahhhh YAY
            var Rasporedi = from a in B1.RsSkolskogs where a.NazivSkole == pomocna && a.Razred==raz  select a;//Uzmemo sve timove koji imaju rez mesto.
            if (Rasporedi.Count() > 0)
            {
                foreach (var a in Rasporedi)
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 5; j++)
                        {
                            if (a.Red == i && a.Kolona == j)
                                Dugmici[i, j].Content = a.id_ucenika;
                        }

            }
        }

        public void NapraviTribinu()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 5; j++)
                {
                    Button MyControl = new Button();
                    MyControl.Content = "_";
                    MyControl.Name = "A" + i.ToString() + j.ToString();
                    MyControl.Width = 60;
                    MyControl.Height = 60;
              
                    MyControl.Background = Brushes.Purple; 
                    MyControl.Click += MyControl_Click;
                    Grid.SetRow(MyControl, i);
                    Grid.SetColumn(MyControl, j);
                    Dugmici[i,j] = MyControl;
                    Gridara.Children.Add(MyControl);
                    
                }
            
        }
        //Funkcija za proveru ko se nalazi u sjedistu.
        /*   public bool DaliSeNalazi(Ucenik U,int red){
                //Treba proveriti ba da li se nalazi ovo gde treba da se nalazi,razumeo ba?
                string pomocna = C1.SelectedItem as string;
                //Proverimo da li ima uopste ikog u tom redu ako nema bujrum(a da nije ucenik iste skole).
                //Proveravamo da li ima nekog u redu ko je iz te skole a nije iz istog razreda.
                var Prazan = from a in B1.RsSkolskogs
                             where a.Red == red && a.NazivSkole == U.Naziv_Skole && a.Razred!=U.Razred
                             select a; 
                //Proverimo da li neko sedi u tom redu.
                var Pr = from a in B1.RsSkolskogs
                         where a.NazivSkole == U.Naziv_Skole && a.Razred == U.Razred
                                && a.Red == red 
                         select a;
                var ImaliGa = from a in B1.RsSkolskogs where U.Id == a.id_ucenika select a;
                if (ImaliGa.Count() == 0)
                {
                    if (Prazan.Count() > 0) //ako nije prazan i ako nije vec taj ucenjak u njemu.
                    {
                        if (Pr.Count() > 0)
                            return true;
                        else
                            return false;
                    }
                    else return true; //ako je prazan ima mesta i moze smestiti u red
                }
                else
                    return false; //ako postoji  vec nista ga na stavljaj.
            }

        */
        public bool ProveriTo(Ucenik U)
        {
            var dali = from a in B1.RsSkolskogs where U.Id == a.id_ucenika select a;
            if (dali.Count() > 0)
                return true;
            else return false;
            
        }
            private void MyControl_Click(object sender, RoutedEventArgs e)
             { //Klik event bukvalno dodaje igraca  na sediste i usput proverava da li je sediste puno/prazno.
                //Treba da ubacimo igraca u dugmice.
                string pomocna = C1.SelectedItem as string;
                Button btn = (Button)sender;
                string sediste = btn.Name as string;
                int RED = sediste[1] - '0';
                
            string p = btn.Content as string;
                
                Ucenik U = (Ucenik)Lw.SelectedItem;
            if (ProveriTo(U) == false)
            {
                if (U != null) //Ako je razlicit od nulle.
                {
                    if (p == "_")
                    {
                        btn.Content = U.Id;
                        MessageBox.Show($"Ucenik Sa idijem{U.Id} je {U.Ime}");
                        RsSkolskog Rs = new RsSkolskog()
                        {
                            id_ucenika = U.Id,
                            Red = sediste[1] - '0',
                            Kolona = sediste[2] - '0',
                            NazivSkole = U.Naziv_Skole,
                            Razred = U.Razred
                        };
                        B1.RsSkolskogs.Add(Rs);
                        B1.SaveChanges();

                    }
                }
            }
           }

        private void C2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Da nadjemo njihove ucenike 
            //I da ih ubacimo u listview...
            Obs.Clear();
            ComboBoxItem c2 = (ComboBoxItem)C2.SelectedItem;

            if (C1.SelectedItem != null && c2!=null)
            {
                Obs.Clear();
                string pomocna = C1.SelectedItem as string;
                List<int> idijevi = new List<int>();
                string pom1 = c2.Content as string;
                int raz = int.Parse(pom1);
                var RazredTimova = from a in B1.Takmicenjes
                                   where a.NazivSkole == pomocna && a.Razred == raz
                                   select a.Razred; //Pokupimo sve timove u takmicenju.OD njih nam je bitan razred

                foreach (var b in RazredTimova)
                {
                    var Timovi = from a in B1.Tims where a.Razred == b  && a.Naziv_Skole==pomocna select a.id_ucenika;
                    idijevi = Timovi.ToList();
                }
                foreach (var a in idijevi)
                {
                    var ucenicki = from c in B1.Uceniks where c.Id == a select c;
                    Obs.Add(ucenicki.FirstOrDefault());
                }
                //Deo sa sedistima.
                //Treba da resetujem celu tabelu sedista. 
                //Zatim da je popunim delovima gde postoje rezervisana mjesta.
                OcistiTribine();
                UpdejtujTribinu();
            }
        }
    }
    }
