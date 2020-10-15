using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.CourseAttributes;

namespace CourseStudio.Api.Services.CourseAttributes
{
    public interface ICourseAttributeServices
    {
		Task<IList<EntityAttributeTypeDto>> GetEntityAttributeTypeAsync(IList<int?> parentAttributeIds);
    }
}

/*
[
    {
        Type: "Field",
        Attributes: [
            {
                Id: "365846bf-7f7d-4710-9742-7f0fdfaf31a1",
                Value: "College"
            },
            {
                Id: "365846bf-7f7d-4710-9742-7f0fdfaf31a1",
                Value: "Hight School"
            }
        ]
    },
    {
        Type: "Institution",
        Attributes: [
            {
                Id: "365846bf-7f7d-4710-9742-7f0fdfaf31a1",
                Value: "UBC"
            },
            {
                Id: "365846bf-7f7d-4710-9742-7f0fdfaf31a1",
                Value: "SFU"
            }
        ]
    },
    {
        Type: "Subject",
        Attributes: [
            {
                Id: "365846bf-7f7d-4710-9742-7f0fdfaf31a1",
                Value: "CMPT"
            },
            {
                Id: "365846bf-7f7d-4710-9742-7f0fdfaf31a1",
                Value: "ECON"
            }
        ]
    },
    {
        Type: "Level",
        Attributes: [
            {
                Id: "365846bf-7f7d-4710-9742-7f0fdfaf31a1",
                Value: "100"
            },
            {
                Id: "365846bf-7f7d-4710-9742-7f0fdfaf31a1",
                Value: "200"
            }
        ]
    }
]
*/
