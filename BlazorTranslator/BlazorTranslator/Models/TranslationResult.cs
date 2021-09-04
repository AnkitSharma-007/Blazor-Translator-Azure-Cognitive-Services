namespace BlazorTranslator.Models
{
    public class TranslationResult
    {
        public DetectedLanguage DetectedLanguage { get; set; }
        public Translation[] Translations { get; set; }
    }
}