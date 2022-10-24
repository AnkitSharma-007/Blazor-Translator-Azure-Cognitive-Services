using BlazorTranslator.Data;
using BlazorTranslator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorTranslator.Pages
{
    public class TranslatorBase : ComponentBase
    {
        [Inject]
        protected TranslationService translationService { get; set; }

        TranslationResult[] translations;
        AvailableLanguage availableLanguages;

        protected string outputLanguage { get; set; }
        protected string inputLanguage { get; set; }
        protected string inputText { get; set; }
        protected string outputText { get; set; }
        protected Dictionary<string, LanguageDetails> LanguageList = new Dictionary<string, LanguageDetails>();

        protected override async Task OnInitializedAsync()
        {
            availableLanguages = await translationService.GetAvailableLanguages();
         
            LanguageList = availableLanguages.Translation;
            Console.WriteLine("count {0}",LanguageList.Count);
        }

        protected void SelectLanguage(ChangeEventArgs langEvent)
        {
            outputLanguage = langEvent.Value.ToString();
        }

        protected async Task Translate()
        {
            if (!string.IsNullOrEmpty(outputLanguage))
            {
                translations = await translationService.GetTranslatation(this.inputText, this.outputLanguage);

                if (translations != null)
                {
                    outputText = translations[0].Translations[0].Text;
                    inputLanguage = translations[0].DetectedLanguage.Language;
                }
                else
                {
                    outputText = "An error occurred in translation. Please try again.";
                }
            }
        }
    }
}
