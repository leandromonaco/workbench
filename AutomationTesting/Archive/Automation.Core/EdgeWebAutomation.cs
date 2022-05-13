using OpenQA.Selenium.Edge;

namespace Automation.UI
{
    public class EdgeWebAutomation : BaseWebAutomation
    {
        public EdgeWebAutomation(string testPath, string driverPath)
        {
            base.Clean("MicrosoftWebDriver");
            Options = new EdgeOptions();
            Driver = new EdgeDriver(driverPath, (EdgeOptions)Options);
            TestPath = testPath;
        }
    }
}
