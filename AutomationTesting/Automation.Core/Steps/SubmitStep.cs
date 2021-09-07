using System.Xml;
using OpenQA.Selenium.Remote;

namespace Automation.UI.Steps
{
    class SubmitStep : BaseStep
    {
        public SubmitStep(XmlNode node, RemoteWebDriver driver) : base(node, driver)
        {

        }

        public override void Execute()
        {
            base.Execute();
            Element.Submit();
        }
    }
}
