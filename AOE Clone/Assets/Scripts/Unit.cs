using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class Unit : Selectable
{
    GameObject UnitUI;
    public Button Building0, Building1;

    public GameObject building0, building1, building2, building3, building4;

    public int PathingIterationCount = 10;
    public bool destinationReached = false;

    public GameObject destPrefab;

    [Range(1, 10)]
    public float movementSpeed = 1;

    public GameObject pathTestPrefab;
    //private Vector3 PathStart;
    //private Vector3 PathEnd;

    private GameObject PathBlocks;

    List<Tile> OpenTiles;
    List<Tile> ClosedTiles = new List<Tile>();
    List<Tile> Path = new List<Tile>();

    public static int[,] walkableMap;
    static int mapSize;

    public Vector3 Destination;

    //bool unitWasGenerated = false; // this bool is to determione whether a unit was spawned during the game or generated at the beginning of the game.

    //private float dt = 0; // what does this fucking thing do? does Pete Keys know?

    public void Awake()
    {
        Destination = transform.position;
    }
    public override void Start()
    {
        PopulationManager.IncPop();
        UnitUI = GameObject.FindWithTag("UnitUI");
        Debug.Log(UnitUI+" yeah");
        UnitUI.gameObject.SetActive(Selected);

        Building0 = UnitUI.GetComponent<UIWrapper>().Button0;
        Building0.onClick.AddListener(delegate { SpawnUnit(building0); });
        Building1 = UnitUI.GetComponent<UIWrapper>().Button1;
        Building1.onClick.AddListener(delegate { SpawnUnit(building1); });

        base.Start();
    }

    public override void Update()
    {
        if (Destination != transform.position)
        {
            CalcPath();
            FollowPath();
            if (new Vector3 (transform.position.x, 0, transform.position.z) == new Vector3((int)Destination.x, 0, (int)Destination.z))
            {
                Destination = transform.position;
            }
        }

        if (Selected)
        {
            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    /*if (Destination == null)//wtf does this even do?        comparing vec3 and null is always false
                    {
                        Destination = hit.point;
                    }
                    else
                    {*/
                        Destination = hit.point;
                    //}
                }
            }
        }

        UnitUI.gameObject.SetActive(Selected);

        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Unit>() != null)
        //if ((other.GetComponent<Unit>() != null)||(other.GetComponent<Building>() != null))
        {
            if (!(Mathf.Abs(Vector3.Distance(Destination, transform.position)) > (transform.localScale.x * 2f)))
            {
                Destination = transform.position;
            }
        }
    }

    public void OnDestroy()
    {
        PopulationManager.DecPop();
    }

    public static void InitializeWalkableMap(int p_Size)
    {
        mapSize = p_Size;
        walkableMap = new int[mapSize, mapSize];
    }

    void CalcPath()
    {
        if (Path.Count == 0)//
        {
            destinationReached = false;
        }
        if ((!destinationReached))//only carry on searching for a path if you havent already worked out how to get to your destination
        {
            OpenTiles = new List<Tile>();
            Path = new List<Tile>();

            Tile currentTile = GetMyMapTile();
            //PathStart = new Vector3(currentTile.xPos, 2, currentTile.zPos);

            Vector2 destTilePos = new Vector2((int)Destination.x, (int)Destination.z);
            //PathEnd = new Vector3(destTilePos.x, 2, destTilePos.y);

            bool Loop = true;
            int loopCount = PathingIterationCount;

            while (Loop)
            //while (currentTile.pos != destTilePos)
            //for(int i = 0; i < 1000;++i)
            {

                Tile[] Adjacent = GetAdjacentTiles(currentTile, true);
                foreach (Tile t in Adjacent)
                {
                    if (t != null)
                    {
                        bool match = false;
                        foreach (Tile oT in OpenTiles)
                        {
                            if (oT.pos == t.pos)
                            {
                                match = true;
                            }
                        }
                        foreach (Tile cT in ClosedTiles)
                        {
                            if (cT.pos == t.pos)
                            {
                                match = true;
                            }
                        }

                        if (!match)
                        {
                            //Add a check to see if tile is occupied then don't move there
                            t.SetParent(currentTile);
                            OpenTiles.Add(t);
                        }
                    }
                }

                ClosedTiles.Add(currentTile);
                OpenTiles.Remove(currentTile);
                if (OpenTiles.Count > 1)
                {
                    OpenTiles.Sort(delegate (Tile t1, Tile t2) { return t1.score.CompareTo(t2.score); });
                }

                if (OpenTiles.Count == 0)
                {
                    break;
                }

                currentTile = OpenTiles[0];//grabs first in list because we want the lowest score
                loopCount -= 1;

                if ((loopCount == 0) || (currentTile.pos == destTilePos))
                {
                    Loop = false;

                    if (currentTile.pos == destTilePos)
                    {
                        destinationReached = true;
                    }
                }
            }

            if (destinationReached)
            {
                ClosedTiles.Clear();
                OpenTiles.Clear();
            }


            do
            {
                Path.Insert(0, currentTile);

                if (!destinationReached)
                {
                    if (ClosedTiles.Remove(currentTile))
                    {
                        OpenTiles.Add(currentTile);
                    }
                }
                if (currentTile.parent != null)
                {
                    currentTile = currentTile.parent;
                }
            }
            while (currentTile.parent != null);
        }
    }
    void FollowPath()
    {
        if (Path.Count > 0)
        {
            Destroy(Instantiate(pathTestPrefab, transform.position, transform.rotation), 0.4f);
            transform.Translate(new Vector3(Path[0].pos.x, transform.position.y, Path[0].pos.y) - transform.position);
            if (transform.position == new Vector3(Path[0].pos.x, transform.position.y, Path[0].pos.y))
            {
                Path.RemoveAt(0);
            }
        }
    }
    Vector2 GetMyMapPos()
    {
        return new Vector2((int)transform.position.x, (int)transform.position.z);
    }
    Tile GetMyMapTile()
    {
        Vector2 MapPos = GetMyMapPos();
        return Tile.CreateTile(MapPos, walkableMap[(int)MapPos.x, (int)MapPos.y]);
    }
    Tile[] GetAdjacentTiles(Tile p_CurrentTile, bool p_Diagonal = false)
    {
        Tile t = p_CurrentTile;
        Tile[] AdjacentTiles;
        int tilesToCheck = 0;
        if (p_Diagonal)
        {
            tilesToCheck = 8;
            AdjacentTiles = new Tile[tilesToCheck];
            AdjacentTiles[0] = GetTile(t.xPos + 1, t.zPos);
            AdjacentTiles[1] = GetTile(t.xPos + 1, t.zPos + 1);
            AdjacentTiles[2] = GetTile(t.xPos, t.zPos + 1);
            AdjacentTiles[3] = GetTile(t.xPos - 1, t.zPos + 1);
            AdjacentTiles[4] = GetTile(t.xPos - 1, t.zPos);
            AdjacentTiles[5] = GetTile(t.xPos - 1, t.zPos - 1);
            AdjacentTiles[6] = GetTile(t.xPos, t.zPos - 1);
            AdjacentTiles[7] = GetTile(t.xPos + 1, t.zPos - 1);
        }
        else
        {
            tilesToCheck = 4;
            AdjacentTiles = new Tile[tilesToCheck];
            AdjacentTiles[0] = GetTile(t.xPos + 1, t.zPos);
            AdjacentTiles[1] = GetTile(t.xPos, t.zPos + 1);
            AdjacentTiles[2] = GetTile(t.xPos - 1, t.zPos);
            AdjacentTiles[3] = GetTile(t.xPos, t.zPos - 1);
        }
        return AdjacentTiles;
    }
    Tile GetTile(Vector2 p_Pos)
    {
        return GetTile(p_Pos.x, p_Pos.y);
    }
    Tile GetTile(float p_X, float p_Z)
    {
        return GetTile((int)p_X, (int)p_Z, false);
    }

    Tile GetTile(int p_X, int p_Z, bool p_Diagonal)
    {

        if (((p_X < mapSize) && (p_Z < mapSize)) && ((p_X >= 0) && (p_Z >= 0)))
        {
            int terrVal;
            if ((terrVal = walkableMap[p_X, p_Z]) > 0)
            {
                float score = 10 + ((2 - terrVal) * 2);
                if (p_Diagonal) { score += 4; }
                score += Vector2.Distance(new Vector2(p_X, p_Z), new Vector2(Destination.x, Destination.z));
                return Tile.CreateTile(p_X, p_Z, score);
            }
        }
        return null;
    }

    public void SpawnUnit(GameObject _Building)
    {
        PlaceBuilding.BuildingToCreate = _Building;
    }

}