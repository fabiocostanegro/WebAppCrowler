using System;
using System.Collections.Generic;
using System.Text;

namespace Fonte.ConsultasFutBin
{
    public static class Util
    {
        public static int FormatarValorJogador(string valorJogador)
        {
            string valorJogadorFormatado = valorJogador.Replace(".","");
            
            if (valorJogador.Contains("."))
                valorJogadorFormatado = valorJogadorFormatado.Replace("K", "00").Replace("M", "00000");
            else
                valorJogadorFormatado = valorJogadorFormatado.Replace("K", "000").Replace("M", "000000");
            
            return Convert.ToInt32(valorJogadorFormatado);
        }
    }
}
