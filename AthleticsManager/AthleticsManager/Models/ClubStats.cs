namespace AthleticsManager.Models
{
    /// <summary>
    /// Represents a statistical summary for a specific athletic club.
    /// This model is used for reporting purposes, aggregating data such as member counts, medal tallies, and age demographics.
    /// </summary>
    public class ClubStats
    {
        /// <summary>
        /// Gets or sets the name of the club.
        /// </summary>
        public string ClubName { get; set; }

        /// <summary>
        /// Gets or sets the name of the region where the club is located.
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the total number of athletes currently registered with the club.
        /// </summary>
        public int AthleteCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of competition entries (results) associated with the club's athletes.
        /// </summary>
        public int TotalEntries { get; set; }

        /// <summary>
        /// Gets or sets the count of first-place finishes (gold medals) achieved by the club.
        /// </summary>
        public int GoldMedals { get; set; }

        /// <summary>
        /// Gets or sets the birth date of the oldest athlete in the club.
        /// Can be null if the club has no registered athletes or data is missing.
        /// </summary>
        public DateTime? OldestAthleteBorn { get; set; }

        /// <summary>
        /// Gets or sets the birth date of the youngest athlete in the club.
        /// Can be null if the club has no registered athletes or data is missing.
        /// </summary>
        public DateTime? YoungestAthleteBorn { get; set; }

        /// <summary>
        /// Gets a formatted string representing the range of birth years within the club (e.g., "1990 - 2010").
        /// Returns a dash ("-") if birth date information is incomplete.
        /// </summary>
        public string AgeRange => (OldestAthleteBorn.HasValue && YoungestAthleteBorn.HasValue) ? $"{OldestAthleteBorn.Value.Year} - {YoungestAthleteBorn.Value.Year}" : "-";
    }
}
