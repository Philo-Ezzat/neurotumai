using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IBlobStorageService
	{
		//Task<string> UploadFileAsync(Stream fileStream, string fileName, string containerName, string contentType);
        Task<string> UploadFileAsync(Stream stream, string fileName, string v);
    }
}
