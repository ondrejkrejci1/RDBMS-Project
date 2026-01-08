namespace AthleticsManager.Models
{
    /// <summary>
    /// Represents a simplified view of a race result intended for display on the dashboard or recent activity lists.
    /// This model aggregates key information into string properties for direct binding to the UI, 
    /// abstracting away the underlying numeric IDs and complex formatting logic.
    /// </summary>
    public class RecentResultView
    {
        /// <summary>
        /// Gets or sets the full name of the athlete.
        /// </summary>
        public string AthleteName { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the discipline (e.g., "100m Sprint").
        /// </summary>
        public string DisciplineName { get; set; }

        /// <summary>
        /// Gets or sets the formatted performance string (e.g., "10.58 s" or "5.20 m").
        /// </summary>
        public string Performance { get; set; }

        /// <summary>
        /// Gets or sets the formatted date string of the competition.
        /// </summary>
        public string DateString { get; set; }
    }
}
