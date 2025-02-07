using DashBe.Application.Interfaces;
using DashBe.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DashBe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationController : ControllerBase
    {
        private readonly IExpressionValidatorService _validator;

        public ValidationController(IExpressionValidatorService validator)
        {
            _validator = validator;
        }

        [HttpPost("validate")]
        public ActionResult<ValidationOPResult> Validate([FromBody] string input)
        {
            if(string.IsNullOrEmpty(input)) 
            {
                var errorResult = new ValidationOPResult(false, "L'input non può essere vuoto");
                return BadRequest(errorResult);
            }

            bool isBalanced = _validator.IsBalanced(input);

            return Ok(new ValidationOPResult(isBalanced, isBalanced ? "input bilanciato" : "input non bilanciato!"));

            //if (string.IsNullOrEmpty(input))
            //{
            //    return BadRequest(new { error = "Input non valido" });
            //}

            //bool isBalanced = _validator.IsBalanced(input);

            //return Ok(new ValidationOPResult
            //{
            //    isBalanced = isBalanced,
            //    sMessage = isBalanced ? "Il risultato è bilanciato" : "Il risultato non è bilanciato!",
            //    dateTime = DateTime.Now
            //});
        }
    }
}
