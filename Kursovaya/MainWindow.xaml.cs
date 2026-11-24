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

namespace Kursovaya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDbContext db;

        public MainWindow()
        {
            InitializeComponent();
            
        db = new AppDbContext();

        db.Database.EnsureCreated();// Инициализация бд
        }

      
        private void btnOpenNewSession_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Создание новых сессий будет доступно после реализации других окон , необходимых для работы этой функции");
        }
    }
}