using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.TicketSpecs
{
    public class TicketSpecifications : BaseSpecifications<Ticket>
    {
        // By LabId
        public TicketSpecifications(int labId)
            : base(t => t.LabId == labId)
        {
        }

        // By DoctorId
        public TicketSpecifications ByDoctorId(int doctorId)
        {
            Criteria = t => t.DoctorId == doctorId;
            return this;
        }

        // By PatientId
        public TicketSpecifications ByPatientId(int patientId)
        {
            Criteria = t => t.PatientId == patientId;
            return this;
        }
    }
}
