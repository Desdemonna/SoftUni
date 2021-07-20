using MilitaryElite.Enumerations;
using MilitaryElite.Interfaces;
using System;

namespace MilitaryElite.Models
{
    public abstract class SpecialisedSoldier : Private, ISpecialisedSoldier
    {
        public SpecialisedSoldier(int id, string firstName, string lastName, decimal salary, SoldierCorpEnum SoldierCorp) 
            : base(id, firstName, lastName, salary)
        {
            this.SoldierCorp = SoldierCorp;
        }

        public SoldierCorpEnum SoldierCorp { get; }

        public override string ToString()
        {
            return base.ToString()
                  + Environment.NewLine
                  + $"Corps: {this.SoldierCorp}"
                  + Environment.NewLine;
        }
    }
}
