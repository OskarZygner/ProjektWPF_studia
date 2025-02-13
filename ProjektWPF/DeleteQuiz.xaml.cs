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
using Microsoft.Data.SqlClient;

namespace ProjektWPF
{
    public partial class DeleteQuizWindow : Window
    {
        
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QuizDB;Integrated Security=True;";

        public DeleteQuizWindow()
        {
            InitializeComponent();
            
            Loaded += async (s, e) => await LoadQuizzesAsync();
        }

        
        private async Task LoadQuizzesAsync()
        {
            List<Quiz> quizzes = new List<Quiz>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT QuizId, Title FROM Quiz ORDER BY Title";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                quizzes.Add(new Quiz
                                {
                                    QuizId = reader.GetInt32(0),
                                    Title = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
                QuizListBox.ItemsSource = quizzes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania quizów: " + ex.Message);
            }
        }

      
        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (QuizListBox.SelectedItem is Quiz selectedQuiz)
            {
                if (MessageBox.Show($"Czy na pewno chcesz usunąć quiz: '{selectedQuiz.Title}'?",
                                    "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            await conn.OpenAsync();
                            string deleteQuery = "DELETE FROM Quiz WHERE QuizId = @QuizId";
                            using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@QuizId", selectedQuiz.QuizId);
                                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Quiz został usunięty.");
                                    await LoadQuizzesAsync();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd podczas usuwania quizu: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać quiz do usunięcia.");
            }
        }

        
        private async void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć wszystkie quizy?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();
                        string deleteAllQuery = "DELETE FROM Quiz";
                        using (SqlCommand cmd = new SqlCommand(deleteAllQuery, conn))
                        {
                            await cmd.ExecuteNonQueryAsync();
                            MessageBox.Show("Wszystkie quizy zostały usunięte.");
                            await LoadQuizzesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas usuwania quizów: " + ex.Message);
                }
            }
        }
    }

    
    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
    }
}