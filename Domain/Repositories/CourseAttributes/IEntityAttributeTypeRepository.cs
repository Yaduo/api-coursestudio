using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.CourseAttributes;

namespace CourseStudio.Domain.Repositories.CourseAttributes
{
	public interface IEntityAttributeTypeRepository: IRepository<EntityAttributeType> 
    {
		Task<EntityAttributeType> GetEntityAttributeTypeByIdAsync(int id);
		Task<IList<EntityAttributeType>> GetEntityAttributeTypesAsync();
    }
}
