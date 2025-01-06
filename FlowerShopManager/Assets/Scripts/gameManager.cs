using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string csvFilePath = "Assets/Scripts/FlowersDB.csv";
    private FlowerDatabase flowerDatabase;

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
}