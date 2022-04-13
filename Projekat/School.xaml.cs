using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for School.xaml
    /// </summary>
    public partial class School : Page
    {
        public School()
        {
            InitializeComponent();
        }
        public BAZA3 B2 = new BAZA3();

        public void DodajSkolu()
        {

            try
            {
                PomocnaSkola S1 = new PomocnaSkola()
                {
                    Naziv = Ime1.Text,
                    Grad = Grad.Text,
                    Sifra = Sifra1.Password

             
                
                };
                B2.PomocnaSkolas.Add(S1);

                B2.SaveChanges();
                MessageBox.Show("SKOLA JE USPESNO DODATA");

            }
            catch (Exception)
            { MessageBox.Show("Morate uneti unikatan Naziv ^__^ "); }
        }
        //Provera istoce sifri
        public bool provera()
        {
            string a = Sifra1.Password;
            string b = Sifra2.Password;
            if (a.Equals(b) != true)
            {
                return false;
            }
            else return true;

        }
        //SIfra
        public bool proveraSifre()
        {
            if (provera() == true)
            {
                string txt = (string)Sifra1.Password; //returns input 
                string txt2 = (string)Sifra2.Password;
                Regex r1 = new Regex("^[a-zA-Z0-9]{8,20}$"); //Sadrzi  slova i brojeve od 0-9
                Regex r2 = new Regex("^[a-zA-Z0-9]{8,20}$");
                if (r1.IsMatch(txt) == true && r2.IsMatch(txt2) == true)
                {
                    Sifra1.BorderBrush = Brushes.Green;
                    Sifra2.BorderBrush = Brushes.Green;
                    return true;
                }
                else
                {
                    Sifra1.BorderBrush = Brushes.Red;
                    Sifra2.BorderBrush = Brushes.Red;
                    return false;
                }
            }
            else
            {
                Sifra1.BorderBrush = Brushes.Red;
                Sifra2.BorderBrush = Brushes.Red;
                return false;
            }



        }

        private void Register(object sender, RoutedEventArgs e)
        {
            if (proveraSifre() == true)
                DodajSkolu();
            else MessageBox.Show("Neispravno Uneta sifra");
            }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow mw = new MainWindow();
            Application.Current.MainWindow = mw;

            Application.Current.MainWindow.Show();
        }
    }
}

