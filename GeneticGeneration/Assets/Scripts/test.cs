using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test {

	public string[] t = new string[] {"a","s","d"};
	public Stack<string> t2;

	public test(){
		t2 = new Stack<string>(t);
	}


	public void tester(GameObject x){
		Debug.Log(t.Length);
		Debug.Log(t2.Pop());
		Debug.Log(t.Length);
	}

}
