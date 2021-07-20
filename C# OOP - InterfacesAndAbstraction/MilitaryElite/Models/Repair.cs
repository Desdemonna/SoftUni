using MilitaryElite.Interfaces;

namespace MilitaryElite.Models
{
    public class Repair : IRepair
    {

        public Repair(string name, int hoursWorked)
        {
            this.PartName = name;
            this.HoursWorked = hoursWorked;
        }

        public string PartName { get; }

        public int HoursWorked { get; }

        public override string ToString()
        {
            return $"Part Name: {this.PartName} Hours Worked: {this.HoursWorked}";
        }
    }
}
