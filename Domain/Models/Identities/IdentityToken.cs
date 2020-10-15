using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Doamin.Models.Identities
{
	public class IdentityToken: IAggregateRoot
    {
		public Guid Id { get; set; }
		public string ApplicationUserId { get; set; }
		[MaxLength(50)]
		public string Issuer { get; set; }
		[MaxLength(50)]
		public string Audience { get; set; }
		public DateTime Expires { get; set; }

        // navigation properties
		public ApplicationUser ApplicationUser { get; set; }
		public IdentityTokenBlacklist IdentityTokenBlacklist { get; set; }

		public void Block()
		{
			IdentityTokenBlacklist = new IdentityTokenBlacklist();
		}
    }
}
