using System;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Collections.Generic;
using DocAPI.Models.TextAPI;

namespace DocAPI.Models.TextAPI
{
    public class AzureTextAnalytics : ITextAPI
    {
        public Uri API { get; set; }

        /// <summary>
        /// basic constructor
        /// </summary>
        public AzureTextAnalytics()
        {
        }

        /// <summary>
        /// Analyzes all the given sentences and queries them against the different apis
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="URLsources"></param>
        /// <param name="Sources"></param>
        /// <returns></returns>
        public List<Plagiat<string>> Check(List<string> origin, List<Uri> URLsources, List<String> Sources)
        {
            //google search with custom search
            GoogleSearch googleSearch = new GoogleSearch(new Uri("https://www.googleapis.com/customsearch/v1"));

            //europa pmc search for academic literatur in life science
            EuropaPMCSearch europaPMCSearch = new EuropaPMCSearch(new Uri("https://www.ebi.ac.uk/europepmc/webservices/rest/search?"));

            //create document statistics
            DocumentStatistics documentStatistics = new DocumentStatistics(origin);
            googleSearch.Check(documentStatistics);
            europaPMCSearch.Check(documentStatistics);

            //starting azure congitive services to interpret sentence
            // Create a client
            ITextAnalyticsAPI client = new TextAnalyticsAPI();
            client.AzureRegion = AzureRegions.Westeurope;
            client.SubscriptionKey = "<placekey>";

            // Extracting language
            LanguageBatchResult languagesDetected = client.DetectLanguage(
                    new BatchInput(documentStatistics.getBatchInput())
                    );

            //store results
            foreach (var document in languagesDetected.Documents)
            {
                documentStatistics.updateSentenceLanguage(document.Id, document.DetectedLanguages[0].Iso6391Name);
            }

            // Getting key-phrases
            KeyPhraseBatchResult keyPhares = client.KeyPhrases(
                    new MultiLanguageBatchInput(documentStatistics.getMultiLanguageBatchInput())
                    );

            // Printing keyphrases
            foreach (var document in keyPhares.Documents)
            {
                documentStatistics.updateKeyPhares(document.Id, (List<string>)document.KeyPhrases);
            }

            return documentStatistics.getPossiblePlagiates();
        }

    }
}