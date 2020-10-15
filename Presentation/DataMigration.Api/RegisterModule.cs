using Microsoft.AspNetCore.Http;
using Autofac;

namespace DataMigration.Api
{
    public class RegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();

			builder.RegisterModule(new CourseStudio.DataSeed.Services.RegisterModule());
        }
    }
}
