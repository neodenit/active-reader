using ActiveReader.Core;
using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using ActiveReader.Persistence;
using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace ActiveReader.DependencyInjection
{
    public static class DependencyInjector
    {
        public static IDependencyResolver GetResolver(Assembly assembly)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(assembly);

            builder.RegisterType<ArticleRepositoryEF>().As<IRepository<Article>>();
            builder.RegisterType<StatRepositoryEF>().As<IRepository<Stat>>();
            builder.RegisterType<StatCollector>().As<IStatCollector>();

            var container = builder.Build();

            return new AutofacWebApiDependencyResolver(container);
        }
    }
}
