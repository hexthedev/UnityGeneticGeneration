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
      public class TraitGenesDNA : IMutatable<TraitGenesDNA>, ICrossoverable<TraitGenesDNA>, IBirthable<Dictionary<ETrait, float>>, IDNA<TraitGenesDNA>, ICloneable<TraitGenesDNA>
      {
        private Dictionary<ETrait, Gene> m_traits = new Dictionary<ETrait, Gene>();

        ///<summary> Create a Random selection of trait genes</summary>
        public TraitGenesDNA(HashSet<ETrait> p_traits, int p_size, Range<float> p_range, int p_mutation_iterations, Range<float> p_mutation_range)
        {
          foreach (ETrait trait in p_traits)
          {
            m_traits.Add(trait, new Gene(p_size, p_range, p_mutation_iterations, p_mutation_range));
          }

          Debug.Log(ToString());
        }

        ///<summary> Manually create TraitGenesDNA. FOR TESTING ONLY -- inputs is mutable</summary>
        public TraitGenesDNA(Dictionary<ETrait, Gene> p_chromos)
        {
          m_traits = new Dictionary<ETrait, Gene>();

          foreach(ETrait trait in p_chromos.Keys){
            m_traits.Add(trait, p_chromos[trait].Clone());
          }
        }

        ///<summary> Mutate each traits genes</summary>
        public TraitGenesDNA mutate()
        {
          TraitGenesDNA mutated = new TraitGenesDNA(m_traits);

          foreach(ETrait trait in mutated.m_traits.Keys){
            mutated.m_traits[trait] = mutated.m_traits[trait].mutate();
          }

          return mutated;
        }

        ///<summary> Create new TraitGenesDNA by performing crossover on each Traits genes</summary>
        public TraitGenesDNA crossover(TraitGenesDNA p_crossover_object)
        {
          TraitGenesDNA crossovered = new TraitGenesDNA(m_traits);
          crossovered.m_traits = new Dictionary<ETrait, Gene>();

          foreach(ETrait trait in crossovered.m_traits.Keys){
            crossovered.m_traits.Add(trait, m_traits[trait].crossover(p_crossover_object.m_traits[trait]));
          }

          return crossovered;
        }

        ///<summary> Translate DNA into Dictionary of ETraits to floats</summary>
        public Dictionary<ETrait, float> birth()
        {
          Dictionary<ETrait, float> born = new Dictionary<ETrait, float>();

          foreach(ETrait trait in m_traits.Keys){
            born.Add(trait, m_traits[trait].GeneValue);
          }

          return born;
        }

        public TraitGenesDNA Clone()
        {
          return new TraitGenesDNA(m_traits);
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
        public IDNA<TraitGenesDNA> DNAcrossover(TraitGenesDNA p_object)
        {
          return crossover(p_object);
        }

        public IDNA<TraitGenesDNA> DNAmutate()
        {
          return mutate();
        }

        public TraitGenesDNA getSelf()
        {
          return this;
        }
      }

      public class TraitGenesSpecies : ISpecies<IDNA<TraitGenesDNA>>
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

        public IDNA<TraitGenesDNA> randomInstance()
        {
          return new TraitGenesDNA(m_traits, m_size, m_range, m_mutation_iterations, m_mutation_range);
        }
      }
    }
  }
}

