using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using MediatR;
using CourseStudio.Application.Events.Coupons;
using CourseStudio.Application.Events.Courses;
using CourseStudio.Application.Events.Identities;
using CourseStudio.Messaging.Services.Emails;
using CourseStudio.Messaging.Services.Emails.EventHandlers;

namespace CourseStudio.Messaging.Services
{
	public class RegisterModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailService>().As<IEmailService>();

            // Event Handlers
			builder.RegisterAssemblyTypes(typeof(CourseReadyEventHandler).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))).Select(i => new KeyedService("IAsyncNotificationHandler", i)))
			       .As<IAsyncNotificationHandler<CourseReadyEvent>>();

			// Event Handlers
            builder.RegisterAssemblyTypes(typeof(NewUserRegisteredEventHandler).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))).Select(i => new KeyedService("IAsyncNotificationHandler", i)))
                   .As<IAsyncNotificationHandler<NewUserRegisteredEvent>>();

			// Event Handlers
			builder.RegisterAssemblyTypes(typeof(CouponDistributionHandler).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))).Select(i => new KeyedService("IAsyncNotificationHandler", i)))
			       .As<IAsyncNotificationHandler<DistributeCouponEvent>>();

            // Event Handlers
            builder.RegisterAssemblyTypes(typeof(ApplyTutorEventHandler).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces().Where(i => i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))).Select(i => new KeyedService("IAsyncNotificationHandler", i)))
                   .As<IAsyncNotificationHandler<ApplyTutorEvent>>();
        }
    }
}
