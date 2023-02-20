using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Fonte.Consultas.ConsultaValorJogador;
using System.Collections.Generic;
using Fonte.ConsultasWebApp.ConsultarValorJogador;

namespace WebAppCrowler
{
    public static class ConsultaPreco
    {
        [FunctionName("ConsultaPreco")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string responseMessage = string.Empty;
            
            if (req.Query["name"] == string.Empty)
                return new BadRequestObjectResult("Parametros invalidos");

            string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

            ConsultaValorJogadorWebApp consulta = new ConsultaValorJogadorWebApp(Fonte.FonteBase.Framework.Selenium, caminhoProfile, 30);
            List<JogadorPrecoPrevisto> lista = new List<JogadorPrecoPrevisto>();
            lista.Add(new JogadorPrecoPrevisto(req.Query["name"], Convert.ToInt32(req.Query["val"]), Convert.ToInt32(req.Query["inc"]), Convert.ToInt32(req.Query["index"])));
            List<JogadorValorMercadoAtual> listaValor = consulta.ConsultarValorJogador(lista, 30);
            return new OkObjectResult(listaValor);
        }
    }
}
