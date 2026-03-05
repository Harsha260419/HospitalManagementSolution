using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Domain.Interfaces
{
    public interface IPatientService
    {
        void AddPatient(Patient patient);

        IEnumerable<Patient> GetPatients();

        Patient GetPatientById(int id);

        void UpdatePatient(Patient patient);

        void DeletePatient(int id);
    }
}
