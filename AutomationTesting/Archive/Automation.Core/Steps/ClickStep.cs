using System.Xml;
using OpenQA.Selenium.Remote;

namespace Automation.UI.Steps
{
    class ClickStep: BaseStep
    {
        public ClickStep(XmlNode node, RemoteWebDriver driver) : base(node, driver)
        {

        }

        public override void Execute()
        {
            base.Execute();
            //Element.Click(); 
            //The implementation below is used to avoid the "could not be scrolled into view" error
            Driver.ExecuteScript("arguments[0].click();", Element);
        }
    }
}
