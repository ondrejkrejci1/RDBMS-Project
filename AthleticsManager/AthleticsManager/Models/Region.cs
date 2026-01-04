using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthleticsManager.Models
{
    public class Region
    {
        public int RegionID { get; protected set; }
        public string Name { get; protected set; }

        public Region(int regionID, string name)
        {
            RegionID = regionID;
            Name = name;
        }
    }
}
