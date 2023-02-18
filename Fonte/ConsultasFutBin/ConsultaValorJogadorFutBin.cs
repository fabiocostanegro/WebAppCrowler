using CrowlerFramework;
using Fonte.Consultas.ConsultaValorJogador;
using Fonte.ConsultasWebApp.ConsultarValorJogador;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte.ConsultasFutBin
{
    public class ConsultaValorJogadorFutBin: Futbin
    {
        public enum TipoJogadorTrade
        {
            OuroNaoRaroMaisCaro = 1,
        }
        public ConsultaValorJogadorFutBin(FonteBase.Framework framework, string caminhoProfile) : base(framework, caminhoProfile)
        {

        }
        public ConsultaValorJogadorFutBin(FonteBase.Framework framework) : base(framework)
        {

        }

        public List<JogadorValorMercadoAtual> ConsultarFifa22Players(string url)
        {
            Console.WriteLine("Iniciando acesso ao WebAPP");
            base.AcessarFutbin(url);
            int quantidadeItens = QuantidadeItemPaginaAtual();
            return null;
            /*"#repTb > tbody > tr:nth-child(2)"
                "#repTb > tbody > tr:nth-child(1) > td.table-row-text.row > div.d-inline.pt-2.pl-3 > div:nth-child(1) > a"
                "#repTb > tbody > tr:nth-child(30) > td.table-row-text.row > div.d-inline.pt-2.pl-3 > div:nth-child(1) > a"*/

        }
        private int QuantidadeItemPaginaAtual()
        {
            int quantidadeItem = 1;
            while(true)
            {
                string seletorElemento = "#repTb > tbody > tr:nth-child(" + quantidadeItem + ") > td.table-row-text.row > div.d-inline.pt-2.pl-3 > div:nth-child(1) > a";
                if (this.navegador.ElementoExiste(seletorElemento))
                    quantidadeItem++;
                else
                    return quantidadeItem;
            }
            
        }
        public void ConsultarHistoricoVendasJogadorFutbin(string url)
        {  

        }
        public void ConsultarJogadoresPorTipo(TipoJogadorTrade tipoJogador)
        {
            string url = ObterUrlPorTipoJogadorTrade(tipoJogador);
            base.AcessarFutbin(url);
            string seletorTabela = "#repTb";
            string seletorLinha = "#repTb > tbody > tr:nth-child(1)";
            string seletorColunaNome = "#repTb > tbody > tr:nth-child(1) > td.table-row-text.row > div.d-inline.pt-2.pl-3 > div:nth-child(1) > a";
            string seletorColunaValor = "#repTb > tbody > tr:nth-child(1) > td:nth-child(6) > span";
            List<Coluna> colunaList = new List<Coluna>();
            colunaList.Add(new Coluna(seletorColunaNome, "NomeJogador"));
            colunaList.Add(new Coluna(seletorColunaValor, "ValorJogador"));

            base.ConsultarListaJogadoresTrade(seletorTabela, seletorLinha, colunaList);


        }
        private string ObterUrlPorTipoJogadorTrade(TipoJogadorTrade tipoJogador)
        {
            switch (tipoJogador)
            {
                case TipoJogadorTrade.OuroNaoRaroMaisCaro:
                    return "https://www.futbin.com/players?page=1&eUnt=1&order=desc&pos_type=all&sort=pc_price&version=gold_nr";
                default:
                    return string.Empty;
            }
        }

    }
}
