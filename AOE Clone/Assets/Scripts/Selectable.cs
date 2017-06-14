using UnityEngine;

public class Selectable : MonoBehaviour
{
    public enum Tier
    {
        ZERO, ONE, TWO, THREE,
    }

    //public static bool SelectedUnit; // this bool will be set to true when the last selected thing was a unit, this will deselect any buildings. it will be false when the last selected thing was a building, this will deselect any units and also any other buildings.

    public bool Selected = false;
    private bool OldSelected = false;

    [Header("Health Bar")]
    public int MaxHP = 100;
    [Range(0, 100)]
    public int CurrentHP = 100;
    [Range(0.01f, 2f)]
    public float HealthBarScaleX = 0.2f;
    [Range(0.01f, 2f)]
    public float HealthBarScaleY = 0.2f;
    private Vector2 HealthBarScaleMultiplier;

    [Header("Pulse Ring")]
    public float PulseIterator = 0;
    public bool PulseUp = true;
    [Range(0, 2)]
    public float PulseSpeedMultiplier;

    [Header("GameObject Links")]
    //public GameObject HealthBar;
    //public GameObject HealthBarBorder;
    //public GameObject Ring;
    public GameObject SelectionVisualsPrefab;
    public static GameObject SelVisPre;
    SelectionVisuals SelectVisuals;

    public virtual void Start ()
    {
        OldSelected = Selected;
        if (SelectionVisualsPrefab)
        {
            SelVisPre = SelectionVisualsPrefab;
        }

        SelectVisuals = Instantiate(SelVisPre, transform).GetComponent<SelectionVisuals>();
        SelectVisuals.gameObject.SetActive(false);
        //HealthBar.gameObject.SetActive(false);
        //HealthBarBorder.gameObject.SetActive(false);
        //Ring.gameObject.SetActive(false);        
    }

    public virtual void Update()
    {
        HealthBarScaleMultiplier = new Vector2(HealthBarScaleX, HealthBarScaleY);

        if (Selected)
        {
            RingPulse();

            SelectVisuals.HealthBar.transform.localScale = new Vector3(((float)CurrentHP / (float)MaxHP) * HealthBarScaleMultiplier.x, HealthBarScaleMultiplier.y, 1f);
            SelectVisuals.HealthBorder.transform.localScale = new Vector3(SelectVisuals.HealthBorder.transform.localScale.x, SelectVisuals.HealthBar.transform.localScale.y);
            ColourFill();

            if (!OldSelected)
            {
                /*HealthBar.gameObject.SetActive(true);
                HealthBarBorder.gameObject.SetActive(true);
                Ring.gameObject.SetActive(true);*/
                SelectVisuals.gameObject.SetActive(true);

            }
        }
        else if ((OldSelected) && (!Selected))
        {
            /*HealthBar.gameObject.SetActive(false);
            HealthBarBorder.gameObject.SetActive(false);
            Ring.gameObject.SetActive(false);*/
            SelectVisuals.gameObject.SetActive(false);

        }

        OldSelected = Selected;
	}

    void RingPulse()
    {
        if (PulseUp)
        {
            if(PulseIterator < CurrentHP)
            {
                PulseIterator += 1 * PulseSpeedMultiplier;
            }
            else
            {
                PulseUp = false;
            }
        }
        else
        {
            if (PulseIterator == 0)
            {
                PulseUp = true;
            }
            else
            {
                PulseIterator -= 1 * PulseSpeedMultiplier;
            }
        }

        if(PulseIterator < 0)
        {
            PulseIterator = 1;
        }
        SelectVisuals.PulseRing.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.green, PulseIterator / CurrentHP);
    }
    void ColourFill()
    {
        Color Col = SelectVisuals.HealthBar.GetComponent<SpriteRenderer>().color;

        Col = Color.Lerp(Color.red, Color.green, ((float)CurrentHP / (float)MaxHP));

        if (Col.r > 0.8)
        {
            Col.g = 0.25f;
        }
        else if (Col.r > 0.4)
        {
            Col.g = Col.r;
        }
        SelectVisuals.HealthBar.GetComponent<SpriteRenderer>().color = Col;
    }

    private void OnMouseDown()
    {
        Selected = !Selected;

        if (Selected)
        {
            Camera.main.GetComponent<CameraFollowUnit>().lookAt.Add(transform);
        }
        else
        {
            Camera.main.GetComponent<CameraFollowUnit>().lookAt.Remove(transform);
        }
    }
}
