using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
