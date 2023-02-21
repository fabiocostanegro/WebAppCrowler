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
            Populares,
            Forragem84,
            Forragem85,
            Forragem86,
            Forragem87,
            Forragem88,
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
        public List<ItensTabela> ConsultarJogadoresPorTipo(TipoJogadorTrade tipoJogador)
        {
            string url = ObterUrlPorTipoJogadorTrade(tipoJogador);
            //base.AcessarFutbin("https://www.futbin.com",true);
            base.AcessarFutbin(url);
            string seletorTabela = "#repTb";
            string seletorColunaNome = "#repTb > tbody > tr:nth-child(<<indexLinha>>) > td.table-row-text.row > div.d-inline.pt-2.pl-3 > div:nth-child(1) > a";
            string seletorColunaValor = "#repTb > tbody > tr:nth-child(<<indexLinha>>) > td:nth-child(6) > span";
            string seletorOverAll = "#repTb > tbody > tr:nth-child(<<indexLinha>>) > td:nth-child(3) > span";
            string versao = "#repTb > tbody > tr:nth-child(<<indexLinha>>) > td.mobile-hide-table-col > div:nth-child(1)";
            List<Coluna> colunaList = new List<Coluna>();
            colunaList.Add(new Coluna(seletorColunaNome, "NomeJogador"));
            colunaList.Add(new Coluna(seletorColunaValor, "ValorJogador"));
            colunaList.Add(new Coluna(seletorOverAll, "OverAll"));
            colunaList.Add(new Coluna(versao, "Versao"));
            List<string> listaSeletores = new List<string>();
            listaSeletores.Add("player_tr_1");
            listaSeletores.Add("player_tr_2");

            List<int> listaIndexLinha = new List<int>();
            listaIndexLinha.Add(1);
            listaIndexLinha.Add(3);

            return base.ConsultarListaJogadoresTrade(seletorTabela, listaSeletores, listaIndexLinha, 4, colunaList);


        }
        private string ObterUrlPorTipoJogadorTrade(TipoJogadorTrade tipoJogador)
        {
            switch (tipoJogador)
            {
                case TipoJogadorTrade.OuroNaoRaroMaisCaro:
                    return "https://www.futbin.com/players?page=1&eClubs=479&eUnt=1&order=desc&pos_type=all&sort=pc_price&version=gold_nr";
                case TipoJogadorTrade.Populares:
                    return "https://www.futbin.com/players?page=1&eUnt=1&order=desc&sort=likes";
                case TipoJogadorTrade.Forragem84:
                    return "https://www.futbin.com/players?page=1&player_rating=84-84&order=desc&pos_type=all&sort=pc_price&version=gold_rare";
                case TipoJogadorTrade.Forragem85:
                    return "https://www.futbin.com/players?page=1&player_rating=85-85&order=desc&pos_type=all&sort=pc_price&version=gold_rare";
                case TipoJogadorTrade.Forragem86:
                    return "https://www.futbin.com/players?page=1&player_rating=86-86&order=desc&pos_type=all&sort=pc_price&version=gold_rare";
                case TipoJogadorTrade.Forragem87:
                    return "https://www.futbin.com/players?page=1&player_rating=87-87&order=desc&pos_type=all&sort=pc_price&version=gold_rare";
                case TipoJogadorTrade.Forragem88:
                    return "https://www.futbin.com/players?page=1&player_rating=88-88&order=desc&pos_type=all&sort=pc_price&version=gold_rare";
                default:
                    return string.Empty;
            }
        }

    }
}
