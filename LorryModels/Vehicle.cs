using System;

namespace LorryModels
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string License { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Int16 Year { get; set; }

        public Vehicle()
        {

        }

        public Vehicle(int id, string name, string license, string make, string model, Int16 year)
        {
            Id = id;
            Name = name;
            Make = make;
            Model = model;
            Year = year;
        }

    }
}
