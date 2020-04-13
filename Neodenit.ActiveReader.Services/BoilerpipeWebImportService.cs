using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.ViewModels;
using Newtonsoft.Json;

namespace Neodenit.ActiveReader.Services
{
    public class BoilerpipeWebImportService : IImportService
    {
        private readonly IHttpClientService httpClientWrapper;

        private readonly Dictionary<string, string> replacements = new Dictionary<string, string>
        {
            { "\n", Environment.NewLine + Environment.NewLine },
            { " ,", "," },
            { " .", "." },
            { " !", "!" },
            { " ?", "?" }
        };

        public BoilerpipeWebImportService(IHttpClientService httpClientWrapper)
        {
            this.httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));
        }

        public async Task<ImportArticleViewModel> GetTextAndTitleAsync(string escapedUrl)
        {
            var fullUrl = $"https://boilerpipe-web.appspot.com/extract?url={escapedUrl}&output=json";

            var uri = new Uri(fullUrl);
            var response = await httpClientWrapper.GetAsync(uri);
            var jsonString = await response.Content.ReadAsStringAsync();

            var json = JsonConvert.DeserializeAnonymousType(jsonString, new
            {
                response = new
                {
                    title = string.Empty,
                    content = string.Empty
                }
            });

            var title = json.response.title;
            var text = json.response.content;

            var formattedText = replacements.Aggregate(text, (s, r) =>
                s.Replace(r.Key, r.Value));

            var result = new ImportArticleViewModel { Title = title, Text = formattedText };
            return result;
        }
    }
}
