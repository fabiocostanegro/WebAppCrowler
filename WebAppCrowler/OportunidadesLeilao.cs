using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAppCrowler
{
    public static class OportunidadesLeilao
    {
        [FunctionName("OportunidadesLeilao")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string responseMessage = string.Empty;

            if (req.Query["name"] == string.Empty)
                return new BadRequestObjectResult("Parametros invalidos");

            string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

            return new OkObjectResult(responseMessage);
        }
    }
}
