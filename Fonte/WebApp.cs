using Fonte.Consultas.ConsultaValorJogador;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte
{
    public class WebApp: FonteBase
    {
        private string url = "https://www.easports.com/fifa/ultimate-team/web-app/";
        protected string cssValorJogador = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.search-prices > div:nth-child(6) > div.ut-numeric-input-spinner-control > input";
        protected string cssValorLanceMaximoJogador = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.search-prices > div:nth-child(3) > div.ut-numeric-input-spinner-control > input";
        protected string cssNomeJogador = "body > main > section > section > div.ut-navigation-container-view--content > div > div.ut-pinned-list-container.ut-content-container > div > div.ut-pinned-list > div.ut-item-search-view > div.inline-list-select.ut-player-search-control > div > div.ut-player-search-control--input-container > input";
        public WebApp(Framework framework, string caminhoProfile) : base(framework, caminhoProfile)
        {
            
        }
        public WebApp(Framework framework, string caminhoProfile, int quantidadeTempoSegundosMaximoEspera) : base(framework, caminhoProfile, quantidadeTempoSegundosMaximoEspera)
        {

        }
        public void AcessarWebAPP()
        {
            string seletorBotaoLogin = "#Login > div > div > button.btn-standard.call-to-action";
            string seletorLoaderPagina = "body > div.ut-click-shield.showing > img";
            string selectorCssPaginaCarregada = "body > main > section > section > div.ut-navigation-bar-view.navbar-style-landscape.currency-purchase > h1";
            

            this.navegador.AcessarPagina(url);
            if (this.navegador.AguardarElementoIndicadorPaginaCarregadaVisivel(seletorBotaoLogin))
            {
                Logar();
            }
            else if(!this.navegador.AguardarElementoIndicadorPaginaCarregadaVisivel(selectorCssPaginaCarregada))
                throw new Exception("Não foi possível fazer login");
                
        }
        public void Logar()
        {
            string selectorHomeWebAPPLogado = "body > main > section > section > div.ut-navigation-bar-view.navbar-style-landscape > h1";
            string seletorBotaoLogin = "#Login > div > div > button.btn-standard.call-to-action";
            this.navegador.Clicar(seletorBotaoLogin, selectorHomeWebAPPLogado);
  

        }
        public void AcessarMenuTransferencias()
        {
            string seletorMenuTransferencia = "body > main > section > nav > button.ut-tab-bar-item.icon-transfer";
                                              //"body > main > section > nav > button.ut-tab-bar-item.icon-transfer"
            string seletorBotaoAcessoPaginaConsultarMercadoTransferencias = "body > main > section > section > div.ut-navigation-container-view--content > div > div > div.tile.col-1-1.ut-tile-transfer-market > div.tileContent";
            string seletorPaginaConsultarMercadoTransferencias = "body > main > section > section > div.ut-navigation-bar-view.navbar-style-landscape > h1";
            this.navegador.Clicar(seletorMenuTransferencia, seletorBotaoAcessoPaginaConsultarMercadoTransferencias);
            this.navegador.Clicar(seletorBotaoAcessoPaginaConsultarMercadoTransferencias, seletorPaginaConsultarMercadoTransferencias);
            
        }
        public void ReiniciarFluxo()
        {
            string seletorInicioFluxo = "body > main > section > nav > button.ut-tab-bar-item.icon-home";
            string seletorPaginaCarregada = "body > main > section > section > div.ut-navigation-bar-view.navbar-style-landscape.currency-purchase > h1";
            this.navegador.Clicar(seletorInicioFluxo, seletorPaginaCarregada);
        }

    }
}
