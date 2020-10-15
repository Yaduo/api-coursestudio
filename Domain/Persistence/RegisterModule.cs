using Autofac;
using Microsoft.EntityFrameworkCore;

namespace CourseStudio.Domain.Persistence
{
    public class RegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var services = new ServiceCollection();

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<CourseContext>()
            //    .AddDefaultTokenProviders();


            //builder.Populate(services);


            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<CourseContext>();
                return optionsBuilder.Options;
            }).AsSelf().InstancePerLifetimeScope();

            // DbContext
            builder.RegisterType<CourseContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

			builder.RegisterModule(new CourseStudio.Lib.Mediator.MediatorModule());
        }
    }
}
