using OpenQA.Selenium.Chrome;

namespace Automation.UI
{
    public class ChromeWebAutomation : BaseWebAutomation
    {
        public ChromeWebAutomation(string testPath, string driverPath)
        {
            base.Clean("chromedriver");
            Options = new ChromeOptions();
            TestPath = testPath;

            ((ChromeOptions)Options).AddArguments("--disable-gpu");
            ((ChromeOptions)Options).AddArguments("--disable-software-rasterizer");
            ((ChromeOptions)Options).AddArguments("--start-maximized");
            //TODO: Investigate this issue further https://stackoverflow.com/questions/49094399/chromedriver-showing-lost-ui-shared-context

            Driver = new ChromeDriver(driverPath, (ChromeOptions)Options);
        }
    }
}
