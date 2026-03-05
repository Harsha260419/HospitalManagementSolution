using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Logging;
using HospitalManagement.Infrastructure.Repositories;

namespace HospitalManagement.ConsoleApp
{
    class Program
    {
        static DoctorRepositoryMemory doctorRepo = new DoctorRepositoryMemory();
        static PatientRepositoryMemory patientRepo = new PatientRepositoryMemory();

        static DoctorService doctorService = new DoctorService(doctorRepo);
        static PatientService patientService = new PatientService(patientRepo, doctorRepo);

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nHospital Management");
                Console.WriteLine("1 Add Doctor");
                Console.WriteLine("2 List Doctors");
                Console.WriteLine("3 Add Patient");
                Console.WriteLine("4 List Patients");
                Console.WriteLine("5 Exit");

                int choice = Convert.ToInt32(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            AddDoctor();
                            break;

                        case 2:
                            ListDoctors();
                            break;

                        case 3:
                            AddPatient();
                            break;

                        case 4:
                            ListPatients();
                            break;

                        case 5:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void AddDoctor()
        {
            Doctor doctor = new Doctor();

            Console.Write("Name: ");
            doctor.Name = Console.ReadLine();

            Console.Write("Specialization: ");
            doctor.Specialization = Console.ReadLine();

            Console.Write("Fee: ");
            doctor.ConsultationFee = Convert.ToDecimal(Console.ReadLine());

            doctorService.AddDoctor(doctor);

            Console.WriteLine("Doctor Added.");
        }

        static void ListDoctors()
        {
            var doctors = doctorService.GetDoctors();

            foreach (var d in doctors)
            {
                Console.WriteLine($"{d.DoctorId} {d.Name} {d.Specialization} ₹{d.ConsultationFee:F2}");
            }
        }

        static void AddPatient()
        {
            Patient patient = new Patient();

            Console.Write("Name: ");
            patient.Name = Console.ReadLine();

            Console.Write("Age: ");
            patient.Age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Condition: ");
            patient.Condition = Console.ReadLine();

            Console.Write("DoctorId: ");
            patient.DoctorId = Convert.ToInt32(Console.ReadLine());

            patient.AppointmentDate = DateTime.Now;

            patientService.AddPatient(patient);

            Console.WriteLine("Patient Added.");
        }

        static void ListPatients()
        {
            var patients = patientService.GetPatients();

            foreach (var p in patients)
            {
                Console.WriteLine($"{p.PatientId} {p.Name} {p.Condition}");
            }
        }
    }
}
