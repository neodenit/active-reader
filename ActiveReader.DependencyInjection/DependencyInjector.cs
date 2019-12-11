using System.Data.Entity;
using System.Reflection;
using System.Web.Http.Dependencies;
using ActiveReader.Core;
using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using ActiveReader.Persistence;
using ActiveReader.Services;
using Autofac;
using Autofac.Integration.WebApi;

namespace ActiveReader.DependencyInjection
{
    public static class DependencyInjector
    {
        public static IDependencyResolver GetResolver(Assembly assembly)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(assembly);

            builder.RegisterType<ActiveReaderDbContext>().As<DbContext>();

            builder.RegisterType<StatRepository>().As<IStatRepository<Stat>>();
            builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IRepository<>));

            builder.RegisterType<StatManager>().As<IStatManager>();
            builder.RegisterType<Converter>().As<IConverter>();

            builder.RegisterType<QuestionsService>().As<IQuestionsService>();
            builder.RegisterType<WordsService>().As<IWordsService>();
            builder.RegisterType<ExpressionsService>().As<IExpressionsService>();

            var container = builder.Build();

            return new AutofacWebApiDependencyResolver(container);
        }
    }
}
