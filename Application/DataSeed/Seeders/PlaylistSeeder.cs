using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Playlists;
using CourseStudio.Domain.Persistence;
using Newtonsoft.Json;

namespace CourseStudio.DataSeed.Services.Seeders
{
    public class PlaylistSeeder
    {
		private CourseContext _context;

		public PlaylistSeeder(
            CourseContext context
        )
        {
            _context = context;
        }

		public async Task Seed()
        {
            var playlists = CreatePlaylistSeed();
            _context.AddRange(playlists);
			await _context.SaveChangesAsync();
        }

        private IList<Playlist> CreatePlaylistSeed()
        {
			var jsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/SeedData/Playlist/playlist.json");
            var playlists = JsonConvert.DeserializeObject<IList<Playlist>>(jsonData);
            return playlists;
        }
    }
}
