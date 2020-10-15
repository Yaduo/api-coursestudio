using System;
namespace CourseStudio.Doamin.Models.Identities
{
	public class IdentityTokenBlacklist: IAggregateRoot
    {
		public int Id { get; set; }
		public Guid IdentityTokenId { get; set; }
		public IdentityToken IdentityToken { get; set; }

		public IdentityTokenBlacklist()
        {
        }
    }
}
