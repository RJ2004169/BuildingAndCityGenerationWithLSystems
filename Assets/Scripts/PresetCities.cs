using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetCities : MonoBehaviour
{
    private static List<Dictionary<string, Dictionary<char, string>>> savedSystemsList;
    private Dictionary<char, string> rules;
    private Dictionary<string, Dictionary<char, string>> savedSystems;
    // Start is called before the first frame update
    void Start()
    {
        presetSystems();
    }

    void presetSystems()
    {
        rules = new Dictionary<char, string>
                {
                    { 'X', "[F-[X+X]+F[+FX]-X]" },
                    { 'F', "FF" }
                };
        savedSystems = new Dictionary<string, Dictionary<char, string>>
        {
            { "City1", rules }
        };
        savedSystemsList = new List<Dictionary<string, Dictionary<char, string>>>();
        savedSystemsList.Add(savedSystems);

        rules = new Dictionary<char, string>
                {
                    { 'X', "[-FX][+FX][FX]" },
                    { 'F', "FF" }
                };
        savedSystems = new Dictionary<string, Dictionary<char, string>>
        {
            { "City2", rules }
        };
        savedSystemsList.Add(savedSystems);

        rules = new Dictionary<char, string>
                {
                    { 'X', "[-FX]X[+FX][+F-FX]" },
                    { 'F', "FF" }
                };
        savedSystems = new Dictionary<string, Dictionary<char, string>>
        {
            { "City3", rules }
        };
        savedSystemsList.Add(savedSystems);

        rules = new Dictionary<char, string>
                {
                    { 'X', "[FF[+XF-F+FX]--F+F-FX]" },
                    { 'F', "FF" }
                };
        savedSystems = new Dictionary<string, Dictionary<char, string>>
        {
            { "City4", rules }
        };
        savedSystemsList.Add(savedSystems);

    }

    public void addCustomSystems(Dictionary<string, Dictionary<char, string>> newSystem)
    {
        savedSystemsList.Add(newSystem);
    }

    public List<Dictionary<string, Dictionary<char, string>>> getSavedSystemsList()
    {
        return savedSystemsList;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
