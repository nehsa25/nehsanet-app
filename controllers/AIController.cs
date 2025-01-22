using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using nehsanet_app.Types;
using nehsanet_app.Services;
using nehsanet_app.db;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class AIController(ILogger<AIController> logger, DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly ILogger _logger = logger;

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
                _logger.LogInformation("ai() - sending request to Gemini: {question}", aiQuestion.Question);
                result = await client.TalkToGemini(aiQuestion.Question, aiQuestion.PreviousAnswer);
                result = result.Replace("\n", " ");
                _logger.LogInformation("ai() - received response from Gemini: {result}", result);
                aiQuestion.Answer = result;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error: {message}", e.Message);
            }

            _logger.LogInformation("Exit: ai(): aiQuestion: {question}",  JsonSerializer.Serialize(aiQuestion));
            if (result != null)
            {
                return Ok(aiQuestion);
            }
            else
            {
                return BadRequest("No response from Gemini");
            }
        }
    }
}