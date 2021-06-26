using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiRental
{
    public class SkiRental
    {
        List<Ski> skies;

        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Count => skies.Count;

        public SkiRental(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            skies = new List<Ski>(); 
        }

        public void Add(Ski ski)
        {
            if (Capacity <= Count)
            {
                return;
            }

            skies.Add(ski);
        }

        public bool Remove(string manufacturer, string model)
        {
            bool isSkiesExist = skies.Exists(x => x.Manufacturer == manufacturer && x.Model == model);
            if (isSkiesExist)
            {
                skies.Remove(skies.FirstOrDefault(x => x.Manufacturer == manufacturer && x.Model == model));
            }
            return isSkiesExist;
        }

        public Ski GetNewestSki()
        {
            return skies.OrderByDescending(x => x.Year).FirstOrDefault();
        }

        public Ski GetSki(string manufacturer, string model)
        {
            return skies.FirstOrDefault(x => x.Manufacturer == manufacturer && x.Model == model);
        }

        public string GetStatistics()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"The skis stored in {Name}:");
            foreach (Ski ski in skies)
            {
                sb.AppendLine($"{ski}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
