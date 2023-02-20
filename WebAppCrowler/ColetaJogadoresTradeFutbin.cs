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
using CrowlerFramework;
using System.Collections.Generic;

namespace WebAppCrowler
{
    public static class ColetaJogadoresTradeFutbin
    {
        [FunctionName("ColetaJogadoresTradeFutbin")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

            ConsultaValorJogadorFutBin consulta = new ConsultaValorJogadorFutBin(Fonte.FonteBase.Framework.Selenium, caminhoProfile);

            if (req.Query["tipo"] == string.Empty)
                return new BadRequestObjectResult("Parametros invalidos");

            List<ItensTabela> lista = new List<ItensTabela>();
            if (req.Query["tipo"] == "OuroNaoRaroMaisCaro")
                lista = consulta.ConsultarJogadoresPorTipo(ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroNaoRaroMaisCaro);
            else if (req.Query["tipo"] == "Populares")
                lista = consulta.ConsultarJogadoresPorTipo(ConsultaValorJogadorFutBin.TipoJogadorTrade.Populares);
            else if (req.Query["tipo"] == "Forragem84")
                lista = consulta.ConsultarJogadoresPorTipo(ConsultaValorJogadorFutBin.TipoJogadorTrade.Forragem84);
            else if (req.Query["tipo"] == "Forragem85")
                lista = consulta.ConsultarJogadoresPorTipo(ConsultaValorJogadorFutBin.TipoJogadorTrade.Forragem85);
            else if (req.Query["tipo"] == "Forragem86")
                lista = consulta.ConsultarJogadoresPorTipo(ConsultaValorJogadorFutBin.TipoJogadorTrade.Forragem86);
            else if (req.Query["tipo"] == "Forragem87")
                lista = consulta.ConsultarJogadoresPorTipo(ConsultaValorJogadorFutBin.TipoJogadorTrade.Forragem87);
            else if (req.Query["tipo"] == "Forragem88")
                lista = consulta.ConsultarJogadoresPorTipo(ConsultaValorJogadorFutBin.TipoJogadorTrade.Forragem88);
            return new OkObjectResult(lista);
        }
    }
}
