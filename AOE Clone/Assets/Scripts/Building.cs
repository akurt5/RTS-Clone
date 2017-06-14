using UnityEngine;
using System.Collections;

public class Building : Selectable
{
    public GameObject markerPrefab;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

    }
    public virtual void Placed()
    {

    }
}
