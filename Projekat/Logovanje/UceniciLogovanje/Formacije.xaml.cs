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

namespace Projekat.Logovanje.UceniciLogovanje
{
    /// <summary>
    /// Interaction logic for Formacije.xaml
    /// </summary>
    public partial class Formacije : Page
    {
        public Button[,] Dugmici = new Button[4, 5]; //posle cemo promijenit mozda
        public BAZA3 B1 = new BAZA3();

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
                    Grid.SetRow(MyControl, i);
                    Grid.SetColumn(MyControl, j);
                    Dugmici[i, j] = MyControl;
                    Gridara.Children.Add(MyControl);

                }

        }
        public int TrenutniId()
        {
            int i;
            IEnumerable<int> TrenutniID = (from a in B1.TrenutnoUlogovanis
                                           select a.Id);
            i = TrenutniID.First(); // Id tr ulogovane skole
            return i;
        }



        public void UpdejtujTribinu()
        {
            int idii = TrenutniId();
            var raz = from a in B1.Uceniks where a.Id == idii select a.Razred;
            var pomocna = from a in B1.Uceniks where a.Id == idii select a.Naziv_Skole;
                

            //Skontahhhh YAY
            var Rasporedi = from a in B1.RsSkolskogs where a.NazivSkole == pomocna.FirstOrDefault() && a.Razred == raz.FirstOrDefault() select a;//Uzmemo sve timove koji imaju rez mesto.
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


        public Formacije()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NapraviTribinu();
            UpdejtujTribinu();
        }
    }
}
