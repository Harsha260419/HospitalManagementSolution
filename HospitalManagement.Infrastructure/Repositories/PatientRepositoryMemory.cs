using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Infrastructure.Repositories
{
    public class PatientRepositoryMemory : IRepository<Patient>
    {
        private static List<Patient> _patients = new List<Patient>();

        public void Add(Patient entity)
        {
            entity.PatientId = _patients.Count + 1;
            _patients.Add(entity);
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patients;
        }

        public Patient GetById(int id)
        {
            return _patients.FirstOrDefault(p => p.PatientId == id);
        }

        public void Update(Patient entity)
        {
            var patient = GetById(entity.PatientId);

            if(patient != null)
            {
                patient.Name = entity.Name;
                patient.Age = entity.Age;
                patient.Condition = entity.Condition;
                patient.AppointmentDate = entity.AppointmentDate;
                patient.DoctorId = entity.DoctorId;
            }
        }

        public void Delete(int id)
        {
            var patient = GetById(id);

            if(patient != null)
            {
                _patients.Remove(patient);
            }
        }
    }
}
