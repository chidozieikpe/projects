using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {
    //the map game object
    public Transform map;
    //the map generator
    Generator mapGenerator;
    //list of names of the rules
    List<string> rules = new List<string>();

    public Dropdown dropdown;
    public Text selectedRule;
    public Text instructions;
    public Text sliderDisplay;

    private void Start()
    {
        if(map != null)
        {
            mapGenerator = map.GetComponent<Generator>();
            rules = mapGenerator.GetRuleNames();
        }
        FillList();
        UpdateRuleDisplay(0);
    }

    private void FixedUpdate()
    {
        DisplaySliderPercent();
    }
    //show or hide instructions
    public void DisplayInstructions(bool visible)
    {
        instructions.enabled = visible;
    }
    //show the float value of the slider
    public void DisplaySliderPercent()
    {
        sliderDisplay.text = mapGenerator.GetFillPercent().ToString() + "%";
    }
    //update the text that displays the current rule
    public void UpdateRuleDisplay(int index)
    {
        if(selectedRule != null)
        {
            selectedRule.text = rules[index];
        }

        if (mapGenerator != null)
        {
            mapGenerator.ChangeRule(rules[index]);
        }
    }
    //changes the fill percent variable using the slider
    public void UpdateSliderValue(float sliderVal)
    {
        mapGenerator.ChangeFillPercent(sliderVal);
    }
    //populate the dropdown list
    void FillList()
    {
        dropdown.AddOptions(rules);
    }
    
}
