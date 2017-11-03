using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;

public class Thing : Controller {
  protected override void act()
  {
    Debug.Log("Happens");
  }

  // Use this for initialization
  protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		base.FixedUpdate();
	}
}
