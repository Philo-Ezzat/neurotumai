using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Entities.MriScan;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IMriScanService
	{
		Task<MriScan> UploadAndProcessMriScanAsync(PredictRequestDto model, string userId);
		Task<IReadOnlyList<MriScan>> GetExpiredUnreviewedScansAsync();
		Task AutoReviewAsync(int mriId);
	}
}
