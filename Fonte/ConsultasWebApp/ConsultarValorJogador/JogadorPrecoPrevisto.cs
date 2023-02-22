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
        public Int32 ValorMaximo;
        public Int32 ValorAtualMercado;
        public JogadorPrecoPrevisto(string pNomeJogador, int pOverAll, string pVersao, Int32 pValorMinimoPrevisto, int pIncrementoValor, int indiceJogador, int valorMaximo, int valorAtualMercado)
        {
            NomeJogador = pNomeJogador;
            OverAll = pOverAll;
            Versao = pVersao;
            ValorMinimoPrevisto = pValorMinimoPrevisto;
            IncrementoValor = pIncrementoValor;
            IndiceJogador = indiceJogador;
            ValorMaximo = valorMaximo;
            ValorAtualMercado = valorAtualMercado;
        }
    }
}
