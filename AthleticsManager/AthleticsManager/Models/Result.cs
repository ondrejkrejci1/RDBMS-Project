namespace AthleticsManager.Models
{
    /// <summary>
    /// Represents a single athletic performance record.
    /// This class serves as the core data entity linking an athlete, a competition, and a discipline 
    /// to a specific result (performance, rank, etc.).
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets the unique identifier for this result record.
        /// </summary>
        public int ResultID { get; protected set; }

        /// <summary>
        /// Gets the identifier of the athlete who achieved this result.
        /// </summary>
        public int AthleteID { get; protected set; }

        /// <summary>
        /// Gets the identifier of the competition where this result was achieved.
        /// </summary>
        public int CompetitionID { get; protected set; }

        /// <summary>
        /// Gets the identifier (or enum value) of the discipline.
        /// </summary>
        public int DisciplineID { get; protected set; }

        /// <summary>
        /// Gets the primary performance value.
        /// This represents either a time (in seconds) or a distance/height (in meters), depending on the discipline.
        /// </summary>
        public decimal Performance { get; protected set; }

        /// <summary>
        /// Gets the wind speed recorded during the performance, if applicable.
        /// </summary>
        public double? Wind { get; protected set; }

        /// <summary>
        /// Gets the rank or position achieved by the athlete in this event.
        /// </summary>
        public int? Placement { get; protected set; }

        /// <summary>
        /// Gets any additional notes or remarks regarding the performance.
        /// </summary>
        public string? Note { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the Result class.
        /// This constructor is typically used when creating a new result that has not yet been saved to the database (ID is not yet assigned).
        /// </summary>
        /// <param name="athleteID">The ID of the athlete.</param>
        /// <param name="competitionID">The ID of the competition.</param>
        /// <param name="disciplineID">The ID of the discipline.</param>
        /// <param name="performance">The performance value.</param>
        /// <param name="wind">The wind reading (optional).</param>
        /// <param name="placement">The placement achieved (optional).</param>
        /// <param name="note">Any additional notes (optional).</param>
        public Result(int athleteID, int competitionID, int disciplineID, decimal performance, double? wind, int? placement, string? note) 
        {
            AthleteID = athleteID;
            CompetitionID = competitionID;
            DisciplineID = disciplineID;
            Performance = performance;
            Wind = wind;
            Placement = placement;
            Note = note;
        }

        /// <summary>
        /// Initializes a new instance of the Result class with an existing ID.
        /// This constructor is typically used when loading existing records from the database.
        /// </summary>
        /// <param name="resultID">The unique ID of the result.</param>
        /// <param name="athleteID">The ID of the athlete.</param>
        /// <param name="competitionID">The ID of the competition.</param>
        /// <param name="disciplineID">The ID of the discipline.</param>
        /// <param name="performance">The performance value.</param>
        /// <param name="wind">The wind reading.</param>
        /// <param name="placement">The placement achieved.</param>
        /// <param name="note">Any additional notes.</param>
        public Result(int resultID, int athleteID, int competitionID, int disciplineID, decimal performance, double? wind, int? placement, string? note)
        {
            ResultID = resultID;
            AthleteID = athleteID;
            CompetitionID = competitionID;
            DisciplineID = disciplineID;
            Performance = performance;
            Wind = wind;
            Placement = placement;
            Note = note;
        }

        /// <summary>
        /// Assigns a unique identifier to the result.
        /// This is primarily used to update the object's state after it has been successfully inserted into the database.
        /// </summary>
        /// <param name="resultID">The new result ID.</param>
        public void SetResultID(int resultID)
        {
            ResultID = resultID;
        }
    }
}
