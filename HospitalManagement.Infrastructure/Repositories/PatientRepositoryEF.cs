using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Exceptions;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Infrastructure.Repositories
{
    public class PatientRepositoryEF : IRepository<Patient>
    {
        private readonly AppDbContext _context;

        public PatientRepositoryEF(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public IEnumerable<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }

        public Patient GetById(int id)
        {
            return _context.Patients.Find(id);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);

            int rows = _context.SaveChanges();

            if (rows == 0)
                throw new PatientNotFoundException("Patient not found.");
        }

        public void Delete(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
                throw new PatientNotFoundException("Patient not found.");

            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }
    }
}
