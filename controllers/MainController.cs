
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using MySqlConnector;
using nehsanet_app.Models;
using nehsanet_app.Types;

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
                    "Maxiumus",
                    "You're my cat."
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
            names.Add(new("Butters", "Polka will never die!"));
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
            "“I get knocked down, but I get up again.” - Chumbawamba"
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
            "Neighborhood Rollerblader",
            "Developer",
            "Student",
            "Father",
            "Ambitious",
            "Homo Sapien",
            "Tester",
            "Game-Player",
            "<a routerLink='/mud'>Game-Maker</a>",
            "SDET",
            "Husband",
            "Adventurous",
            "Nice",
            "Skier",
            "Book-Reader",
            "Learner",
            "<a href=\"https://synthridersvr.com/\">Synth-Rider</a>"
        ];

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
            results += ". A jack-of-all-trades, smorgasbord of a human (like everyone else).";
            dynamic jsonresults = JsonSerializer.Serialize(results);
            _logger.LogInformation($"Exit: GetPositiveAdjective(): results: ${jsonresults}");
            return jsonresults;
        }

        [HttpPost]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string UpdateName(NameType namePerson)
        {
            _logger.LogInformation("Enter: UpdateName() [POST]");
            dynamic results = JsonSerializer.Serialize<string>("Not Implemented yet but you sent: " + namePerson.Name);
            _logger.LogInformation($"Exit: GetQuote(): results: ${JsonSerializer.Serialize(results)}");
            return results;
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
        public async Task<string> GetComment([FromServices] MySqlDataSource db, string page, int numberToReturn=5)
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