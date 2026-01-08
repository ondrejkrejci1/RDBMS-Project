using AthleticsManager.Models.EnumHelpers;

namespace AthleticsManager.Models
{
    /// <summary>
    /// Represents a summary record of a top athletic performance.
    /// This model is primarily used for displaying statistical data, such as "Hall of Fame" or top results lists,
    /// and contains logic to format raw performance data into a user-friendly format.
    /// </summary>
    public class TopPerformance
    {
        /// <summary>
        /// Gets or sets the unique identifier of the discipline.
        /// This ID is used to map the raw data to the Discipline enum for formatting logic.
        /// </summary>
        public int DisciplineID { get; set; }

        /// <summary>
        /// Gets or sets the name of the discipline.
        /// </summary>
        public string Discipline { get; set; }

        /// <summary>
        /// Gets or sets the raw numerical value of the performance.
        /// This could represent time in seconds (for track events) or distance in meters (for field events).
        /// </summary>
        public decimal Performance { get; set; }

        /// <summary>
        /// Gets or sets the full name of the athlete who achieved the result.
        /// </summary>
        public string AthleteName { get; set; }

        /// <summary>
        /// Gets or sets the name of the competition where the result was achieved.
        /// </summary>
        public string CompetitionName { get; set; }

        /// <summary>
        /// Gets or sets the date when the competition took place.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets a short string representation of the competition date.
        /// Useful for simplifying data binding in the user interface.
        /// </summary>
        public string DateString => Date.ToShortDateString();

        /// <summary>
        /// Gets the performance value formatted as a user-friendly string.
        /// This property utilizes the Discipline enum helpers to convert the raw numerical value 
        /// into the standard format for that sport (e.g., "MM:SS.ms") and appends the correct unit (e.g., "s", "m").
        /// </summary>
        public string FormattedPerformance
        {
            get
            {
                var disciplineEnum = (Discipline)DisciplineID;

                string value = disciplineEnum.FormatPerformance(Performance);

                string unit = disciplineEnum.GetUnit();

                return $"{value} {unit}";
            }
        }
    }
}
