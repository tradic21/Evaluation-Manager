using DBLayer;
using Evaluation_Manager.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Evaluation_Manager.Repositories
{
    public static Student GetStudent(int id)
    {
        //Inicijalizacija varijable student na null.
        //Ova varijabla će se koristiti za pohranu rezultata dohvaćanja studenta.
        Student student = null;
        //Kreiranje SQL upita koji će dohvatiti sve podatke o studentu s određenim id-om.
        string sql = $"SELECT * FROM Students WHERE Id = {id}";
        //Otvaranje veze prema bazi podataka.
        DB.OpenConnection();
        //Dohvaćanje DataReader objekta koji sadrži rezultate SQL upita.
        var reader = DB.GetDataReader(sql);
        if(reader.HasRows)
        {
            //Čitanje prvog reda rezultata iz DataReader-a.
            reader.Read();
            //Kreiranje objekta student na temelju podataka iz DataReader-a.
            student = CreateObject(reader);
            reader.Close();
        }
        //Zatvaranje veze prema bazi podataka.
        DB.CloseConnection();
        //Vraćanje objekta student.
        //Ako DataReader nije imao rezultate ili se dogodila greška, objekt će ostati null.
        return student;
    }

    //deklaracija metode u kojoj je definirano da je to statička metoda koja vraća listu studenata.
    public static List<Student> GetStudents()
    {
        //instancira novu listu za tip Student.
        List<Student> students = new List<Student>();

        //„Dohvati sve iz tablice Students“.
        string sql = "SELECT * FROM Students";
        //otvara vezu s bazom. Podsjetnik: naredbu OpenConnection implementirao je izmišljeni „drugi
        //tim“, pristup kôdu te metode zaštićen je (za sada).
        DB.OpenConnection();
        //vraća objekt iz baze i stavlja ga u varijablu reader. Tip podatka varijable reader nije var, već
        //ključna riječ „var“ zadaje zadatak kompajleru da zaključi koji je to tip uzevši u obzir što vraća metoda!
        var reader = DB.GetDataReader(sql);
        //glava petlje kojoj je zadatak da „izvuče“ sve dohvaćene studente iz objekta iz baze i da njima
        //napuni listu studenata.
        while (reader.Read())
        {
            Student student = CreateObject(reader);
            students.Add(student);
        }

        reader.Close();
        DB.CloseConnection();


        return students;
    }

    private static Student CreateObject(SqlDataReader reader)
    {
        int id = int.Parse(reader["id"].ToString());
        string firstName = reader["FirstName"].ToString();
        string lastName = reader["LastName"].ToString();
        //Ako je ocjena „NULL“ u bazi, očito je da se taj znakovni
        //niz ne može pretvoriti u cjelobrojnu vrijednost i metoda će jednostavno stvoriti novu varijablu grade i
        //dat će joj vrijednost 0.
        int.TryParse(reader["Grade"].ToString(), out int grade);

        var student = new Student
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Grade = grade
        };
        return student;

    }
}
