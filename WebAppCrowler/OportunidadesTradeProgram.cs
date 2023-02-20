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
using System.Collections.Generic;
using CrowlerFramework;
using Fonte.Consultas.ConsultaValorJogador;
using Fonte.ConsultasWebApp.ConsultarValorJogador;

namespace WebAppCrowler
{
    public static class OportunidadesTradeProgram
    {
        [FunctionName("OportunidadesTradeProgram")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (req.Query["tipo"] == string.Empty)
                return new BadRequestObjectResult("Parametros invalidos");

            List<ItensTabela> listaJogadoresTrade = ConsultarJogadoresTradeFutbin(ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroNaoRaroMaisCaro);
            List<JogadorValorMercadoAtual> listaPrecoAtual = ConsultaPrecoWebApp(listaJogadoresTrade, ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroNaoRaroMaisCaro);

            return new OkObjectResult(listaPrecoAtual);
        }
        private static List<ItensTabela> ConsultarJogadoresTradeFutbin(ConsultaValorJogadorFutBin.TipoJogadorTrade tipo)
        {
            string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

            ConsultaValorJogadorFutBin consulta = new ConsultaValorJogadorFutBin(Fonte.FonteBase.Framework.Selenium, caminhoProfile);

            List<ItensTabela> lista = new List<ItensTabela>();

            lista = consulta.ConsultarJogadoresPorTipo(tipo);

            return lista;
        }
        private static List<JogadorValorMercadoAtual> ConsultaPrecoWebApp(List<ItensTabela> listaJogadores, ConsultaValorJogadorFutBin.TipoJogadorTrade tipo) 
        {
            string responseMessage = string.Empty;

            string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

            ConsultaValorJogadorWebApp consulta = new ConsultaValorJogadorWebApp(Fonte.FonteBase.Framework.Selenium, caminhoProfile, 30);
            List<JogadorPrecoPrevisto> lista = new List<JogadorPrecoPrevisto>();
            int incrementoValor = RetornaIncrementoValorJogador(tipo);
            foreach (ItensTabela item in listaJogadores)
            {
                string nome = item.Colunas[0].ValorColuna;
                int overAll = Convert.ToInt32(item.Colunas[2].ValorColuna);
                string versao = item.Colunas[3].ValorColuna;
                int valor = Util.FormatarValorJogador(item.Colunas[1].ValorColuna);
                lista.Add(new JogadorPrecoPrevisto(nome, overAll, versao, valor, incrementoValor, 0));
            }
            List<JogadorValorMercadoAtual> listaValor = consulta.ConsultarValorJogador(lista, 30);
            return null;
        }
        private static int RetornaIncrementoValorJogador(ConsultaValorJogadorFutBin.TipoJogadorTrade tipo)
        {
            switch (tipo)
            {
                case ConsultaValorJogadorFutBin.TipoJogadorTrade.Populares:
                    return 1000;
                default:
                    return 500;
            }
        }

    }
}
