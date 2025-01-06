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
    public List<DialogueEntry> DialogueEntries;

    // Array of colors mentioned in the CSV file
    public string[] Colors = new string[]
    {
        "White",
        "Pink",
        "Purple",
        "Blue",
        "Red",
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

    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.Instance;
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
            new DialogueEntry(GetRandomDialogue("Can I have a bouquet of {0}s please? It’s my Girlfriend’s favourite flower."), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("Can I have a bunch of only {2} flowers, please? The meaning doesn’t matter, I just like the colour."), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("My little sister loves {0}. Can I have a bouquet with those as a focus, please?"), new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("My dad’s favorite flower is a {0}. Can I have a bouquet where that’s a part of it please? Even better if there’s also {2}."), new List<string> { "" }),
            new DialogueEntry("I… My parent died. What flowers do you have for a funeral?", new List<string> { "" }),
            new DialogueEntry("My friend is moving into a new house, what would be a good housewarming gift for them?", new List<string> { "" }),
            new DialogueEntry("My friend is pregnant, and I want to get them something to show them congratulations.", new List<string> { "" }),
            new DialogueEntry(GetRandomDialogue("Could I get a {1} bouquet?", false), new List<string> { "" }),
        };

        // Populate required flower meanings based on dialogue text
        PopulateRequiredFlowerMeanings();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string GetRandomDialogue(string template, bool useGeneralEvents = true)
    {
        var randomFlowerFamily = RandomFlowerFamily();
        var randomEvent = useGeneralEvents ? EventsGeneral[Random.Range(0, EventsGeneral.Length)] : EventsOther[Random.Range(0, EventsOther.Length)];
        var randomColor = Colors[Random.Range(0, Colors.Length)];
        return string.Format(template, randomFlowerFamily.Name, randomEvent, randomColor);
    }

    private FlowerFamily RandomFlowerFamily()
    {
        var flowerFamilies = GM.flowerDatabase.FlowerFamilies;
        return flowerFamilies[Random.Range(0, flowerFamilies.Count)];
    }

    private void PopulateRequiredFlowerMeanings()
    {
        foreach (var entry in DialogueEntries)
        {
            var keywords = ExtractKeywords(entry.DialogueText);
            foreach (var keyword in keywords)
            {
                var flowers = GM.SearchFlowersByMeaning(keyword);
                foreach (var flower in flowers)
                {
                    if (!entry.RequiredFlowerMeanings.Contains(flower.PositiveMeaning) && flower.PositiveMeaning != null)
                    {
                        entry.RequiredFlowerMeanings.Add(flower.PositiveMeaning);
                    }
                    if (!entry.RequiredFlowerMeanings.Contains(flower.NegativeMeaning) && flower.NegativeMeaning != null)
                    {
                        entry.RequiredFlowerMeanings.Add(flower.NegativeMeaning);
                    }
                }
            }
        }
    }

    private List<string> ExtractKeywords(string dialogueText)
    {
        // Example implementation: extract keywords based on specific words in the dialogue text
        var keywords = new List<string>();
        if (dialogueText.Contains("graduation"))
        {
            keywords.Add("happiness");
            keywords.Add("achievement");
            keywords.Add("joy");
            keywords.add("celebration");
            keywords.Add("success");
            keywords.Add("cheerfullness");
        }
        if (dialogueText.Contains("wedding"))
        {
            keywords.Add("purity");
        }
        if (dialogueText.Contains("birthday")) keywords.Add("joy");
        if (dialogueText.Contains("get well soon")) {keywords.Add("good health"); keywords.Add("health");}
        if (dialogueText.Contains("valentine's day")) keywords.Add("valentine's day");
        if (dialogueText.Contains("mother's day")) keywords.Add("mother");
        if (dialogueText.Contains("funeral")) {keywords.Add("death"); keywords.Add("sadness");}
        if (dialogueText.Contains("housewarming")) {keywords.Add("house"); keywords.Add("change");}
        if (dialogueText.Contains("friendship")) {keywords.Add("admiration"); keywords.Add("appreciation");}
        if (dialogueText.Contains("supportive")) keywords.Add("support");
        if (dialogueText.Contains("crush")) keywords.Add("love");
        if (dialogueText.Contains("date")) keywords.Add("love");
        return keywords;
    }

    private void FindFlowers()
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