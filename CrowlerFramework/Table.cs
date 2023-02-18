using System;
using System.Collections.Generic;
using System.Text;

namespace CrowlerFramework
{
    public  class Table
    {
        public string SeletorTabela { get; set; }
        public List<Coluna> ColunasTabela { get; set; }
        public string SeletorLinha { get; set; }
        public Table(string pSeletorTabela, string pSeletorLinha, List<Coluna> pColunas)
        {
            SeletorTabela = pSeletorTabela;
            ColunasTabela = pColunas;
            SeletorLinha = pSeletorLinha;
        }

    }
    public class Coluna
    {
        public string SeletorColuna { get; set; }
        public string NomeColuna { get; set; }
        public string ValorColuna { get; set; }
        public Coluna(string seletorColuna, string nomeColuna)
        {
            SeletorColuna = seletorColuna;
            NomeColuna = nomeColuna;
        }
    }
    public class ItensTabela
    {
        public int NumItem { get; set; }
        public List<Coluna> Colunas { get; set; }
        public ItensTabela()
        {
            Colunas = new List<Coluna>();
        }
    }
}
