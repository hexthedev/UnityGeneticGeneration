using System.Collections;
using System.Collections.Generic;
using JTools.Calc.Base;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class HFlineFollowCONFIG {

	//General Creature
	public static float CreatureTimeAlive { get { return 3f;} }
	public static float CreatureDecisionLatency { get { return 0.05f;} }
	public static float CreatureFitnessLatency { get { return 0.05f;} }

	//General Controller
	public static float ControllerSpawnLatency { get { return 0.25f;} }
	public static int ControllerCreatureSpawnAmount { get { return 1;} }


	//Traits
	public static int TraitGeneSize { get { return 5;} }
	public static Range<float> TraitRange { get { return new Range<float>(0.25f, 1);} }
	public static Range<float> TraitMutationAddition { get { return new Range<float>(-0.5f, 0.5f);} }

	//Behaviour
	public static Range<float> BehaviourMutationMultiplier { get { return new Range<float>(0.8f, 1.2f);} }

	//General
	public static float GenMutationChance { get { return 0.05f;} }
	public static int GenGenepoolSize { get { return 25;} }
	public static float GenDNAAgeingMultiplier{ get { return 0.90f;} }
	public static float GenDNAAgeingLatency { get { return 1f;} }
	
	


	//Setup
	public static void loadScene(){

		switch(AIkey){
			case "DecisionNet":
				SceneManager.LoadScene( "HFLineFollowDNScene" );
				break;
			case "DecisionTree":
				SceneManager.LoadScene( "HFLineFollowDTScene" );
			break;
			case "NeuralNet":
				SceneManager.LoadScene( "HFLineFollowNNScene" );
			break;
		}
	}

	private static string[] AIkeys = { "DecisionNet", "DecisionTree", "NeuralNet" };

	public static void nextAiKey(){
		int index = 0;

		for(;index<AIkeys.Length; index++){
			if(AIkeys[index] == AIkey){
				break;
			}
		}

		AIkey = (index+1 == AIkeys.Length) ? AIkeys[0] : AIkeys[index+1];
		Debug.Log(AIkey);
	}

	public static string AIkey = "BehaviourTree";

	public static string DataSuffix = "HFTest";

	public static int timePerTrial = 3600;

}
