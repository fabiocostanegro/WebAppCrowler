using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fonte;
using Fonte.ConsultasWebApp.ConsultarValorJogador;

namespace Fonte.Consultas.ConsultaValorJogador
{
    public class ConsultaValorJogadorWebApp : WebApp
    {
        public ConsultaValorJogadorWebApp(FonteBase.Framework framework, string caminhoProfile) : base(framework, caminhoProfile)
        {

        }
        public ConsultaValorJogadorWebApp(FonteBase.Framework framework, string caminhoProfile, int quantidadeTempoSegundosMaximoEspera) : base(framework, caminhoProfile, quantidadeTempoSegundosMaximoEspera)
        {

        }
        public List<JogadorValorMercadoAtual> ConsultarValorJogador(List<JogadorPrecoPrevisto> listaJogadores, int qtdMinutosRestantesMinimo)
        {
            Console.WriteLine("Iniciando acesso ao WebAPP");
            base.AcessarWebAPP();
            Console.WriteLine("WebApp acessado com sucesso");
            Console.WriteLine("Acessando menu de transferencias");
            base.AcessarMenuTransferencias();
            Console.WriteLine("Menu de transferencia acessado com sucesso");
            return ConsultarValorAtualJogador(listaJogadores, qtdMinutosRestantesMinimo);

        }
        public int ConsultarExisteJogadorLance(JogadorPrecoPrevisto jogador)
        {
            Console.WriteLine("Iniciando acesso ao WebAPP");
            base.AcessarWebAPP();
            Console.WriteLine("WebApp acessado com sucesso");
            Console.WriteLine("Acessando menu de transferencias");
            base.AcessarMenuTransferencias();
            Console.WriteLine("Menu de transferencia acessado com sucesso");
            return ConsultarValorJogadorLance(jogador);

        }
        public List<JogadorValorMercadoAtual> ConsultarValorAtualJogador(List<JogadorPrecoPrevisto> listaJogadores, int qtdMinutosRestantesMinimo)
        {
            List<JogadorValorMercadoAtual> listaValorMercado = new List<JogadorValorMercadoAtual>();
            foreach (JogadorPrecoPrevisto item in listaJogadores)
            {
                Console.WriteLine("Consultando jogador " + item.NomeJogador);
                Int32 valorMercadoAtual = ConsultarJogador(item, qtdMinutosRestantesMinimo);
                Console.WriteLine("Jogador " + item.NomeJogador + " consultado com sucesso");
                listaValorMercado.Add(new JogadorValorMercadoAtual(item.NomeJogador, valorMercadoAtual));
                LimparCamposConsulta();

            }
            this.navegador.FecharPagina();
            return listaValorMercado;
        }
        private Int32 ConsultarJogador(JogadorPrecoPrevisto jogador, int qtdMinutosRestantesMinimo)
        {
            Int32 valorAtualMercado = jogador.ValorMinimoPrevisto;

            ConsultarJogador(jogador.NomeJogador);
            bool achouJogador = ConsultarValorJogador(valorAtualMercado);
            while(achouJogador) //tratamento para quando o valor informado está acima do que o jogador vale de fato.
            {
                VoltarTelaPesquisa();
                valorAtualMercado = valorAtualMercado - jogador.IncrementoValor;
                achouJogador = ConsultarValorJogador(valorAtualMercado);
                if(!achouJogador)
                {
                    VoltarTelaPesquisa();
                    return valorAtualMercado - jogador.IncrementoValor;
                }
            }

            while (!achouJogador)
            {
                VoltarTelaPesquisa();
                valorAtualMercado = valorAtualMercado + jogador.IncrementoValor;
                achouJogador = ConsultarValorJogador(valorAtualMercado);
            }
            VoltarTelaPesquisa();
            return valorAtualMercado;
            
        }
        private void LimparCamposConsulta()
        {
            this.navegador.ApagarTexto(cssValorJogador);
            this.navegador.ApagarTexto(cssNomeJogador);
        }
        private void VoltarTelaPesquisa()
        {
            this.navegador.Clicar("body > main > section > section > div.ut-navigation-bar-view.navbar-style-landscape > button.ut-navigation-button-control", cssValorJogador);

            this.navegador.ApagarTexto(cssValorJogador);
        }

        private bool ConsultarValorJogador(int valorJogador, int qtdMinutosRestantesMinimo)
        {
            this.navegador.DigitarTexto(cssValorJogador, valorJogador.ToString());
            this.navegador.Clicar("body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.button-container > button.btn-standard.call-to-action");
            this.navegador.EsperarCarregamento(2000);
            if (this.navegador.ElementoExiste("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-navigation-container-view.ui-layout-right > div > div > div.DetailPanel > div.bidOptions > button.btn-standard.call-to-action.bidButton"))
            {
                string textoMinutos = this.navegador.RetornarTexto("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-pinned-list-container.SearchResults.ui-layout-left > div > ul > li.listFUTItem.has-auction-data.selected > div > div.auction > div.auction-state > span.time");


                string pattern = "\\d+";
                Regex rg = new Regex(pattern);
                Match m = rg.Match(textoMinutos);

                if (Convert.ToInt32(m.Value) >= qtdMinutosRestantesMinimo)
                    return true;
            }
            return false;

        }
        private bool ConsultarValorJogador(int valorJogador)
        {
            this.navegador.DigitarTexto(cssValorJogador, valorJogador.ToString());
            this.navegador.Clicar("body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.button-container > button.btn-standard.call-to-action");
            this.navegador.EsperarCarregamento(2000);
            if (this.navegador.ElementoExiste("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-navigation-container-view.ui-layout-right > div > div > div.DetailPanel > div.bidOptions > button.btn-standard.call-to-action.bidButton"))
                return true;
            return false;

        }
        private int ConsultarValorJogadorLance(JogadorPrecoPrevisto jogador)
        {
            this.ConsultarJogador(jogador.NomeJogador);
            this.navegador.DigitarTexto(cssValorLanceMaximoJogador, jogador.ValorMinimoPrevisto.ToString());
            this.navegador.Clicar("body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.button-container > button.btn-standard.call-to-action");
            this.navegador.EsperarCarregamento(2000);
            if (this.navegador.ElementoExiste("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-navigation-container-view.ui-layout-right > div > div > div.DetailPanel > div.bidOptions > button.btn-standard.call-to-action.bidButton"))
            {

                string cssResultadoPesquisa = "listFUTItem";
                return this.navegador.RetornarQuantidadeItensTabela(cssResultadoPesquisa);
            }
            return 0;
        }
        private void ConsultarJogador(string nomeJogador)
        {
            string cssSelectorBoxPesquisa = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.has-selection.contract-text-input.is-open > div > div.inline-list > ul > button > span.btn-text";
            this.navegador.DigitarTexto(cssNomeJogador, cssSelectorBoxPesquisa, nomeJogador);
            this.navegador.Clicar(cssSelectorBoxPesquisa);
        }
        public void FecharPagina()
        {
            this.navegador.FecharPagina();
        }
    }
}

