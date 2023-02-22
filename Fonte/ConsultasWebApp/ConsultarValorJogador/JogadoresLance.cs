using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte.ConsultasWebApp.ConsultarValorJogador
{
    public class JogadoresLance
    {
        public string NomeJogador { get; set; }
        public int OverAll { get; set; }
        public int QuantidadeCartas { get; set; }
        public int ValorAtualMercado { get; set; }
        public int ValorOportunidadeLance { get; set; }

        public JogadoresLance(string nomeJogador, int quantidadeCartas, int overAll, int valorAtualMercado, int valorOportunidadeLance)
        {
            NomeJogador = nomeJogador;
            QuantidadeCartas = quantidadeCartas;
            OverAll = overAll;
            ValorAtualMercado = valorAtualMercado;
            ValorOportunidadeLance = valorOportunidadeLance;
        }
    }
}
