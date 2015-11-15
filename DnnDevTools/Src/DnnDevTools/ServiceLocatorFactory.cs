using System.Web;
using weweave.DnnDevTools.Service;

namespace weweave.DnnDevTools
{
    internal class ServiceLocatorFactory
    {
        private const string HttpContextItemsKey = "DnnDevToolsServiceLocator";

        public static ServiceLocator Instance
        {
            get
            {
                var serviceLocator = HttpContext.Current.Items[HttpContextItemsKey] as ServiceLocator;
                if (serviceLocator != null) return serviceLocator;

                serviceLocator = new ServiceLocator();
                HttpContext.Current.Items[HttpContextItemsKey] = serviceLocator;

                return serviceLocator;
            }
        }
    }
}
