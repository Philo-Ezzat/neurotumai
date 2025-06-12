using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class AddTicketDto
	{
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string DrInitialDesc { get; set; }
        public bool NeedMRI { get; set; }
        public string AppId { get; set; }

    }
}
