﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CrowlerFramework;
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
        public JogadoresLance ConsultarExisteJogadorLance(JogadorPrecoPrevisto jogador)
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

            ConsultarJogadorOverAll(jogador.NomeJogador, jogador.OverAll);
            bool achouJogador = ConsultarValorJogador(valorAtualMercado);
            while(achouJogador) //tratamento para quando o valor informado está acima do que o jogador vale de fato.
            {
                VoltarTelaPesquisa();
                valorAtualMercado = valorAtualMercado - jogador.IncrementoValor;
                achouJogador = ConsultarValorJogador(valorAtualMercado);
                if(!achouJogador)
                {
                    VoltarTelaPesquisa();
                    return valorAtualMercado + jogador.IncrementoValor; //ultimo valor achado
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
        private JogadoresLance ConsultarValorJogadorLance(JogadorPrecoPrevisto jogador)
        {
            this.ConsultarJogador(jogador.NomeJogador, jogador.IndiceJogador);
            this.navegador.DigitarTexto(cssValorLanceMaximoJogador, jogador.ValorMinimoPrevisto.ToString());
            this.navegador.Clicar("body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.button-container > button.btn-standard.call-to-action");
            this.navegador.EsperarCarregamento(2000);
            if (this.navegador.ElementoExiste("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-navigation-container-view.ui-layout-right > div > div > div.DetailPanel > div.bidOptions > button.btn-standard.call-to-action.bidButton"))
            {
                string textoTempo = this.navegador.RetornarTexto("body > main > section > section > div.ut-navigation-container-view--content > div > div > section.ut-pinned-list-container.SearchResults.ui-layout-left > div > ul > li.listFUTItem.has-auction-data.selected > div > div.auction > div.auction-state > span.time");
                if (!textoTempo.Contains("Horas"))
                    return new JogadoresLance(jogador.NomeJogador,this.navegador.RetornarQuantidadeItensTabela("listFUTItem"));
                else
                    return new JogadoresLance(jogador.NomeJogador, 0);
            }
            return new JogadoresLance(jogador.NomeJogador, 0);
        }
        private void ConsultarJogador(string nomeJogador)
        {
            string cssSelectorBoxPesquisa = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.has-selection.contract-text-input.is-open > div > div.inline-list > ul > button > span.btn-text";
            this.navegador.DigitarTexto(cssNomeJogador, cssSelectorBoxPesquisa, nomeJogador);
            this.navegador.Clicar(cssSelectorBoxPesquisa);
        }
        private void ConsultarJogador(string nomeJogador, int index)
        {
            string cssSeletorJogador = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.is-open.has-selection.contract-text-input > div > div.inline-list > ul > button:nth-child(" + index + ") > span.btn-text";
            string cssSelectorBoxPesquisa = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control.has-selection.contract-text-input.is-open > div > div.inline-list > ul > button > span.btn-text";
            
            this.navegador.DigitarTexto(cssNomeJogador, cssSelectorBoxPesquisa, nomeJogador);
            this.navegador.Clicar(cssSeletorJogador);
        }
        private void ConsultarJogadorOverAll(string nomeJogador, int overAll)
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

            int indice = lista.FindIndex(e => (e.Colunas[0].ValorColuna == nomeJogador && e.Colunas[1].ValorColuna == overAll.ToString()));
            this.navegador.Clicar(seletorColunaNome.Replace("<<indexLinha>>",(indice+1).ToString()));
        }
        public void FecharPagina()
        {
            this.navegador.FecharPagina();
        }
    }
}

