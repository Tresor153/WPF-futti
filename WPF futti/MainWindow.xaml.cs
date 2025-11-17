using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_futti
{
    public partial class MainWindow : Window
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex = 0;
        private int score = 0;
        private int correctAnswerIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
            InitializeQuestions();
            LoadQuestion();
        }

        private void InitializeQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Wer hat die meisten Ballon d'Or Auszeichnungen gewonnen?",
                    Answers = new[] { "Cristiano Ronaldo", "Lionel Messi", "Michel Platini", "Johan Cruyff" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "In welchem Jahr gewann Deutschland seine letzte Weltmeisterschaft?",
                    Answers = new[] { "2006", "2010", "2014", "2018" },
                    CorrectAnswerIndex = 2
                },
                new QuizQuestion
                {
                    Question = "Welcher Verein hat die meisten Champions League Titel gewonnen?",
                    Answers = new[] { "AC Milan", "Bayern München", "Real Madrid", "FC Barcelona" },
                    CorrectAnswerIndex = 2
                },
                new QuizQuestion
                {
                    Question = "Wer ist der Rekordtorschütze der Fußball-Weltmeisterschaft aller Zeiten?",
                    Answers = new[] { "Pelé", "Ronaldo Nazário", "Miroslav Klose", "Gerd Müller" },
                    CorrectAnswerIndex = 2
                },
                new QuizQuestion
                {
                    Question = "In welchem Jahr wurde die Bundesliga gegründet?",
                    Answers = new[] { "1954", "1963", "1972", "1949" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "Welcher Spieler wechselte für die Rekordsumme von 222 Millionen Euro?",
                    Answers = new[] { "Kylian Mbappé", "Neymar Jr.", "Philippe Coutinho", "Eden Hazard" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "Wie viele Spieler stehen bei einem regulären Fußballspiel auf dem Feld?",
                    Answers = new[] { "20", "22", "24", "18" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "Welches Land gewann die erste Fußball-Weltmeisterschaft 1930?",
                    Answers = new[] { "Brasilien", "Argentinien", "Uruguay", "Italien" },
                    CorrectAnswerIndex = 2
                },
                new QuizQuestion
                {
                    Question = "Wer ist der erfolgreichste Bundesliga-Torschütze aller Zeiten?",
                    Answers = new[] { "Gerd Müller", "Robert Lewandowski", "Klaus Fischer", "Jupp Heynckes" },
                    CorrectAnswerIndex = 0
                },
                new QuizQuestion
                {
                    Question = "In welchem Stadion fand das WM-Finale 2014 statt?",
                    Answers = new[] { "Estádio do Maracanã", "Allianz Arena", "Wembley Stadium", "Aztekenstadion" },
                    CorrectAnswerIndex = 0
                }
            };

            Random rng = new Random();
            questions = questions.OrderBy(x => rng.Next()).ToList();
        }

        private void LoadQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                var question = questions[currentQuestionIndex];
                QuestionText.Text = question.Question;
                Answer1.Content = question.Answers[0];
                Answer2.Content = question.Answers[1];
                Answer3.Content = question.Answers[2];
                Answer4.Content = question.Answers[3];

                correctAnswerIndex = question.CorrectAnswerIndex;

                ResetButtonStyles();
                EnableButtons(true);

                QuestionCounter.Text = $"Frage {currentQuestionIndex + 1}/{questions.Count}";
                NextButton.Visibility = Visibility.Collapsed;
                FeedbackText.Text = "";
            }
            else
            {
                ShowGameOver();
            }
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            int selectedAnswer = -1;

            if (clickedButton == Answer1) selectedAnswer = 0;
            else if (clickedButton == Answer2) selectedAnswer = 1;
            else if (clickedButton == Answer3) selectedAnswer = 2;
            else if (clickedButton == Answer4) selectedAnswer = 3;

            CheckAnswer(selectedAnswer);
            EnableButtons(false);
            NextButton.Visibility = Visibility.Visible;
        }

        private void CheckAnswer(int selectedAnswer)
        {
            Button[] buttons = { Answer1, Answer2, Answer3, Answer4 };

            if (selectedAnswer == correctAnswerIndex)
            {
                score++;
                buttons[selectedAnswer].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A6E3A1"));
                FeedbackText.Text = "✓ Richtig!";
                FeedbackText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A6E3A1"));
            }
            else
            {
                buttons[selectedAnswer].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F38BA8"));
                buttons[correctAnswerIndex].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A6E3A1"));
                FeedbackText.Text = "✗ Falsch!";
                FeedbackText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F38BA8"));
            }

            ScoreLabel.Text = $"Punkte: {score}";
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            LoadQuestion();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex = 0;
            score = 0;
            ScoreLabel.Text = "Punkte: 0";
            GameOverPanel.Visibility = Visibility.Collapsed;
            InitializeQuestions();
            LoadQuestion();
        }

        private void ShowGameOver()
        {
            FinalScore.Text = $"Deine Punktzahl: {score}/{questions.Count}";
            GameOverPanel.Visibility = Visibility.Visible;
        }

        private void ResetButtonStyles()
        {
            Button[] buttons = { Answer1, Answer2, Answer3, Answer4 };
            foreach (var button in buttons)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#45475A"));
                button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CDD6F4"));
            }
        }

        private void EnableButtons(bool enable)
        {
            Answer1.IsEnabled = enable;
            Answer2.IsEnabled = enable;
            Answer3.IsEnabled = enable;
            Answer4.IsEnabled = enable;
        }
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public string[] Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }
    }
}