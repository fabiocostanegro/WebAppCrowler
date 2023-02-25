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
            
            double lucroPercentual = 10;
            
            List<ItensTabela> listaJogadoresTrade = ConsultarJogadoresTradeFutbin(ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroPopulares100k);
            List<JogadorValorMercadoAtual> listaPrecoAtual = ConsultaPrecoWebApp(listaJogadoresTrade, ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroPopulares100k);
            List<JogadorPrecoPrevisto> listaOportunidades = RetornarListaConsultaOportunidades(listaPrecoAtual,lucroPercentual);
            List<JogadorValorMercadoAtual> listaOportunidadesLance = ConsultarOportunidadesLance(listaOportunidades);
            return new OkObjectResult(listaOportunidadesLance);
        }
        private static List<ItensTabela> ConsultarJogadoresTradeFutbin(ConsultaValorJogadorFutBin.TipoJogadorTrade tipo)
        {
            List<ItensTabela> lista = new List<ItensTabela>();
            try
            {
                string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

                ConsultaValorJogadorFutBin consulta = new ConsultaValorJogadorFutBin(Fonte.FonteBase.Framework.Selenium, caminhoProfile);

                lista = consulta.ConsultarJogadoresPorTipo(tipo);
            }
            catch(Exception ex)
            {
                return null;
            }
            return lista;
            
        }
        private static List<JogadorValorMercadoAtual> ConsultaPrecoWebApp(List<ItensTabela> listaJogadores, ConsultaValorJogadorFutBin.TipoJogadorTrade tipo) 
        {
            string responseMessage = string.Empty;

            string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

            ConsultaValorJogadorWebApp consulta = new ConsultaValorJogadorWebApp(Fonte.FonteBase.Framework.Selenium, caminhoProfile, 30);
            List<JogadorPrecoPrevisto> lista = new List<JogadorPrecoPrevisto>();
            int incrementoValor = RetornaIncrementoValorJogador(tipo);
            int valorMaximo = RetornaIncrementoValorMaximo(tipo);
            foreach (ItensTabela item in listaJogadores)
            {
                string nome = item.Colunas[0].ValorColuna;
                int overAll = Convert.ToInt32(item.Colunas[2].ValorColuna);
                string versao = item.Colunas[3].ValorColuna;
                int valor = Util.FormatarValorJogador(item.Colunas[1].ValorColuna);
                lista.Add(new JogadorPrecoPrevisto(nome, overAll, versao, valor, incrementoValor, 0,valorMaximo,0));
            }
            List<JogadorValorMercadoAtual> listaValor = consulta.ConsultarValorJogador(lista, 30,2, ConsultaValorJogadorWebApp.TipoConsulta.BIN);
            return listaValor;
        }
        private static List<JogadorValorMercadoAtual> ConsultarOportunidadesLance(List<JogadorPrecoPrevisto> lista)
        {
            string caminhoProfile = "user-data-dir=C:\\Users\\55319\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 3";

            ConsultaValorJogadorWebApp consulta = new ConsultaValorJogadorWebApp(Fonte.FonteBase.Framework.Selenium, caminhoProfile, 30);

            List<JogadorValorMercadoAtual> qtdJogadoreslance = consulta.ConsultarValorJogador(lista, 30, 2, ConsultaValorJogadorWebApp.TipoConsulta.BID);

            consulta.FecharPagina();
            
            return qtdJogadoreslance;
        }
        private static int RetornaIncrementoValorJogador(ConsultaValorJogadorFutBin.TipoJogadorTrade tipo)
        {
            switch (tipo)
            {
                case ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroPopulares100k:
                    return 1000;
                case ConsultaValorJogadorFutBin.TipoJogadorTrade.Icon100k:
                    return 2000;
                case ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroNaoRaroMaisCaro:
                    return 500;
                default:
                    return 1000;
            }
        }
        private static int RetornaIncrementoValorMaximo(ConsultaValorJogadorFutBin.TipoJogadorTrade tipo)
        {
            switch (tipo)
            {
                case ConsultaValorJogadorFutBin.TipoJogadorTrade.OuroPopulares100k:
                    return 100000;
                case ConsultaValorJogadorFutBin.TipoJogadorTrade.Icon100k:
                    return 120000;
                default:
                    return 10000;
            }
        }
        private static List<JogadorPrecoPrevisto> RetornarListaConsultaOportunidades(List<JogadorValorMercadoAtual> listaJogadores, double percentualLucro)
        {
            List<JogadorPrecoPrevisto> listaOportunidades = new List<JogadorPrecoPrevisto>();
            foreach (JogadorValorMercadoAtual item in listaJogadores)
            {
                double perc = percentualLucro / 100;
                double valorPerc = Convert.ToDouble(item.ValorAtualMercado) * perc;
                double valorConsulta = Convert.ToDouble(item.ValorAtualMercado) - valorPerc;
                listaOportunidades.Add(new JogadorPrecoPrevisto(item.NomeJogador, item.Overall,"", 0, 0, 0, Convert.ToInt32(valorConsulta),item.ValorAtualMercado));
            }
            return listaOportunidades;
        }

    }
}
