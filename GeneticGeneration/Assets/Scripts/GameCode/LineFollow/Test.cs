using System;
using System.Threading.Tasks;

using UnityEngine;

public class Test {

	Test(){
		Task t = Task.Run( () => { int x = 2+2; });
	}

}
