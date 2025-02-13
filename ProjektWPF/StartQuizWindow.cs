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
    public partial class StartQuizWindow : Window
    {
        private List<QuizQuestion> questions = new List<QuizQuestion>();
        private int currentQuestionIndex = 0;
        private int score = 0;

        public StartQuizWindow()
        {
            InitializeComponent();
            
            Loaded += async (s, e) => await LoadQuestionsAsync();
        }

        private async Task LoadQuestionsAsync()
        {
           
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QuizDB;Integrated Security=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = @"SELECT QuestionId, QuizId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption 
                                     FROM Question 
                                     ORDER BY QuestionId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            QuizQuestion question = new QuizQuestion
                            {
                                QuestionId = reader.GetInt32(0),
                                QuizId = reader.GetInt32(1),
                                QuestionText = reader.GetString(2),
                                OptionA = reader.GetString(3),
                                OptionB = reader.GetString(4),
                                OptionC = reader.GetString(5),
                                OptionD = reader.GetString(6),
                                CorrectOption = reader.GetString(7)
                            };

                            questions.Add(question);
                        }
                    }
                }

                if (questions.Count > 0)
                {
                    DisplayCurrentQuestion();
                }
                else
                {
                    MessageBox.Show("Brak pytań w bazie danych.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania pytań: " + ex.Message);
                Close();
            }
        }

        private void DisplayCurrentQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                var question = questions[currentQuestionIndex];
                QuestionTextBlock.Text = question.QuestionText;
                rbOptionA.Content = "A. " + question.OptionA;
                rbOptionB.Content = "B. " + question.OptionB;
                rbOptionC.Content = "C. " + question.OptionC;
                rbOptionD.Content = "D. " + question.OptionD;

                
                rbOptionA.IsChecked = false;
                rbOptionB.IsChecked = false;
                rbOptionC.IsChecked = false;
                rbOptionD.IsChecked = false;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
           
            string selectedOption = null;
            if (rbOptionA.IsChecked == true)
                selectedOption = "A";
            else if (rbOptionB.IsChecked == true)
                selectedOption = "B";
            else if (rbOptionC.IsChecked == true)
                selectedOption = "C";
            else if (rbOptionD.IsChecked == true)
                selectedOption = "D";

            if (string.IsNullOrEmpty(selectedOption))
            {
                MessageBox.Show("Proszę zaznaczyć odpowiedź.");
                return;
            }

            
            var currentQuestion = questions[currentQuestionIndex];
            if (selectedOption.Equals(currentQuestion.CorrectOption, StringComparison.OrdinalIgnoreCase))
            {
                score++;
            }

            currentQuestionIndex++;
            if (currentQuestionIndex < questions.Count)
            {
                DisplayCurrentQuestion();
            }
            else
            {
                MessageBox.Show($"Koniec quizu! Twój wynik: {score} na {questions.Count}");
               
                Close();
            }
        }
    }
}