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
            Includes.Add(t => t.Patient);
            Includes.Add(t => t.Patient.ApplicationUser);
            Includes.Add(t => t.Doctor);
            Includes.Add(t => t.Doctor.ApplicationUser);
        }

        // By DoctorId
        public TicketSpecifications ByDoctorId(int doctorId)
        {
            Criteria = t => t.DoctorId == doctorId;
            Includes.Add(t => t.Patient);
            Includes.Add(t => t.Patient.ApplicationUser);
            Includes.Add(t => t.Lab);
            return this;
        }


        // By PatientId
        public TicketSpecifications ByPatientId(int patientId)
        {
            Criteria = t => t.PatientId == patientId;
            Includes.Add(t => t.Doctor);
            Includes.Add(t => t.Doctor.ApplicationUser);
            Includes.Add(t => t.Lab);
            return this;
        }
    }
}
