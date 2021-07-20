using System;
using System.Collections.Generic;
using System.Text;
using FoodShortage.Interfaces;

namespace FoodShortage.Interfaces
{
    public interface IRebel : IPerson
    {
        public string Group { get; }
    }
}
