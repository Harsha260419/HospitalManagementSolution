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
    public class PatientRepositoryADO : IRepository<Patient>
    {
        private readonly string _connectionString;

        public PatientRepositoryADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Patient patient)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = @"INSERT INTO Patients(Name,Age,Condition,AppointmentDate,DoctorId)
                             VALUES(@Name,@Age,@Condition,@Date,@DoctorId)";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Name", patient.Name);
            cmd.Parameters.AddWithValue("@Age", patient.Age);
            cmd.Parameters.AddWithValue("@Condition", patient.Condition);
            cmd.Parameters.AddWithValue("@Date", patient.AppointmentDate);
            cmd.Parameters.AddWithValue("@DoctorId", patient.DoctorId);

            conn.Open();

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Patient> GetAll()
        {
            List<Patient> patients = new();

            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = "SELECT * FROM Patients";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                patients.Add(new Patient
                {
                    PatientId = Convert.ToInt32(reader["PatientId"]),
                    Name = reader["Name"].ToString(),
                    Age = Convert.ToInt32(reader["Age"]),
                    Condition = reader["Condition"].ToString(),
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                    DoctorId = Convert.ToInt32(reader["DoctorId"])
                });
            }

            return patients;
        }

        public Patient GetById(int id)
        {
            Patient patient = null;

            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = "SELECT * FROM Patients WHERE PatientId=@Id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                patient = new Patient
                {
                    PatientId = Convert.ToInt32(reader["PatientId"]),
                    Name = reader["Name"].ToString(),
                    Age = Convert.ToInt32(reader["Age"]),
                    Condition = reader["Condition"].ToString(),
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                    DoctorId = Convert.ToInt32(reader["DoctorId"])
                };
            }

            return patient;
        }

        public void Update(Patient patient)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = @"UPDATE Patients 
                             SET Name=@Name,Age=@Age,Condition=@Condition
                             WHERE PatientId=@Id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Id", patient.PatientId);
            cmd.Parameters.AddWithValue("@Name", patient.Name);
            cmd.Parameters.AddWithValue("@Age", patient.Age);
            cmd.Parameters.AddWithValue("@Condition", patient.Condition);

            conn.Open();

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                throw new PatientNotFoundException("Patient not found.");
        }

        public void Delete(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = "DELETE FROM Patients WHERE PatientId=@Id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                throw new PatientNotFoundException("Patient not found.");
        }
    }
}
