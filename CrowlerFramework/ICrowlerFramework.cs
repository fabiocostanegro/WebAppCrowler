using System;
using System.Collections.Generic;
using System.Text;

namespace CrowlerFramework
{
    public interface ICrowlerFramework
    {
        public void DigitarTexto(string seletorCSS, string texto);
        public void DigitarTexto(string seletorCSS, string elementoIndicadorPaginaCompleta, string texto);
        public void ApagarTexto(string seletorCSS);
        public void Clicar(string seletorCSS);
        public void Clicar(string seletorCSS, string elementoIndicadorPaginaCompleta);
        public void AcessarPagina(string url);
        public void AcessarPagina(string url, string elementoIndicadorPaginaCompleta);
        public void EsperarCarregamento(int tempoMs);
        public bool ElementoExiste(string seletorCSS);
        public bool ElementoHabilitado(string seletorCSS);
        public void FecharPagina();
        public bool AguardarElementoIndicadorPaginaCarregadaVisivel(string seletorCSS);
        public void AguardarLoaderFinalizar(string seletorCSS);
        public string RetornarTexto(string seletorCSS);
        public string RetornarTabela(string seletorCSS);
        public int RetornarQuantidadeItensTabela(string seletorCSS);
        public List<ItensTabela> ConstruirTabela(string pSeletorTabela, string pSeletorLinha, List<Coluna> pColunas);

    }
}
