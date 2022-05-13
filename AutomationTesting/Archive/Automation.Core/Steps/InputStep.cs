using System;
using System.Xml;
using OpenQA.Selenium.Remote;

namespace Automation.UI.Steps
{
    public class InputStep : BaseStep
    {
        public InputStep(XmlNode node, RemoteWebDriver driver) : base(node,driver)
        {
            
        }

        public override void Execute()
        {
            base.Execute();
            Element.Clear();
            if (Value.Equals("{RandomValue}"))
            {
                var rnd = new Random();
                var randomNumber = rnd.Next(999);
                var random = "random" + randomNumber;
                Element.SendKeys(random);
            }
            else
            {
                Element.SendKeys(Value);
            }
        }
    }
}
