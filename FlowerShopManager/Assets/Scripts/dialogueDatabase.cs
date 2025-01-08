using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DialogueEntry
{
    public string DialogueText;
    public List<string> RequiredFlowerMeanings;
    public List<Flower> RequiredFlowers;

    public DialogueEntry(string dialogueText, List<string> requiredFlowerMeanings, List<Flower> requiredFlowers = null)
    {
        DialogueText = dialogueText;
        RequiredFlowerMeanings = requiredFlowerMeanings;
        RequiredFlowers = requiredFlowers ?? new List<Flower>();
    }
}

public class DialogueDatabase : MonoBehaviour
{
    // Array of colors
    public string[] Colors = new string[]
    {
        "Yellow",
        "Black",
        "Orange",
        "Green",
        "Any Color"
    };

    // Array of general events
    public string[] EventsGeneral = new string[]
    {
        "Wedding",
        "Birthday",
        "Graduation"
    };

    // Array of other events
    public string[] EventsOther = new string[]
    {
        "Birthday",
        "Get Well Soon",
        "Graduation",
        "Valentine's Day",
        "Mother's Day",
        "Father's Day"
    };

    private GameManager GM;
    private FlowerDatabase flowerDatabase;

    public List<DialogueEntry> DialogueEntries { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.Instance;
        flowerDatabase = GM.flowerDatabase;

        // Example of adding dialogue entries
        DialogueEntries = new List<DialogueEntry>
        {
            new DialogueEntry(GetRandomDialogue("Do you have- what was it… Oh. Right. Do you have any {0}? They’re for my friend’s birthday."), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("Would I be able to get a bouquet to give to my brother for his {1}, please?", true), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("Could I please get a trial bridal bouquet where the main flowers are {0}?"), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("I want to give a bunch of {0} to this girl I’ve got a crush on; do you have anything like that?"), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("Do you know what {0} I’d need to give someone to let them down easily…?"), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("I’m going on this first date with this guy I really like, and I don’t want to mess it up. So I want to get him a bunch of {0}, but he is allergic to so many of them. Can you work around that at all?"), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("My kid just came out to me, and I want to show that I’m supportive of them. Can you even show that in a bouquet of {0}?"), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("Can you make a {1} bouquet, please?", false), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("Can I have a bunch of {0}, please? It’s for my partner."), new List<string> { "" }),
        };

        // Populate required flower meanings based on dialogue text
        PopulateRequiredFlowerMeanings();
    }

    private string GetRandomDialogue(string template, bool isEvent = false)
    {
        // Implementation of GetRandomDialogue method
        // This method should return a formatted string based on the template and isEvent flag
        return string.Format(template, isEvent ? GetRandomEvent() : GetRandomColor());
    }

    private string GetRandomColor()
    {
        // Implementation of GetRandomColor method
        // This method should return a random color from the Colors array
        return Colors[Random.Range(0, Colors.Length)];
    }

    private string GetRandomEvent()
    {
        // Implementation of GetRandomEvent method
        // This method should return a random event from the EventsGeneral or EventsOther arrays
        var allEvents = new List<string>(EventsGeneral);
        allEvents.AddRange(EventsOther);
        return allEvents[Random.Range(0, allEvents.Count)];
    }

    private void PopulateRequiredFlowerMeanings()
    {
        foreach (var entry in DialogueEntries)
        {
            List<string> colors = new List<string>();
            List<string> types = new List<string>();

            // Search for colors in the dialogue text
            foreach (var color in Colors)
            {
                if (entry.DialogueText.Contains(color, System.StringComparison.OrdinalIgnoreCase))
                {
                    colors.Add(color);
                }
            }

            // Search for flower types in the dialogue text
            foreach (var family in GM.flowerDatabase.FlowerFamilies)
            {
                if (entry.DialogueText.Contains(family.Name, System.StringComparison.OrdinalIgnoreCase))
                {
                    types.Add(family.Name);
                }
            }

            // Get top matching flowers based on the found colors and types
            var flowers = GM.GetTopMatchingFlowers(
                colors.FirstOrDefault() ?? string.Empty,
                types.FirstOrDefault() ?? string.Empty,
                true, // Assuming hypoallergenic is true for this example
                true, // Assuming pet safe is true for this example
                entry.RequiredFlowerMeanings
            );

            // Add the found flowers to the RequiredFlowers list
            foreach (var flower in flowers)
            {
                if (!entry.RequiredFlowers.Contains(flower))
                {
                    entry.RequiredFlowers.Add(flower);
                }
            }
        }
    }
}