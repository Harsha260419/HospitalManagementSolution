using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Domain.Interfaces
{
    public interface IDoctorService
    {
        void AddDoctor(Doctor doctor);

        IEnumerable<Doctor> GetDoctors();

        Doctor GetDoctorById(int id);

        void UpdateDoctor(Doctor doctor);

        void DeleteDoctor(int id);
    }
}
