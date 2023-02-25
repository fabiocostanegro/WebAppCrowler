using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CrowlerFramework;
using Fonte;
using Fonte.ConsultasWebApp.ConsultarValorJogador;

namespace Fonte.Consultas.ConsultaValorJogador
{
    public class ConsultaValorJogadorWebApp : WebApp
    {
        public enum TipoConsulta
        {
            BID = 0,
            BIN = 1
        }
        public ConsultaValorJogadorWebApp(FonteBase.Framework framework, string caminhoProfile) : base(framework, caminhoProfile)
        {

        }
        public ConsultaValorJogadorWebApp(FonteBase.Framework framework, string caminhoProfile, int quantidadeTempoSegundosMaximoEspera) : base(framework, caminhoProfile, quantidadeTempoSegundosMaximoEspera)
        {

        }
        public List<JogadorValorMercadoAtual> ConsultarValorJogador(List<JogadorPrecoPrevisto> listaJogadores, int qtdMinutosRestantesMinimo, int qtdRetentativas, TipoConsulta tipoConsulta)
        {
            bool sucesso = false;
            int tentativas = 1;
            Console.WriteLine("Iniciando acesso ao WebAPP");
            base.AcessarWebAPP();
            while (!sucesso)
            {
                try
                {
                    Console.WriteLine("WebApp acessado com sucesso");
                    Console.WriteLine("Acessando menu de transferencias");
                    base.AcessarMenuTransferencias();
                    Console.WriteLine("Menu de transferencia acessado com sucesso");
                    sucesso = true;
                }
                catch (Exception ex)
                {
                    if (tentativas <= qtdRetentativas)
                    {
                        ReiniciarFluxo();
                        tentativas++;
                    }
                    else
                        throw ex;
                }
                
            }
            return ConsultarValorAtualJogador(listaJogadores, qtdMinutosRestantesMinimo, qtdRetentativas, tipoConsulta);

        }
        private void ReiniciarFluxoPesquisa()
        {
            base.ReiniciarFluxo();
            Console.WriteLine("WebApp acessado com sucesso");
            Console.WriteLine("Acessando menu de transferencias");
            base.AcessarMenuTransferencias();
            Console.WriteLine("Menu de transferencia acessado com sucesso");
        }
        public List<JogadorValorMercadoAtual> ConsultarValorAtualJogador(List<JogadorPrecoPrevisto> listaJogadores, int qtdMinutosRestantesMinimo, int qtdRetentativas, TipoConsulta tipoConsulta)
        {
            List<JogadorValorMercadoAtual> listaValorMercado = new List<JogadorValorMercadoAtual>();
            foreach (JogadorPrecoPrevisto item in listaJogadores)
            {
                Console.WriteLine("Consultando jogador " + item.NomeJogador);
                try
                {
                    bool sucesso = false;
                    int tentativas = 1;
                    while (!sucesso) // Politica de retentativas
                    {
                        Int32 valorMercadoAtual = 0;
                        if (tipoConsulta == TipoConsulta.BIN)
                            valorMercadoAtual = ConsultarJogador(item, qtdMinutosRestantesMinimo, out sucesso);
                        else
                            valorMercadoAtual = ConsultarValorJogadorLance(item, out sucesso);
                        if (!sucesso)
                        {
                            if (tentativas <= qtdRetentativas)
                            {
                                ReiniciarFluxoPesquisa();
                                tentativas++;
                            }
                            else
                            {
                                Console.WriteLine("Jogador " + item.NomeJogador + " não foi consultado por erro no fluxo");
                                ReiniciarFluxoPesquisa();
                                break;
                            }
                        }
                        if (valorMercadoAtual >= 0)
                        {
                            if(tipoConsulta == TipoConsulta.BID)
                                listaValorMercado.Add(new JogadorValorMercadoAtual(item.NomeJogador, item.ValorAtualMercado, item.OverAll, valorMercadoAtual));
                            else
                                listaValorMercado.Add(new JogadorValorMercadoAtual(item.NomeJogador, valorMercadoAtual, item.OverAll));
                        }
                        LimparCamposConsulta();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Erro ao consultar o jogador: " + item.NomeJogador);
                    LimparCamposConsulta();
                }
            }
            this.navegador.FecharPagina();
            return listaValorMercado;
        }
        private Int32 ConsultarJogador(JogadorPrecoPrevisto jogador, int qtdMinutosRestantesMinimo, out bool sucesso)
        {
            sucesso = true;
            try
            {
                Int32 valorAtualMercado = jogador.ValorMinimoPrevisto;
                int primeiroValorRetornado = 0;
                bool achouJogador = ConsultarJogadorOverAll(jogador.NomeJogador, jogador.OverAll);
                if(!achouJogador) //Não achou o jogador pelo nome
                {
                    Console.WriteLine("Jogador " + jogador.NomeJogador + " não encontrado");
                    return primeiroValorRetornado;
                }
                achouJogador = ConsultarValorJogador(valorAtualMercado);
                while (achouJogador && valorAtualMercado < jogador.ValorMaximo) //tratamento para quando o valor informado está acima do que o jogador vale de fato.
                {

                    primeiroValorRetornado = Convert.ToInt32(this.navegador.RetornarTexto("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-pinned-list-container.SearchResults.ui-layout-left > div > ul > li.listFUTItem.has-auction-data.selected > div > div.auction > div:nth-child(3) > span.currency-coins.value").Replace(",", ""));
                    VoltarTelaPesquisa();
                    valorAtualMercado = valorAtualMercado - jogador.IncrementoValor;
                    if (valorAtualMercado < 0)
                        sucesso = false;
                    achouJogador = ConsultarValorJogador(valorAtualMercado);
                    if (!achouJogador)
                    {
                        VoltarTelaPesquisa();
                        return primeiroValorRetornado;
                    }
                }

                while (!achouJogador && valorAtualMercado < jogador.ValorMaximo)
                {
                    VoltarTelaPesquisa();
                    valorAtualMercado = valorAtualMercado + jogador.IncrementoValor;
                    achouJogador = ConsultarValorJogador(valorAtualMercado);
                }
                if (achouJogador)
                    primeiroValorRetornado = Convert.ToInt32(this.navegador.RetornarTexto("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-pinned-list-container.SearchResults.ui-layout-left > div > ul > li.listFUTItem.has-auction-data.selected > div > div.auction > div:nth-child(3) > span.currency-coins.value").Replace(",", ""));
                else
                    primeiroValorRetornado = 0;
                VoltarTelaPesquisa();
                return primeiroValorRetornado;
            }
            catch
            {
                sucesso = false;
                return 0;
            }
            
        }
        private void LimparCamposConsulta()
        {
            this.navegador.ApagarTexto(cssValorJogador);
            this.navegador.ApagarTexto(cssNomeJogador);
        }
        private void LimparCamposConsultaLance()
        {
            this.navegador.Clicar("body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.search-prices > div:nth-child(1) > button");
            this.navegador.Clicar("body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.has-selection.contract-text-input > div > div.ut-player-search-control--input-container > button");
        }
        private void VoltarTelaPesquisa()
        {
            this.navegador.Clicar("body > main > section > section > div.ut-navigation-bar-view.navbar-style-landscape > button.ut-navigation-button-control", cssValorJogador);

            this.navegador.ApagarTexto(cssValorJogador);
            this.navegador.ApagarTexto(cssNomeJogador);

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
            {
                return true;    
                
                /*string textoTempo = this.navegador.RetornarTexto("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-pinned-list-container.SearchResults.ui-layout-left > div > ul > li.listFUTItem.has-auction-data.selected > div > div.auction > div.auction-state > span.time");
                if (!(textoTempo.Contains("Horas") || textoTempo.Contains("Hours")))
                    return true;
                else
                    return false;*/
            }
            return false;

        }
        private Int32 ConsultarValorJogadorLance(JogadorPrecoPrevisto item, out bool sucesso)
        {
            sucesso = true;
            Int32 primeiroValorRetornado = 0;
            try
            {
                this.ConsultarJogadorOverAll(item.NomeJogador, item.OverAll);
                this.navegador.DigitarTexto(cssValorLanceMaximoJogador, item.ValorMaximo.ToString());
                this.navegador.Clicar("body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.button-container > button.btn-standard.call-to-action");
                this.navegador.EsperarCarregamento(2000);
                if (this.navegador.ElementoExiste("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-navigation-container-view.ui-layout-right > div > div > div.DetailPanel > div.bidOptions > button.btn-standard.call-to-action.bidButton"))
                {
                    string textoTempo = this.navegador.RetornarTexto("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-pinned-list-container.SearchResults.ui-layout-left > div > ul > li.listFUTItem.has-auction-data.selected > div > div.auction > div.auction-state > span.time");
                    if (!textoTempo.Contains("Horas"))
                        primeiroValorRetornado = Convert.ToInt32(this.navegador.RetornarTexto("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-pinned-list-container.SearchResults.ui-layout-left > div > ul > li.listFUTItem.has-auction-data.selected > div > div.auction > div.auctionStartPrice.auctionValue > span.currency-coins.value").Replace(",", ""));

                }
                VoltarTelaPesquisa();
                LimparCamposConsultaLance();
            }
            catch (Exception ex)
            {
                sucesso = false;
            }
            return primeiroValorRetornado;
            
        }
        private void ConsultarJogador(string nomeJogador)
        {
            string cssSelectorBoxPesquisa = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.has-selection.contract-text-input.is-open > div > div.inline-list > ul > button > span.btn-text";
            this.navegador.DigitarTexto(cssNomeJogador, cssSelectorBoxPesquisa, nomeJogador);
            this.navegador.Clicar(cssSelectorBoxPesquisa);
        }
        private bool ConsultarJogadorOverAll(string nomeJogador, int overAll)
        {
            string seletorTabela = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.has-selection.contract-text-input.is-open > div > div.inline-list";

            string classeLinha = "btn-text";
                                  
            string seletorColunaNome = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.has-selection.contract-text-input.is-open > div > div.inline-list > ul > button:nth-child(<<indexLinha>>) > span.btn-text";
            string seletorOverAll = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.has-selection.contract-text-input.is-open > div > div.inline-list > ul > button:nth-child(<<indexLinha>>) > span.btn-subtext";
            List<Coluna> colunaList = new List<Coluna>();
            colunaList.Add(new Coluna(seletorColunaNome, "NomeJogador"));
            colunaList.Add(new Coluna(seletorOverAll, "OverAll"));

            this.navegador.DigitarTexto(cssNomeJogador, seletorTabela, nomeJogador);
            
            List<ItensTabela> lista = this.navegador.ConstruirTabela(seletorTabela, classeLinha, colunaList);

            int indice = lista.FindIndex(e => (e.Colunas[1].ValorColuna == overAll.ToString()));
            if (indice == -1)
                return false;
            else
                this.navegador.Clicar(seletorColunaNome.Replace("<<indexLinha>>", (indice + 1).ToString()));
            return true;
        }
        public void FecharPagina()
        {
            this.navegador.FecharPagina();
        }
    }
}

