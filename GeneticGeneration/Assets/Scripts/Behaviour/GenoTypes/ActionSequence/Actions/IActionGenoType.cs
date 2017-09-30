using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IActionGenoType: IMutatable, IRandomizable{
	IAction phenotype(ActionSequence p_parent);
}