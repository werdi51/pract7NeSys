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

namespace pract7correct
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User CurrentUser = new User();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ResetUser(object sender, RoutedEventArgs e)
        {
            var currentUser = (User)Resources["CurrentUser"];
            currentUser.Id = 0;
            currentUser.Name = "Имя пользователя";
            currentUser.Email = "user@example.com";
            currentUser.Age = 18;
        }
    }
}