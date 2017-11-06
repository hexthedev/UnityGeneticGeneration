using JTools.Calc.Base;
using JTools.Calc.Rand;
using JTools.Calc.Array;
using JTools.Calc.Bool;

using JTools.Interfaces;

using Genetic.Base;


namespace Genetic
{

  namespace Traits
  {

    namespace Base
    {
      public enum ETrait
      {
        ATTACK, DEFENSE, SPEED, HP, CRED, CBLUE, CGREEN
      }

      ///<summary> A crossoverable, mutatable array of floats</summary>
      public class Gene : IMutatable<Gene>, ICrossoverable<Gene>, ICloneable<Gene>
      {

        private float[] m_genes;
        private int m_mutation_iterations;
        private Range<float> m_mutation_range;

        public Gene(int p_number, Range<float> p_range, int p_mutation_iterations, Range<float> p_mutation_range)
        {
          m_genes = new float[p_number];

          for (int i = 0; i < m_genes.Length; i++)
          {
            m_genes[i] = RandomCalc.Rand(p_range);
          }

          m_mutation_iterations = p_mutation_iterations;
          m_mutation_range = p_mutation_range;
        }

        public float GeneValue { get { return ArrayCalc.floatArraySum(m_genes); } }

        private float getGene(int index)
        {
          return m_genes[index];
        }

        public Gene Clone()
        {
          Gene clone = new Gene(m_genes.Length, new Range<float>(0, 0), m_mutation_iterations, m_mutation_range);

          for (int i = 0; i < m_genes.Length; i++)
          {
            clone.m_genes[i] = m_genes[i];
          }

          return clone;
        }

        public Gene mutate()
        {
          Gene clone = this.Clone();

          for (int i = 0; i < m_mutation_iterations; i++)
          {
            int index = ArrayCalc.randomIndex(clone.m_genes);
            clone.m_genes[index] += RandomCalc.Rand(m_mutation_range);
          }

          return clone;
        }

        public Gene crossover(Gene p_crossover_object)
        {
          Gene new_gene = new Gene(m_genes.Length, new Range<float>(0, 0), m_mutation_iterations, m_mutation_range);

          for (int i = 0; i < new_gene.m_genes.Length; i++)
          {
            new_gene.m_genes[i] = BoolCalc.random() ? m_genes[i] : p_crossover_object.m_genes[i];
          }

          return new_gene;
        }

        public override string ToString(){
          string str = "[ ";

          foreach(float x in m_genes){
            str += x + " ";
          }

          str += "]";

          return str;
        }
      }

    }


  }

}
