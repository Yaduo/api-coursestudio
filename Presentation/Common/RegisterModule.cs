using Autofac;
using Microsoft.AspNetCore.Authorization;
using CourseStudio.Presentation.Common.AuthorizationPolicies;

namespace CourseStudio.Presentation.Common
{
    public class RegisterModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccessTokenVerificationAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();

            builder.RegisterType<CouponEditAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            builder.RegisterType<CouponViewAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();

            builder.RegisterType<CourseViewAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            builder.RegisterType<CourseEditAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            builder.RegisterType<CourseAuditingAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();

            builder.RegisterType<OrderViewAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            builder.RegisterType<OrderCancelAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            builder.RegisterType<OrderRefundAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();

            builder.RegisterType<PlaylistViewAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            builder.RegisterType<PlaylistEditAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();

            builder.RegisterType<RoleViewAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            builder.RegisterType<RoleEditAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();

            builder.RegisterType<UserEditAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
            builder.RegisterType<UserViewAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();
        }
    }
}
