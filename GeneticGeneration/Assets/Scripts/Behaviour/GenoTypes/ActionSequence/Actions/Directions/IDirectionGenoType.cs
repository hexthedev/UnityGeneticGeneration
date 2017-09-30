using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirectionGenoType: IMutatable, IRandomizable{
	IDirection phenotype(VSequenceAction p_parent);
}