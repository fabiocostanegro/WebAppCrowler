using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Fonte.ConsultasFutBin;

namespace WebAppCrowler
{
    public static class ColetaJogadoresTradeFutbin
    {
        [FunctionName("ColetaJogadoresTradeFutbin")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string responseMessage = "ok";


            string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

            ConsultaValorJogadorFutBin consulta = new ConsultaValorJogadorFutBin(Fonte.FonteBase.Framework.Selenium, caminhoProfile);

            consulta.ConsultarJogadoresPorTipo(ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroNaoRaroMaisCaro);

            return new OkObjectResult(responseMessage);
        }
    }
}
