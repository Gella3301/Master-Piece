using Kursovaya.Entities;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для NewSessionWindow.xaml
    /// </summary>
    public partial class NewSessionWindow : Window
    {
        private DispatcherTimer _timer;
        private TimeSpan _elapsedTime;
        private bool _isRunning = false;
        private DateTime _startTime;

        public NewSessionWindow()
        {
            InitializeComponent();
            LoadPieces(); // Загружаем список произведений
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        // Загрузка произведений в ComboBox
        private void LoadPieces()
        {
            using (var db = new AppDbContext())
            {
                var pieces = db.Pieces.ToList();
                cmbPieces.ItemsSource = pieces;
                cmbPieces.DisplayMemberPath = "title"; // Показываем название
                cmbPieces.SelectedValuePath = "pieceId"; // Запоминаем ID
                if (pieces.Count > 0) cmbPieces.SelectedIndex = 0;
            }
        }

        // Обработчик кнопки Старт/Стоп
        private void btnToggleTimer_Click(object sender, RoutedEventArgs e)
        {
            if (!_isRunning)
            {
                // СТАРТ
                if (cmbPieces.SelectedValue == null)
                {
                    MessageBox.Show("Выберите произведение!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _startTime = DateTime.Now;
                _elapsedTime = TimeSpan.Zero;
                lblTimer.Text = "00:00:00";
                _timer.Start();
                _isRunning = true;

                btnToggleTimer.Content = "⏹ Стоп";
                btnToggleTimer.Background = System.Windows.Media.Brushes.IndianRed;
                lblStatus.Text = "Идет тренировка...";
                cmbPieces.IsEnabled = false; // Блокируем смену песни
            }
            else
            {
                // СТОП
                _timer.Stop();
                _isRunning = false;

                btnToggleTimer.Visibility = Visibility.Collapsed;
                pnlStartStop.Visibility = Visibility.Collapsed;
                pnlRating.Visibility = Visibility.Visible;
                lblStatus.Text = "Тренировка завершена!";
                cmbPieces.IsEnabled = true;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
            lblTimer.Text = _elapsedTime.ToString(@"hh\:mm\:ss");
        }

        // Обработчик кнопки "Сохранить сессию"
        private void btnSaveSession_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSelfRate.SelectedItem == null)
            {
                MessageBox.Show("Выберите оценку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    // Находим пользователя (временно первого)
                    var user = db.Users.FirstOrDefault();
                    if (user == null) return;

                    // Создаем сессию
                    Session newSession = new Session
                    {
                        StartTime = _startTime,
                        EndTime = DateTime.Now,
                        selfrate = int.Parse((cmbSelfRate.SelectedItem as ComboBoxItem).Content.ToString()),
                        type = "Тренировка",
                        Piece = db.Pieces.Find((int)cmbPieces.SelectedValue) // Привязываем песню
                    };

                    db.Sessions.Add(newSession);
                    db.SaveChanges();
                }

                MessageBox.Show("Сессия успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
