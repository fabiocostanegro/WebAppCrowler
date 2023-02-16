using System;
using System.Drawing;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Collections;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace CrowlerFramework
{
    public class SeleniumCrowlerFramework : ICrowlerFramework, IDisposable
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        public SeleniumCrowlerFramework(string pCaminhoProfile, int quantidadeTempoSegundosMaximoEspera)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument(pCaminhoProfile);
            //options.AddArgument("--headless");

            driver = new ChromeDriver(@"C:\Labs\WebAppCrowler\WebAppCrowler\bin\Debug\net6.0", options);
            TimeSpan tempo = new TimeSpan(0, 0, quantidadeTempoSegundosMaximoEspera);
            wait = new WebDriverWait(driver, tempo);
        }
        public SeleniumCrowlerFramework(string pCaminhoProfile)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument(pCaminhoProfile);

            driver = new ChromeDriver(@"C:\Labs\WebAppCrowler\WebAppCrowler\bin\Debug\net6.0",options);
        }
        public SeleniumCrowlerFramework()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            driver = new ChromeDriver(@"C:\Labs\WebAppCrowler\WebAppCrowler\bin\Debug\net6.0", options);
        }

        public void AcessarPagina(string url)
        {
            driver.Navigate().GoToUrl(url);
        }
        public void AcessarPagina(string url, string elementoIndicadorPaginaCompleta)
        {
            driver.Navigate().GoToUrl(url);
            AguardarElementoIndicadorPaginaCarregadaVisivel(elementoIndicadorPaginaCompleta);
        }

        public void DigitarTexto(string seletorCSS, string texto)
        {
            IWebElement element = driver.FindElement(By.CssSelector(seletorCSS));
            element.SendKeys(texto);
        }

        public void ApagarTexto(string seletorCSS)
        {
            IWebElement element = driver.FindElement(By.CssSelector(seletorCSS));
            element.Clear();
        }

        public void Clicar(string seletorCSS)
        {
            driver.FindElement(By.CssSelector(seletorCSS)).Click();
        }
        public void Clicar(string seletorCSS, string elementoIndicadorPaginaCompleta)
        {
            driver.FindElement(By.CssSelector(seletorCSS)).Click();
            AguardarElementoIndicadorPaginaCarregadaVisivel(elementoIndicadorPaginaCompleta);
        }

        public bool ElementoExiste(string seletorCSS)
        {
            try
            {
                return driver.FindElement(By.CssSelector(seletorCSS)).Displayed;
            }
            catch(Exception ex)
            {
                return false;
            }

        }
        public bool ElementoHabilitado(string seletorCSS)
        {
            try
            {
                return driver.FindElement(By.CssSelector(seletorCSS)).Enabled;
            }
            catch
            {
                return false;
            }

        }

        public void EsperarCarregamento(int tempoMs)
        {
            Thread.Sleep(tempoMs);
        }
        public void FecharPagina()
        {
            driver.Quit();
        }
        public void Dispose()
        {
            driver.Quit();
        }

        public void AguardarLoaderFinalizar(string seletorCSS)
        {

            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(seletorCSS)));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(seletorCSS)));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(seletorCSS)));
        }
        public bool AguardarElementoIndicadorPaginaCarregadaVisivel(string seletorCSS)
            {
            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector(seletorCSS)));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(seletorCSS)));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(seletorCSS)));
                return true;
            }
            catch
            {
                return false; ;
            }
        }
        public void DigitarTexto(string seletorCSS, string elementoIndicadorPaginaCompleta, string texto)
        {
            IWebElement element = driver.FindElement(By.CssSelector(seletorCSS));
            element.SendKeys(texto);
            AguardarElementoIndicadorPaginaCarregadaVisivel(elementoIndicadorPaginaCompleta);
        }
        public string RetornarTexto(string seletorCSS)
        {
            IWebElement element = driver.FindElement(By.CssSelector(seletorCSS));
            return element.Text;
        }
        public string RetornarTabela(string seletorCSS)
        {
            IWebElement element = driver.FindElement(By.CssSelector(seletorCSS));
            return element.Text;
        }
        public List<List<string>> ConstruirTabela(string seletorTabela, string seletorItens, List<string> seletorColunas)
        {
            IWebElement elementoTabela = driver.FindElement(By.CssSelector(seletorTabela));
            ReadOnlyCollection<IWebElement> elementoItens = driver.FindElements(By.CssSelector(seletorItens));
            List<List<string>> tabela = new List<List<string>>();
            for (int i = 0; i < elementoItens.Count; i++)
            {
                List<string> coluna = new List<string>();
                for(int j=0;j<seletorColunas.Count; j++)
                {
                    string valorColuna = elementoItens[i].FindElement(By.CssSelector(seletorColunas[j])).Text;
                    coluna.Add(valorColuna);
                }
            }
            return tabela;
        }
        public int RetornarQuantidadeItensTabela(string seletorCSS)
        {
            try
            {
                ReadOnlyCollection<IWebElement> element = driver.FindElements(By.ClassName(seletorCSS));
                return element.Count;
            }
            catch
            {
                throw;
            }
        }
    }
}
