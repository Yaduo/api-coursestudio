using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using MediatR;
using CourseStudio.Domain.Events.Auditing;
using CourseStudio.Domain.Events.Course;
using CourseStudio.Domain.Services.Auditing;
using CourseStudio.Domain.Services.Trades;
using CourseStudio.Domain.Events.Trades;
using CourseStudio.Domain.Services.Courses;
using CourseStudio.Domain.Services.Users;

namespace CourseStudio.Domain.Services
{
    public class RegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			builder.RegisterAssemblyTypes(typeof(CourseStateUpdateEventHandler).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))).Select(i => new KeyedService("IAsyncNotificationHandler", i)))
			       .As<IAsyncNotificationHandler<CourseStateUpdateDomainEvent>>();

			builder.RegisterAssemblyTypes(typeof(CourseAuditingUpdateEventHandler).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))).Select(i => new KeyedService("IAsyncNotificationHandler", i)))
			       .As<IAsyncNotificationHandler<CourseAuditingUpdateDomainEvent>>();

            // Register event handler
			builder.RegisterType<UserCourseEnrollWhenOrderCompleteEventHandler>()
			       .As<IAsyncNotificationHandler<OrderCompleteDomainEvent>>();

            builder.RegisterType<ShoppingCartClearWhenOrderCompleteEventHandler>()
                   .As<IAsyncNotificationHandler<OrderCompleteDomainEvent>>();
        }
    }
}
