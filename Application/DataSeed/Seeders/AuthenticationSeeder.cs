using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Persistence;
using CourseStudio.Domain.TraversalModel.Identities;
using Microsoft.AspNetCore.Identity;

namespace CourseStudio.DataSeed.Services.Seeders
{
	public class AuthenticationSeeder
    {
		private readonly CourseContext _context;
        private readonly UserManager<ApplicationUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;

		public AuthenticationSeeder(
            CourseContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _context = context;
            _userMgr = userManager;
            _roleMgr = roleManager;
        }

		public async Task Seed()
        {
            await SeedRoles();
            await SeedUsers();
        }

        private async Task SeedRoles()
        {
            var staffRole = await _roleMgr.FindByNameAsync(ApplicationPolicies.DefaultRoles.Staff);
            if (staffRole == null)
            {
                staffRole = new IdentityRole(ApplicationPolicies.DefaultRoles.Staff);
                await _roleMgr.CreateAsync(staffRole);
            }

            var rootRole = await _roleMgr.FindByNameAsync(ApplicationPolicies.DefaultRoles.Root);
            if (rootRole == null)
            {
                rootRole = new IdentityRole(ApplicationPolicies.DefaultRoles.Root);
                await _roleMgr.CreateAsync(rootRole);
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.CourseMgnt.View));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.CourseMgnt.Auditing));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.CouponMgnt.View));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.CouponMgnt.Edit));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.OrderMgnt.View));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.OrderMgnt.Cancel));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.OrderMgnt.Refund));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.PlaylistMgnt.View));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.PlaylistMgnt.Edit));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.RoleMgnt.View));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.RoleMgnt.Edit));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.UserMgnt.View));
                await _roleMgr.AddClaimAsync(rootRole, new Claim("scopes", ApplicationPolicies.Claims.UserMgnt.Edit));
            }

            var tutorRole = await _roleMgr.FindByNameAsync(ApplicationPolicies.DefaultRoles.Tutor);
            if (tutorRole == null)
            {
                tutorRole = new IdentityRole(ApplicationPolicies.DefaultRoles.Tutor);
                await _roleMgr.CreateAsync(tutorRole);
                await _roleMgr.AddClaimAsync(tutorRole, new Claim("scopes", ApplicationPolicies.Claims.CourseMgnt.View));
                await _roleMgr.AddClaimAsync(tutorRole, new Claim("scopes", ApplicationPolicies.Claims.CourseMgnt.Edit));
            }

            var studentRole = await _roleMgr.FindByNameAsync(ApplicationPolicies.DefaultRoles.Student);
            if (studentRole == null)
            {
                studentRole = new IdentityRole(ApplicationPolicies.DefaultRoles.Student);
                await _roleMgr.CreateAsync(studentRole);
                //await _roleMgr.AddClaimAsync(studentRole, new Claim("scopes", ApplicationPolicies.Claims.CourseMgnt.View));
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
			var founder = await _userMgr.FindByNameAsync("yaduolhotmailcom");
            if (founder == null)
            {
				founder = await ApplicationUser.CreateAsync(
                    _userMgr, 
                    "yaduol@hotmail.com", 
                    "Course123$", 
                    "Founder", 
                    "Co", 
                    "default.jpg",
                    new List<string>() { ApplicationPolicies.DefaultRoles.Root, ApplicationPolicies.DefaultRoles.Staff }
                    );
				founder.EmailConfirmed = true;
				var adminModel = Administrator.Create();
				founder.Administrator = adminModel;
				founder.Administrator.Activate();
				await _context.SaveChangesAsync();
            }

			var tutor = await _userMgr.FindByNameAsync("yaduolmailinatorcom");
            if (tutor == null)
            {
				tutor = await ApplicationUser.CreateAsync(
                    _userMgr, 
                    "yaduol@mailinator.com",
                    "Course123$", 
                    "Tutor", 
                    "Co", 
                    "default.jpg",
                    new List<string>() { ApplicationPolicies.DefaultRoles.Tutor }
                    );
				tutor.EmailConfirmed = true;
				tutor.Tutor = Tutor.Create("Hello! I am the first tutor.", 0.5);
                tutor.Tutor.Apply();
                tutor.Tutor.Approve(founder, "default approved");
                await _context.SaveChangesAsync();
            }

			var tutor2 = await _userMgr.FindByNameAsync("tutormailinatorcom");
			if (tutor2 == null)
            {
                tutor2 = await ApplicationUser.CreateAsync(
                    _userMgr,
                    "tutor@mailinator.com",
                    "Course123$",
                    "Tutor2",
                    "Co2",
                    "default.jpg",
                    new List<string>() { ApplicationPolicies.DefaultRoles.Tutor }
                    );
				tutor2.EmailConfirmed = true;
				tutor2.Tutor = Tutor.Create("Hello! I am the first tutor.", 0.5);
                tutor2.Tutor.Apply();
                tutor2.Tutor.Approve(founder, "default approved");
                await _context.SaveChangesAsync();
            }

			var student = await _userMgr.FindByNameAsync("aliumailinatorcom");
            if (student == null)
            {
				student = await ApplicationUser.CreateAsync(
                    _userMgr, 
                    "aliu@mailinator.com",
                    "Course123$", 
                    "Student", 
                    "Co", 
                    "default.jpg"
                    );
				student.EmailConfirmed = true;
				await _context.SaveChangesAsync();
            }
        }
    }
}
