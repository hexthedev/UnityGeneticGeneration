using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Base;
using JTools.Calc.Rand;

using JTools.Interfaces;

namespace Genetic{

	namespace Base {

		///<summary>Type T refers to the Concrete DNA object</summary>
		public class DNABasedEvolutionManager<T> where T:ADNA<T>{

			private ISpecies<ADNA<T>> m_species;
			public FitnessList<ADNA<T>> m_gene_pool;
			private float m_mutation_chance_percentage;
			private int m_creatures_birthed = 0;

			///<summary>Pass in a species and a number between 0 and 1 as mutation chance</summary>
			public DNABasedEvolutionManager(ISpecies<ADNA<T>> p_species, float p_mutation_chance_percentage, int p_size){
				m_gene_pool = new FitnessList<ADNA<T>>(p_size);
				m_mutation_chance_percentage = p_mutation_chance_percentage;
				m_species = p_species;
			}

			///<summary>DNA will be added by fitness and sorted</summary>
			public void addDNA(ADNA<T> p_dna, float fitness){
				m_gene_pool.add(fitness, p_dna);
				Debug.Log(m_gene_pool);
			} 

			public void addRandom(){
				m_gene_pool.add(0, m_species.randomInstance());
			}

			///<summary>Return a piece of DNA by doing crossover and mutation on random DNA in gene pool weighted by fitness</summary>
			public T birth(){

				ADNA<T> dna = m_gene_pool.getRandomObject().DNAcrossover(m_gene_pool.getRandomObject().getSelf() );
				
				if(RandomCalc.ChanceRoll(m_mutation_chance_percentage*100)){
					dna = dna.DNAmutate();
				}

				m_creatures_birthed++;
				return dna.getSelf();
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
		public abstract class ADNA<T> : ISelf<T>, ICrossoverable<T>, IMutatable<T> where T:ADNA<T>{

			///<summary>Translate IDNA back to T for concrete functions</summary>
      public abstract T getSelf();

			///<summary>Implmentation of crossover on concrete level</summary>
			public abstract T crossover(T p_crossover_object);

			///<summary>Implmentation of mutation on the concrete level</summary>
      public abstract T mutate();

      ///<summary>Crossover with another object of same type returns object type</summary>
      public ADNA<T> DNAcrossover(T p_object){
				return crossover(p_object);
			}

			///<summary>This object can mutate</summary>
			public ADNA<T> DNAmutate(){
				return mutate();
			}
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
			private List<FitnessObject<T>> m_objects;

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

