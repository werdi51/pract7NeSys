using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;


namespace _7
{
    public class Doctor : INotifyPropertyChanged
    {
        private int _id;
        private string _name="";
        private string _lastName="";
        private string _middleName = "";
        private string _specialisation = "";
        private string _password = "";

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set{ _lastName = value; OnPropertyChanged(); }
        }

        public string MiddleName
        {
            get => _middleName;
            set { _middleName = value; OnPropertyChanged(); }
        }

        public string Specialisation
        {
            get => _specialisation;
            set { _specialisation = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string DisplayInfo => $"{Id} {LastName} {Name} {MiddleName}";
        public override string ToString()
        {
            return DisplayInfo;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
