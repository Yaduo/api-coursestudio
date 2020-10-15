using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using CourseStudio.Doamin.Models;

namespace CourseStudio.Domain.Persistence
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, CourseContext ctx)
        {
            var domainEntities = ctx.ChangeTracker.Entries<Entity>().Where(x => x.Entity.DomainEvents() != null && x.Entity.DomainEvents().Any());
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents()).ToList();
            domainEntities.ToList().ForEach(entity => entity.Entity.DomainEvents().Clear());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishAsync(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
