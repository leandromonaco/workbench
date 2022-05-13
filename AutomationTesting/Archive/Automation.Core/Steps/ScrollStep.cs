using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Automation.UI.Steps
{
    class ScrollStep: BaseStep
    {
        public ScrollStep(XmlNode node, RemoteWebDriver driver) : base(node, driver)
        {

        }

        public override void Execute()
        {
            base.Execute();
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView();", Element);
        }
    }
}
