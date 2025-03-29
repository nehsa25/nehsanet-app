using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using nehsanet_app.utilities;
using nehsanet_app.Types;
using nehsanet_app.db;
using nehsanet_app.Models;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class Main(ILogger<Main> logger, DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly ILogger _logger = logger;

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
            "“Breathe and allow things to pass.” - Warren Buffett",
            "“Let everything happen to you: beauty and terror. Just keep going. No feeling is final.” - Rainer Maria Rilke",
            "“Take me as I come 'cause I can't stay long.” - Tom Petty and the Heartbreakers",
            "rm -recurse ./node_modules; rm -recurse ./dist; rm ./package-lock.json; npm install; tsc",
            "The little things we get mad about are like snowflakes on the mountains, and we if wait too long then we're just one sneeze away from an avalanche that'll kill us all.” - Will Kitman",
            "Go fix yourself, instead of someone else.” - Placebo",
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
            "Camper",
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
            "Learner"
        ];

        [HttpGet]
        [Route("/v1/positiveadjective")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> GetPositiveAdjective()
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                await Task.Run(() =>
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
                    results += ".";
                    response.Data = JsonSerializer.Serialize(results);
                });
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetPositiveAdjective");
            }

            _logger.LogInformation("Exit: GetPositiveAdjective(): response: {r}", JsonSerializer.Serialize(response));
            return response;
        }

        [HttpGet]
        [Route("/v1/getweather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> GetWeather([FromQuery] string city, [FromQuery] string units, [FromQuery] string weatherType)
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                _logger.LogInformation("Enter: GetWeather() [GET]: {city}, {units}, {weatherType}", city, units, weatherType);

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

                string url = $"http://omen-pc:8080/{urlstem}";
                _logger.LogInformation("GetWeather url: {url}", url);
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
                _logger.LogInformation(e, "GetWeather");
            }

            _logger.LogInformation("Exit: GetWeather(): response: {response}", JsonSerializer.Serialize(response));
            return response;
        }

        [HttpGet]
        [Route("/v1/Scraper")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> Scraper([FromQuery] string scrapeUrl)
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                _logger.LogInformation("Enter: Scraper() [GET]: {scrapeURL}", scrapeUrl);

                // check city
                if (string.IsNullOrEmpty(scrapeUrl))
                    throw new ArgumentNullException(nameof(scrapeUrl), "url is required.");

                string urlstem = $"scraper?url={scrapeUrl}";
                string url = $"http://omen-pc:8081/{urlstem}";
                string content = "";
                _logger.LogInformation("Scraper url: {url}", url);
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
                _logger.LogInformation(e, "Scraper");
            }

            _logger.LogInformation("Exit: Scraper(): response: {r}", JsonSerializer.Serialize(response));
            return response;
        }

        [HttpPost]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> UpdateName(NameType namePerson)
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                await Task.Run(() =>
                {
                    _logger.LogInformation("Enter: UpdateName() [POST]");
                    response.Data = JsonSerializer.Serialize<string>("Not Implemented yet but you sent: " + namePerson.Name);
                    response.Success = true;
                });
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "UpdateName");
            }

            _logger.LogInformation("Exit: UpdateName(): response: {r}", JsonSerializer.Serialize(response));
            return response;
        }

        [HttpGet]
        [Route("/v1/quote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> GetQuote()
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                await Task.Run(() =>
                {
                    _logger.LogInformation("Enter: GetQuote()");
                    response.Data = JsonSerializer.Serialize<string>(quotes[Random.Shared.Next(quotes.Count)]);
                    response.Success = true;
                });
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetQuote");
            }

            _logger.LogInformation("Exit: GetQuote(): response: {r}", JsonSerializer.Serialize(response));
            return response;
        }

        [HttpGet]
        [Route("/v1/related")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> GetRelated([FromQuery] string page)
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                _logger.LogInformation("Enter: GetRelated [GET]");
                var connection = new RelatedPagesUtility(_context, _logger);
                dynamic db_result = await connection.GetRelatedPages(page);
                int page_count = db_result.Count;
                _logger.LogInformation("GetRelated: Found {db_result} related pages.", page_count);
                response.Data = JsonSerializer.Serialize(db_result);
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetRelated");
            }

            _logger.LogInformation("Exit: GetRelated(): response: {response}", JsonSerializer.Serialize(response));
            return response;
        }

        [HttpGet]
        [Route("/v1/dbhealth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> GetDBHealth()
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                _logger.LogInformation("Enter: GetDBHealth()");
                await _context.CheckConnection();
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "DBHealth");
            }

            _logger.LogInformation("Exit: GetDBHealth(): response: {h}", JsonSerializer.Serialize(response));
            return response;
        }
    }
}