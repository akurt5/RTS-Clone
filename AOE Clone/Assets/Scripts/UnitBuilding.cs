using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitBuilding : Building
{
    public GameObject unit0, unit1, unit2, unit3, unit4;
    public Vector3 SpawnLocation;

    GameObject BuildingUI;
    public Button Spawn0;

    bool oldSelected = false;

	public override void Start ()
    {
        SpawnLocation = transform.position;

        BuildingUI = GameObject.FindWithTag("BuildingUI");
        BuildingUI.gameObject.SetActive(Selected);

        Spawn0 = BuildingUI.GetComponent<UIWrapper>().Button0;
        Spawn0.onClick.AddListener(delegate { SpawnUnit(unit0); });

        base.Start();
	}

    public override void Update ()
    {
        if (Selected)
        {
            if ((SpawnLocation != transform.position)&&(oldSelected != Selected))
            {
                Instantiate(markerPrefab, SpawnLocation, new Quaternion());
            }
            //SpawnLocation.SetActive(true);//was doing this the same as in the start function but i already had to access the Selected variable to catch the button presses

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                // 
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SpawnUnit(unit0);
            }

            if (Input.GetMouseButton(1))
            {
                //float OGy = SpawnLocation.transform.position.y;

                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    SpawnLocation = hit.point;
                    Instantiate(markerPrefab, SpawnLocation, new Quaternion());

                    //SpawnLocation.transform.position = hit.point;
                    //SpawnLocation.transform.Translate(0, OGy, 0);
                }
            }
        }
        else
        {
            //SpawnLocation.SetActive(false);//was doing this the same as in the start function but i already had to access the Selected variable to catch the button presses
        }
        BuildingUI.gameObject.SetActive(Selected);

        oldSelected = Selected;

        base.Update();
	}
    public void SpawnUnit(GameObject _Unit)
    {
        if (PopulationManager.IncPop())
        {
            Vector2 spawnPos = (Random.insideUnitCircle.normalized * transform.localScale.x);
            GameObject newUnit = Instantiate(_Unit, new Vector3(spawnPos.x + transform.position.x, _Unit.transform.position.y, spawnPos.y + transform.position.z), _Unit.transform.rotation) as GameObject;
            newUnit.GetComponent<Unit>().Destination = SpawnLocation;
        }
    }
}
