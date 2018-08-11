using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DocAPI.Models;
using DocAPI.Controllers;
using DocAPI.Models.TextAPI;
namespace XUnitTestDocAPI
{
    public class TestTextSearch
    {
        [Fact]
        public void EuropePmcSearch_SourceFound()
        {
            Boolean assertion = false;
            List<string> origin = new List<string>();

            origin.Add("Tonight, museums across Europe will take part in the Long Night of Museums");
            DocumentStatistics documentStatistics = new DocumentStatistics(origin);
            EuropaPMCSearch europaPMCSearch = new EuropaPMCSearch(new Uri("https://www.ebi.ac.uk/europepmc/webservices/rest/search?"));
            europaPMCSearch.Check(documentStatistics);
            foreach (Plagiat<String> source in documentStatistics.getPossiblePlagiates())
            {
                if (source.Source == "http://europepmc.org/articles/PMC1298459?pdf=render")
                {
                    assertion = true;
                }
            }
            Assert.True(assertion);
        }
        [Fact]
        public void EuropePmcSearch_NoResult()
        {
            Boolean assertion = false;
            List<string> origin = new List<string>();

            origin.Add("Asd asd ASd jasdfnaLasddasda");
            DocumentStatistics documentStatistics = new DocumentStatistics(origin);
            EuropaPMCSearch europaPMCSearch = new EuropaPMCSearch(new Uri("https://www.ebi.ac.uk/europepmc/webservices/rest/search?"));
            europaPMCSearch.Check(documentStatistics);

            if (documentStatistics.getPossiblePlagiates().Count == 0)
            {
                assertion = true;
            }
            Assert.True(assertion);
        }

        [Fact]
        public void GoogleCustomSearch_SourceFound()
        {
            Boolean assertion = false;
            List<string> origin = new List<string>();

            origin.Add("Platons Lehrer Sokrates disku­tiert mit dem berühm­ten Redner Gorgias von Leontinoi, nach dem der Dialog benannt ist, sowie dessen Schüler Polos und dem vorneh­men Athener Kallikles.");
            DocumentStatistics documentStatistics = new DocumentStatistics(origin);
            GoogleSearch googleSearch = new GoogleSearch(new Uri("https://www.googleapis.com/customsearch/v1"));
            googleSearch.Check(documentStatistics);
            foreach (Plagiat<String> source in documentStatistics.getPossiblePlagiates())
            {
                if (source.Source == "https://de.wikipedia.org/wiki/Gorgias_(Platon)")
                {
                    assertion = true;
                }
            }
            Assert.True(assertion);
        }
        [Fact]
        public void GoogleCustomSearch_NoResult()
        {
            Boolean assertion = false;
            List<string> origin = new List<string>();

            origin.Add("ASDDDDDDDDDASDAAAAAAAAAAAAAAAAAAAasdaskffsafak");
            DocumentStatistics documentStatistics = new DocumentStatistics(origin);
            GoogleSearch googleSearch = new GoogleSearch(new Uri("https://www.googleapis.com/customsearch/v1"));
            googleSearch.Check(documentStatistics);
            if (documentStatistics.getPossiblePlagiates().Count == 0)
            {
                assertion = true;
            }
            Assert.True(assertion);
        }

        [Fact]
        public void Search_BothSourcesFound()
        {
            Boolean assertion = false;
            Boolean wikipediaFound = false;
            Boolean europaPmcFound = false;
            List<string> origin = new List<string>();

            origin.Add("Tonight, museums across Europe will take part in the Long Night of Museums");
            origin.Add("Platons Lehrer Sokrates disku­tiert mit dem berühm­ten Redner Gorgias von Leontinoi, nach dem der Dialog benannt ist, sowie dessen Schüler Polos und dem vorneh­men Athener Kallikles.");
            AzureTextAnalytics azureTextAnalytics = new AzureTextAnalytics();

            foreach (Plagiat<String> source in azureTextAnalytics.Check(origin, null, null))
            {
                if (source.Source == "https://de.wikipedia.org/wiki/Gorgias_(Platon)")
                {
                    wikipediaFound = true;
                }
                else if (source.Source == "http://europepmc.org/articles/PMC1298459?pdf=render")
                {
                    europaPmcFound = true;
                }
            }
            if (europaPmcFound && wikipediaFound)
            {
                assertion = true;
            }
            Assert.True(assertion);
        }
    }
}
