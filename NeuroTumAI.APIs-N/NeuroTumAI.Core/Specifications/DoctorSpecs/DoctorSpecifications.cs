using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.DoctorSpecs
{
	public class DoctorSpecifications : BaseSpecifications<Doctor>
	{
		public DoctorSpecifications()
			: base()
		{

		}
		public DoctorSpecifications(string ApplicationUserId)
			: base(D => D.ApplicationUserId == ApplicationUserId && D.IsApproved == true)
		{
			Includes.Add(D => D.ApplicationUser);
		}

		public DoctorSpecifications(int doctorId)
			: base(D => D.Id == doctorId)
		{
			Includes.Add(D => D.ApplicationUser);
			Includes.Add(D => D.Reviews);
		}
	}
}
