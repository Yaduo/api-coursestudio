using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.DataSeed.Services;
using CourseStudio.Presentation.Common;

namespace DataMigration.Api.Controllers
{
	[Produces("application/json")]
	[Route("api/migrations")]
	public class MigrationsController: BaseController
    {
		private readonly IDatabaseInitializeService _databaseInitializeService;

		public MigrationsController
        (
			IDatabaseInitializeService databaseInitializeService,
            ILogger<MigrationsController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_databaseInitializeService = databaseInitializeService;
        }

		// GET api/migrations/ensure
        [HttpGet("ensure")]
		public async Task<IActionResult> EnsureData()
        {
            try
            {
				await _databaseInitializeService.EnsureCreated();
				await _databaseInitializeService.Seed();

				return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"EnsureData() Error: {ex}");
                return StatusCode(500, "Internal Server Error " + ex.Message);
            }

        }
    }
}
