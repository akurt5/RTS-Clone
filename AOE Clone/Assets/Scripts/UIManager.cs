using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{

    public enum UIState
    {
        NO_BUILDING,
        TOWN_CENTRE,
        NULL,
    }

    public static UIState CurrentUIstate
    {
       get { return CurrentUIstate; }
       private set { CurrentUIstate = value; }
    }
        
    void Start()
    {
        ChangeUIState(UIState.NULL);
    }

    void ChangeUIState(UIState p_NewState)
    {
        CurrentUIstate = p_NewState;
    }
}
