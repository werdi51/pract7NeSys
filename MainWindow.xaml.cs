using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        List<Doctor> medik = new List<Doctor>();
        List<Pacient> Patients = new List<Pacient>();

        private Pacient _foundPatient = null;

        private Doctor _currentDoctor = new Doctor();
        public Doctor CurrentDoctor
        {
            get => _currentDoctor;
            set
            {
                _currentDoctor = value;
            }
        }

        private Doctor _loggedInDoctor;
        public Doctor LoggedInDoctor
        {
            get => _loggedInDoctor;
            set
            {
                _loggedInDoctor = value;
                UpdateDoctorInfo();
            }
        }

        private Pacient _newPatient = new Pacient();

        public Pacient NewPatient
        {
            get => _newPatient;
            set { _newPatient = value; }
        }

        private Pacient _displayPatient = new Pacient();
        public Pacient DisplayPatient
        {
            get => _displayPatient;
            set { _displayPatient = value; }
        }

        private Pacient _editPatient = new Pacient();
        public Pacient EditPatient
        {
            get => _editPatient;
            set { _editPatient = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            LoadJson();

            RegisterDoctorPanel.Visibility = Visibility.Visible;
            LoginDoctorPanel.Visibility = Visibility.Visible;
            CurrentDoctorPanel.Visibility = Visibility.Collapsed;
            PatientSearchPanel.Visibility = Visibility.Collapsed;
            RegisterPatientPanel.Visibility = Visibility.Collapsed;
            PatientInfoPanel.Visibility = Visibility.Collapsed;
            PatientEditPanel.Visibility = Visibility.Collapsed;
            StatsPanel.Visibility = Visibility.Visible;

            UpdateDoctorsComboBox();
            UpdateCounts();

            if (NewPatientBirthdayPicker != null)
                NewPatientBirthdayPicker.DisplayDateEnd = DateTime.Today;

            if (EditPatientBirthdayPicker != null)
                EditPatientBirthdayPicker.DisplayDateEnd = DateTime.Today;
        }
        private void LoadJson()
        {
            medik = JSON.LoadAllDoctors();

            Patients = JSON.LoadAllPatients();

            foreach (var patient in Patients)
            {
                UpdatePatientDoctorDisplay(patient);
            }
        }
        private void UpdatePatientDoctorDisplay(Pacient pacient)
        {
            if (pacient.LastDoctorId == 0)
            {
                pacient.LastDoctorDisplay = "Не указан";
            }
            else
            {
                var doctor = medik.Find(d => d.Id == pacient.LastDoctorId);
                if (doctor != null)
                {
                    pacient.LastDoctorDisplay = $"{doctor.Id} - {doctor.LastName} {doctor.Name}";
                }
                else
                {
                    pacient.LastDoctorDisplay = $"ID: {pacient.LastDoctorId} не найден)";
                }
            }
        }
        private void UpdateCounts()
        {
            JSON.UpdateCounts(this);
        }
        private void UpdateDoctorInfo()
        {
            if (LoggedInDoctor != null)
            {
                DoctorIdTextBlock.Text = LoggedInDoctor.Id.ToString();
                DoctorNameTextBlock.Text = LoggedInDoctor.Name;
                DoctorSpecializationTextBlock.Text = LoggedInDoctor.Specialisation ?? "";
                DoctorLastNameTextBlock.Text = LoggedInDoctor.LastName;
                DoctorMiddleNameTextBlock.Text = LoggedInDoctor.MiddleName;
                DoctorPasswordTextBlock.Text = LoggedInDoctor.Password;

            }
            else
            {
                DoctorIdTextBlock.Text = "";
                DoctorNameTextBlock.Text = "";
                DoctorSpecializationTextBlock.Text = "";
                DoctorLastNameTextBlock.Text = "";
                DoctorMiddleNameTextBlock.Text = "";
                DoctorPasswordTextBlock.Text = "";


            }
        }

        private void ComboBoxSelections(int doctorId)
        {
            if (EditPatientDoctorComboBox == null || EditPatientDoctorComboBox.Items.Count == 0)
                return;

            if (doctorId == 0)
            {
                EditPatientDoctorComboBox.SelectedIndex = 0;
                return;
            }

            foreach (var item in EditPatientDoctorComboBox.Items)
            {
                if (item is Doctor doctor && doctor.Id == doctorId)
                {
                    EditPatientDoctorComboBox.SelectedItem = doctor;
                    return;
                }
            }

            EditPatientDoctorComboBox.SelectedIndex = 0;
        }

        private void UpdateDoctorsComboBox()
        {
            if (EditPatientDoctorComboBox == null)
                return;

            EditPatientDoctorComboBox.Items.Clear();
            EditPatientDoctorComboBox.Items.Add("Не выбран");

            foreach (var doctor in medik)
            {
                EditPatientDoctorComboBox.Items.Add(doctor);
            }

            ComboBoxSelections(EditPatient.LastDoctorId);
        }

        private int Randomiser(bool doctor)
        {
            int randomID;
            bool isUnique;

            do
            {
                isUnique = true;

                if (doctor)
                {
                    randomID = rnd.Next(10000, 99999);
                    foreach (var doc in medik)
                    {
                        if (doc.Id == randomID)
                        {
                            isUnique = false;
                            break;
                        }
                    }
                }
                else
                {
                    randomID = rnd.Next(1000000, 9999999);
                    foreach (var patient in Patients)
                    {
                        if (patient.Id == randomID)
                        {
                            isUnique = false;
                            break;
                        }
                    }
                }

            } while (!isUnique);

            return randomID;
        }

        private void DoctorAdd(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentDoctor.Name) ||
       string.IsNullOrWhiteSpace(CurrentDoctor.LastName) ||
       string.IsNullOrWhiteSpace(CurrentDoctor.MiddleName) ||
       string.IsNullOrWhiteSpace(CurrentDoctor.Specialisation) ||
       string.IsNullOrWhiteSpace(PasswordBox1.Password) ||
       string.IsNullOrWhiteSpace(PasswordBox2.Password))
            {
                MessageBox.Show("все поля должны быть заполнены", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (PasswordBox1.Password != PasswordBox2.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            Doctor NewMedik = new Doctor();

            NewMedik.Id = Randomiser(true);
            NewMedik.Name = CurrentDoctor.Name;
            NewMedik.LastName = CurrentDoctor.LastName;
            NewMedik.MiddleName = CurrentDoctor.MiddleName;
            NewMedik.Specialisation = CurrentDoctor.Specialisation;
            NewMedik.Password = PasswordBox1.Password;

            JSON.SaveDoctor(NewMedik);

            medik.Add(NewMedik);

            UpdateDoctorsComboBox();
            UpdateCounts();

            CurrentDoctor.Name = "";
            CurrentDoctor.LastName = "";
            CurrentDoctor.MiddleName = "";
            CurrentDoctor.Specialisation = "";

            SpecialisationComboBox.SelectedIndex = -1;

            PasswordBox1.Password = "";
            PasswordBox2.Password = "";

            MessageBox.Show($"Доктор добавлен\n ID: {NewMedik.Id}");
        }

        private void DoctorFind(object sender, RoutedEventArgs e)
        {

            string enteredId = LoginIdTextBox.Text;
            string enteredPassword = LoginPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(enteredId) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show("ведите ID и пароль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(enteredId, out int doctorId))
            {
                MessageBox.Show("ID должен быть числом", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Doctor foundDoctor = null;

            foreach (var doctor in medik)
            {
                if (doctor.Id == doctorId && doctor.Password == enteredPassword)
                {
                    foundDoctor = doctor;
                    break;
                }
            }

            if (foundDoctor == null)
            {
                MessageBox.Show("неверный ID или пароль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoggedInDoctor = foundDoctor;

            RegisterPatientPanel.Visibility = Visibility.Visible;
            PatientSearchPanel.Visibility = Visibility.Visible;

            CurrentDoctorPanel.Visibility = Visibility.Visible;

            LoginIdTextBox.Text = "";
            LoginPasswordBox.Password = "";

            MessageBox.Show($"Добро пожаловать, {foundDoctor.Name} {foundDoctor.LastName}!");
        }

        private void PatientAdd(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewPatient.Name) ||
    string.IsNullOrWhiteSpace(NewPatient.LastName) ||
    string.IsNullOrWhiteSpace(NewPatient.MiddleName) ||
    !NewPatient.BirthdayDate.HasValue)
            {
                MessageBox.Show("Заполните поля\n(Имя, Фамилия, Отчество, День рождения)",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Pacient newPatient = new Pacient();
            newPatient.Id = Randomiser(false);
            newPatient.Name = NewPatient.Name;
            newPatient.LastName = NewPatient.LastName;
            newPatient.MiddleName = NewPatient.MiddleName;
            newPatient.BirthdayDate = NewPatient.BirthdayDate;
            newPatient.LastAppointment = "";
            newPatient.LastDoctorId = 0;
            newPatient.LastDoctorDisplay = "Не указан";
            newPatient.Diagnosis = "";
            newPatient.Recommendations = "";

            JSON.SavePatient(newPatient);

            Patients.Add(newPatient);
            UpdateCounts();

            NewPatient = new Pacient();
            if (NewPatientBirthdayPicker != null)
                NewPatientBirthdayPicker.SelectedDate = null;

            MessageBox.Show($"Пациент зарегистрирован\n\n" +
                           $"ID : {newPatient.Id}\n" +
                           $"ФИО: {newPatient.LastName} {newPatient.Name} {newPatient.MiddleName}\n" +
                           $"Дата рождения: {newPatient.BirthdayString}",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PatientSave(object sender, RoutedEventArgs e)
        {
            if (_foundPatient == null)
            {
                MessageBox.Show("Сначала найдите пациента",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _foundPatient.Name = EditPatient.Name;
            _foundPatient.LastName = EditPatient.LastName;
            _foundPatient.MiddleName = EditPatient.MiddleName;
            _foundPatient.BirthdayDate = EditPatient.BirthdayDate;
            _foundPatient.LastAppointment = EditPatient.LastAppointment;
            _foundPatient.Diagnosis = EditPatient.Diagnosis ?? "";
            _foundPatient.Recommendations = EditPatient.Recommendations ?? "";
            _foundPatient.LastDoctorId = EditPatient.LastDoctorId;

            UpdatePatientDoctorDisplay(_foundPatient);

            JSON.SavePatient(_foundPatient);

            DisplayPatient.Name = _foundPatient.Name;
            DisplayPatient.LastName = _foundPatient.LastName;
            DisplayPatient.MiddleName = _foundPatient.MiddleName;
            DisplayPatient.BirthdayDate = _foundPatient.BirthdayDate;
            DisplayPatient.LastAppointment = _foundPatient.LastAppointment;
            DisplayPatient.LastDoctorId = _foundPatient.LastDoctorId;
            DisplayPatient.Diagnosis = _foundPatient.Diagnosis;
            DisplayPatient.Recommendations = _foundPatient.Recommendations;
            UpdatePatientDoctorDisplay(DisplayPatient);

            MessageBox.Show($"Данные  обновлены\n\n" +
                           $"ID: {_foundPatient.Id}\n" +
                           $"ФИО: {_foundPatient.LastName} {_foundPatient.Name} {_foundPatient.MiddleName}",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
         

        private void PatientReset(object sender, RoutedEventArgs e)
        {
            if (_foundPatient != null)
            {
                EditPatient.Name = _foundPatient.Name;
                EditPatient.LastName = _foundPatient.LastName;
                EditPatient.MiddleName = _foundPatient.MiddleName;
                EditPatient.BirthdayDate = _foundPatient.BirthdayDate;
                EditPatient.LastAppointment = _foundPatient.LastAppointment;
                EditPatient.Diagnosis = _foundPatient.Diagnosis;
                EditPatient.Recommendations = _foundPatient.Recommendations;

                EditPatient.LastDoctorId = _foundPatient.LastDoctorId;
                UpdatePatientDoctorDisplay(EditPatient);

                UpdateDoctorsComboBox();

                MessageBox.Show("Данные сброшены", "Сброс",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void PatientSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchIdText = PatientSearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(searchIdText))
            {
                MessageBox.Show("Введите ID для поиска", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(searchIdText, out int patientId))
            {
                MessageBox.Show("ID должен быть числом", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _foundPatient = Patients.Find(p => p.Id == patientId);

            if (_foundPatient == null)
            {
                _foundPatient = JSON.FindPatientById(patientId);
                if (_foundPatient != null)
                {
                    Patients.Add(_foundPatient);
                    UpdateCounts();
                }
            }

            if (_foundPatient == null)
            {
                PatientInfoPanel.Visibility = Visibility.Collapsed;
                PatientEditPanel.Visibility = Visibility.Collapsed;
                MessageBox.Show($"Пациент {patientId} не найден", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                DisplayPatient = new Pacient();
                EditPatient = new Pacient();
                if (EditPatientBirthdayPicker != null)
                    EditPatientBirthdayPicker.SelectedDate = null;
                ComboBoxSelections(0);
                EditPatientDoctorComboBox.SelectedIndex = 0;
                return;
            }

            PatientInfoPanel.Visibility = Visibility.Visible;
            PatientEditPanel.Visibility = Visibility.Visible;

            DisplayPatient.Id = _foundPatient.Id;
            DisplayPatient.Name = _foundPatient.Name;
            DisplayPatient.LastName = _foundPatient.LastName;
            DisplayPatient.MiddleName = _foundPatient.MiddleName;
            DisplayPatient.BirthdayDate = _foundPatient.BirthdayDate;
            DisplayPatient.LastAppointment = _foundPatient.LastAppointment;
            DisplayPatient.LastDoctorId = _foundPatient.LastDoctorId;
            DisplayPatient.Diagnosis = _foundPatient.Diagnosis;
            DisplayPatient.Recommendations = _foundPatient.Recommendations;
            UpdatePatientDoctorDisplay(DisplayPatient);

            EditPatient.Id = _foundPatient.Id;
            EditPatient.Name = _foundPatient.Name;
            EditPatient.LastName = _foundPatient.LastName;
            EditPatient.MiddleName = _foundPatient.MiddleName;
            EditPatient.BirthdayDate = _foundPatient.BirthdayDate;
            EditPatient.LastAppointment = _foundPatient.LastAppointment;
            EditPatient.LastDoctorId = _foundPatient.LastDoctorId;
            EditPatient.Diagnosis = _foundPatient.Diagnosis ?? "";
            EditPatient.Recommendations = _foundPatient.Recommendations ?? "";
            UpdatePatientDoctorDisplay(EditPatient);

            UpdateDoctorsComboBox();

            MessageBox.Show($"Пациент\n" +
                           $"{_foundPatient.LastName} {_foundPatient.Name} {_foundPatient.MiddleName}",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditPatientDoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditPatientDoctorComboBox.SelectedItem != null)
            {
                if (EditPatientDoctorComboBox.SelectedItem is string selectedString)
                {
                    if (selectedString == "Не выбрано")
                    {
                        EditPatient.LastDoctorId = 0;
                        EditPatient.LastDoctorDisplay = "Не указан";
                    }
                }
                else if (EditPatientDoctorComboBox.SelectedItem is Doctor selectedDoctor)
                {
                    EditPatient.LastDoctorId = selectedDoctor.Id;
                    EditPatient.LastDoctorDisplay = $"{selectedDoctor.Id} - {selectedDoctor.LastName} {selectedDoctor.Name}";
                }
            }
        }
    }
}