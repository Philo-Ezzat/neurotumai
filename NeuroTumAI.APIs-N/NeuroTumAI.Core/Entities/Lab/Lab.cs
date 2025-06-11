using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities
{
	public class Lab : BaseEntity
	{
		public string Name { get; set; }
		public string? PhoneNumber { get; set; }
        public string? ImagePath { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
