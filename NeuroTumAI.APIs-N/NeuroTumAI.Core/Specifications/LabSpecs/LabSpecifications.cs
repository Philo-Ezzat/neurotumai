using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.LabSpecs
{
	public class LabSpecifications : BaseSpecifications<Lab>
	{
		public LabSpecifications()
			: base()
		{

		}
		public LabSpecifications(string ApplicationUserId)
			: base(D => D.ApplicationUserId == ApplicationUserId)
		{
			Includes.Add(D => D.ApplicationUser);
		}

		public LabSpecifications(int labId)
			: base(D => D.Id == labId)
		{
			Includes.Add(D => D.ApplicationUser);
		}
	}
}
