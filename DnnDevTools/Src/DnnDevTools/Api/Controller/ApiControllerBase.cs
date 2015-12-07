using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Service;

namespace weweave.DnnDevTools.Api.Controller
{
    public abstract class ApiControllerBase : DnnApiController
    {

        protected static ServiceLocator ServiceLocator => ServiceLocatorFactory.Instance;

    }
}
