using System;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CourseStudio.Presentation.Common.AuthorizationPolicies;

namespace CourseStudioManager.Api
{
	public class RegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
			builder.RegisterType<AccessTokenVerificationAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();

            builder.RegisterModule(new CourseStudio.Presentation.Common.RegisterModule());
            builder.RegisterModule(new CourseStudioManager.Api.Services.RegisterModule());
            builder.RegisterModule(new CourseStudio.Messaging.Services.RegisterModule());
            builder.RegisterModule(new CourseStudio.Lib.Mediator.MediatorModule());
        }
    }
}
