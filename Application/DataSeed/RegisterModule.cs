using System;
using Autofac;

namespace CourseStudio.DataSeed.Services
{
	public class RegisterModule: Autofac.Module
    {
		protected override void Load(ContainerBuilder builder)
		{ 
			builder.RegisterType<DatabaseInitializeService>().As<IDatabaseInitializeService>();
		
			builder.RegisterModule(new CourseStudio.Domain.Repositories.RegisterModule());
		}
    }
}
