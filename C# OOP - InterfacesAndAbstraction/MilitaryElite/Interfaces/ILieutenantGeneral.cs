namespace MilitaryElite.Interfaces
{
    using System.Collections.Generic;

    public interface ILieutenantGeneral
    {
        public ICollection<IPrivate> Privates { get; }
    }
}
