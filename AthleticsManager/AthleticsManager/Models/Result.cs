namespace AthleticsManager.Models
{
    public class Result
    {
        public int ResultID { get; protected set; }
        public int AthleteID { get; protected set; }
        public int CompetitionID { get; protected set; }
        public int DisciplineID { get; protected set; }
        public decimal Performance { get; protected set; }
        public double? Wind { get; protected set; }
        public int? Placement { get; protected set; }
        public string? Note { get; protected set; }

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

        public void SetResultID(int resultID)
        {
            ResultID = resultID;
        }
    }
}
