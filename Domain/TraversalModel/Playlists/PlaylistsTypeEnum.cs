using System;
using System.ComponentModel;

namespace CourseStudio.Domain.TraversalModel.Playlists
{
	public enum PlaylistsTypeEnum
    {
		[Description("Recommend")]
		Recommend,
		[Description("Favorite")]
		Favorite,
		[Description("Viewed")]
		Viewed,
		[Description("Other")]
		Other
    }
}
