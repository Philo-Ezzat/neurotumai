using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Appointments
{
	public class AppointmentWithDoctorDto
	{
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public string Status { get; set; }
        public string DoctorName { get; set; }
        public string DoctorProfilePicture { get; set; }
        public string Address { get; set; }
    }
}
