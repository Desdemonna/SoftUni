using MilitaryElite.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MilitaryElite.Models
{
    public class LieutenantGeneral : Private, ILieutenantGeneral
    {
        public LieutenantGeneral(int id, string firstName, string lastName, decimal salary, ICollection<IPrivate> privates) 
            : base(id, firstName, lastName, salary)
        {
            this.Privates = privates;
        }
        
        public ICollection<IPrivate> Privates { get; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString()
                + Environment.NewLine
                + "Privates:");

            foreach (var @private in this.Privates)
            {
                stringBuilder.AppendLine($"  {@private}");
            }

            return stringBuilder
                .ToString()
                .TrimEnd();
        }
    }
}
