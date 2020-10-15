using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.CourseAttributes
{
	public interface IEntityAttributeRepository: IRepository<EntityAttribute> 
    {
		Task<PagedList<EntityAttribute>> GetPagedEntityAttributesByTypeAsync(int entityAttributeTypeId, int pageNumber, int pageSize);
		Task<IList<EntityAttribute>> GetEntityAttributeByIdsAsync(IList<int> ids);
		Task<IList<EntityAttribute>> GetEntityAttributeByParentIdsAsync(IList<int?> parentIds);
    }
}
