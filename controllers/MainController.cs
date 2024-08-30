
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using nehsanet_app.Models;
using nehsanet_app.Types;
using nehsanet_app.Secrets;
using System.Net;
using System.Text.Encodings.Web;

namespace nehsanet_app.Controllers
{
    [ApiController]
    [Route("/")]
    public class Main : ControllerBase
    {
        private readonly ILogger _logger;

        List<NameAbout> names = new();

        public Main(ILogger<Main> logger)
        {
            _logger = logger;
            names.Add(
                new(
                    "Kvothe",
                    "[You] have stolen princesses back from sleeping barrow kings. [You] burned down the town of Trebon. [You] have spent the night with Felurian and left with both my sanity and my life. I was expelled from the University at a younger age than most people are allowed in. I tread paths by moonlight that others fear to speak of during day. [you] have talked to Gods, loved women, and written songs that make the minstrels weep. ― Patrick Rothfuss, The Name of the Wind"
                )
            );
            names.Add(
                new(
                    "Wilem",
                    "You're a taciturn individual!"
                )
            );
            names.Add(
                new(
                    "Sanya",
                    "Wield Esperacchius with honor!"
                )
            );
            names.Add(
                new(
                    "Maximus",
                    "You're my cat. I love you."
                )
            );
            names.Add(
                new(
                    "Mab",
                    "You keep your bargains, My Queen."
                )
            );
            names.Add(
                new(
                    "Elodin",
                    "You'll go to the Crockery if YOU'RE A RAVEN!"
                )
            );
            names.Add(
                new(
                    "Dresden",
                    "Forzare!"
                )
            );
            names.Add(
                new(
                    "Perrin",
                    "Howl!"
                )
            );
            names.Add(new("Butters", "POLKA will never die!"));
            names.Add(new("Joe", ""));
            names.Add(new("Emma", ""));
            names.Add(new("Isabella", ""));
            names.Add(new("Akari", ""));
            names.Add(new("Gorg", ""));
            names.Add(new("Tom", ""));
            names.Add(new("Shane", ""));
            names.Add(new("Shuri", ""));
            names.Add(new("Thomas", ""));
            names.Add(new("Daphne", ""));
            names.Add(new("Felicity", ""));
            names.Add(new("Bonnie", ""));
            names.Add(new("Tabs", ""));
            names.Add(new("Dot", ""));
            names.Add(new("Ambrose", ""));
            names.Add(new("Crossen", ""));
            names.Add(new("Dunstan", ""));
            names.Add(new("Bink", ""));
            names.Add(new("Ivar", ""));
            names.Add(new("Ivan", ""));
            names.Add(new("Beatrice", "Subaru Subaru Subaru!"));
            names.Add(new("Subaru", ""));
            names.Add(new("Roswaal", ""));
            names.Add(new("Ashen", ""));
            names.Add(new("Sigrid", ""));
            names.Add(new("Renkath", ""));
            names.Add(new("Kelsek", ""));
            names.Add(new("Ash", ""));
            names.Add(new("Jay", ""));
            names.Add(new("Bob", ""));
            names.Add(new("Fred", ""));
            names.Add(new("Mike", ""));
            names.Add(new("James", ""));
            names.Add(new("Jones", ""));
            names.Add(new("Tim", ""));
            names.Add(new("Timm", ""));
            names.Add(new("Harry", "The Burger King!"));
            names.Add(new("John", ""));
            names.Add(new("Jack", ""));
            names.Add(new("May", ""));
            names.Add(new("Sally", ""));
            names.Add(new("Candie", ""));
            names.Add(new("Jesse", "YOU ARE ME! AM I YOU?"));
            names.Add(new("Ethan", ""));
        }

        readonly List<string> quotes =
        [
            "“You live and learn. At any rate, you live.” - Douglas Adams",
            "“It works on my computer.” - Every Developer Ever",
            "“A learning experience is one of those things that says, 'You know that thing you just did? Don't do that.’” - Douglas Adams",
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
            "“But just because you're paranoid doesn't mean there isn't an invisible demon about to eat your face.” - Harry Dresden",
            "“It's nice and quiet, but soon again, starts another big riot.” - Bj&#xf6;rk",
            "“May the swamp be gentle.” - Google Gemini",
            "“Shoulda Coulda Woulda” - Jayne Cobb",
            "Find win-wins.",
            "“I believe in a thing called love, hoo-ooh!” - The Darkness",
            "“Kitty at my foot and I wanna touch it.” - The Presidents of the United States",
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
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetName()
        {
            _logger.LogInformation("Enter: GetName() [GET]");
            dynamic results = JsonSerializer.Serialize(names[Random.Shared.Next(names.Count)]);
            _logger.LogInformation($"Exit: GetQuote(): results: ${JsonSerializer.Serialize(results)}");
            return results;
        }

        [HttpGet]
        [Route("/v1/name/{numToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetNames(int numToReturn)
        {
            _logger.LogInformation("Enter: names() [GET]");
            int founditems = 0;
            List<dynamic> items = [];
            while (founditems < numToReturn)
            {
                dynamic item = names[Random.Shared.Next(names.Count)];
                if (!items.Contains(item))
                {
                    items.Add(item);
                    founditems++;
                }
            }
            dynamic jsonresults = JsonSerializer.Serialize(items);
            _logger.LogInformation($"Exit: names(): results: ${jsonresults}");
            return jsonresults;
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
        [Route("/v1/scaper")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> Scaper([FromQuery] string scrapeUrl)
        {
            _logger.LogInformation("Enter: Scaper() [GET]: " + scrapeUrl);

            // check city
            if (string.IsNullOrEmpty(scrapeUrl))
                throw new ArgumentNullException(nameof(scrapeUrl), "url is required.");

            string urlstem = $"scraper?url={scrapeUrl}";
            string url = $"http://192.168.68.105:8081/{urlstem}";
            string content = "";
            _logger.LogInformation($"Scaper url: ${url}");
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    content = await response.Content.ReadAsStringAsync();
                else
                    content = "Scape data not found.";
            }
            dynamic jsonresults = JsonSerializer.Serialize(content);
            _logger.LogInformation($"Exit: Scaper(): results: ${jsonresults}");
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
        [Route("/v1/comment/{page}/{numberToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetComment([FromServices] MySqlDataSource db, string page, int numberToReturn = 5)
        {
            _logger?.LogInformation("Enter: GetComment/id [GET]");
            var connection = new CommentsRepository(db, _logger);
            List<CommentPost> db_result = (List<CommentPost>)await connection.GetLastXCommentsByPage(page, numberToReturn);
            dynamic results = JsonSerializer.Serialize(db_result);
            _logger?.LogInformation($"Exit: GetComment/id: results: ${results}");
            return results;
        }

        [HttpPost]
        [Route("/v1/comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> PostComment([FromServices] MySqlDataSource db, [FromBody] CommentPost commentPost)
        {
            _logger?.LogInformation("Enter: PostComment [POST]");
            var connection = new CommentsRepository(db, _logger);
            if (HttpContext.Connection.RemoteIpAddress != null)
                commentPost.ip = HttpContext.Connection.RemoteIpAddress.ToString();
            await connection.AddComment(commentPost);
            dynamic results = JsonSerializer.Serialize<string>("OK");
            _logger?.LogInformation($"Exit: PostComment: results: ${results}");
            return results;
        }

        [HttpGet]
        [Route("/v1/dbhealth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetDBHealth([FromServices] MySqlDataSource db)
        {
            _logger?.LogInformation("Enter: GetDBHealth()");
            var connection = new CommentsRepository(db, _logger);
            var db_result = await connection.GetLastXCommentsByPage("this_website", 1);
            dynamic results = JsonSerializer.Serialize(db_result);
            _logger?.LogInformation($"Exit: GetDBHealth(): results: ${JsonSerializer.Serialize(results)}");
            return results;
        }
    }
}