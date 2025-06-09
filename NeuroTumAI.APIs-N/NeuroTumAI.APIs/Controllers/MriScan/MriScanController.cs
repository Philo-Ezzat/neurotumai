using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.MriScan
{
	public class MriScanController : BaseApiController
	{
		private readonly IMriScanService _mriScanService;

		public MriScanController(IMriScanService mriScanService)
        {
			_mriScanService = mriScanService;
		}

        [Authorize(Roles = "Patient")]
		[HttpPost("upload")]
		public async Task<ActionResult> UploadMriScan([FromForm] PredictRequestDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var result = await _mriScanService.UploadAndProcessMriScanAsync(model, userId);

			return Ok(new { result.Id });
		}
	}
}
