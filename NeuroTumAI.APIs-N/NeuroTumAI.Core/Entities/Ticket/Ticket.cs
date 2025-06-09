using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Entities.MriScan;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities
{
    public enum TicketStatus
    {
        Opened,
        NoMri,
        HasMri,
        Closed
    }
    public class Ticket : BaseEntity
	{
        public int PatientId { get; set; }
 		public int DoctorId { get; set; }
		public string? DrInitialDesc { get; set; }
		public bool NeedMRI { get; set; } = false;
        public int? LabId { get; set; }
        public string? LabDescribtion { get; set; }
        public string? ImagePath { get; set; }
        public string? DetectionClass { get; set; }
        public string? Confidence { get; set; }
        public string? AiGeneratedFilePath { get; set; }
        public string? DrFinalDesc { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Opened;
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Lab Lab { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
