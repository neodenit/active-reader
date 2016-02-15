using ActiveReader.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ActiveReader.Web
{
    public static class DependenciesConfig
    {
        public static void RegisterDependencies()
        {
            var config = GlobalConfiguration.Configuration;

            var assembly = Assembly.GetExecutingAssembly();

            var resolver = DependencyInjector.GetResolver(assembly);

            config.DependencyResolver = resolver;
        }
    }
}