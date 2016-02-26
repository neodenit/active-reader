﻿using ActiveReader.Core;
using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using ActiveReader.Persistence;
using ActiveReader.Services;
using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            builder.RegisterType<ActiveReaderDbContext>().As<DbContext>();
            builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType<StatCollector>().As<IStatCollector>();
            builder.RegisterType<ArticleConverter>().As<IArticleConverter>();
            builder.RegisterType<QuestionsService>().As<IQuestionsService>();

            var container = builder.Build();

            return new AutofacWebApiDependencyResolver(container);
        }
    }
}
