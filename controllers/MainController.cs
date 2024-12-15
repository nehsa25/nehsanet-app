using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using nehsanet_app.Models;
using nehsanet_app.utilities;
using nehsanet_app.Types;
using nehsanet_app.Services;
using nehsanet_app.db;
using static nehsanet_app.utilities.ControllerUtility;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class Main(ILoggingProvider logger, DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly ILoggingProvider _logger = logger;

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
            "[object Object]",
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

        [HttpGet]
        [Route("/v1/positiveadjective")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetPositiveAdjective()
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                await Task.Run(() =>
                {
                    _logger.Log("Enter: GetPositiveAdjective() [GET]");
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
                    response.Data = JsonSerializer.Serialize(results);
                });
            }
            catch (Exception e)
            {
                _logger.Log(e, "GetPositiveAdjective");
            }

            _logger.Log($"Exit: GetPositiveAdjective(): response: ${JsonSerializer.Serialize(response)}");
            return response;
        }

        [HttpGet]
        [Route("/v1/getweather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetWeather([FromQuery] string city, [FromQuery] string units, [FromQuery] string weatherType)
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                _logger.Log("Enter: GetWeather() [GET]: " + city + " " + units + " " + weatherType);

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
                _logger.Log($"GetWeather url: ${url}");
                using (var client = new HttpClient())
                {
                    var data = await client.GetAsync(url);
                    if (data.IsSuccessStatusCode)
                        response.Data = await data.Content.ReadAsStringAsync();
                    else
                        response.Data = "City not found.";
                }
                response.Data = JsonSerializer.Serialize(response.Data);
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.Log(e, "GetWeather");
            }

            _logger.Log($"Exit: GetWeather(): response: ${JsonSerializer.Serialize(response)}");
            return response;
        }

        [HttpGet]
        [Route("/v1/Scraper")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> Scraper([FromQuery] string scrapeUrl)
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                _logger.Log("Enter: Scraper() [GET]: " + scrapeUrl);

                // check city
                if (string.IsNullOrEmpty(scrapeUrl))
                    throw new ArgumentNullException(nameof(scrapeUrl), "url is required.");

                string urlstem = $"scraper?url={scrapeUrl}";
                string url = $"http://192.168.68.105:8081/{urlstem}";
                string content = "";
                _logger.Log($"Scraper url: ${url}");
                using (var client = new HttpClient())
                {
                    var data = await client.GetAsync(url);
                    if (data.IsSuccessStatusCode)
                        response.Data = await data.Content.ReadAsStringAsync();
                    else
                        response.Data = "Scape data not found.";
                }
                response.Data = JsonSerializer.Serialize(content);
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.Log(e, "Scraper");
            }

            _logger.Log($"Exit: Scraper(): response: ${JsonSerializer.Serialize(response)}");
            return response;
        }

        [HttpPost]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> UpdateName(NameType namePerson)
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                await Task.Run(() =>
                {
                    _logger.Log("Enter: UpdateName() [POST]");
                    response.Data = JsonSerializer.Serialize<string>("Not Implemented yet but you sent: " + namePerson.Name);
                    response.Success = true;
                });
            }
            catch (Exception e)
            {
                _logger.Log(e, "UpdateName");
            }

            _logger.Log($"Exit: UpdateName(): response: ${JsonSerializer.Serialize(response)}");
            return response;
        }

        [HttpGet]
        [Route("/v1/quote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetQuote()
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                await Task.Run(() =>
                {
                    _logger.Log("Enter: GetQuote()");
                    response.Data = JsonSerializer.Serialize<string>(quotes[Random.Shared.Next(quotes.Count)]);
                    response.Success = true;
                });
            }
            catch (Exception e)
            {
                _logger.Log(e, "GetQuote");
            }

            _logger.Log($"Exit: GetQuote(): response: ${JsonSerializer.Serialize(response)}");
            return response;
        }

        [HttpGet]
        [Route("/v1/related")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetRelated([FromQuery] string page)
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                _logger.Log("Enter: GetRelated [GET]");
                var connection = new RelatedPagesUtility(_context, _logger);
                List<Page> db_result = await connection.GetRelatedPages(page);
                _logger.Log($"GetRelated: Found {db_result.Count} related pages.");
                response.Data = JsonSerializer.Serialize(db_result);
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.Log(e, "GetRelated");
            }

            _logger.Log($"Exit: GetRelated(): response: ${JsonSerializer.Serialize(response)}");
            return response;
        }

        [HttpGet]
        [Route("/v1/dbhealth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetDBHealth()
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                _logger.Log("Enter: GetDBHealth()");
                await _context.CheckConnection();
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.Log(e, "DBHealth");
            }

            _logger.Log($"Exit: GetDBHealth(): response: ${JsonSerializer.Serialize(response)}");
            return response;
        }
    }
}