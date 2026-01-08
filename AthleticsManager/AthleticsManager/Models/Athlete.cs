namespace AthleticsManager.Models
{
    /// <summary>
    /// Represents an individual athlete in the system.
    /// This class stores personal information such as name, birth date, gender, and the association to a specific athletic club.
    /// </summary>
    public class Athlete
    {
        /// <summary>
        /// Gets the unique identifier for the athlete.
        /// </summary>
        public int AthleteID { get; protected set; }

        /// <summary>
        /// Gets the athlete's first name.
        /// </summary>
        public string FirstName { get; protected set; }

        /// <summary>
        /// Gets the athlete's last name.
        /// </summary>
        public string LastName { get; protected set; }

        /// <summary>
        /// Gets the athlete's date of birth.
        /// </summary>
        public DateTime BirthDate { get; protected set; }

        /// <summary>
        /// Gets the athlete's gender (e.g., "M", "F").
        /// </summary>
        public string Gender { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the athlete is currently active.
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Gets the identifier of the club the athlete is affiliated with.
        /// </summary>
        public int ClubID { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the Athlete class.
        /// This constructor is used when creating a new athlete who has not yet been assigned a database ID.
        /// </summary>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="birthDate">The athlete's date of birth.</param>
        /// <param name="gender">The athlete's gender.</param>
        /// <param name="isActive">Indicates whether the athlete is currently active.</param>
        /// <param name="clubID">The ID of the club the athlete belongs to.</param>
        public Athlete(string firstName, string lastName, DateTime birthDate, string gender, bool isActive, int clubID)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            IsActive = isActive;
            ClubID = clubID;
        }

        /// <summary>
        /// Initializes a new instance of the Athlete class with an existing ID.
        /// This constructor is used when loading existing athlete records from the database.
        /// </summary>
        /// <param name="athleteID">The unique ID of the athlete.</param>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="birthDate">The athlete's date of birth.</param>
        /// <param name="gender">The athlete's gender.</param>
        /// <param name="isActive">Indicates whether the athlete is currently active.</param>
        /// <param name="clubID">The ID of the club the athlete belongs to.</param>
        public Athlete(int athleteID, string firstName, string lastName, DateTime birthDate, string gender, bool isActive, int clubID)
        {
            AthleteID = athleteID;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            IsActive = isActive;
            ClubID = clubID;
        }

        /// <summary>
        /// Returns the athlete's birth date formatted as a string (dd.MM.yyyy).
        /// </summary>
        /// <returns>A string representation of the birth date.</returns>
        public string BirthDateString()
        {
            return BirthDate.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Assigns a unique identifier to the athlete.
        /// This is primarily used to update the object's state after it has been successfully inserted into the database.
        /// </summary>
        /// <param name="athleteID">The new athlete ID.</param>
        public void SetAthleteID(int athleteID)
        {
            AthleteID = athleteID;
        }
    }
}
