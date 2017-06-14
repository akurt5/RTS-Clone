using UnityEngine;
using System.Collections.Generic;

public class CameraFollowUnit : MonoBehaviour
{
    [SerializeField]
    public List<Transform> lookAt = new List<Transform>();
    private Vector3 lastPos = new Vector3();
    private Vector3 keyPosBuffer;

    public bool updateCamPos = false;

    //GSideH = 100 = flatDist
    //GSideA = 70 = flatDist * IsoscelesRatio
    //GSideB = GSideA

    //VSideH = flatDist / sin(VAngle2)
    //VSideA = VSideH * cos(VAngle2)
    //VSideB = flatDist

    //VAngle1 = 90
    //VAngle2 = 30
    //VAngle3 = 60

    [Range(40, 250)]
    public float flatDist = 40;
    //private float oldFlatDist = 40;
    float IsoscelesRatio = 0.7071f;
    float CamHeightRatio = 0.5773503f;

    [Range(1, 50)]
    public float keyMoveSpeedX = 10;
    [Range(1, 50)]
    public float keyMoveSpeedY = 14;

    [Range(1, 10)]
    public float zoomMultiplier;

    private int focusUnitIterator = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (lookAt.Count > 0))
        {
            if (focusUnitIterator > lookAt.Count - 1) { focusUnitIterator = 0; }
            transform.parent.position = lookAt[focusUnitIterator].position;
            focusUnitIterator++;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            transform.parent.eulerAngles = new Vector3(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y + 90, transform.parent.eulerAngles.z);
        }

            CheckKeyMovement();
        UpdateCamZoom();
        //oldFlatDist = flatDist;
    }

    public Vector3 CalculateAveragePos()
    {
        if (lookAt.Count > 0)
        {
            Vector3 AvgPos = new Vector3();
            foreach (Transform Unit in lookAt)
            {
                AvgPos += Unit.position;
            }
            return AvgPos /= lookAt.Count;
        }
        if (lastPos != Vector3.zero)
        {
            lastPos = transform.parent.position;
        } 
        return lastPos;
    }
    
    void UpdateCameraPos()
    {
        Vector3 avgPos = CalculateAveragePos();

        Vector3 newPos = new Vector3(avgPos.x + (flatDist * IsoscelesRatio), avgPos.y + (flatDist * CamHeightRatio), avgPos.z + (flatDist * IsoscelesRatio));

        if (lastPos != newPos)
        {
            lastPos = newPos;
        }
        transform.parent.position = newPos;
    }

    void UpdateCamZoom()
    {
        flatDist -= Input.GetAxis("Mouse ScrollWheel") * zoomMultiplier;

        if (flatDist < 40) { flatDist = 40; }
        if (flatDist > 250) { flatDist = 250; }

        GetComponent<Camera>().orthographicSize = flatDist * 0.5f;
    }

    void CheckKeyMovement()
    {
        if (Input.anyKey)
        {
            keyPosBuffer = new Vector2();

            if (Input.GetKey(KeyCode.A)) { keyPosBuffer -= (keyMoveSpeedX * Time.deltaTime) * Camera.main.transform.parent.right; }
            if (Input.GetKey(KeyCode.D)) { keyPosBuffer += (keyMoveSpeedX * Time.deltaTime) * Camera.main.transform.parent.right; }
            if (Input.GetKey(KeyCode.W)) { keyPosBuffer += (keyMoveSpeedY * Time.deltaTime) * Camera.main.transform.parent.forward; }
            if (Input.GetKey(KeyCode.S)) { keyPosBuffer -= (keyMoveSpeedY * Time.deltaTime) * Camera.main.transform.parent.forward; }

            transform.parent.position += keyPosBuffer;
        }
    }
}
