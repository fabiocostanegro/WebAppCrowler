using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CrowlerFramework
{
    public class FutbinSeleniumCrowler: SeleniumCrowlerFramework
    {
        public List<ItensTabela> ConstruirTabelaConsultaJogadores(string pSeletorTabela, List<string> pClasseLinha, List<int> pIndiceInicioLinha, int pIncrementoLinha, List<Coluna> pColunas)
        {
            Table tabela = new Table(pSeletorTabela, pColunas);
            List<ItensTabela> ItensList = new List<ItensTabela>();
            IWebElement elementoTabela = base.driver.FindElement(By.CssSelector(pSeletorTabela));

            ReadOnlyCollection<IWebElement> elementoItens = new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            for (int i = 0; i < pClasseLinha.Count; i++)
            {
                elementoItens = elementoTabela.FindElements(By.ClassName(pClasseLinha[i]));
                int indiceLinha = pIndiceInicioLinha[i];
                string seletorColunaTemp = string.Empty;
                for (int j = 0; j < elementoItens.Count; j++)
                {
                    ItensList.Add(new ItensTabela());
                    for (int k = 0; k < pColunas.Count; k++)
                    {
                        seletorColunaTemp = pColunas[k].SeletorColuna.Replace("<<indexLinha>>", indiceLinha.ToString());
                        Coluna colunaTemp = new Coluna(pColunas[k].SeletorColuna, pColunas[k].NomeColuna);
                        colunaTemp.ValorColuna = elementoItens[j].FindElement(By.CssSelector(seletorColunaTemp)).Text;
                        ItensList[ItensList.Count - 1].Colunas.Add(colunaTemp);
                    }
                    indiceLinha = indiceLinha + pIncrementoLinha;
                }
            }

            return ItensList;
        }
    }
}
