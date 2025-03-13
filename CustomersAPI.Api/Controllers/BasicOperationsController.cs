using CustomersAPI.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomersAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicOperationsController(OperationsService operationsService) : ControllerBase
    {
        private readonly OperationsService _operationsService = operationsService;

        [HttpGet("Add")]
        public async Task<IActionResult> Add(int num1, int num2)
        {
            var result = await _operationsService.Sum(num1, num2);

            return Ok(result);
        }

        [HttpGet("Substract")]
        public async Task<IActionResult> Substract(int num1, int num2)
        {
            var result = await _operationsService.Substrae(num1, num2);

            return Ok(result);
        }

        [HttpGet("Multiply")]
        public async Task<IActionResult> Multiply(int num1, int num2)
        {
            var result = await _operationsService.Multi(num1, num2);

            return Ok(result);
        }

        [HttpGet("Divide")]
        public async Task<IActionResult> Divide(int num1, int num2)
        {
            var result = await _operationsService.Divide(num1, num2);

            return Ok(result);
        }
    }
}
