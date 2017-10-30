using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Calc;
using Calc.Rand;

namespace Genetic{

	namespace Base {

		///<summary>Type T refers to </summary>
		public class DNABasedEvolutionController<T1, T2> {

			private ISpecies<IDNA<T1, T2>> m_species;
			private FitnessList<IDNA<T1, T2>> m_gene_pool;
			private float m_mutation_chance_percentage;
			private int m_creatures_birthed = 0;

			///<summary>Pass in a species and a number between 0 and 100 as mutation chance</summary>
			public DNABasedEvolutionController(ISpecies<IDNA<T1, T2>> p_species, float p_mutation_chance_percentage){
				m_gene_pool = new FitnessList<IDNA<T1, T2>>(25);
				m_mutation_chance_percentage = p_mutation_chance_percentage;
			}

			///<summary>DNA will be added by fitness and sorted</summary>
			public void addDNA(IDNA<T1, T2> p_dna, float fitness){
				m_gene_pool.add(fitness, p_dna);
			} 

			///<summary>Return a piece of DNA by doing crossover and mutation on random DNA in gene pool weighted by fitness</summary>
			public IDNA<T1, T2> birth(){

				IDNA<T1, T2> dna = m_gene_pool.getRandomObject().crossover(m_gene_pool.getRandomObject().getSelf() );
				
				if(RandomCalc.ChanceRoll(m_mutation_chance_percentage)){
					dna = dna.mutate();
				}

				m_creatures_birthed++;
				return dna;
			}

			public int CreaturesBirthed { get { return m_creatures_birthed; } } 

			public override string ToString(){
				return m_gene_pool.ToString();
			}

		}


		/* @@@@@@@@@@@@@@@@@
				DNA
			@@@@@@@@@@@@@@@@@ */

		///<summary>DNA must be able to do crossover and mutate itself</summary>
		public interface IDNA<T1, T2> : ICrossoverable<T1, T2>, IMutatable<T1, T2>, IBirthable<T1, T2>, ISelf<T1>{ }

		///<summary>Return object as it's specific type.!-- Useful for cing objects own type maps to a generic type</summary>
		public interface ISelf<T>{
			///<summary>Should always implement as return this;</summary>
			T getSelf();
		}

		///<summary>Object can perform crossover and return IDNA</summary>
		public interface ICrossoverable<T1, T2>{

			///<summary>Crossover with another object of same type returns object type</summary>
			IDNA<T1, T2> crossover(T1 p_object);
		}

		///<summary>Object can mutate and return IDNA</summary>
		public interface IMutatable<T1, T2>{

			///<summary>This object can mutate</summary>
			IDNA<T1,T2> mutate();
		}

		///<summary>Object can take in T1 and bith T2</summary>
		public interface IBirthable<T1, T2>{
			T2 birth(T1 p_birth_object);
		}

    /* @@@@@@@@@@@@@@@@@
        Species
       @@@@@@@@@@@@@@@@@ */

    ///<summary>Structure of a genetic creature. Some implementations of DNA require a strict structure</summary>
    public interface ISpecies<T>{

		int ID { get;}

		///<summary>A Random instance of a spceies is DNA whos structure adhers to the species structure</summary>
		T randomInstance();
	}


/* @@@@@@@@@@@@@@@@@
		FITNESS LIST
 	 @@@@@@@@@@@@@@@@@ */

		///<summary>Data Structure holds fitness and objects in an order list</summary>
		public class FitnessList<T>
		{
			List<FitnessObject<T>> m_objects;

			int m_size;

			public FitnessList(int p_size)
			{
				m_objects = new List<FitnessObject<T>>();
				m_size = p_size;
			}

			public void add(float p_fitness, T p_object){
				m_objects.Add(new FitnessObject<T>(p_fitness, p_object));
				m_objects.Sort(new FitnessComparer<T>());

				while(m_objects.Count > m_size){
					m_objects.Remove(m_objects[m_objects.Count-1]);
				}
			}

			///<summary>Returns a random object with fitter objects more likely to be chosen.</summary>
			public T getRandomObject(){
				float indexNum = Mathf.Pow(RandomCalc.Rand(new Range<float>(0f, 1f)), 2);
				int index = (int)Mathf.Round(indexNum*(m_objects.Count-1));

				return m_objects[index].Object;
			}

			///<summary>Change the fitness of all entries using fitness mod function</summary>
			public void modifyFitness(DFitnessMod p_mod){
				foreach(FitnessObject<T> fitobj in m_objects){
					fitobj.modifyFitness(p_mod);
				}
			}

			public override string ToString(){
				string ret = "[";

				foreach(FitnessObject<T> x in m_objects){
					ret += x.Fitness + ",";
				}

				ret += "]";

				return ret;
			}
			

		}


		///<summary>Holds and object and its fitness</summary>
		public class FitnessObject<T> {

			private float m_fitness;
			private T m_object;

			public FitnessObject(float p_fitness, T p_object)
			{
				m_fitness = p_fitness;
				m_object = p_object;	
			}

			public float Fitness { get { return m_fitness; } }

			public T Object { get { return m_object; } }

			///<summary>Change this objects fitness with modification function</summary>
			public void modifyFitness(DFitnessMod p_mod){
				m_fitness = p_mod(m_fitness);
			}

		}

		///<summary>Generic FitnessObject Comparer</summary>
		public class FitnessComparer<T> : IComparer<FitnessObject<T>>
		{
			public int Compare(FitnessObject<T> x, FitnessObject<T> y)
			{
				int to_return = 0;

				if(x.Fitness < y.Fitness){
					to_return = 1;
				} else if(x.Fitness > y.Fitness) {
					to_return = -1;
				}

				return to_return;
			}
		}

		///<summary>Used to modify the fitness of a fitness object. Basically a mapping from float to float</summary>
		public delegate float DFitnessMod(float p_mod);

	}

}

