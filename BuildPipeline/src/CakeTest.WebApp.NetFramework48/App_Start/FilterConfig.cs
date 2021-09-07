using System.Web;
using System.Web.Mvc;

namespace CakeTest.WebApp.NetFramework48
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
