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
            LoadPieces();
        }

      
        private void btnOpenNewSession_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Создание новых сессий будет доступно после реализации других окон , необходимых для работы этой функции");
        }

      

        private void AddPieceBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPieceWindow addWin = new AddPieceWindow();
            addWin.ShowDialog();
            LoadPieces();
        }
        public void LoadPieces()
        {
            // Очищаем панель перед загрузкой (чтобы не дублировались)
            PiecesPanel.Children.Clear();

            // Получаем данные через Фасад
            DataFacade facade = new DataFacade();
            var pieces = facade.GetAllPieces();

            foreach (var piece in pieces)
            {
                // Создаем контейнер для одной плитки
                Border card = new Border
                {
                    Width = 180,
                    Margin = new Thickness(0, 0, 20, 20),
                    Background = System.Windows.Media.Brushes.White,
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(10)
                };

                // Внутренний StackPanel для содержимого
                StackPanel stack = new StackPanel();

                // 1. Картинка (если есть)
                Image img = new Image
                {
                    Height = 140,
                    Margin = new Thickness(0, 0, 0, 10),
                    Stretch = System.Windows.Media.Stretch.UniformToFill
                };

                if (!string.IsNullOrEmpty(piece.coverUrl))
                {
                    try
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new System.Uri(piece.coverUrl, System.UriKind.Absolute);
                        bitmap.EndInit();
                        img.Source = bitmap;
                    }
                    catch { /* Если картинка не загрузилась, просто оставляем пустой */ }
                }
             

                // 2. Название
                TextBlock lblTitle = new TextBlock
                {
                    Text = piece.title,
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    Foreground = System.Windows.Media.Brushes.Black,
                    TextTrimming = TextTrimming.CharacterEllipsis
                };

                // 3. Композитор
                TextBlock lblComposer = new TextBlock
                {
                    Text = piece.Composer,
                    FontSize = 12,
                    Foreground = System.Windows.Media.Brushes.Gray,
                    Margin = new Thickness(0, 2, 0, 0)
                };

                // Собираем плитку
                stack.Children.Add(img);
                stack.Children.Add(lblTitle);
                stack.Children.Add(lblComposer);

                card.Child = stack;
                PiecesPanel.Children.Add(card);
            }
        }
    }
}
    
