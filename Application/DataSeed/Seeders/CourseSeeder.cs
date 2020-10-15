using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Persistence;
using CourseStudio.Domain.Repositories.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CourseStudio.DataSeed.Services.Seeders
{
    public class CourseSeeder
    {
		private readonly CourseContext _context;
		private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userMgr;

		public CourseSeeder(
            CourseContext context,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
			_userRepository = userRepository;
            _userMgr = userManager;
        }

		public async Task Seed()
        {         
            // step 1: setup the Course Attribute
            var entityAttributeTypes = CreateEntityAttributeTypeSeed();
            if (!_context.EntityAttributeTypes.Any())
            {
                // add static data for course arrtribute
                _context.AddRange(entityAttributeTypes);
				await _context.SaveChangesAsync();
            }
            
            // step 2: setup the Entity Attribute values
            var entityAttributes = CreateEntityAttributeSeed();
            if (!_context.EntityAttributes.Any())
            {
				// TODO: Move the transaction block out!!!
				// must to use transaction + SET IDENTITY_INSERT ON to preset Id for entityAttribute datas
				using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [course].[EntityAttributes] ON");
					_context.AddRange(entityAttributes);
                    await _context.SaveChangesAsync();
					_context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [course].[EntityAttributes] OFF");
                    transaction.Commit();
                }
            }

            //// step 3: setup the courses
			var user = await _userRepository.GetUserByUserNameAsync("yaduolmailinatorcom");
            if (user == null)
            {
                return;
            }

			//var courses = CreateCourseSeed(user);
			var courses = new List<Course>
			{
				// K12
				GetCourseSeed(user, "K12/K12_Calculus_AP.json"),
				GetCourseSeed(user, "K12/K12_Chemistry_11.json"),
				GetCourseSeed(user, "K12/K12_Chemistry_AP.json"),
				GetCourseSeed(user, "K12/K12_Chemistry_IB.json"),
				GetCourseSeed(user, "K12/K12_Chemistry_SAT.json"),
				GetCourseSeed(user, "K12/K12_MATH_SAT.json"),
				GetCourseSeed(user, "K12/K12_MYP1_IB.json"),
				GetCourseSeed(user, "K12/K12_MYP2_IB.json"),
				GetCourseSeed(user, "K12/K12_MYP3_IB.json"),
				GetCourseSeed(user, "K12/K12_MYP4_IB.json"),
				GetCourseSeed(user, "K12/K12_MYP5_IB.json"),
				GetCourseSeed(user, "K12/K12_MYP5_PLUS_IB.json"),
				GetCourseSeed(user, "K12/K12_Physics_AP.json"),
				GetCourseSeed(user, "K12/K12_Physics_IB.json"),
				GetCourseSeed(user, "K12/K12_Physics_SAT.json"),

				// College
                // SFU
				GetCourseSeed(user, "College/SFU/Sfu_BUEC_232.json"),
				GetCourseSeed(user, "College/SFU/Sfu_BUEC_333.json"),
				GetCourseSeed(user, "College/SFU/Sfu_BUS_200.json"),
				GetCourseSeed(user, "College/SFU/Sfu_BUS_251.json"),
				GetCourseSeed(user, "College/SFU/Sfu_CHEM_110.json"),
				GetCourseSeed(user, "College/SFU/Sfu_CMPT_120.json"),
				GetCourseSeed(user, "College/SFU/Sfu_CMPT_165.json"),
				GetCourseSeed(user, "College/SFU/Sfu_ECON_103.json"),
				GetCourseSeed(user, "College/SFU/Sfu_ECON_105.json"),
				GetCourseSeed(user, "College/SFU/Sfu_LING_111.json"),
				GetCourseSeed(user, "College/SFU/Sfu_MATH_100.json"),
				GetCourseSeed(user, "College/SFU/Sfu_MATH_157.json"),
				GetCourseSeed(user, "College/SFU/Sfu_PHIL_105.json"),
				GetCourseSeed(user, "College/SFU/Sfu_PHIL_110.json"),
				GetCourseSeed(user, "College/SFU/Sfu_STAT_203.json"),
				// UBC
				GetCourseSeed(user, "College/UBC/Ubc_CHEM_111.json"),
				GetCourseSeed(user, "College/UBC/Ubc_CHEM_121.json"),
				GetCourseSeed(user, "College/UBC/Ubc_COMM_204.json"),
				GetCourseSeed(user, "College/UBC/Ubc_ECON_101.json"),
				GetCourseSeed(user, "College/UBC/Ubc_LPI.json"),
				GetCourseSeed(user, "College/UBC/Ubc_MATH_100.json"),
				GetCourseSeed(user, "College/UBC/Ubc_MATH_101.json"),
				GetCourseSeed(user, "College/UBC/Ubc_MATH_102.json"),
				GetCourseSeed(user, "College/UBC/Ubc_PSYC_101.json"),
				GetCourseSeed(user, "College/UBC/Ubc_STAT_200.json"),
	
                // Certifications
				GetCourseSeed(user, "Cer/CFA_1.json"),
				GetCourseSeed(user, "Cer/CFA_2.json"),
				GetCourseSeed(user, "Cer/CPA_1.json"),
				GetCourseSeed(user, "Cer/CPA_2.json"),
				GetCourseSeed(user, "Cer/CSC_Volume_1.json"),
				GetCourseSeed(user, "Cer/CSC_Volume_2.json"),
				GetCourseSeed(user, "Cer/NCA_1.json"),
				GetCourseSeed(user, "Cer/NCA_2.json"),
				GetCourseSeed(user, "Cer/Realtor_1.json")
			};

            if (!_context.Courses.Any())
            {
                // add courses
                _context.AddRange(courses);
				await _context.SaveChangesAsync();
            }
        }

        private IList<EntityAttributeType> CreateEntityAttributeTypeSeed()
        {
			var jsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/SeedData/Course/entityattributetypes.json");
            IList<EntityAttributeType> entityAttributeTypes = JsonConvert.DeserializeObject<IList<EntityAttributeType>>(jsonData);
            return entityAttributeTypes;
        }

        private IList<EntityAttribute> CreateEntityAttributeSeed()
        {
			var jsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/SeedData/Course/entityattributes.json");
            IList<EntityAttribute> entityAttributes = new List<EntityAttribute>();
            IList<dynamic> container = JsonConvert.DeserializeObject<IList<dynamic>>(jsonData);
            foreach (dynamic c in container)
            {
                EntityAttribute attribute = new EntityAttribute();
				int.TryParse(c.Id.ToString(), out int id);
				attribute.Id = id;
                attribute.Value = c.Value;
                attribute.EntityAttributeTypeId = c.EntityAttributeTypeId;
                if (c.ParentId != null)
                {
					int.TryParse(c.ParentId.ToString(), out int parentId);
					attribute.ParentId = parentId;
                }
                attribute.IsSearchable = c.IsSearchable;
                entityAttributes.Add(attribute);
            }
            return entityAttributes;
        }

   //     private IList<Course> CreateCourseSeed(ApplicationUser user)
   //     {
   //         var jsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/SeedData/Course/courses.json");
   //         IList<Course> courses = JsonConvert.DeserializeObject<IList<Course>>(jsonData);
   //         // set up the foreigner key with application user
			//courses.ToList().ForEach(c => c.TutorId = user.Tutor.Id);
        //    return courses;
        //}

		private Course GetCourseSeed(ApplicationUser user, string filePath)
        {
			var jsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/SeedData/Course/"+filePath);
            Course course = JsonConvert.DeserializeObject<Course>(jsonData);
			course.ImageUrl = "https://coursestuidostorageprd.blob.core.windows.net/courseimages/default.jpg";
			// set up the foreigner key with application user
			course.TutorId = user.Tutor.Id;
			return course;
        }

    }
}
