﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building 
{
	public override void Placed () 
	{
        PopulationManager.IncPopCap();
	}
}
