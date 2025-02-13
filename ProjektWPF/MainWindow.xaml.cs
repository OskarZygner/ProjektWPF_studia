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
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;


namespace ProjektWPF
{
    public partial class MainWindow : Window
    {
        
         public string QuizTitle { get; set; }
         public string QuestionText { get; set; }
         public string OptionA { get; set; }
         public string OptionB { get; set; }
         public string OptionC { get; set; }
         public string OptionD { get; set; }
         public string CorrectOption { get; set; }

        public MainWindow()
        {
            InitializeComponent();
           
             DataContext = this;
        }

        private async void AddQuiz_Click(object sender, RoutedEventArgs e)
        {
           //two-way binding 
            string quizTitle = this.QuizTitle?.Trim();
            string questionText = this.QuestionText?.Trim();
            string optionA = this.OptionA?.Trim();
            string optionB = this.OptionB?.Trim();
            string optionC = this.OptionC?.Trim();
            string optionD = this.OptionD?.Trim();


           
            string correctOption = null;
            if (CorrectAnswerComboBox.SelectedItem is ComboBoxItem item)
            {
                correctOption = item.Content.ToString();
            }

            // Validation
            if (string.IsNullOrEmpty(quizTitle) ||
                string.IsNullOrEmpty(questionText) ||
                string.IsNullOrEmpty(optionA) ||
                string.IsNullOrEmpty(optionB) ||
                string.IsNullOrEmpty(optionC) ||
                string.IsNullOrEmpty(optionD) ||
                string.IsNullOrEmpty(correctOption))
            {
                MessageBox.Show("Proszę wypełnić wszystkie pola!");
                return;
            }

           
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QuizDB;Integrated Security=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Concurrency
                    await conn.OpenAsync();

                
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        
                        string insertQuizQuery = "INSERT INTO Quiz (Title) VALUES (@Title); SELECT SCOPE_IDENTITY();";
                        SqlCommand cmdQuiz = new SqlCommand(insertQuizQuery, conn, transaction);
                        cmdQuiz.Parameters.AddWithValue("@Title", quizTitle);

                        object quizIdObj = await cmdQuiz.ExecuteScalarAsync();
                        int quizId = Convert.ToInt32(quizIdObj);

                       
                        string insertQuestionQuery = @"INSERT INTO Question 
                                                       (QuizId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
                                                       VALUES 
                                                       (@QuizId, @QuestionText, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectOption)";
                        SqlCommand cmdQuestion = new SqlCommand(insertQuestionQuery, conn, transaction);
                        cmdQuestion.Parameters.AddWithValue("@QuizId", quizId);
                        cmdQuestion.Parameters.AddWithValue("@QuestionText", questionText);
                        cmdQuestion.Parameters.AddWithValue("@OptionA", optionA);
                        cmdQuestion.Parameters.AddWithValue("@OptionB", optionB);
                        cmdQuestion.Parameters.AddWithValue("@OptionC", optionC);
                        cmdQuestion.Parameters.AddWithValue("@OptionD", optionD);
                        cmdQuestion.Parameters.AddWithValue("@CorrectOption", correctOption);

                        await cmdQuestion.ExecuteNonQueryAsync();

                     
                        transaction.Commit();
                    }
                }

                MessageBox.Show("Quiz został dodany!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas zapisywania danych: " + ex.Message);
            }
        }

        
        private void Correct_answer_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { }
        private void QuizName_box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
        private void AnswerB_Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
        private void AnswerC_box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
        private void AnswerD_Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
        private void AnswerA_Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
    }
}