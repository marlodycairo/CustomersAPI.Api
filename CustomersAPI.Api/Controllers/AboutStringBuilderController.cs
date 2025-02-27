using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace CustomersAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutStringBuilderController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetResultAsync()
        {
            //string text = "Under the bridge";
            //var sb = new StringBuilder();
            //var sb2 = new StringBuilder(text);

            //var responseSb = sb.ToString();

            //var responseSb2 = sb2.ToString();

            //sb2.Append(", is not sure.");
            //responseSb2 = sb2.ToString();

            //sb.Append("Insert data");

            //sb.Insert(11, ", insert more data");

            //responseSb = sb.ToString();

            //sb.Replace("insert", "adding");

            //responseSb = sb.ToString();

            //sb.Remove(11, 9);

            //responseSb = sb.ToString();
            //rendimiento
            int iteraciones = 10000;

            // Usando string (malo para rendimiento)
            Stopwatch sw = Stopwatch.StartNew();
            string texto = "";
            for (int i = 0; i < iteraciones; i++)
            {
                texto += "a"; // Se crea una nueva cadena en cada iteración
            }
            sw.Stop();
            Console.WriteLine("Tiempo con string: " + sw.ElapsedMilliseconds + " ms");

            // Usando StringBuilder (mucho más rápido)
            sw.Restart();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iteraciones; i++)
            {
                sb.Append("a"); // Se modifica el mismo objeto
            }
            sw.Stop();
            Console.WriteLine("Tiempo con StringBuilder: " + sw.ElapsedMilliseconds + " ms");

            return Ok($"Texto => {texto} - StringBuilder => {sb}");
        }
    }
}
