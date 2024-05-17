using Assignment1.DTOs;
using Assignment1.Models;
using Assignment1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Assignment1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ICosmosDbService _questionDbService; // Injected QuestionService

        public AnswerController(ICosmosDbService cosmosDbService, ICosmosDbService questionDbService)
        {
            _cosmosDbService = cosmosDbService;
            _questionDbService = questionDbService;
        }

        /// <summary>
        /// Submit answers to questions.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/answer
        ///     [
        ///         {
        ///             "questionId": "1",
        ///             "response": "Yes"
        ///         },
        ///         {
        ///             "questionId": "2",
        ///             "response": "Blue"
        ///         }
        ///     ]
        ///
        /// </remarks>
        /// <param name="answerDtos">List of answer DTOs</param>
        /// <returns>Returns Ok if answers are successfully submitted</returns>
        /// <response code="200">Returns Ok if answers are successfully submitted</response>
        /// <response code="400">If answerDtos is null, empty, or contains invalid data</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<AnswerDto> answerDtos)
        {
            // Check if answerDtos is null or empty
            if (answerDtos == null || !answerDtos.Any())
            {
                return BadRequest("No answers provided.");
            }

            var answers = new List<Answer>();

            foreach (var answerDto in answerDtos)
            {
                // Check if QuestionId or Response is null or empty
                if (string.IsNullOrWhiteSpace(answerDto.QuestionId) || string.IsNullOrWhiteSpace(answerDto.Response))
                {
                    return BadRequest("Question ID or response is missing or empty.");
                }

                // Check if the question ID exists in the database
                var question = await GetQuestionFromDb(answerDto.QuestionId);
                if (question == null)
                {
                    return BadRequest($"Question with ID '{answerDto.QuestionId}' does not exist.");
                }

                var answer = new Answer
                {
                    Id = Guid.NewGuid().ToString(),
                    QuestionId = answerDto.QuestionId,
                    Response = answerDto.Response
                };

                answers.Add(answer);
            }

            // Add answers to Cosmos DB
            foreach (var answer in answers)
            {
                await _cosmosDbService.AddItemAsync(answer);
            }

            return Ok();
        }

        // Helper method to get question from the database
        private async Task<Question> GetQuestionFromDb(string id)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id);
            var questions = await _questionDbService.GetItemsAsync<Question>(query);
            return questions.FirstOrDefault();
        }
    }
}
