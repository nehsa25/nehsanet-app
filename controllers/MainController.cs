
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Nehsa.Controllers
{
    [ApiController]
    [Route("/")]
    public class Main : ControllerBase
    {
        List<NamePerson> names = new();

        public Main()
        {
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
                    "Weird Esperacchius with honor!"
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
                    "You keep your bargains."
                )
            );
            names.Add(
                new(
                    "Elodin",
                    "You'll go to the Crockery if you're a raven!"
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
            names.Add(
                new(
                    "Butters",
                    "Polka will never die!"
                )
            );
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
        "“When the elevator tries to bring you down, go crazy!” - Prince"
        ];


        [HttpGet]
        [Route("/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string Get()
        {
            Trace.WriteLine("Hello, World!");
            return "Hello, World!";
        }

        [HttpGet]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetName()
        {
            return JsonSerializer.Serialize(names[Random.Shared.Next(names.Count)]);
        }

        [HttpPost]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string UpdateName(ContactMe contactMe)
        {
            return JsonSerializer.Serialize<string>("Not Implemented yet but you sent: " + contactMe);
        }

        [HttpGet]
        [Route("/v1/quote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetQuote()
        {
            return JsonSerializer.Serialize<string>(quotes[Random.Shared.Next(quotes.Count)]);
        }

        [HttpPost]
        [Route("/v1/contactme")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string PostContactMe(ContactMe contactMe)
        {
            return JsonSerializer.Serialize<string>("Not Implemented but you send: " + contactMe);
        }
    }
}