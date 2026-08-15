namespace LanguageLearning.Models
{
    /// <summary>
    /// Stellt eine einzelne Vokabelkarte dar.
    /// </summary>
    public sealed class Flashcard
    {
        /// <summary>
        /// Ruft das Wort in der Fremdsprache ab oder legt es fest.
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Ruft die Übersetzung in der Muttersprache ab oder legt sie fest.
        /// </summary>
        public string Translation { get; set; }

        /// <summary>
        /// Ruft die Kategorie der Vokabel ab oder legt sie fest.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Ruft das Sprachpaar ab oder legt es fest.
        /// </summary>
        public string LanguagePair { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob die Vokabel bereits bekannt ist, oder legt ihn fest.
        /// </summary>
        public bool IsKnown { get; set; }
    }
}
