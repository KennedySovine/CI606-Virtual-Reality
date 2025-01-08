using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerTabletManager : MonoBehaviour
{
    public Text titleText;
    public Text description;

    public GameObject dogIcon;
    public GameObject hypoallergenicIcon;

    private GameManager gameManager;

    public List<FlowerFamily> flowerFamilies;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        titleText.text = "Daisy";

        List<FlowerFamily> flowerFamilies = GetFlowerFamilies();
        foreach (var family in flowerFamilies)
        {
            Debug.Log($"Flower Family: {family.Name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//No button but oh well
    public void SetTitleText(string newText)
    {
        if (titleText == "NULL"){

        }
    }

    public void SetDescriptionText(string newText)
    {
        if (description != null)
        {
            description.text = newText;
        }
    }

    public List<FlowerFamily> GetFlowerFamilies()
    {
        if (gameManager != null)
        {
            return gameManager.FlowerData;
        }
        return new List<FlowerFamily>();
    }
}
