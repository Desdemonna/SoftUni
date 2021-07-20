using System;
using System.Collections.Generic;
using System.Text;
using FoodShortage.Interfaces;

namespace FoodShortage.Models
{
    public class Rebel : IRebel
    {
        public Rebel(string name, int age, string group)
        {
            Group = group;
            Name = name;
            Age = age;
        }

        public string Group { get; private set; }

        public string Name { get; private set; }

        public int Age { get; private set; }

        public int Food { get; private set; }


        public void BuyFood()
        {
            this.Food += 5;
        }
    }
}
