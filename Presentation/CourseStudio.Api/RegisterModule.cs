using Microsoft.AspNetCore.Http;
using Autofac;

namespace CourseStudio.Api
{
    public class RegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();

            builder.RegisterModule(new CourseStudio.Presentation.Common.RegisterModule());
            builder.RegisterModule(new CourseStudio.Api.Services.RegisterModule());
			builder.RegisterModule(new CourseStudio.Messaging.Services.RegisterModule());
			builder.RegisterModule(new CourseStudio.Lib.Mediator.MediatorModule());
        }
    }
}
