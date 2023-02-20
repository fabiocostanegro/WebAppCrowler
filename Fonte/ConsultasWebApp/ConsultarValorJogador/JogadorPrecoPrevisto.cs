using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte.Consultas.ConsultaValorJogador
{
    public class JogadorPrecoPrevisto
    {
        public string NomeJogador;
        public int OverAll;
        public string Versao;
        public Int32 ValorMinimoPrevisto;
        public int IncrementoValor;
        public int IndiceJogador;
        public JogadorPrecoPrevisto(string pNomeJogador, int pOverAll, string pVersao, Int32 pValorMinimoPrevisto, int pIncrementoValor, int indiceJogador)
        {
            NomeJogador = pNomeJogador;
            OverAll = pOverAll;
            Versao = pVersao;
            ValorMinimoPrevisto = pValorMinimoPrevisto;
            IncrementoValor = pIncrementoValor;
            IndiceJogador = indiceJogador;
        }
    }
}
