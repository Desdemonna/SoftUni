using MilitaryElite.Enumerations;
using MilitaryElite.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace MilitaryElite.Models
{
    public class Engineer : SpecialisedSoldier, IEngineer
    {
        public Engineer(int id, string firstName, string lastName, decimal salary, SoldierCorpEnum SoldierCorp, ICollection<IRepair> repairs) 
            : base(id, firstName, lastName, salary, SoldierCorp)
        {
            this.Repairs = repairs;
        }

        public ICollection<IRepair> Repairs { get ; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString() + "Repairs:");

            foreach (var repair in this.Repairs)
            {
                stringBuilder.AppendLine($"  {repair}");
            }

            return stringBuilder
                .ToString()
                .TrimEnd();
        }
    }
}
