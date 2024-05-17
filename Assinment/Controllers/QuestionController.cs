using Assignment1.DTOs;
using Assignment1.Models;
using Assignment1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Assignment1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public QuestionController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        ///<summary>
        /// Create a new question.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/question
        ///     {
        ///        "type": "Dropdown",
        ///        "description": "What is your favorite color?",
        ///        "options": ["Red", "Blue", "Green"]
        ///     }
        ///
        /// </remarks>
        /// <param name="questionDto">Question data</param>
        /// <returns>A newly created question</returns>
        /// <response code="201">Returns the newly created question</response>
        [HttpPost]
        [ProducesResponseType(typeof(Question), 201)]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Type = questionDto.Type,// this can be Paragraph, YesNo, Dropdown, MultipleChoice, Date, Number
                Description = questionDto.Description,// this is the question description ex: What is your name?
                Options = questionDto.Options
            };

            await _cosmosDbService.AddItemAsync(question);
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
        }

        /// <summary>
        /// Update an existing question.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/question
        ///     {
        ///        "type": "Dropdown",
        ///        "description": "What is your favorite color?",
        ///        "options": ["Red", "Blue", "Green"]
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The ID of the question to update</param>
        /// <param name="questionDto">Updated question data</param>
        /// <returns>No content</returns>
        /// <response code="204">No content</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateQuestion(string id, [FromBody] QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = id,
                Type = questionDto.Type,
                Description = questionDto.Description,// this is the question description ex: What is your name?
                Options = questionDto.Options
            };

            await _cosmosDbService.UpdateItemAsync(id, question);
            return NoContent();
        }

        /// <summary>
        /// Get a question by its ID.
        /// </summary>
        /// <param name="id">The ID of the question to retrieve</param>
        /// <returns>The requested question</returns>
        /// <response code="200">Returns the requested question</response>
        /// <response code="404">If the question is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Question), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestion(string id)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id);
            var questions = await _cosmosDbService.GetItemsAsync<Question>(query);

            var question = questions.FirstOrDefault();
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        /// <summary>
        /// Get all questions.
        /// </summary>
        /// <returns>All questions</returns>
        /// <response code="200">Returns all questions</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Question>), 200)]
        public async Task<IActionResult> GetQuestions()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var questions = await _cosmosDbService.GetItemsAsync<Question>(query);
            return Ok(questions);
        }

        /// <summary>
        /// Delete a question by its ID.
        /// </summary>
        /// <param name="id">The ID of the question to delete</param>
        /// <returns>Result of the delete operation</returns>
        /// <response code="200">Returns the result of the delete operation</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteQuestion(string id)
        {
            var result = await _cosmosDbService.DeleteItemAsync<Question>(id);
            return Ok(result);
        }
    }
}


