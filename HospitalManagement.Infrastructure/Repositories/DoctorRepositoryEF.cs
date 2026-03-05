using HospitalManagement.Domain.Entities;
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
    public class DoctorRepositoryEF : IRepository<Doctor>
    {
        private readonly AppDbContext _context;

        public DoctorRepositoryEF(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Doctor doctor)
        {
            try
            {
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
            }
            catch (SqlException ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occurred while adding doctor.");
            }
        }

        public IEnumerable<Doctor> GetAll()
        {
            try
            {
                return _context.Doctors.ToList();
            }
            catch (SqlException ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occurred while fetching doctors.");
            }
        }

        public Doctor GetById(int id)
        {
            try
            {
                return _context.Doctors.Find(id);

            }
            catch (SqlException ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occured while fetching doctors");
            }
        }

        public void Update(Doctor doctor)
        {
            try
            {
                var existingDoctor = _context.Doctors.Find(doctor.DoctorId);

                if (existingDoctor == null)
                    throw new DoctorNotFoundException("Doctor not found.");

                existingDoctor.Name = doctor.Name;
                existingDoctor.Specialization = doctor.Specialization;
                existingDoctor.ConsultationFee = doctor.ConsultationFee;

                _context.SaveChanges();
            }
            catch (SqlException ex)
            {
                Logger.Log(ex);
                throw new DatabaseConnectionException("Database error occurred while updating doctor.");
            }
        }

        public void Delete(int id)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                _context.SaveChanges();
            }
        }
    }
}
