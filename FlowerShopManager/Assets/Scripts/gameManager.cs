using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string csvFilePath = "Assets/Scripts/Flowers - Sheet1.csv";
    public List<FlowerFamily> FlowerData => flowerDatabase.FlowerFamilies;

    public FlowerDatabase flowerDatabase { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
        flowerDatabase = new FlowerDatabase();
        LoadFlowerData();
    }

    void Start()
    {
        // Other initialization code can go here
    }

    void LoadFlowerData()
    {
        flowerDatabase.LoadFromCsv(csvFilePath);
        Debug.Log("Flower data loaded successfully.");
        
        foreach (var family in flowerDatabase.FlowerFamilies)
        {
            Debug.Log($"Flower Family: {family.Name}");
            foreach (var flower in family.FlowersInFamily)
            {
                Debug.Log($"  Color: {flower.Color}, Positive Meaning: {flower.PositiveMeaning}, Negative Meaning: {flower.NegativeMeaning}, Hypoallergenic: {flower.Hypoallergenic}, Pet Safe: {flower.PetSafe}");
            }
        }
    }

    public List<Flower> SearchFlowersByMeaning(string meaning)
    {
        return flowerDatabase.FlowerFamilies
            .SelectMany(family => family.FlowersInFamily)
            .Where(flower => (flower.PositiveMeaning != null && flower.PositiveMeaning.Contains(meaning)) ||
                             (flower.NegativeMeaning != null && flower.NegativeMeaning.Contains(meaning)))
            .ToList();
    }

    public List<Flower> SearchFlowersByColor(string color)
    {
        return flowerDatabase.FlowerFamilies
            .SelectMany(family => family.FlowersInFamily)
            .Where(flower => flower.Color == color)
            .ToList();
    }

    public List<Flower> SearchFlowersByHypoallergenic(bool hypoallergenic)
    {
        return flowerDatabase.FlowerFamilies
            .SelectMany(family => family.FlowersInFamily)
            .Where(flower => flower.Hypoallergenic == hypoallergenic)
            .ToList();
    }

    public List<Flower> SearchFlowersByPetSafe(bool petSafe)
    {
        return flowerDatabase.FlowerFamilies
            .SelectMany(family => family.FlowersInFamily)
            .Where(flower => flower.PetSafe == petSafe)
            .ToList();
    }

    public List<Flower> SearchFlowersByType(string type)
    {
        return flowerDatabase.FlowerFamilies
            .Where(family => family.Name == type)
            .SelectMany(family => family.FlowersInFamily)
            .ToList();
    }

    public List<Flower> GetTopMatchingFlowers(string color, string type, bool hypoallergenic, bool petSafe, List<string> meanings)
    {
        var flowersByColor = SearchFlowersByColor(color);
        var flowersByType = SearchFlowersByType(type);
        var flowersByHypoallergenic = SearchFlowersByHypoallergenic(hypoallergenic);
        var flowersByPetSafe = SearchFlowersByPetSafe(petSafe);
        var flowersByMeanings = meanings.SelectMany(SearchFlowersByMeaning).Distinct().ToList();
    
        var allFlowers = flowersByColor
            .Union(flowersByType)
            .Union(flowersByHypoallergenic)
            .Union(flowersByPetSafe)
            .Union(flowersByMeanings)
            .Distinct()
            .ToList();
    
        var rankedFlowers = allFlowers
            .Select(flower => new
            {
                Flower = flower,
                Score = (flowersByColor.Contains(flower) ? 1 : 0) +
                        (flowersByType.Contains(flower) ? 1 : 0) +
                        (flowersByHypoallergenic.Contains(flower) ? 1 : 0) +
                        (flowersByPetSafe.Contains(flower) ? 1 : 0) +
                        (flowersByMeanings.Contains(flower) ? 1 : 0)
            })
            .OrderByDescending(f => f.Score)
            .ThenBy(f => flowerDatabase.FlowerFamilies.First(family => family.FlowersInFamily.Contains(f.Flower)).Name)
            .Take(5)
            .Select(f => f.Flower)
            .ToList();
    
        return rankedFlowers;
    }
}