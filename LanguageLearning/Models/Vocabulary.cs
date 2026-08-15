using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageLearning.Models
{
    /// <summary>
    /// Stellt statische Methoden für den Zugriff auf Vokabeln unterschiedlicher Sprachpaare bereit.
    /// </summary>
    public sealed class Vocabulary
    {
        private static readonly Random Rng = new Random();

        /// <summary>
        /// Gibt alle verfügbaren Vokabelkarten für alle Sprachpaare zurück.
        /// </summary>
        /// <returns>Eine Liste aller Vokabelkarten.</returns>
        public static List<Flashcard> GetAllFlashcards()
        {
            var cards = new List<Flashcard>();

            cards.AddRange(BuildSet("Englisch-Deutsch", "Essen",
                new[] { "apple", "bread", "water", "milk", "cheese" },
                new[] { "Apfel", "Brot", "Wasser", "Milch", "Käse" }));

            cards.AddRange(BuildSet("Englisch-Deutsch", "Reisen",
                new[] { "train", "airport", "hotel", "ticket", "map" },
                new[] { "Zug", "Flughafen", "Hotel", "Ticket", "Karte" }));

            cards.AddRange(BuildSet("Englisch-Deutsch", "Zahlen",
                new[] { "one", "two", "three", "four", "five" },
                new[] { "eins", "zwei", "drei", "vier", "fünf" }));

            cards.AddRange(BuildSet("Englisch-Deutsch", "Begrüßung",
                new[] { "hello", "goodbye", "thank you", "please", "yes" },
                new[] { "Hallo", "Auf Wiedersehen", "Danke", "Bitte", "Ja" }));

            cards.AddRange(BuildSet("Englisch-Deutsch", "Farben",
                new[] { "red", "blue", "green", "yellow", "white" },
                new[] { "rot", "blau", "grün", "gelb", "weiß" }));

            cards.AddRange(BuildSet("Spanisch-Deutsch", "Essen",
                new[] { "manzana", "pan", "agua", "leche", "queso" },
                new[] { "Apfel", "Brot", "Wasser", "Milch", "Käse" }));

            cards.AddRange(BuildSet("Spanisch-Deutsch", "Reisen",
                new[] { "tren", "aeropuerto", "hotel", "billete", "mapa" },
                new[] { "Zug", "Flughafen", "Hotel", "Ticket", "Karte" }));

            cards.AddRange(BuildSet("Spanisch-Deutsch", "Zahlen",
                new[] { "uno", "dos", "tres", "cuatro", "cinco" },
                new[] { "eins", "zwei", "drei", "vier", "fünf" }));

            cards.AddRange(BuildSet("Spanisch-Deutsch", "Begrüßung",
                new[] { "hola", "adiós", "gracias", "por favor", "sí" },
                new[] { "Hallo", "Auf Wiedersehen", "Danke", "Bitte", "Ja" }));

            cards.AddRange(BuildSet("Spanisch-Deutsch", "Farben",
                new[] { "rojo", "azul", "verde", "amarillo", "blanco" },
                new[] { "rot", "blau", "grün", "gelb", "weiß" }));

            cards.AddRange(BuildSet("Französisch-Deutsch", "Essen",
                new[] { "pomme", "pain", "eau", "lait", "fromage" },
                new[] { "Apfel", "Brot", "Wasser", "Milch", "Käse" }));

            cards.AddRange(BuildSet("Französisch-Deutsch", "Reisen",
                new[] { "train", "aéroport", "hôtel", "billet", "carte" },
                new[] { "Zug", "Flughafen", "Hotel", "Ticket", "Karte" }));

            cards.AddRange(BuildSet("Französisch-Deutsch", "Zahlen",
                new[] { "un", "deux", "trois", "quatre", "cinq" },
                new[] { "eins", "zwei", "drei", "vier", "fünf" }));

            cards.AddRange(BuildSet("Französisch-Deutsch", "Begrüßung",
                new[] { "bonjour", "au revoir", "merci", "s'il vous plaît", "oui" },
                new[] { "Hallo", "Auf Wiedersehen", "Danke", "Bitte", "Ja" }));

            cards.AddRange(BuildSet("Französisch-Deutsch", "Farben",
                new[] { "rouge", "bleu", "vert", "jaune", "blanc" },
                new[] { "rot", "blau", "grün", "gelb", "weiß" }));

            return cards;
        }

        /// <summary>
        /// Gibt die Vokabelkarten für das angegebene Sprachpaar zurück.
        /// </summary>
        /// <param name="languagePair">Das Sprachpaar, z. B. "Englisch-Deutsch".</param>
        /// <returns>Eine Liste der Vokabelkarten für das Sprachpaar.</returns>
        public static List<Flashcard> GetFlashcardsByLanguagePair(string languagePair)
        {
            return GetAllFlashcards().Where(f => f.LanguagePair == languagePair).ToList();
        }

        /// <summary>
        /// Gibt eine zufällige Auswahl an Vokabelkarten für den Quizmodus zurück.
        /// </summary>
        /// <param name="languagePair">Das Sprachpaar.</param>
        /// <param name="count">Die Anzahl der gewünschten Karten.</param>
        /// <returns>Eine zufällig sortierte Liste von Vokabelkarten.</returns>
        public static List<Flashcard> GetRandomFlashcards(string languagePair, int count)
        {
            var cards = GetFlashcardsByLanguagePair(languagePair);
            var shuffled = cards.ToList();
            Shuffle(shuffled);
            return shuffled.Take(Math.Min(count, shuffled.Count)).ToList();
        }

        /// <summary>
        /// Erzeugt eine Liste von Vokabelkarten aus Wort- und Übersetzungsarrays.
        /// </summary>
        /// <param name="languagePair">Das Sprachpaar.</param>
        /// <param name="category">Die Kategorie der Vokabeln.</param>
        /// <param name="words">Die Wörter in der Fremdsprache.</param>
        /// <param name="translations">Die dazugehörigen Übersetzungen.</param>
        /// <returns>Eine Aufzählung von Vokabelkarten.</returns>
        private static IEnumerable<Flashcard> BuildSet(string languagePair, string category,
            string[] words, string[] translations)
        {
            for (int i = 0; i < words.Length; i++)
            {
                yield return new Flashcard
                {
                    Word = words[i],
                    Translation = translations[i],
                    Category = category,
                    LanguagePair = languagePair,
                    IsKnown = false
                };
            }
        }

        /// <summary>
        /// Mischt eine Liste zufällig nach dem Fisher-Yates-Verfahren.
        /// </summary>
        /// <typeparam name="T">Der Typ der Listenelemente.</typeparam>
        /// <param name="list">Die zu mischende Liste.</param>
        private static void Shuffle<T>(List<T> list)
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
