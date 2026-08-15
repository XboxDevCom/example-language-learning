using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using LanguageLearning.Models;

namespace LanguageLearning
{
    /// <summary>
    /// Hauptseite der Sprachlern-Anwendung mit Karteikarten- und Quizmodus.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Flashcard> _flashcards;
        private int _currentIndex;
        private bool _isFlipped;

        private const int QuizQuestionCount = 10;
        private List<Flashcard> _quizCards;
        private int _quizIndex;
        private int _quizScore;
        private Flashcard _currentQuizCard;
        private bool _quizAnswered;

        private static readonly Random Rng = new Random();

        /// <summary>
        /// Initialisiert eine neue Instanz der MainPage-Klasse.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Wird aufgerufen, wenn zur Seite navigiert wird. Lädt den anfänglichen Wortschatz.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadVocabulary();
        }

        /// <summary>
        /// Lädt den Wortschatz für das aktuell ausgewählte Sprachpaar.
        /// </summary>
        private void LoadVocabulary()
        {
            string pair = GetSelectedLanguagePair();
            _flashcards = new ObservableCollection<Flashcard>(
                Vocabulary.GetFlashcardsByLanguagePair(pair));
            _currentIndex = 0;

            if (ModeSelector.SelectedIndex == 0)
            {
                ShowFlashcard();
            }
            else
            {
                StartQuiz();
            }
        }

        /// <summary>
        /// Gibt das aktuell ausgewählte Sprachpaar zurück.
        /// </summary>
        private string GetSelectedLanguagePair()
        {
            switch (LanguageSelector.SelectedIndex)
            {
                case 1: return "Spanisch-Deutsch";
                case 2: return "Französisch-Deutsch";
                default: return "Englisch-Deutsch";
            }
        }

        /// <summary>
        /// Zeigt die Karteikarte am aktuellen Index an.
        /// </summary>
        private void ShowFlashcard()
        {
            if (_flashcards == null || _flashcards.Count == 0)
                return;

            _isFlipped = false;
            CardWord.Text = _flashcards[_currentIndex].Word;
            CardTranslation.Text = _flashcards[_currentIndex].Translation;
            CardTranslation.Visibility = Visibility.Collapsed;
            FlipButton.Visibility = Visibility.Visible;
            CorrectButton.Visibility = Visibility.Visible;
            IncorrectButton.Visibility = Visibility.Visible;
            NextCardButton.Visibility = Visibility.Visible;
            FlipButton.Content = "Übersetzung anzeigen";
            ProgressText.Text = "Karte " + (_currentIndex + 1) + "/" + _flashcards.Count;
        }

        /// <summary>
        /// Wird beim Klick auf den Übersetzung-Button aufgerufen und deckt die Übersetzung auf oder verbirgt sie.
        /// </summary>
        private void FlipButton_Click(object sender, RoutedEventArgs e)
        {
            _isFlipped = !_isFlipped;

            if (_isFlipped)
            {
                CardTranslation.Visibility = Visibility.Visible;
                FlipButton.Content = "Übersetzung verbergen";
            }
            else
            {
                CardTranslation.Visibility = Visibility.Collapsed;
                FlipButton.Content = "Übersetzung anzeigen";
            }
        }

        /// <summary>
        /// Markiert die aktuelle Karteikarte als bekannt und wechselt zur nächsten.
        /// </summary>
        private void CorrectButton_Click(object sender, RoutedEventArgs e)
        {
            _flashcards[_currentIndex].IsKnown = true;
            GoToNextCard();
        }

        /// <summary>
        /// Markiert die aktuelle Karteikarte als unbekannt und wechselt zur nächsten.
        /// </summary>
        private void IncorrectButton_Click(object sender, RoutedEventArgs e)
        {
            _flashcards[_currentIndex].IsKnown = false;
            GoToNextCard();
        }

        /// <summary>
        /// Wird beim Klick auf den Weiter-Button aufgerufen.
        /// </summary>
        private void NextCardButton_Click(object sender, RoutedEventArgs e)
        {
            GoToNextCard();
        }

        /// <summary>
        /// Wechselt zur nächsten Karteikarte oder zeigt den Abschluss an.
        /// </summary>
        private void GoToNextCard()
        {
            if (_currentIndex < _flashcards.Count - 1)
            {
                _currentIndex++;
                ShowFlashcard();
            }
            else
            {
                int known = _flashcards.Count(f => f.IsKnown);
                CardWord.Text = "Lernsession abgeschlossen!";
                CardTranslation.Text = "Du kanntest " + known + " von " + _flashcards.Count + " Vokabeln.";
                CardTranslation.Visibility = Visibility.Visible;
                ProgressText.Text = "Fertig!";
                FlipButton.Visibility = Visibility.Collapsed;
                CorrectButton.Visibility = Visibility.Collapsed;
                IncorrectButton.Visibility = Visibility.Collapsed;
                NextCardButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Startet eine neue Quiz-Runde mit zufällig ausgewählten Vokabeln.
        /// </summary>
        private void StartQuiz()
        {
            _quizCards = Vocabulary.GetRandomFlashcards(GetSelectedLanguagePair(), QuizQuestionCount);
            _quizIndex = 0;
            _quizScore = 0;
            ShowQuizQuestion();
        }

        /// <summary>
        /// Zeigt die aktuelle Quizfrage mit vier Antwortmöglichkeiten an.
        /// </summary>
        private void ShowQuizQuestion()
        {
            if (_quizIndex >= _quizCards.Count)
            {
                QuizQuestion.Text = "Quiz beendet!";
                QuizFeedback.Text = "Dein Ergebnis: " + _quizScore + "/" + _quizCards.Count + " richtig.";
                QuizScore.Text = "";
                Answer1.Visibility = Visibility.Collapsed;
                Answer2.Visibility = Visibility.Collapsed;
                Answer3.Visibility = Visibility.Collapsed;
                Answer4.Visibility = Visibility.Collapsed;
                NextQuestion.Visibility = Visibility.Collapsed;
                return;
            }

            _quizAnswered = false;
            _currentQuizCard = _quizCards[_quizIndex];
            QuizQuestion.Text = _currentQuizCard.Word;
            QuizScore.Text = "Punkte: " + _quizScore + "/" + _quizCards.Count +
                "  –  Frage " + (_quizIndex + 1) + "/" + _quizCards.Count;
            QuizFeedback.Text = "";
            NextQuestion.Visibility = Visibility.Collapsed;

            List<string> answers = GenerateQuizAnswers(_currentQuizCard);
            Answer1.Content = answers[0];
            Answer2.Content = answers[1];
            Answer3.Content = answers[2];
            Answer4.Content = answers[3];

            Answer1.Visibility = Visibility.Visible;
            Answer2.Visibility = Visibility.Visible;
            Answer3.Visibility = Visibility.Visible;
            Answer4.Visibility = Visibility.Visible;
            Answer1.IsEnabled = true;
            Answer2.IsEnabled = true;
            Answer3.IsEnabled = true;
            Answer4.IsEnabled = true;
        }

        /// <summary>
        /// Generiert vier Antwortmöglichkeiten mit einer korrekten und drei falschen Antworten.
        /// </summary>
        /// <param name="correct">Die korrekte Vokabelkarte.</param>
        /// <returns>Eine gemischte Liste mit vier Antwortmöglichkeiten.</returns>
        private List<string> GenerateQuizAnswers(Flashcard correct)
        {
            var wrong = _flashcards
                .Where(f => f.Translation != correct.Translation)
                .ToList();
            Shuffle(wrong);
            var answers = wrong.Take(3).Select(f => f.Translation).ToList();
            answers.Add(correct.Translation);
            Shuffle(answers);
            return answers;
        }

        /// <summary>
        /// Wird beim Klick auf eine Antwortmöglichkeit aufgerufen.
        /// </summary>
        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            if (_quizAnswered)
                return;

            _quizAnswered = true;
            var button = (Button)sender;
            string selectedAnswer = button.Content.ToString();

            Answer1.IsEnabled = false;
            Answer2.IsEnabled = false;
            Answer3.IsEnabled = false;
            Answer4.IsEnabled = false;

            if (selectedAnswer == _currentQuizCard.Translation)
            {
                _quizScore++;
                QuizFeedback.Text = "Richtig!";
            }
            else
            {
                QuizFeedback.Text = "Falsch! Richtig ist: " + _currentQuizCard.Translation;
            }

            QuizScore.Text = "Punkte: " + _quizScore + "/" + _quizCards.Count +
                "  –  Frage " + (_quizIndex + 1) + "/" + _quizCards.Count;
            NextQuestion.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Wird beim Klick auf den Nächste-Frage-Button aufgerufen.
        /// </summary>
        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            _quizIndex++;
            ShowQuizQuestion();
        }

        /// <summary>
        /// Wird beim Ändern der Sprachpaarauswahl aufgerufen und lädt den Wortschatz neu.
        /// </summary>
        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CardWord == null)
                return;

            LoadVocabulary();
        }

        /// <summary>
        /// Wird beim Ändern der Modusauswahl aufgerufen und wechselt zwischen Karteikarten- und Quizmodus.
        /// </summary>
        private void ModeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FlashcardPanel == null || _flashcards == null)
                return;

            if (ModeSelector.SelectedIndex == 0)
            {
                FlashcardPanel.Visibility = Visibility.Visible;
                QuizPanel.Visibility = Visibility.Collapsed;
                _currentIndex = 0;
                ShowFlashcard();
            }
            else
            {
                FlashcardPanel.Visibility = Visibility.Collapsed;
                QuizPanel.Visibility = Visibility.Visible;
                StartQuiz();
            }
        }

        /// <summary>
        /// Mischt eine Liste zufällig nach dem Fisher-Yates-Verfahren.
        /// </summary>
        /// <typeparam name="T">Der Typ der Listenelemente.</typeparam>
        /// <param name="list">Die zu mischende Liste.</param>
        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
