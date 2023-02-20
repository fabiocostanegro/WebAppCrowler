using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte.ConsultasWebApp.ConsultarValorJogador
{
    public class JogadoresLance
    {
        public string NomeJogador { get; set; }
        public int QuantidadeCartas { get; set; }

        public JogadoresLance(string nomeJogador, int quantidadeCartas)
        {
            NomeJogador = nomeJogador;
            QuantidadeCartas = quantidadeCartas;        
        }
    }
}
