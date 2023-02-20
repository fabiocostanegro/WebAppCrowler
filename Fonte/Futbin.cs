using CrowlerFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte
{
    public class Futbin : FonteBase
    {
        public Futbin(Framework framework, string caminhoProfile): base(framework, caminhoProfile)
        {

        }
        public Futbin(Framework framework) : base(framework)
        {

        }
        public void AcessarFutbin(string url)
        {
            this.navegador.AcessarPagina(url);
            this.navegador.EsperarCarregamento(10000);
        }
        public List<ItensTabela> ConsultarListaJogadoresTrade(string pSeletorTabela, string pSeletorLinha, List<string> pListaSeletores, List<int> pListaIndexLinha, int incrementoLinha, List<Coluna> pColunas)
        {
            List<ItensTabela> lista = this.navegador.ConstruirTabela(pSeletorTabela, pListaSeletores, pListaIndexLinha,incrementoLinha, pColunas);
            
            this.navegador.FecharPagina();

            return lista;

        }
        
    }
}
