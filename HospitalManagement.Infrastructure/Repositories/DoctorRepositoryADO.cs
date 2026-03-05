using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Exceptions;
using HospitalManagement.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Infrastructure.Repositories
{
    public class DoctorRepositoryADO : IRepository<Doctor>
    {
        private readonly string _connectionString;

        public DoctorRepositoryADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Doctor doctor)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);

                string query = @"INSERT INTO Doctors(Name, Specialization, ConsultationFee)
                                 VALUES(@Name,@Specialization,@Fee)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Name", doctor.Name);
                cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                cmd.Parameters.AddWithValue("@Fee", doctor.ConsultationFee);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new DatabaseConnectionException("Database error while adding doctor.");
            }
        }

        public IEnumerable<Doctor> GetAll()
        {
            List<Doctor> doctors = new();

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);

                string query = "SELECT * FROM Doctors";

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorId = Convert.ToInt32(reader["DoctorId"]),
                        Name = reader["Name"].ToString(),
                        Specialization = reader["Specialization"].ToString(),
                        ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"])
                    });
                }
            }
            catch (SqlException)
            {
                throw new DatabaseConnectionException("Database error while fetching doctors.");
            }

            return doctors;
        }

        public Doctor GetById(int id)
        {
            Doctor doctor = null;

            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = "SELECT * FROM Doctors WHERE DoctorId=@Id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                doctor = new Doctor
                {
                    DoctorId = Convert.ToInt32(reader["DoctorId"]),
                    Name = reader["Name"].ToString(),
                    Specialization = reader["Specialization"].ToString(),
                    ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"])
                };
            }

            return doctor;
        }

        public void Update(Doctor doctor)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = @"UPDATE Doctors 
                             SET Name=@Name,Specialization=@Spec,ConsultationFee=@Fee
                             WHERE DoctorId=@Id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Id", doctor.DoctorId);
            cmd.Parameters.AddWithValue("@Name", doctor.Name);
            cmd.Parameters.AddWithValue("@Spec", doctor.Specialization);
            cmd.Parameters.AddWithValue("@Fee", doctor.ConsultationFee);

            conn.Open();

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = "DELETE FROM Doctors WHERE DoctorId=@Id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();

            cmd.ExecuteNonQuery();
        }
    }
}
