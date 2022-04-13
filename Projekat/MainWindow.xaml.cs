using Projekat.Logovanje;
using Projekat.Logovanje.UceniciLogovanje;
using System;
using System.Collections.Generic;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        
        public BAZA3 B1 = new BAZA3();
        public string x, y, z;


        //Funkcija za brisanje elemenaata iz trenutno ulogovanih,kada se neko novi uloguje 
        public void Obrisi(){
            var records = from m in B1.TrenutnoUlogovanis
                          select m;
            foreach (var record in records)
            {
                B1.TrenutnoUlogovanis.Remove(record);
            }
            B1.SaveChanges();

        }

        private void LogIn(object sender, RoutedEventArgs e)
        {
            //Instanca objekta trenutno ulogovanih
            TrenutnoUlogovani Tr= new TrenutnoUlogovani();
            //Querry Pomocu kojih znam ko je ulogovan //Skola//admin/ucenik
            var ProveraZaSkolu = (from s in B1.Skolas
                            where Name.Text== s.Naziv && Pass.Text==s.Sifra 
                           select "SKOLA");
            var ProveraZaAdmina=(from a in B1.Administrators
                            where Name.Text== a.Ime && Pass.Text == a.Sifra
                           select "ADMIN");
            var ProveraZaUcenika= (from uce in B1.Uceniks
                                   where Name.Text == uce.Ime && Pass.Text == uce.Sifra
                                   select "UCENIK");
            x = ProveraZaSkolu.FirstOrDefault();
            y = ProveraZaAdmina.FirstOrDefault();
            z = ProveraZaUcenika.FirstOrDefault();
            //Pomocne varijable i stranica za admina
            AdminLog a1 = new AdminLog();
            SkolaLog s1 = new SkolaLog();
            UlogovaniUcenik u1 = new UlogovaniUcenik();
            Obrisi();

            //Radi ako se tek ulogujes ali sta ako se vise njih uloguje...
            if (y == "ADMIN")
            {
                IEnumerable<int> TrenutniID = (from a in B1.Administrators
                                               where Name.Text == a.Ime && Pass.Text == a.Sifra
                                               select a.Id);
                int i = TrenutniID.First();
                Tr.Id = i;
                B1.TrenutnoUlogovanis.Add(Tr);
                B1.SaveChanges();
                this.Content = a1;

            }
            if(x=="SKOLA")
                {
                IEnumerable<int> TrenutniID = (from a in B1.Skolas
                                               where Name.Text == a.Naziv && Pass.Text == a.Sifra
                                               select a.Id);
                int i = TrenutniID.First();
                Tr.Id = i;
                B1.TrenutnoUlogovanis.Add(Tr);
                B1.SaveChanges();
                this.Content = s1;
                    
                
            }
                
            if (z == "UCENIK")
            {
                IEnumerable<int> TrenutniID = (from a in B1.Uceniks
                                               where Name.Text == a.Ime && Pass.Text == a.Sifra
                                               select a.Id);
                int i = TrenutniID.First();
                Tr.Id = i;
                B1.TrenutnoUlogovanis.Add(Tr);
                B1.SaveChanges();
                this.Content = u1;
            }
        
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            A.Visibility = Visibility.Visible;
            U.Visibility = Visibility.Visible;
            S.Visibility = Visibility.Visible;
        }

        private void Admin(object sender, RoutedEventArgs e)
        {
            Admin a = new Admin();
            this.Content = a;
        }

        private void U_Click(object sender, RoutedEventArgs e)
        {
            Page1 p = new Page1();
            this.Content = p;
        }

        private void S_Click(object sender, RoutedEventArgs e)
        {

            School s = new  School();
            this.Content = s;
        
        }
    }
}
