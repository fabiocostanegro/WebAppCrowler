using Fonte.Consultas.ConsultaValorJogador;
using Fonte.ConsultasWebApp.ConsultarValorJogador;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte.ConsultasFutBin
{
    public class ConsultaValorJogadorFutBin: Futbin
    {
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
            base.AcessarFutbin(url);
            string seletorTabela = ".row.w-100.pt-1.main-row.px-3.text-center";
            string seletorItens = ".row.w-100.pt-1.main-row.px-3.text-center";
            List<string> seletorColunas = new List<string>();    
            seletorColunas.Add(".col-md-3.text-left.py-1.col-12");
            seletorColunas.Add("body > div.site-sales-page > div.container.pb-5 > div:nth-child(2) > div.col-md-9 > div.row.left-area-row-top.w-100.sales-holder.text-center.justify-content-center.sales-block.z-depth-1 > div > div.sales-inner.col-12.text-left.px-0 > div:nth-child(1) > div:nth-child(2)");
            this.navegador.ConstruirTabela(seletorTabela, seletorItens, seletorColunas);
            /*
            IWebElement element = driver.FindElement(By.CssSelector(".row.w-100.pt-1.main-row.px-3.text-center"));
            ReadOnlyCollection<IWebElement> tabelaHistoricoVenda = driver.FindElements(By.CssSelector(".row.w-100.pt-1.main-row.px-3.text-center"));
            for (int i = 0; i < tabelaHistoricoVenda.Count; i++)
            {
                string dataHistorico = tabelaHistoricoVenda[i].FindElement(By.CssSelector(".col-md-3.text-left.py-1.col-12")).Text;
                ReadOnlyCollection<IWebElement> precos = tabelaHistoricoVenda[i].FindElements(By.CssSelector(".col-md-2.col-3"));
                string elementoBinOrBid = tabelaHistoricoVenda[i].FindElement(By.CssSelector(".col-md-1.col-1.text-right")).GetAttribute("innerHTML");
                int listadoPor = Convert.ToInt32(precos[0].Text.Replace(",", ""));
                if (listadoPor == 0)
                    break;
                int vendidoPor = Convert.ToInt32(precos[1].Text.Replace(",", ""));
                string tipoVenda = "";
                if (vendidoPor != 0)
                    tipoVenda = elementoBinOrBid.Contains("BID") ? "BID" : "BIN";
                DateTime dataHoraHistorico = ConverterDataHistoricoVenda(dataHistorico);
                per.InserirHistoricoVendaJogador(item.IdJogador, listadoPor, vendidoPor, tipoVenda, dataHoraHistorico);

            }*/

        }

    }
}
