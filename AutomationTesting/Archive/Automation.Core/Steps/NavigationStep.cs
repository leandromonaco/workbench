using System.Xml;
using OpenQA.Selenium.Remote;

namespace Automation.UI.Steps
{
    public class NavigationStep:BaseStep
    {
        public NavigationStep(XmlNode node, RemoteWebDriver driver) : base(node,driver)
        {
            
        }

        public override void Execute()
        {
            base.Execute();
            Driver.Navigate().GoToUrl(Value);
        }
    }
}
