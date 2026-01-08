namespace AthleticsManager.Models
{
    /// <summary>
    /// Represents a flattened view model for displaying a single result record within a competition context.
    /// This class is designed for UI data binding, providing pre-formatted strings for athlete names, disciplines, and performance metrics.
    /// </summary>
    public class CompetitionResultView
    {
        /// <summary>
        /// Gets or sets the friendly display name of the athletic discipline (e.g., "100m Sprint").
        /// </summary>
        public string DisciplineName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the athlete who competed.
        /// </summary>
        public string AthleteName { get; set; }

        /// <summary>
        /// Gets or sets the formatted performance string (e.g., "10.58").
        /// This value is typically pre-processed to match the specific formatting rules of the discipline.
        /// </summary>
        public string Performance { get; set; }

        /// <summary>
        /// Gets or sets the unit of measurement associated with the performance (e.g., "s" for seconds, "m" for meters).
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the athlete's final placement or rank in the event.
        /// </summary>
        public int Rank { get; set; }
    }
}
