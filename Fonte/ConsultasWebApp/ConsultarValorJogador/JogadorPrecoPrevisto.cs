using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte.Consultas.ConsultaValorJogador
{
    public class JogadorPrecoPrevisto
    {
        public string NomeJogador;
        public Int32 ValorMinimoPrevisto;
        public int IncrementoValor;
        public int IndiceJogador;
        public JogadorPrecoPrevisto(string pNomeJogador, Int32 pValorMinimoPrevisto, int pIncrementoValor, int indiceJogador)
        {
            NomeJogador = pNomeJogador;
            ValorMinimoPrevisto = pValorMinimoPrevisto;
            IncrementoValor = pIncrementoValor;
            IndiceJogador = indiceJogador;
        }
    }
}
