using System;

namespace weweave.DnnDevTools.Service
{
    internal abstract class ServiceBase
    {
        protected readonly IServiceLocator ServiceLocator;

        protected ServiceBase(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null) throw new ArgumentNullException("serviceLocator");
            ServiceLocator = serviceLocator;
        }

    }
}
