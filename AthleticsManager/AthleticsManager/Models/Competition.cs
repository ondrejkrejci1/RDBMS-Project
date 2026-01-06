namespace AthleticsManager.Models
{
    public class Competition
    {
        public int CompetitionId { get; protected set; }
        public string Name { get; protected set; }
        public DateTime Date { get; protected set; }
        public string Venue { get; protected set; }
        public string Type { get; protected set; }

        public Competition(string name, DateTime date, string venue, string type) 
        {
            Name = name;
            Date = date;
            Venue = venue;
            Type = type;
        }

        public Competition(int competitionId, string name, DateTime date, string venue, string type)
        {
            CompetitionId = competitionId;
            Name = name;
            Date = date;
            Venue = venue;
            Type = type;
        }

        public void SetCompetitionId(int competitionId)
        {
            CompetitionId = competitionId;
        }

        public string DateString
        {
            get
            {
                return Date.ToString("dd.MM.yyyy");
            }
        }

    }
}
