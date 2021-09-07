using OpenQA.Selenium.Firefox;

namespace Automation.UI
{
    public class FirefoxWebAutomation : BaseWebAutomation
    {
        public FirefoxWebAutomation(string testPath, string driverPath)
        {
            base.Clean("geckodriver");
            base.Clean("firefox");
            Options = new FirefoxOptions();
            TestPath = testPath;

            ((FirefoxOptions)Options).AddAdditionalCapability("acceptInsecureCerts", true, true);
            Driver = new FirefoxDriver(driverPath, (FirefoxOptions) Options);
        }
    }
}
