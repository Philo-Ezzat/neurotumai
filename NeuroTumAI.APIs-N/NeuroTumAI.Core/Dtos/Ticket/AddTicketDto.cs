using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class AddTicketDto
	{
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string DrInitialDesc { get; set; }
        public bool NeedMRI { get; set; }
    }
}
