using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _7
{
    public static class JSON
    {
        private static string doctorsFolder = "Doctors";
        private static string patientsFolder = "Patients";

        public static void SaveDoctor(Doctor doctor)
        {
            if (!Directory.Exists(doctorsFolder))
                Directory.CreateDirectory(doctorsFolder);

            string fileName = $"D_{doctor.Id}.json";
            string filePath = Path.Combine(doctorsFolder, fileName);

            var json = JsonSerializer.Serialize(doctor);

            File.WriteAllText(filePath, json);
        }

        public static List<Doctor> LoadAllDoctors()
        {
            var doctors = new List<Doctor>();

            if (!Directory.Exists(doctorsFolder))
                return doctors;

            string[] files = Directory.GetFiles(doctorsFolder, "D_*.json");

            foreach (string file in files)
            {

                    string json = File.ReadAllText(file);

                    var doctor = JsonSerializer.Deserialize<Doctor>(json);

                    doctors.Add(doctor);
                

            }

            return doctors;
        }


        public static void SavePatient(Pacient patient)
        {
            if (!Directory.Exists(patientsFolder))
                Directory.CreateDirectory(patientsFolder);

            string fileName = $"P_{patient.Id}.json";
            string filePath = Path.Combine(patientsFolder, fileName);

            var json = JsonSerializer.Serialize(patient);

            File.WriteAllText(filePath, json);
        }

        public static List<Pacient> LoadAllPatients()
        {
            var patients = new List<Pacient>();

            if (!Directory.Exists(patientsFolder))
                return patients;

            string[] files = Directory.GetFiles(patientsFolder, "P_*.json");

            foreach (string file in files)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var patient = JsonSerializer.Deserialize<Pacient>(json);
                    patients.Add(patient);
                }
                catch
                {
                }
            }

            return patients;
        }

        public static Pacient FindPatientById(int patientId)
        {
            string fileName = $"P_{patientId}.json";
            string filePath = Path.Combine(patientsFolder, fileName);

            if (!File.Exists(filePath))
                return null;

            try
            {
                string json = File.ReadAllText(filePath);
                var patient = JsonSerializer.Deserialize<Pacient>(json);
                return patient;
            }
            catch
            {
                return null;
            }
        }


        public static void UpdateCounts(MainWindow window)
        {
            int doctorCount = 0;
            int patientCount = 0;

            if (Directory.Exists(doctorsFolder))
                doctorCount = Directory.GetFiles(doctorsFolder, "D_*.json").Length;

            if (Directory.Exists(patientsFolder))
                patientCount = Directory.GetFiles(patientsFolder, "P_*.json").Length;

            window.DoctorCountText.Text = doctorCount.ToString();
            window.PatientCountText.Text = patientCount.ToString();
        }
    }
}
