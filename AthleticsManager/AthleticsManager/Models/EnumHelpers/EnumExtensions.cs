namespace AthleticsManager.Models.EnumHelpers
{
    public static class EnumExtensions
    {
        public static string ToFriendlyString(this Discipline discipline)
        {
            switch (discipline)
            {
                case Discipline.Run50m: return "50m";
                case Discipline.Run60m: return "60m";
                case Discipline.Run100m: return "100m";
                case Discipline.Run150m: return "150m";
                case Discipline.Run200m: return "200m";
                case Discipline.Run300m: return "300m";
                case Discipline.Run400m: return "400m";
                case Discipline.Run500m: return "500m";
                case Discipline.Run600m: return "600m";
                case Discipline.Run800m: return "800m";
                case Discipline.Run1000m: return "1000m";
                case Discipline.Run1500m: return "1500m";
                case Discipline.Run1Mile: return "1 Mile";
                case Discipline.Run2000m: return "2000m";
                case Discipline.Run3000m: return "3000m";
                case Discipline.Run5000m: return "5000m";
                case Discipline.Run10000m: return "10000m";

                case Discipline.Hurdles50m: return "50m Hurdles";
                case Discipline.Hurdles60m: return "60m Hurdles";
                case Discipline.Hurdles80m: return "80m Hurdles";
                case Discipline.Hurdles100m: return "100m Hurdles";
                case Discipline.Hurdles110m: return "110m Hurdles";
                case Discipline.Hurdles200m: return "200m Hurdles";
                case Discipline.Hurdles300m: return "300m Hurdles";
                case Discipline.Hurdles400m: return "400m Hurdles";

                case Discipline.Steeplechase1500m: return "1500m Steeplechase";
                case Discipline.Steeplechase2000m: return "2000m Steeplechase";
                case Discipline.Steeplechase3000m: return "3000m Steeplechase";

                case Discipline.LongJump: return "Long Jump";
                case Discipline.TripleJump: return "Triple Jump";
                case Discipline.HighJump: return "High Jump";
                case Discipline.PoleVault: return "Pole Vault";
                case Discipline.StandingLongJump: return "Standing Long Jump";

                case Discipline.ShotPut: return "Shot Put";
                case Discipline.DiscusThrow: return "Discus Throw";
                case Discipline.JavelinThrow: return "Javelin Throw";
                case Discipline.HammerThrow: return "Hammer Throw";
                case Discipline.CricketBallThrow: return "Cricket Ball Throw";

                case Discipline.Run4x60m: return "4x60m Relay";
                case Discipline.Run4x100m: return "4x100m Relay";
                case Discipline.Run4x200m: return "4x200m Relay";
                case Discipline.Run4x300m: return "4x300m Relay";
                case Discipline.Run4x400m: return "4x400m Relay";

                case Discipline.Walk3000m: return "3000m Walk";
                case Discipline.Walk5000m: return "5000m Walk";
                case Discipline.Walk10km: return "10km Walk";
                case Discipline.Walk20km: return "20km Walk";

                case Discipline.Triathlon: return "Triathlon";
                case Discipline.Tetrathlon: return "Tetrathlon";
                case Discipline.Pentathlon: return "Pentathlon";
                case Discipline.Heptathlon: return "Heptathlon";
                case Discipline.Decathlon: return "Decathlon";

                default: return discipline.ToString();
            }
        }

        public static string GetUnit(this Discipline discipline)
        {
            switch (discipline)
            {
                case Discipline.Run50m:
                case Discipline.Run60m:
                case Discipline.Run100m:
                case Discipline.Run150m:
                case Discipline.Run200m:
                case Discipline.Run300m:
                case Discipline.Run400m:
                case Discipline.Run500m:
                case Discipline.Run600m:
                case Discipline.Run800m:
                case Discipline.Run1000m:
                case Discipline.Run1500m:
                case Discipline.Run1Mile:
                case Discipline.Run2000m:
                case Discipline.Run3000m:
                case Discipline.Run5000m:
                case Discipline.Run10000m:
                case Discipline.Hurdles50m:
                case Discipline.Hurdles60m:
                case Discipline.Hurdles80m:
                case Discipline.Hurdles100m:
                case Discipline.Hurdles110m:
                case Discipline.Hurdles200m:
                case Discipline.Hurdles300m:
                case Discipline.Hurdles400m:
                case Discipline.Steeplechase1500m:
                case Discipline.Steeplechase2000m:
                case Discipline.Steeplechase3000m:
                case Discipline.Run4x60m:
                case Discipline.Run4x100m:
                case Discipline.Run4x200m:
                case Discipline.Run4x300m:
                case Discipline.Run4x400m:
                case Discipline.Walk3000m:
                case Discipline.Walk5000m:
                case Discipline.Walk10km:
                case Discipline.Walk20km:
                    return "s";

                case Discipline.LongJump:
                case Discipline.TripleJump:
                case Discipline.HighJump:
                case Discipline.PoleVault:
                case Discipline.StandingLongJump:
                case Discipline.ShotPut:
                case Discipline.DiscusThrow:
                case Discipline.JavelinThrow:
                case Discipline.HammerThrow:
                case Discipline.CricketBallThrow:
                    return "m";

                case Discipline.Triathlon:
                case Discipline.Tetrathlon:
                case Discipline.Pentathlon:
                case Discipline.Heptathlon:
                case Discipline.Decathlon:
                    return "pts";

                default:
                    return "";
            }
        }

        public static string ToFriendlyString(this Region region)
        {
            switch (region)
            {
                case Region.HlavníměstoPraha: return "Hlavní město Praha";
                case Region.Středočeskýkraj: return "Středočeský kraj";
                case Region.Jihočeskýkraj: return "Jihočeský kraj";
                case Region.Plzeňskýkraj: return "Plzeňský kraj";
                case Region.Karlovarskýkraj: return "Karlovarský kraj";
                case Region.Ústeckýkraj: return "Ústecký kraj";
                case Region.Libereckýkraj: return "Liberecký kraj";
                case Region.Královéhradeckýkraj: return "Královéhradecký kraj";
                case Region.Pardubickýkraj: return "Pardubický kraj";
                case Region.KrajVysočina: return "Kraj Vysočina";
                case Region.Jihomoravskýkraj: return "Jihomoravský kraj";
                case Region.Olomouckýkraj: return "Olomoucký kraj";
                case Region.Zlínskýkraj: return "Zlínský kraj";
                case Region.Moravskoslezskýkraj: return "Moravskoslezský kraj";
                case Region.Other: return "Other";

                default: return "Unknown region";
            }
        }

        public static string FormatPerformance(this Discipline discipline, decimal value)
        {
            string name = discipline.ToString();

            bool isMiddleOrLongDistance =
        discipline == Discipline.Run800m ||
        discipline == Discipline.Run1000m ||
        discipline == Discipline.Run1500m ||
        discipline == Discipline.Run1Mile ||
        discipline == Discipline.Run2000m ||
        discipline == Discipline.Run3000m ||
        discipline == Discipline.Run5000m ||
        discipline == Discipline.Run10000m ||
        name.Contains("Walk") ||
        name.Contains("Steeplechase");

            if (isMiddleOrLongDistance)
            {
                TimeSpan time = TimeSpan.FromSeconds((double)value);
                if (time.TotalHours >= 1)
                    return time.ToString(@"h\:mm\:ss");

                return time.ToString(@"m\:ss\.ff");
            }

            if (name.Contains("Jump") || name.Contains("Vault"))
            {
                if (value > 30)
                {
                    return (value / 100m).ToString("F2");
                }
            }

            return value.ToString("F2");
        }
    }
}
