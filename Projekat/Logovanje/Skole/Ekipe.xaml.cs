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

namespace Projekat.Logovanje.Skole
{
    /// <summary>
    /// Interaction logic for Ekipe.xaml
    /// </summary>
    public partial class Ekipe : Page
    {
        public Ekipe()
        {
            InitializeComponent();
        }
        public BAZA3 B1 = new BAZA3();
        public ObservableCollection<Ucenik> Djaci { get; set; } = new ObservableCollection<Ucenik>();





    }
}
