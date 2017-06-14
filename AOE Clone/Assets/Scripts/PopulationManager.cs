using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{
    public GameObject PopulationDisplay;

    const string DefaultText = "Pop Cap: @C\nCurr Pop : @P";

    static int Population = 0;
    static int PopulationCap = 5;

    public static PopulationManager instance;

    public void Start()
    {
        if (instance != null)
        {
            Debug.Log("Hey bro, you tried to have 2 population managerinstances or something. fuck you.");
            Destroy(this);
        }
        instance = this;

        UpdateTextDisplay();
    }

    public static bool IncPop()
    {
        if (Population < PopulationCap)
        {
            Population++;
            UpdateTextDisplay();
        }
        else
        {
            return false;
        }
        return true;
    }

    public static void DecPop()
    {
        Population--;
        UpdateTextDisplay();
    }

    public static void IncPopCap()
    {
        PopulationCap++;
        UpdateTextDisplay();
    }

    public static void DecPopCap()
    {
        PopulationCap--;
        UpdateTextDisplay();
    }

    private static void UpdateTextDisplay()
    {
        string newText = DefaultText;

        newText = newText.Replace("@C", PopulationCap.ToString());
        newText = newText.Replace("@P", Population.ToString());

        if (PopulationManager.instance.PopulationDisplay != null)
        {
            PopulationManager.instance.PopulationDisplay.GetComponent<Text>().text = newText;
        }
    }

    public void Update()
    {
        PopulationManager.instance.PopulationDisplay.GetComponent<Text>().color = Random.ColorHSV(); // this weird shit is probably just for debug.
    }
}
