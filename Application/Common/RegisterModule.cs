using System;
using Autofac;
using CourseStudio.Application.Common.Identities;

namespace CourseStudio.Application.Common
{
	public class RegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdentityService>().As<IIdentityService>();
        }
    }
}
