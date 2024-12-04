using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using nehsanet_app.models;
using nehsanet_app.utilities;
using nehsanet_app.Types;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class Main : ControllerBase
    {
        private readonly ILogger _logger;
      
        public Main(ILogger<Main> logger)
        {
            _logger = logger;
        }

        readonly List<string> quotes =
        [
            "Huzzah!",
            "“You live and learn. At any rate, you live.” - Douglas Adams",
            "“Don't worry. Be happy.” - Bobby McFerrin",
            "“Birds flying high, you know how I feel.” - Nina Simone",
            "“It works on my computer.” - Every Developer Ever",
            "“A learning experience is one of those things that says, 'You know that thing you just did?' Don't do that.” - Douglas Adams",
            "“I may not have gone where I intended to go, but I think I have ended up where I needed to be.” - Douglas Adams",
            "“The quality of any advice anybody has to offer has to be judged against the quality of life they actually lead.” - Douglas Adams",
            "“I refuse to answer that question on the grounds that I don't know the answer.” - Douglas Adams",
            "“I love deadlines. I love the whooshing noise they make as they go by.” - Douglas Adams",
            "“Anything that thinks logically can be fooled by something else that thinks at least as logically as it does.” - Douglas Adams",
            "“Time is an illusion. Lunchtime doubly so” - Douglas Adams",
            "“Life is wasted on the living.” - Douglas Adams",
            "“Don't Panic.” - Douglas Adams",
            "“Don't believe anything you read on the net. Except this. Well, including this, I suppose.” - Douglas Adams",
            "“The impossible often has a kind of integrity to it which the merely improbable lacks.” - Douglas Adams",
            "“When the elevator tries to bring you down, go crazy!” - Prince",
            "“I get knocked down, but I get up again.” - Chumbawamba",
            "“It's nice and quiet, but soon again, starts another big riot.” - Bj&#xf6;rk",
            "“May the swamp be gentle.” - Google Gemini",
            "“Shoulda Coulda Woulda” - Jayne Cobb",
            "“If I want to contact my friends, I have to email them...like I'm a pilgrim!” - Seaside, Oregon Resident",
            "Find win-wins.",
            "“YES!” - Gimli",
            "“I am a large, semi-muscular man. I can take it.” - Wash",
            "“Anyone who has never made a mistake has never tried anything new.” - Albert Einstein",
            "“I believe in a thing called love, hoo-ooh!” - The Darkness",
            "“Kitty at my foot and I wanna touch it.” - The Presidents of the United States",
            "“He cursed like a drunken sailor with a broken leg, but only at his donkeys.” - Kvothe",
            "“I'm an egotistical bastard, and I name all my projects after myself. First Linux, now git.” - Linus Torvalds",
            "“When he steps, his whole foot treads the ground.” - Wilem",
            "Let Bygones be Bygones.",
            "“The only person who never makes mistakes is the person who never does anything.” - Theodore Roosevelt",
            "“You will continue to suffer if you have an emotional reaction to everything that is said to you.” - Warren Buffett",
            "“Breathe and allow things to pass.” - Warren Buffett"
        ];

        readonly List<string> positiveaffirmations =
        [
            "Generous",
            "Loyal",
            "Humorous",
            "Adventurous",
            "Affable",
            "Amicable",
            "Cheerful",
            "Considerate",
            "Diligent",
            "Optimistic",
            "Empathetic",
            "Helpful",
            "Adaptable",
            "Ambitious",
            "Bright",
            "Courageous",
            "Creative",
            "Intuitional",
            "Passionate",
            "Sincere",
            "Wise",
            "Tent Camper (someday pop-up tent camper!)",
            "Neighborhood Rollerblader",
            "Developer",
            "Student",
            "Father",
            "Ambitious",
            "Homo Sapien",
            "Tester",
            "Game-Player",
            "Game-Maker",
            "SDET",
            "Husband",
            "Adventurous",
            "Nice",
            "Skier",
            "Book-Reader",
            "Learner",
            "a <a href=\"https://synthridersvr.com/\">Synth-Rider</a>"
        ];
        public class GeminiClient
        {
            private readonly HttpClient _httpClient;

            public GeminiClient()
            {
                _httpClient = new HttpClient();
            }

            public async Task<string> TalkToGemini(string question, string previousAnswer)
            {
                string output = "";
                await Task.Run(() =>
                {

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "python3.11",
                        Arguments = $"talk.py \"{question}\" \"{previousAnswer}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    using (var process = new Process())
                    {
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                        output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();

                    }
                });
                return output;
            }
        }

        public class GeminiResponse
        {
            public required string GeneratedText { get; set; }
            // Other properties as needed
        }

        [HttpGet]
        [Route("/v1/positiveadjective")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetPositiveAdjective()
        {
            _logger.LogInformation("Enter: GetPositiveAdjective() [GET]");
            int numToReturn = 15;
            int founditems = 0;
            string results = "";
            List<string> items = [];
            while (founditems < numToReturn)
            {
                string item = positiveaffirmations[Random.Shared.Next(positiveaffirmations.Count)];
                if (!items.Contains(item))
                {
                    items.Add(item);
                    founditems++;
                }
            }
            results = string.Join(", ", items);
            results += ". A sm&ouml;rg&aring;sbord of a human!";
            dynamic jsonresults = JsonSerializer.Serialize(results);
            _logger.LogInformation($"Exit: GetPositiveAdjective(): results: ${jsonresults}");
            return jsonresults;
        }

        [HttpGet]
        [Route("/v1/getweather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetWeather([FromQuery] string city, [FromQuery] string units, [FromQuery] string weatherType)
        {
            _logger.LogInformation("Enter: GetWeather() [GET]: " + city + " " + units + " " + weatherType);

            // check city
            if (string.IsNullOrEmpty(city))
                throw new ArgumentNullException(nameof(city), "City is required.");

            // check units
            if (string.IsNullOrEmpty(units))
                units = "imperial";
            else
                units = units.ToLower();

            // check weatherType
            if (string.IsNullOrEmpty(weatherType))
                weatherType = "full";
            weatherType = weatherType.ToLower();

            string urlstem;
            switch (weatherType.ToLower())
            {
                case "words":
                    urlstem = $"weather_description?city={city}&units={units}";
                    break;
                case "temperature":
                    urlstem = $"weather_temp?city={city}&units={units}";
                    break;
                case "full":
                    urlstem = $"weather_all?city={city}&units={units}";
                    break;
                case "ascii":
                    urlstem = $"weather_acsii?city={city}&units={units}";
                    break;
                case "emoji":
                    urlstem = $"weather_emoji?city={city}&units={units}";
                    break;
                default:
                    urlstem = $"weather_all?city={city}&units={units}";
                    break;
            }

            string url = $"http://192.168.68.105:8080/{urlstem}";
            string content = "";
            _logger.LogInformation($"GetWeather url: ${url}");
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    content = await response.Content.ReadAsStringAsync();
                else
                    content = "City not found.";
            }
            dynamic jsonresults = JsonSerializer.Serialize(content);
            _logger.LogInformation($"Exit: GetWeather(): results: ${jsonresults}");
            return jsonresults;
        }

        [HttpGet]
        [Route("/v1/Scraper")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> Scraper([FromQuery] string scrapeUrl)
        {
            _logger.LogInformation("Enter: Scraper() [GET]: " + scrapeUrl);

            // check city
            if (string.IsNullOrEmpty(scrapeUrl))
                throw new ArgumentNullException(nameof(scrapeUrl), "url is required.");

            string urlstem = $"scraper?url={scrapeUrl}";
            string url = $"http://192.168.68.105:8081/{urlstem}";
            string content = "";
            _logger.LogInformation($"Scraper url: ${url}");
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    content = await response.Content.ReadAsStringAsync();
                else
                    content = "Scape data not found.";
            }
            dynamic jsonresults = JsonSerializer.Serialize(content);
            _logger.LogInformation($"Exit: Scraper(): results: ${jsonresults}");
            return jsonresults;
        }

        [HttpPost]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string UpdateName(NameType namePerson)
        {
            _logger.LogInformation("Enter: UpdateName() [POST]");
            dynamic results = JsonSerializer.Serialize<string>("Not Implemented yet but you sent: " + namePerson.Name);
            _logger.LogInformation($"Exit: UpdateName(): results: ${JsonSerializer.Serialize(results)}");
            return results;
        }

        [HttpPost]
        [Route("/v1/ai")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CometAI(AIQuestion aiQuestion)
        {
            _logger.LogInformation("Enter: ai() [POST]");
            var client = new GeminiClient();
            var result = "";

            try
            {
                _logger.LogInformation("ai() - sending request to Gemini: " + aiQuestion.Question);
                result = await client.TalkToGemini(aiQuestion.Question, aiQuestion.PreviousAnswer);
                result = result.Replace("\n", " ");
                _logger.LogInformation("ai() - received response from Gemini: " + result);
                aiQuestion.Answer = result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
            }

            _logger.LogInformation($"Exit: ai(): aiQuestion: ${JsonSerializer.Serialize(aiQuestion)}");
            if (result != null)
            {
                return Ok(aiQuestion);
            }
            else
            {
                return BadRequest("No response from Gemini");
            }
        }

        [HttpGet]
        [Route("/v1/quote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetQuote()
        {
            _logger.LogInformation("Enter: GetQuote()");
            dynamic results = JsonSerializer.Serialize<string>(quotes[Random.Shared.Next(quotes.Count)]);
            _logger.LogInformation($"Exit: GetQuote(): results: ${JsonSerializer.Serialize(results)}");
            return results;
        }

        [HttpGet]
        [Route("/v1/related")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetRelated([FromServices] MySqlDataSource db, [FromQuery] string page)
        {
            _logger?.LogInformation("Enter: related [GET]");
            var connection = new RelatedPagesUtility(db, _logger);
            List<RelatedPages> db_result = await connection.GetRelatedPages(page);
            dynamic results = JsonSerializer.Serialize(db_result);
            _logger?.LogInformation($"Exit: related: results: ${results}");
            return results;
        }

        [HttpGet]
        [Route("/v1/dbhealth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetDBHealth([FromServices] MySqlDataSource db)
        {
            _logger?.LogInformation("Enter: GetDBHealth()");
            var connection = new CommentsUtility(db, _logger);
            var db_result = await connection.GetLastXCommentsByPage("this_website", 1);
            dynamic results = JsonSerializer.Serialize(db_result);
            _logger?.LogInformation($"Exit: GetDBHealth(): results: ${JsonSerializer.Serialize(results)}");
            return results;
        }
    }
}