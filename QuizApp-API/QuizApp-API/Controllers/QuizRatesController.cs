using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;

namespace QuizApp_API.Controllers
{
    [Route("api/rates")]
    [Authorize]
    public class QuizRatesController(IRatesService rates) : ControllerBase
    {
        private readonly IRatesService _rates = rates;

        [HttpPost("add")]
        public async Task<IActionResult> RateQuiz([FromBody] UserRate userRate)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _rates.AddRateAsync(userRate.quizId, userId, userRate.rate);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GetCurrentUserId()
        {
            if (User.Claims != null && User.Claims.Any(c => c.Type == "UserId"))
            {
                var userClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userClaim != null)
                    return userClaim.Value;
                return string.Empty;
            }
            else return string.Empty;
        }
    }

    public class UserRate
    {
        public string quizId { get; set; }
        public int rate { get; set; }
    }
}
