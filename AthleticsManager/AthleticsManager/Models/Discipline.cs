namespace AthleticsManager.Models
{
    public class Discipline
    {
        public int DisciplineID { get; protected set; }
        public string Name { get; protected set; }
        public string UnitType { get; protected set; }

        public Discipline(string name, string unitType)
        {
            Name = name;
            UnitType = unitType;
        }
        public Discipline(int disciplineID, string name, string unitType)
        {
            DisciplineID = disciplineID;
            Name = name;
            UnitType = unitType;
        }

    }
}
