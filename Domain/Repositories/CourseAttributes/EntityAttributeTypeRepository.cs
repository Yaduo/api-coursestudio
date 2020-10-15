using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Domain.Persistence;

namespace CourseStudio.Domain.Repositories.CourseAttributes
{
	public class EntityAttributeTypeRepository : RepositoryBase<EntityAttributeType>, IEntityAttributeTypeRepository
	{
		public EntityAttributeTypeRepository(CourseContext context)
			: base(context)
		{
		}

		public async Task<EntityAttributeType> GetEntityAttributeTypeByIdAsync(int id)
		{
			return await _context.EntityAttributeTypes.Include(t => t.EntityAttributes).SingleOrDefaultAsync(t => t.Id == id);
		}

		public async Task<IList<EntityAttributeType>> GetEntityAttributeTypesAsync()
        {
			return await _context.EntityAttributeTypes.ToListAsync();
        }


	}

}
