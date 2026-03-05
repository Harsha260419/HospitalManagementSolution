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
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Doctor> _doctorRepository;

        public PatientService(
            IRepository<Patient> patientRepository,
            IRepository<Doctor> doctorRepository)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public void AddPatient(Patient patient)
        {
            if (patient.Age <= 0)
                throw new Exception("Age must be greater than 0.");

            var doctor = _doctorRepository.GetById(patient.DoctorId);

            if (doctor == null)
                throw new InvalidDoctorException("Doctor does not exist.");

            if (patient.AppointmentDate < DateTime.Now.Date)
                throw new Exception("Appointment date cannot be past.");

            _patientRepository.Add(patient);
        }

        public IEnumerable<Patient> GetPatients()
        {
            return _patientRepository.GetAll();
        }

        public Patient GetPatientById(int id)
        {
            var patient = _patientRepository.GetById(id);

            if (patient == null)
                throw new PatientNotFoundException("Patient not found.");

            return patient;
        }

        public void UpdatePatient(Patient patient)
        {
            _patientRepository.Update(patient);
        }

        public void DeletePatient(int id)
        {
            var patient = _patientRepository.GetById(id);

            if (patient == null)
                throw new PatientNotFoundException("Patient not found.");

            _patientRepository.Delete(id);
        }

        public Patient FindPatientByName(string name)
        {
            var patient = _patientRepository.GetAll()
                .FirstOrDefault(p => p.Name.ToLower() == name.ToLower());

            if (patient == null)
                throw new PatientNotFoundException("Patient not found.");

            return patient;
        }
    }
}
