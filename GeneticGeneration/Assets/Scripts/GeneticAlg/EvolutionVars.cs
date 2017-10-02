using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EvolutionVars {

	//CHROMOS

	//Number of times genes mutate per mutation
	public static int chromo_mutation_iterations = 5;
	//Genes per chromo
	public static int chromo_genes = 5;
	//Numbers random genes in chromo can take on construction
	public static float chromo_start_value() { return Random.Range(0.2f, 2f); }
	//Number that is added to a gene during mutation
	public static float chromo_mutation_value() { return Random.Range(-1f, 2f); }



	//DIRECTIONS
	//multiplier for mutating directions
	public static float direction_mutation_multip() { return Random.Range(-1.5f, 1.5f); }


	//ACTIONS
	//Mutation multiplier for time limits
	public static float action_time_limit_rand_mult() { return Random.Range(0.5f, 1.5f); }
	//Random value when initizalizing time limits
	public static float action_time_limit_rand_value() { return Random.Range(0f, 1f); }

	//Mutation multiplier for speed changes
	public static float action_speed_rand_mult() { return Random.Range(0.5f, 1.5f); }		
	//Random value when initizalizing speed
	public static float action_speed_rand_value() { return Random.Range(0.2f, 1f); }		


	//DETECTORS
	public static float det_angle_rand_mult() { return Random.Range(0.5f, 1.5f); }	
	public static float det_count_rand_mult() { return Random.Range(0.5f, 1.5f); }	
	public static int det_count_rand_val() { return Random.Range(1, 3); }	
	public static float det_trait_threshold_rand_mult() { return Random.Range(0.5f, 1.5f); }	
	public static int det_trait_threshold_rand_val() { return Random.Range(0, 10); }	
	public static float det_prox_threshold_rand_mult() { return Random.Range(0.5f, 1.5f); }	
	public static float det_prox_threshold_rand_val() { return Random.Range(0f, 5f); }	

}
