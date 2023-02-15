using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte.ConsultasWebApp.ConsultarValorJogador
{
    public class JogadorValorMercadoAtual
    {
        public string NomeJogador;
        public Int32 ValorAtualMercado;
        public int Overall;
        public int IdJogadorFutbin;
        public JogadorValorMercadoAtual(string pNomeJogador, Int32 pValorAtualMercado)
        {
            NomeJogador = pNomeJogador;
            ValorAtualMercado = pValorAtualMercado;
        }
        public JogadorValorMercadoAtual(string pNomeJogador, Int32 pValorAtualMercado, int pOverall)
        {
            NomeJogador = pNomeJogador;
            ValorAtualMercado = pValorAtualMercado;
            Overall = pOverall;
        }
    }
}
