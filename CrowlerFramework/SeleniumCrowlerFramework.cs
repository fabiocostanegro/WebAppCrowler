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
            driver = new ChromeDriver(@"C:\Labs\WebAppCrowler\WebAppCrowler\bin\Debug\net6.0", RetornarOptionsAntiCaptcha(pCaminhoProfile));
            TimeSpan tempo = new TimeSpan(0, 0, quantidadeTempoSegundosMaximoEspera);
            wait = new WebDriverWait(driver, tempo);
        }
        public SeleniumCrowlerFramework(string pCaminhoProfile)
        {
            driver = new ChromeDriver(@"C:\Labs\WebAppCrowler\WebAppCrowler\bin\Debug\net6.0",RetornarOptionsAntiCaptcha(pCaminhoProfile));
        }
        public SeleniumCrowlerFramework()
        {
            driver = new ChromeDriver(@"C:\Labs\WebAppCrowler\WebAppCrowler\bin\Debug\net6.0", RetornarOptionsAntiCaptcha(string.Empty));
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
        public List<ItensTabela> ConstruirTabela(string pSeletorTabela, string pSeletorLinha, List<Coluna> pColunas)
        {
            Table tabela = new Table(pSeletorTabela, pSeletorLinha, pColunas);
            List<ItensTabela> ItensList = new List<ItensTabela>();
            IWebElement elementoTabela = driver.FindElement(By.CssSelector(pSeletorTabela));
            ReadOnlyCollection<IWebElement> elementoItens = elementoTabela.FindElements(By.CssSelector(pSeletorLinha));
            for (int i = 0; i < elementoItens.Count; i++)
            {
                foreach (Coluna item in pColunas)
                {
                    item.ValorColuna = elementoItens[i].FindElement(By.CssSelector(item.SeletorColuna)).Text;
                    ItensList.Add(new ItensTabela());
                    ItensList[i].Colunas.Add(item); 
                }
                
            }
            return ItensList;
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
        private string RetornaUserAgent()
        {
            Random r = new Random();
            int rInt = r.Next(0, 4);
            List<string> lista = new List<string>();
            lista.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            lista.Add("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36");
            lista.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            lista.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
            lista.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
            return lista[rInt];

        }
        private ChromeOptions RetornarOptionsAntiCaptcha(string pCaminhoProfile)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument(pCaminhoProfile);
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            options.AddArgument("--user-agent=" + RetornaUserAgent());
            options.AddArgument("--disable-blink-features");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArguments(new List<string>() { "enable-automation" });
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            return options;
        }
    }
}
