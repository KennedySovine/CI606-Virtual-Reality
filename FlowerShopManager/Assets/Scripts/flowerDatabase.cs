using System;
using System.Collections.Generic;
using System.IO;

public class Flower
{
    public string Color { get; set; }
    public string PositiveMeaning { get; set; }
    public string NegativeMeaning { get; set; }
    public bool Hypoallergenic { get; set; }
    public bool PetSafe { get; set; }

    public Flower(string color, string positiveMeaning, string negativeMeaning, bool hypoallergenic, bool petSafe)
    {
        Color = color;
        PositiveMeaning = string.IsNullOrEmpty(positiveMeaning) ? null : positiveMeaning;
        NegativeMeaning = string.IsNullOrEmpty(negativeMeaning) ? null : negativeMeaning;
        Hypoallergenic = hypoallergenic;
        PetSafe = petSafe;
    }
}

public class FlowerFamily
{
    public string Name { get; set; }
    public List<Flower> FlowersInFamily { get; set; }

    public FlowerFamily(string name)
    {
        Name = name;
        FlowersInFamily = new List<Flower>();
    }
}

public class FlowerDatabase
{
    public List<FlowerFamily> FlowerFamilies { get; private set; }

    public FlowerDatabase()
    {
        FlowerFamilies = new List<FlowerFamily>();
    }

    public void LoadFromCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        {
            // Skip the header line
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                var familyName = values[0];
                var color = values[1];
                var positiveMeaning = values[2];
                var negativeMeaning = values[3];
                var hypoallergenic = bool.Parse(values[4]);
                var petSafe = bool.Parse(values[5]);

                var flower = new Flower(color, positiveMeaning, negativeMeaning, hypoallergenic, petSafe);

                var family = FlowerFamilies.Find(f => f.Name == familyName);
                if (family == null)
                {
                    family = new FlowerFamily(familyName);
                    FlowerFamilies.Add(family);
                }
                family.FlowersInFamily.Add(flower);
            }
        }
    }
}