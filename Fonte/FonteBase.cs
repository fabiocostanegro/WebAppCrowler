using CrowlerFramework;
using System;

namespace Fonte
{
    public class FonteBase
    {
        protected ICrowlerFramework navegador;
        public enum Framework
        {
            Selenium
        };
        public FonteBase(Framework framework, string caminhoProfile)
        {
            if (framework == Framework.Selenium)
            {
                navegador = new SeleniumCrowlerFramework(caminhoProfile);
            }
                
        }
        public FonteBase(Framework framework, string caminhoProfile, int numeroMaximoTentativasCarregamento)
        {
            if (framework == Framework.Selenium)
            {
                navegador = new SeleniumCrowlerFramework(caminhoProfile,numeroMaximoTentativasCarregamento);
            }

        }
        public FonteBase(Framework framework)
        {
            if (framework == Framework.Selenium)
                navegador = new SeleniumCrowlerFramework();
        }
        public FonteBase()
        {

        }
    }
}
