using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthleticsManager.Models
{
    public class Athlete
    {
        public int AthleteID { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public DateTime BirthDate { get; protected set; }
        public string Gender { get; protected set; }
        public int ClubID { get; protected set; }

        public Athlete(string firstName, string lastName, DateTime birthDate, string gender, int clubID)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            ClubID = clubID;
        }

        public Athlete(int athleteID, string firstName, string lastName, DateTime birthDate, string gender, int clubID)
        {
            AthleteID = athleteID;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            ClubID = clubID;
        }

        public string BirthDateString()
        {
            return BirthDate.ToString("dd.MM.yyyy");
        }

        public void SetAthleteID(int athleteID)
        {
            AthleteID = athleteID;
        }
    }
}
