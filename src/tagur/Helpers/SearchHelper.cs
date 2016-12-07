using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tagur.Models;

namespace tagur.Helpers
{
    public static class SearchHelper
    {
        public async static Task<List<SearchResultInformation>> SearchAsync(string searchQuery)
        {
            List<SearchResultInformation> searchResults = new List<SearchResultInformation>();

            SearchServiceClient serviceClient = new SearchServiceClient("tagur", new SearchCredentials(SearchConstants.SearchApiKey));

            var sp = new SearchParameters();           

            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient("tags");
            DocumentSearchResult response = await indexClient.Documents.SearchAsync(searchQuery, sp);

            var results = response.Results;
            var next = results.Select(s => s.Document).Where(w => w.ContainsKey("id"));

            searchResults = (from result in next
                             select new SearchResultInformation()
                             {
                                 Id = new Guid((string)result["id"]),
                                 Caption = ((string)result["caption"] + "").ToFirstCharUpper(),
                                 Tags = ((string[])result["tags"]).ToList(),

                             }).ToList();
            
            return searchResults;
        }

        public async static Task<IEnumerable<string>> GetSuggestionsAsync(string searchQuery)
        {
            List<string> suggestions = new List<string>();

            SearchServiceClient serviceClient = new SearchServiceClient(SearchConstants.SearchServiceName, new SearchCredentials(SearchConstants.SearchApiKey));

            var sp = new SearchParameters();

            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(SearchConstants.SearchIndexName);
            DocumentSearchResult response = await indexClient.Documents.SearchAsync(searchQuery.Trim() + "*", sp);

            var results = response.Results.Select(s => s.Document).ToList();
            
            foreach(var result in results)
            {               
                suggestions.AddRange((string[])result["tags"]);
            }

            return suggestions.Where(w => w.StartsWith(searchQuery, StringComparison.OrdinalIgnoreCase)).Distinct();
        }

       
    }
}
