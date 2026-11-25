using Kursovaya.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для AddPieceWindow.xaml
    /// </summary>
    public partial class AddPieceWindow : Window
    {
        public AddPieceWindow()
        {
            InitializeComponent();
            LoadGenres();
            
            
        }

        /// <summary>
        /// Метод подгрузки жанров
        /// </summary>
        private void LoadGenres()
        {
            using (var db = new AppDbContext())
            {
                // создаем базовые жанры
                if (!db.Genres.Any())
                {
                    db.Genres.Add(new Genre { genreName = "Классика" });
                    db.Genres.Add(new Genre { genreName = "Джаз" });
                    db.Genres.Add(new Genre { genreName = "Рок" });
                    db.Genres.Add(new Genre { genreName = "Метал" });
                    db.Genres.Add(new Genre { genreName = "Поп" });
                    db.SaveChanges();
                }

                // Привязываем данные к ComboBox
                cmbGenre.ItemsSource = db.Genres.ToList();
                cmbGenre.DisplayMemberPath = "genreName"; 
                cmbGenre.SelectedValuePath = "genreId"; 
               
            }
        }


        private static readonly HttpClient client = new HttpClient();
       

        // Обработчик кнопки "Искать в iTunes"
        private async void btnSearchApi_Click(object sender, RoutedEventArgs e)
        {
            string query = txtTitle.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("Введите название произведения для поиска.");
                return;
            }

            try
            {
                // Формируем URL. term - поисковый запрос, media=music, entity=song
                string url = $"https://itunes.apple.com/search?term={Uri.EscapeDataString(query)}&media=music&entity=song&limit=1";

                // Асинхронный GET-запрос
                string jsonResponse = await client.GetStringAsync(url);

                // Парсим JSON (нам нужен только первый результат)
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                var results = doc.RootElement.GetProperty("results");

                if (results.GetArrayLength() > 0)
                {
                    var firstResult = results[0];

                    // Получаем ссылку на обложку (artworkUrl100) и меняем размер на 600x600 для качества
                    if (firstResult.TryGetProperty("artworkUrl100", out JsonElement artworkElement))
                    {
                        string artworkUrl = artworkElement.GetString();
                        // iTunes отдает картинки 100x100. Заменяем на 600x600.
                        artworkUrl = artworkUrl.Replace("100x100bb.jpg", "600x600bb.jpg");

                        // Загружаем картинку и показываем в Image
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(artworkUrl, UriKind.Absolute);
                        bitmap.EndInit();
                        imgCover.Source = bitmap;

                        // Сохраняем URL в Tag картинки, чтобы потом взять при сохранении
                        imgCover.Tag = artworkUrl;
                    }

                    // Также можно автоматически заполнить композитора, если поле пустое
                    if (string.IsNullOrEmpty(txtComposer.Text) && firstResult.TryGetProperty("artistName", out JsonElement artistElement))
                    {
                        txtComposer.Text = artistElement.GetString();
                    }
                }
                else
                {
                    MessageBox.Show("Ничего не найдено. Попробуйте изменить название.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}");
            }
        }

        /// <summary>
        /// Кнопка сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Пожалуйста, введите название произведения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    // 1. Получаем или создаем пользователя
                    var user = db.Users.FirstOrDefault();
                    if (user == null)
                    {
                        user = new User
                        {
                            username = "DefaultUser",
                            email = "test@test.com",
                            password = "123"
                           
                        };
                        db.Users.Add(user);
                        db.SaveChanges();
                    }

                    // 2. Получаем жанр
                    int selectedGenreId = (int)cmbGenre.SelectedValue;

                    // 3. Создаем произведение
                    Piece newPiece = new Piece
                    {
                        title = txtTitle.Text.Trim(),
                        Composer = txtComposer.Text.Trim(),
                        difficulty = int.TryParse(txtDifficulty.Text, out int diff) ? diff : 1,
                        status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "В планах",
                        coverUrl = imgCover.Tag as string,
                        userId = user.userId,
                        genreId = selectedGenreId
                    };

                    // 4. Сохраняем через Фасад
                    DataFacade facade = new DataFacade();
                    facade.AddPiece(newPiece);
                }

                MessageBox.Show("Произведение успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         /// <summary>
         /// Кнопка закрытия окна
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

