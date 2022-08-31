using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static bool cityBuilderMode;
    private static Vector3 startingPosition;
    private static Quaternion startingRotation;

    public TMPro.TMP_Dropdown buildingSelector, citySelector;
    public TMPro.TMP_Text rulesDisplayed;
    public Button presetTab, customTab, settingsTab, cityTab;
    public Slider overlapSlider;
    public GameObject mainGO;
    public Canvas mainCanvas;


    List<Dictionary<string, Dictionary<char, string>>> savedList, citySavedList;
    List<string> options;
    Dictionary<string, Dictionary<char, string>> newSystem;
    Dictionary<char, string> newProduction;
    KeyValuePair<string, Dictionary<char, string>> selectedSystem, citySelectedSystem;


    char character;
    string rule, name, axiom;
    int iterations;
    float angle, overlap;

    // Start is called before the first frame update
    void Start()
    {
        
        newSystem = new Dictionary<string, Dictionary<char, string>>();
        options = new List<string>();
        newProduction = new Dictionary<char, string>();
        savedList = new List<Dictionary<string, Dictionary<char, string>>>();
        citySavedList = new List<Dictionary<string, Dictionary<char, string>>>();
        selectedSystem = new KeyValuePair<string, Dictionary<char, string>>();
        citySelectedSystem = new KeyValuePair<string, Dictionary<char, string>>();
        cityBuilderMode = false;
        

        getSavedSystemsListAndPopulateFields();
        getSavedCitiesAndPopulateFields();
        presetTab.Select();
        presetTabClicked();
        
    }

    void getSavedCitiesAndPopulateFields()
    {
        citySavedList = mainGO.GetComponent<PresetCities>().getSavedSystemsList();
        if (citySavedList == null)
        {
            Debug.Log("city saved list is null");
        }
        else
        {
            createListOfOptions(citySavedList);
        }
        PopulateCityDropdown();

    }

    void getSavedSystemsListAndPopulateFields()
    {
        savedList = mainGO.GetComponent<PresetRules>().getSavedSystemsList();
        if(savedList == null)
        {
            Debug.Log("saved list is null");
        }
        else
        {
            createListOfOptions(savedList);
        }
        PopulateDropdown();
    }

    void createListOfOptions(List<Dictionary<string, Dictionary<char, string>>> passedList)
    {
        options.Clear();
        foreach(Dictionary<string, Dictionary<char, string>> systemWithName in passedList)
        {
            foreach(KeyValuePair<string, Dictionary<char, string>> kvp in systemWithName)
            {
                options.Add(kvp.Key);
            }
        }

        
    }

    void PopulateCityDropdown()
    {
        citySelector.ClearOptions();
        citySelector.AddOptions(options);
        cityDropDownValueChanged(0);
    }

    void PopulateDropdown()
    {
        buildingSelector.ClearOptions();
        buildingSelector.AddOptions(options);
        dropDownValueChanged(0);
    }

    public void dropDownValueChanged(int currentValue)
    {
        rulesDisplayed.SetText("");
        int i = 0;
        foreach (Dictionary<string, Dictionary<char, string>> systemWithName in savedList)
        {
            if(i == currentValue)
            {
                
                foreach (KeyValuePair<string, Dictionary<char, string>> kvp in systemWithName)
                {
                    selectedSystem = kvp;
                    foreach (KeyValuePair<char, string> kvp2 in kvp.Value)
                    {
                        rulesDisplayed.SetText(rulesDisplayed.text + kvp2.Key + " -> " + kvp2.Value + "\n");

                    }
                }                
            }
            i++;
            
        }
    }

    public void cityDropDownValueChanged(int currentValue)
    {
        rulesDisplayed.SetText("");
        int i = 0;
        foreach (Dictionary<string, Dictionary<char, string>> systemWithName in citySavedList)
        {
            if (i == currentValue)
            {

                foreach (KeyValuePair<string, Dictionary<char, string>> kvp in systemWithName)
                {
                    citySelectedSystem = kvp;
                    foreach (KeyValuePair<char, string> kvp2 in kvp.Value)
                    {
                        rulesDisplayed.SetText(rulesDisplayed.text + kvp2.Key + " -> " + kvp2.Value + "\n");

                    }
                }
            }
            i++;

        }
    }

    public void presetTabClicked()
    {
        hideTabChildren(customTab);
        hideTabChildren(settingsTab);
        hideTabChildren(cityTab);
        showTabChildren(presetTab);
        buildingSelector.SetValueWithoutNotify(0);
        dropDownValueChanged(0);

    }

    public void customTabClicked()
    {
        hideTabChildren(cityTab);
        hideTabChildren(settingsTab);
        hideTabChildren(presetTab);
        showTabChildren(customTab);
        rulesDisplayed.SetText("");
    }

    public void settingsTabClicked()
    {
        hideTabChildren(customTab);
        hideTabChildren(cityTab);
        hideTabChildren(presetTab);
        showTabChildren(settingsTab);

    }

    private void hideTabChildren(Button button)
    {
        for(int i = 1; i < button.transform.childCount; i++)
        {
            button.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    private void showTabChildren(Button button)
    {
        for(int i = 1; i < button.transform.childCount; i++)
        {
            button.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void cityTabClicked()
    {
        hideTabChildren(customTab);
        hideTabChildren(settingsTab);
        hideTabChildren(presetTab);
        showTabChildren(cityTab);
    }

    public void saveCharacter(string newCharacter)
    {
        character = newCharacter.ToCharArray()[0];
    }

    public void saveRule(string newRule)
    {
        rule = newRule;
    }

    public void saveName(string newName)
    {
        name = newName;
    }

    public void saveAxiom(string newAxiom)
    {
        axiom = newAxiom;
    }

    public void saveIterations(string newIterations)
    {
        iterations = int.Parse(newIterations);
    }

    public void saveAngle(string newAngle)
    {
        angle = float.Parse(newAngle);
        //newAngle = angle.ToString();
    }

    public void saveOverlapPercentage()
    {
        overlap = overlapSlider.value;
    }

    public void generateModel()
    {
        mainGO.GetComponent<LSystem>().setRules(selectedSystem);
        mainGO.GetComponent<LSystem>().setValuesAndGenerate(axiom, iterations, angle, overlap);
//        LSystem.setRules(selectedSystem);
//        LSystem.setValuesAndGenerate(axiom, iterations, angle, overlap);
        mainCanvas.enabled = false;
    }

    public void generateCity()
    {
        mainGO.GetComponent<CityGenerator>().setRules(citySelectedSystem);
        
        mainCanvas.enabled = false;
    }

    public void addProduction()
    {
        newProduction.Add(character, rule);
        rulesDisplayed.SetText(rulesDisplayed.text + character + " -> " + rule + "\n");
    }

    public void saveSystem()
    {
        newSystem.Add(name, newProduction);
        mainGO.GetComponent<PresetRules>().addCustomSystems(newSystem);
        savedList = mainGO.GetComponent<PresetRules>().getSavedSystemsList();
        createListOfOptions(savedList);
    }    

    public static void cityBlockClickEvent(Vector3 blockPosition, Quaternion blockRotation)
    {
        cityBuilderMode = true;
        startingPosition = blockPosition;
        startingRotation = blockRotation;
    }

    public void cityBlockClicked(Vector3 blockPosition, Quaternion blockRotation)
    {
        mainCanvas.enabled = true;
        mainGO.GetComponent<LSystem>().setTransformBeforeGenerating(blockPosition, blockRotation);
        presetTab.Select();
        presetTabClicked();
    }

    public void closeCanvas()
    {
        mainCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            mainCanvas.enabled = true;
            presetTab.Select();
            presetTabClicked();
        }
        if(cityBuilderMode)
        {
            cityBlockClicked(startingPosition, startingRotation);
            cityBuilderMode = false;
        }
    }
}
