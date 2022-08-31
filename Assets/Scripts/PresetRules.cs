using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetRules : MonoBehaviour
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
                    { 'X', "[F[+FX][*+FX][/+FX]]" },
                    { 'F', "FF" }
                };
        savedSystems = new Dictionary<string, Dictionary<char, string>>
        {
            { "Building1", rules }
        };
        savedSystemsList = new List<Dictionary<string, Dictionary<char, string>>>();
        savedSystemsList.Add(savedSystems);

        rules = new Dictionary<char, string>
                {
                    { 'X', "[*+FX]X[+FX][/+F-FX]" },
                    { 'F', "FF" }
                };
        savedSystems = new Dictionary<string, Dictionary<char, string>>
        {
            { "Building2", rules }
        };
        savedSystemsList.Add(savedSystems);

        rules = new Dictionary<char, string>
                {
                    { 'X', "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]" },
                    { 'F', "FF" }
                };
        savedSystems = new Dictionary<string, Dictionary<char, string>>
        {
            { "Building3", rules }
        };
        savedSystemsList.Add(savedSystems);

        rules = new Dictionary<char, string>
                {
                    { 'X', "[F[+FX]X[*+FX]X[/+FX]]" },
                    { 'F', "FF" }
                };
        savedSystems = new Dictionary<string, Dictionary<char, string>>
        {
            { "Building4", rules }
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
