using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;

using JTools.Calc;
using JTools.Calc.Rand;
using JTools.Calc.Array;
using JTools.Calc.Bool;

namespace Genetic
{
  namespace Numerical
  {

    public class TraitGenesDNA
    {

      private Dictionary<ETrait, Gene> m_chromos = new Dictionary<ETrait, Gene>();

      public PhysicalDNA()
      {
        foreach (ETrait trait in System.Enum.GetValues(typeof(ETrait)))
        {
          m_chromos.Add(trait, new Gene());
        }
      }

      public PhysicalDNA(Dictionary<ETrait, Gene> p_chromos)
      {
        m_chromos = p_chromos;
      }

      //This is the stat representation
      private Gene getChromo(ETrait p_trait)
      {
        return m_chromos[p_trait];
      }

      public float getTraitValue(ETrait p_trait)
      {
        return m_chromos[p_trait].getGeneValue();
      }

      //Clonable
      public PhysicalDNA clone()
      {
        Dictionary<ETrait, Gene> clone_chromos = new Dictionary<ETrait, Gene>();

        foreach (ETrait trait in System.Enum.GetValues(typeof(ETrait)))
        {
          clone_chromos.Add(trait, m_chromos[trait].clone());
        }

        return new PhysicalDNA(clone_chromos);
      }


      //ToString
      public override string ToString()
      {
        return "DNA -- At: " + m_chromos[ETrait.ATTACK].getGeneValue() + ", De: " + m_chromos[ETrait.DEFENSE].getGeneValue() + ", Sp: " + m_chromos[ETrait.SPEED].getGeneValue() + ", Hp: " + m_chromos[ETrait.HP].getGeneValue();
      }

      //DNA Expression Functions
      public Dictionary<ETrait, StatTuple> expressDNA()
      {
        Dictionary<ETrait, StatTuple> to_return = new Dictionary<ETrait, StatTuple>();

        foreach (ETrait stat in m_chromos.Keys)
        {
          float value = getTraitValue(stat);
          to_return.Add(stat, new StatTuple(value, value));
        }

        return to_return;
      }

      //Evolution Functions
      public PhysicalDNA mutate()
      {
        PhysicalDNA clone = this.clone();

        foreach (ETrait trait in System.Enum.GetValues(typeof(ETrait)))
        {
          clone.m_chromos[trait] = clone.m_chromos[trait].mutate();
        }

        return clone;
      }

      public PhysicalDNA crossover(PhysicalDNA p_dna2)
      {
        return new PhysicalDNA();
      }

      public static PhysicalDNA crossover(PhysicalDNA p_dna1, PhysicalDNA p_dna2)
      {
        Dictionary<ETrait, Gene> chromos = new Dictionary<ETrait, Gene>();

        foreach (ETrait trait in System.Enum.GetValues(typeof(ETrait)))
        {
          chromos.Add(trait, Gene.crossover(p_dna1.getChromo(trait), p_dna2.getChromo(trait)));
        }

        return new PhysicalDNA(chromos);
      }

      //Data Collection
      public string[] getStatsCSV(int creature, float p_fitness)
      {
        string[] stats = { creature.ToString(), m_chromos[ETrait.ATTACK].getGeneValue().ToString(), m_chromos[ETrait.DEFENSE].getGeneValue().ToString(), m_chromos[ETrait.SPEED].getGeneValue().ToString(), m_chromos[ETrait.HP].getGeneValue().ToString(), p_fitness + "" };
        return stats;
      }

    }

    ///<summary> A crossoverable, mutatable array of floats</summary>
    public class Gene : IMutatable<Gene>, ICrossoverable<Gene>{

      private float[] m_genes;

      private int m_mutation_iterations;
      private Range<float> m_mutation_range;

      public Gene(int p_number, Range<float> p_range, int p_mutation_iterations, Range<float> p_mutation_range){
        m_genes = new float[p_number];

        for(int i = 0; i<m_genes.Length; i++){
          m_genes[i] = RandomCalc.Rand(p_range);
        }

        m_mutation_iterations = p_mutation_iterations;
        m_mutation_range = p_mutation_range;
      }

      public float GeneValue { get { return ArrayCalc.floatArraySum(m_genes); } }

      private float getGene(int index){
        return m_genes[index];
      }   

      public Gene Clone(){
        Gene clone = new Gene(m_genes.Length, new Range<float>(0,0), m_mutation_iterations, m_mutation_range);

        for(int i = 0; i<m_genes.Length; i++){
          clone.m_genes[i] = m_genes[i];
        }

        return clone;
      }

      public Gene mutate()
      {
          Gene clone = this.Clone();

          for(int i = 0; i<m_mutation_iterations; i++){
            int index = Random.Range(0,5);
            clone.m_genes[index] += RandomCalc.Rand(m_mutation_range);
          }
          
          return clone;
      }

      public Gene crossover(Gene p_crossover_object)
      {
          Gene new_gene = new Gene(m_genes.Length, new Range<float>(0,0), m_mutation_iterations, m_mutation_range);

          for(int i=0; i < new_gene.m_genes.Length; i++){
            new_gene.m_genes[i] = BoolCalc.random() ? m_genes[i]: p_crossover_object.m_genes[i];
          }

          return new_gene;
      }
    }

  }


}

