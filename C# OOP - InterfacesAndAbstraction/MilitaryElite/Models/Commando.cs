using MilitaryElite.Enumerations;
using MilitaryElite.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace MilitaryElite.Models
{
    public class Commando : SpecialisedSoldier, ICommando
    {
        public Commando(int id, string firstName, string lastName, decimal salary, SoldierCorpEnum SoldierCorp, ICollection<IMission> missions) 
            : base(id, firstName, lastName, salary, SoldierCorp)
        {
            this.Missions = missions;
        }

        public ICollection<IMission> Missions { get; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString()
                + "Missions:");

            foreach (var mission in this.Missions)
            {
                stringBuilder.AppendLine($"  {mission}");
            }

            return stringBuilder
                .ToString()
                .TrimEnd();
        }
    }
}
