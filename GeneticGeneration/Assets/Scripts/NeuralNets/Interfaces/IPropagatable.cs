using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPropagatable {

	Matrix propagate();
	int count();

}
