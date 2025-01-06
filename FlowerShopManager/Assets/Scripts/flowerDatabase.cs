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

public class FlowerInit
{
    public List<FlowerFamily> FlowerDatabase { get; private set; }

    public FlowerInit()
    {
        FlowerDatabase = new List<FlowerFamily>();
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

                var flowerFamilyName = values[0];
                var flower = new Flower(
                    values[1], // Color
                    values[2], // Positive Meaning
                    values[3], // Negative Meaning
                    bool.Parse(values[4]), // Hypoallergenic
                    bool.Parse(values[5]) // PetSafe
                );

                var flowerFamily = FlowerDatabase.Find(f => f.Name == flowerFamilyName);
                if (flowerFamily == null)
                {
                    flowerFamily = new FlowerFamily(flowerFamilyName);
                    FlowerDatabase.Add(flowerFamily);
                }

                flowerFamily.FlowersInFamily.Add(flower);
            }
        }
    }
}