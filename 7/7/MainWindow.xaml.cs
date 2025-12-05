using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        List<Doctor> medik = new List<Doctor>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private int Randomiser(bool doctor)
        {
            if (doctor)
            {
                int randomID = rnd.Next(10000, 99999);
                return randomID;
            }
            else
            {
                int randomID = rnd.Next(1000000, 9999999);
                return randomID;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Doctor NewMedik = new Doctor();

        }
    }
}