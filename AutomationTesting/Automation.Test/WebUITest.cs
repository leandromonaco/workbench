using Automation.UI;
using Xunit;

namespace Automation.Test
{
    public class WebUiTest
    {
        [Fact]
        public void SearchOnGoogle()
        {
            FirefoxWebAutomation firefoxWebAutomation = new FirefoxWebAutomation(@"C:\GitHub\Automation\Automation\Automation.Test\Scenarios\Scenario_1.xml", 
                                                                                 @"C:\GitHub\Automation\Automation\Automation.Test\Drivers\");
            firefoxWebAutomation.RunProcess();
        }
    }
}
