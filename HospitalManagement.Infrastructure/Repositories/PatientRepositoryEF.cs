using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Exceptions;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using HospitalManagement.Infrastructure.Logging;
using HospitalManagement.Domain.Exceptions;
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
            try
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occurred while adding patient.");
            }
        }

        public IEnumerable<Patient> GetAll()
        {
            try
            {
                return _context.Patients.ToList();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occurred while fetching patients.");
            }
        }

        public Patient GetById(int id)
        {
            try
            {
                return _context.Patients.Find(id);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occurred while retrieving patient.");
            }
        }

        public void Update(Patient patient)
        {
            try
            {
                var existingPatient = _context.Patients.Find(patient.PatientId);

                if (existingPatient == null)
                    throw new PatientNotFoundException("Patient not found.");

                existingPatient.Name = patient.Name;
                existingPatient.Age = patient.Age;
                existingPatient.Condition = patient.Condition;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occurred while updating patient.");
            }
        }

        public void Delete(int id)
        {
            try
            {
                var patient = _context.Patients.Find(id);

                if (patient == null)
                    throw new PatientNotFoundException("Patient not found.");

                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occurred while deleting patient.");
            }
        }
    }
}
