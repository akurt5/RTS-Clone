using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    public static GameObject BuildingToCreate; // if this is not null the building assigned will imedaiately be created and then the variable will be set to null.
    private GameObject currBuilding;
    private Vector3 SnapPos;

    void Update()
    {
        if (BuildingToCreate != null)
        {
            currBuilding = Instantiate(BuildingToCreate);
            BuildingToCreate = null;
        }
        if(currBuilding != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                SnapPos = hit.point;
                SnapPos = new Vector3((int)SnapPos.x, 0.5f, (int)SnapPos.z);

                currBuilding.transform.position = SnapPos;

                if (Input.GetMouseButtonDown(0))
                {
                    currBuilding.GetComponent<Building>().Placed();
                    currBuilding = null;
                }
            }
        }
    }
}
