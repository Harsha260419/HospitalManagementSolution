using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Logging;
using HospitalManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace HospitalManagement.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IRepository<Doctor>, DoctorRepositoryEF>();
            services.AddScoped<IRepository<Patient>, PatientRepositoryEF>();

            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientService, PatientService>();

            var serviceProvider = services.BuildServiceProvider();

            var doctorService = serviceProvider.GetService<IDoctorService>();
            var patientService = serviceProvider.GetService<IPatientService>();

            RunMenu(doctorService, patientService);
        }


        static void AddDoctor(IDoctorService doctorService)
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

        static void ListDoctors(IDoctorService doctorService)
        {
            var doctors = doctorService.GetDoctors();

            foreach (var d in doctors)
            {
                Console.WriteLine($"{d.DoctorId} {d.Name} {d.Specialization} Rs. {d.ConsultationFee}");
            }
        }

        static void AddPatient(IPatientService patientService)
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

        static void ListPatients(IPatientService patientService)
        {
            var patients = patientService.GetPatients();

            foreach (var p in patients)
            {
                Console.WriteLine($"{p.PatientId} {p.Name} {p.Condition}");
            }
        }

        static void EditPatient(IPatientService patientService)
        {
            Patient patient = new Patient();

            Console.Write("Patient Id: ");
            patient.PatientId = Convert.ToInt32(Console.ReadLine());

            Console.Write("New Name: ");
            patient.Name = Console.ReadLine();

            Console.Write("New Age: ");
            patient.Age = Convert.ToInt32(Console.ReadLine());

            Console.Write("New Condition: ");
            patient.Condition = Console.ReadLine();

            patientService.UpdatePatient(patient);

            Console.WriteLine("Patient updated successfully.");
        }

        static void DeletePatient(IPatientService patientService)
        {
            Console.Write("Enter Patient Id: ");

            int id = Convert.ToInt32(Console.ReadLine());

            patientService.DeletePatient(id);

            Console.WriteLine("Patient deleted successfully.");
        }


        static void RunMenu(IDoctorService doctorService, IPatientService patientService)
        {
            while (true)
            {
                Console.WriteLine("\nHospital Management");
                Console.WriteLine("1 Add Doctor");
                Console.WriteLine("2 List Doctors");
                Console.WriteLine("3 Add Patient");
                Console.WriteLine("4 List Patients");
                Console.WriteLine("5 Edit Patient");
                Console.WriteLine("6 Delete Patient");
                Console.WriteLine("7 Exit");


                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddDoctor(doctorService);
                        break;

                    case 2:
                        ListDoctors(doctorService);
                        break;

                    case 3:
                        AddPatient(patientService);
                        break;

                    case 4:
                        ListPatients(patientService);
                        break;

                    case 5:
                        EditPatient(patientService);
                        break;

                    case 6:
                        DeletePatient(patientService);
                        break;

                    case 7:
                        Console.WriteLine("Exiting program...");
                        return;
                }
            }
        }
    }
}