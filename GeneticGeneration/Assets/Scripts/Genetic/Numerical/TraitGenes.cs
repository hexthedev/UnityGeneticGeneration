using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Base;
using JTools.Calc.Rand;
using JTools.Calc.Array;
using JTools.Calc.Bool;
using JTools.Calc.DataStructures;

using JTools.Interfaces;

using Genetic.Base;
using Genetic.Numerical.Base;

namespace Genetic
{
  namespace Numerical
  {
    namespace TraitGenes
    {
      public class TraitGenesDNA :  ADNA<TraitGenesDNA>, IExpressable<Dictionary<ETrait, float>>, ICloneable<TraitGenesDNA>
      {
        int m_species;
        private Dictionary<ETrait, Gene> m_traits = new Dictionary<ETrait, Gene>();

        ///<summary> Create a Random selection of trait genes</summary>
        public TraitGenesDNA(int p_species, HashSet<ETrait> p_traits, int p_size, Range<float> p_range, int p_mutation_iterations, Range<float> p_mutation_range)
        {
          m_species = p_species;
          foreach (ETrait trait in p_traits)
          {
            m_traits.Add(trait, new Gene(p_size, p_range, p_mutation_iterations, p_mutation_range));
          }

          Debug.Log(ToString());
        }

        ///<summary> Manually create TraitGenesDNA. FOR TESTING ONLY -- inputs is mutable</summary>
        public TraitGenesDNA(int p_species, Dictionary<ETrait, Gene> p_chromos)
        {
          m_traits = new Dictionary<ETrait, Gene>();

          foreach(ETrait trait in p_chromos.Keys){
            m_traits.Add(trait, p_chromos[trait].Clone());
          }
        }

        ///<summary> Mutate each traits genes</summary>
        public override TraitGenesDNA mutate()
        {
          

          TraitGenesDNA mutated = new TraitGenesDNA(m_species, m_traits);

          foreach(ETrait trait in m_traits.Keys){
            Gene mut_gene = mutated.m_traits[trait].mutate();
            mutated.m_traits.Remove(trait);
            mutated.m_traits.Add(trait, mut_gene);
          }

          
          
          return mutated;
        }

        ///<summary> Create new TraitGenesDNA by performing crossover on each Traits genes</summary>
        public override TraitGenesDNA crossover(TraitGenesDNA p_crossover_object)
        {
          Debug.Log("Crossing=" + m_traits[0]);
          Debug.Log("Crossing=" + p_crossover_object.m_traits[0]);

          TraitGenesDNA crossovered = new TraitGenesDNA(m_species, m_traits);
          crossovered.m_traits = new Dictionary<ETrait, Gene>();

          foreach(ETrait trait in m_traits.Keys){
            crossovered.m_traits.Add(trait, m_traits[trait].crossover(p_crossover_object.m_traits[trait]));
          }

          Debug.Log("Crossed=" + crossovered.m_traits[0]);

          return crossovered;
        }

        ///<summary> Translate DNA into Dictionary of ETraits to floats</summary>
        public Dictionary<ETrait, float> express()
        {
          Dictionary<ETrait, float> born = new Dictionary<ETrait, float>();

          foreach(ETrait trait in m_traits.Keys){
            born.Add(trait, m_traits[trait].GeneValue);
          }

          return born;
        }

        public TraitGenesDNA Clone()
        {
          return new TraitGenesDNA(m_species, m_traits);
        }

        public override string ToString()
        {
          string to_return = "TraitDNA: [";
          
          foreach(ETrait trait in m_traits.Keys){
            to_return += trait.ToString() + ":" + m_traits[trait].GeneValue + ",";
          }

          to_return.Remove(to_return.Length-1);

          return to_return + "]";
        }

        //IDNA FUNCTIONS DESIGNED FOR EVOLUTION CONTROLLER GENERALIZATION
        public override TraitGenesDNA getSelf()
        {
          return this;
        }
      }

      public class TraitGenesSpecies : ISpecies<ADNA<TraitGenesDNA>>
      {
        int m_id;
        HashSet<ETrait> m_traits;
        int m_size;
        Range<float> m_range;        
        int m_mutation_iterations;
        Range<float> m_mutation_range;

        public TraitGenesSpecies(int p_id, HashSet<ETrait> p_traits, int p_size, Range<float> p_range, int p_mutation_iterations, Range<float> p_mutation_range){
          m_id = p_id;
          m_traits = HashSetCalc.ShallowClone(p_traits);
          m_size = p_size;
          m_range = p_range.Clone();
          m_mutation_iterations = p_mutation_iterations;
          m_mutation_range = p_mutation_range.Clone();
        }
        
        public int ID { get {return m_id;} }

        public ADNA<TraitGenesDNA> randomInstance()
        {
          return new TraitGenesDNA(m_id, m_traits, m_size, m_range, m_mutation_iterations, m_mutation_range);
        }
      }
    }
  }
}

