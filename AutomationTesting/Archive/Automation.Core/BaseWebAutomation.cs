using System;
using System.Diagnostics;
using System.Xml;
using Automation.UI.Steps;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Automation.UI
{
    public abstract class BaseWebAutomation
    {
        public RemoteWebDriver Driver { get; set; }
        public DriverOptions Options { get; set; }
        public string TestPath { get; set; }

        public void RunProcess()
        {
            try
            {
                int timeOutInSeconds = 3600; //1 hour

                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutInSeconds);
                Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeOutInSeconds);
                Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(timeOutInSeconds);

                var xml = new XmlDocument();

                xml.Load(TestPath);

                foreach (XmlNode node in xml.ChildNodes[0].ChildNodes)
                {
                    if (!node.Name.Equals("Step") && !node.Name.Equals("Loop")) continue;


                    IStep step;

                    switch (node.Name)
                    {
                        case "Step":
                            step = GetStepObject(node, Driver);
                            step.Execute();
                            break;
                        case "Loop":
                            var stopwatch = new Stopwatch();
                            stopwatch.Start();
                            int durationInMinutes = int.Parse(node.Attributes["DurationInMinutes"].Value);
                            while (stopwatch.Elapsed.Minutes < durationInMinutes)
                            {
                                foreach (XmlNode loopnode in node.ChildNodes)
                                {
                                    if (loopnode.Name.Equals("Step"))
                                    {
                                        step = GetStepObject(loopnode, Driver);
                                        step.Execute();
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IStep GetStepObject(XmlNode node, RemoteWebDriver driver)
        {
            BaseStep step = null;
            //TODO: Improve this with OOD
            switch (node.Attributes["type"].Value)
            {
                case "Navigation":
                    step = new NavigationStep(node, driver);
                    break;
                case "Checkbox":
                case "Button":
                case "Link":
                case "RadioButton":
                    step = new ClickStep(node, driver);
                    break;
                case "Submit":
                    step = new SubmitStep(node, driver);
                    break;
                case "Combobox":
                case "Textbox":
                    step = new InputStep(node, driver);
                    break;
                case "ScrollTo":
                    step = new ScrollStep(node, driver);
                    break;
            }

            return step;

        }

        public void Clean(string processName)
        {
            foreach (Process proc in Process.GetProcessesByName(processName))
            {
                proc.Kill();
            }
        }
    }
}
