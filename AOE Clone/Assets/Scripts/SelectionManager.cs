using UnityEngine;
using System.Collections;

public class SelectionManager : MonoBehaviour 
{
    public GameObject SelectionAreaPrefab;
    private GameObject SelectionAreaInstance;

    private Vector3 startPos;

	void Update () 
	{
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickPos = Input.mousePosition;
            clickPos = Camera.main.ScreenToWorldPoint(clickPos);
            //clickPos.y = 0.5f;
            startPos = clickPos;

            SelectionAreaInstance = Instantiate(SelectionAreaPrefab, clickPos, new Quaternion()) as GameObject;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 currPos = Input.mousePosition;
            currPos = Camera.main.ScreenToWorldPoint(currPos);
            SelectionAreaInstance.transform.localScale = currPos - startPos;
            //currPos.y = 0.5f;
            /*if (SelectionAreaInstance.transform.localScale.x < 1)
            {
                SelectionAreaInstance.transform.localScale = Vector3.one;
            }*/

            SelectionAreaInstance.transform.position = new Vector3(/*startPos.x -*/ (SelectionAreaInstance.transform.localScale.x * 0.5f), 0.5f,/* startPos.z - */(SelectionAreaInstance.transform.localScale.z * 0.5f));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Destroy(SelectionAreaInstance.gameObject);
        }
        
	}
}
