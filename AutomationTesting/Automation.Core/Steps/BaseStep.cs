using System.Threading;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Automation.UI.Steps
{
    public abstract class BaseStep :IStep
    {
        protected BaseStep(XmlNode node, RemoteWebDriver driver)
        {
            this.Driver = driver;
            this.Type = node.Attributes["type"] != null ? node.Attributes["type"].Value : string.Empty;
            this.Value = node.Attributes["value"] != null ? node.Attributes["value"].Value : string.Empty;
            this.SearchBy = node.Attributes["searchBy"] != null ? node.Attributes["searchBy"].Value : string.Empty;
            this.Key = node.Attributes["key"] != null ? node.Attributes["key"].Value : string.Empty;
            this.Wait = node.Attributes["wait"] != null ? int.Parse(node.Attributes["wait"].Value) : 0;
        }

        public virtual void Execute()
        {
            Thread.Sleep(Wait*60);
            if (!string.IsNullOrEmpty(SearchBy) && !string.IsNullOrEmpty(Key))
            {
                var searchByCriteria = GetSearchByCriteria(SearchBy, Key);
                Element = GetElement(searchByCriteria);
            }
        }

        public RemoteWebDriver Driver { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string SearchBy { get; set; }
        public string Key { get; set; }
        public int Wait { get; set; }
        public IWebElement Element { get; set; }

        protected By GetSearchByCriteria(string searchBy, string key)
        {
            //TODO: Improve this with OOD
            By SearchByCriteria = null;
            switch (searchBy)
            {
                case "id":
                    SearchByCriteria = By.Id(key);
                    break;
                case "name":
                    SearchByCriteria = By.Name(key);
                    break;
                case "LinkText":
                    SearchByCriteria = By.LinkText(key);
                    break;
                case "PartialLinkText":
                    SearchByCriteria = By.PartialLinkText(key);
                    break;
                case "xpath":
                    SearchByCriteria = By.XPath(key);
                    break;
                case "class":
                    SearchByCriteria = By.ClassName(key);
                    break;
                case "tagName":
                    SearchByCriteria = By.TagName(key);
                    break;
                default:
                    break;
            }
            return SearchByCriteria;
        }

        protected IWebElement GetElement(By searchByCriteria)
        {
            return Driver.FindElement(searchByCriteria);
        }
    }
}
