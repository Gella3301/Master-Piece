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
            NewSessionWindow sessionWin = new NewSessionWindow();
            sessionWin.Owner = this;
            sessionWin.ShowDialog();
            LoadSessions();
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
        public void LoadSessions()
        {
            // Очищаем панель перед загрузкой
            SessionsList.Children.Clear();

            DataFacade facade = new DataFacade();
            var sessions = facade.GetAllSessionsWithPieces();

            if (sessions.Count == 0)
            {
                TextBlock emptyText = new TextBlock
                {
                    Text = "У вас пока нет сыгранных сессий. Начните новую!",
                    Foreground = System.Windows.Media.Brushes.Gray,
                    FontSize = 14,
                    Margin = new Thickness(10)
                };
                SessionsList.Children.Add(emptyText);
                return;
            }

            foreach (var session in sessions)
            {
                // Карточка сессии
                Border card = new Border
                {
                    Background = System.Windows.Media.Brushes.White,
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15)
                };

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                // Левая часть: Название, время, оценка
                StackPanel leftPanel = new StackPanel();
                leftPanel.VerticalAlignment = VerticalAlignment.Center;

                // Название произведения (берем из связанного Piece)
                TextBlock lblTitle = new TextBlock
                {
                    Text = session.Piece?.title ?? "Неизвестное произведение",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = System.Windows.Media.Brushes.Black
                };

                // Дата и время
                string dateStr = session.StartTime.ToString("dd.MM.yyyy • HH:mm") + " - " + session.EndTime.ToString("HH:mm");
                string infoStr = $"{dateStr}   |   Оценка: {session.selfrate}/10";

                TextBlock lblInfo = new TextBlock
                {
                    Text = infoStr,
                    FontSize = 13,
                    Foreground = System.Windows.Media.Brushes.Gray,
                    Margin = new Thickness(0, 8, 0, 0)
                };

                leftPanel.Children.Add(lblTitle);
                leftPanel.Children.Add(lblInfo);
                Grid.SetColumn(leftPanel, 0);

                // Правая часть: Длительность
                TimeSpan duration = session.EndTime - session.StartTime;
                TextBlock lblDuration = new TextBlock
                {
                    Text = duration.ToString(@"hh\:mm\:ss"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(79, 70, 229)), // Фиолетовый #4F46E5
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(20, 0, 0, 0)
                };
                Grid.SetColumn(lblDuration, 1);

                grid.Children.Add(leftPanel);
                grid.Children.Add(lblDuration);

                card.Child = grid;
                SessionsList.Children.Add(card);
            }
        }
    }
}
    
