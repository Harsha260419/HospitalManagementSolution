using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Infrastructure.Repositories
{
    public class DoctorRepositoryMemory : IRepository<Doctor>
    {
        private static List<Doctor> _doctors = new List<Doctor>();

        public void Add(Doctor entity)
        {
            entity.DoctorId = _doctors.Count + 1;
            _doctors.Add(entity);
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _doctors;
        }

        public Doctor GetById(int id)
        {
            return _doctors.FirstOrDefault(d => d.DoctorId == id);
        }

        public void Update(Doctor entity)
        {
            var doctor = GetById(entity.DoctorId);
            if (doctor != null)
            {
                doctor.Name = entity.Name;
                doctor.Specialization = entity.Specialization;
                doctor.ConsultationFee = entity.ConsultationFee;
            }
        }

        public void Delete(int id)
        {
            var doctor = GetById(id);
            if (doctor != null)
                _doctors.Remove(doctor);
        }
    }
}
