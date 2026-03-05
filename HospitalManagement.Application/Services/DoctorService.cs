using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Exceptions;
using HospitalManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorService(IRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public void AddDoctor(Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.Name))
                throw new InvalidDoctorException("Doctor name cannot be empty.");

            if (string.IsNullOrWhiteSpace(doctor.Specialization))
                throw new InvalidDoctorException("Specialization cannot be empty.");

            if (doctor.ConsultationFee <= 0)
                throw new InvalidDoctorException("Consultation fee must be greater than 0.");

            _doctorRepository.Add(doctor);
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return _doctorRepository.GetAll();
        }

        public Doctor GetDoctorById(int id)
        {
            var doctor = _doctorRepository.GetById(id);

            if (doctor == null)
                throw new DoctorNotFoundException("Doctor not found.");

            return doctor;
        }

        public void UpdateDoctor(Doctor doctor)
        {
            _doctorRepository.Update(doctor);
        }

        public void DeleteDoctor(int id)
        {
            _doctorRepository.Delete(id);
        }
    }
}
