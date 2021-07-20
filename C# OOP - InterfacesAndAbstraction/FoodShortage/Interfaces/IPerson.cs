using System;
using System.Collections.Generic;
using System.Text;
using FoodShortage.Interfaces;

namespace FoodShortage.Interfaces
{
    public interface IPerson : IBuyer
    {
        public string Name { get;}
        public int Age { get; }
    }
}
