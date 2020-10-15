using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;

namespace CourseStudio.Domain.Repositories.CourseAttributes
{
	public class EntityAttributeRepository: RepositoryBase<EntityAttribute>, IEntityAttributeRepository
    {
		public EntityAttributeRepository(CourseContext context)
			: base(context)
		{ 
		}

		public async Task<PagedList<EntityAttribute>> GetPagedEntityAttributesByTypeAsync(int entityAttributeTypeId, int pageNumber, int pageSize)
        {         
			var result = _context.EntityAttributes
			                     .Include(e => e.EntityAttributeType)
			                     .Include(e => e.CourseAttributes)
			                     .ThenInclude(ca => ca.Course)
			                     .Where(e => e.EntityAttributeTypeId == entityAttributeTypeId);
			return await PagedList<EntityAttribute>.Create(result.OrderBy(c => c.Id), pageNumber, pageSize);
        }
        
		public async Task<IList<EntityAttribute>> GetEntityAttributeByIdsAsync(IList<int> Ids)
		{        
			return await _context.EntityAttributes.Where(e => Ids.Contains(e.Id)).ToListAsync();
		}
        
		public async Task<IList<EntityAttribute>> GetEntityAttributeByParentIdsAsync(IList<int?> ParentIds)
        {
			IQueryable<EntityAttribute> result = _context.EntityAttributes;
			if(ParentIds == null || !ParentIds.Any()) 
			{
				result = result.Where(e => e.ParentId == null);
			} else {
				result = result.Where(e => ParentIds.Contains(e.ParentId) || e.ParentId == null);
			}
			return await result.ToListAsync();
        }

    }
}
