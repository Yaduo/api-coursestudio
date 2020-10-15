using Autofac;

namespace CourseStudio.Application.Dtos
{
    public class RegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			builder.RegisterType<ApplicationDtoMapper>();
        }
    }
}
