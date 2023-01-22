using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StabilometryAnalysis
{
    public class Patient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Notes { get; set; }

        public Patient(int ID, string Name, string Surname, string Notes)
        {
            this.ID = ID;
            this.Name = Name;
            this.Surname = Surname;
            this.Notes = Notes;
        }

        public Patient(string Name, string Surname, string Notes)
        {
            this.ID = -1;
            this.Name = Name;
            this.Surname = Surname;
            this.Notes = Notes;
        }

        public string FullName()
        {
            if (Surname != null && Surname != "")
                return $"{Name} {Surname}";

            //else;
            return Name;
        }
    }
}