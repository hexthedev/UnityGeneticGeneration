using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Interfaces;
using JTools.Calc.Matrices;
using JTools.Calc.Array;
using JTools.Calc.Base;
using JTools.Calc.Rand;

using MathNet.Numerics.LinearAlgebra;

using Genetic.Base;

namespace Genetic
{

  namespace Behaviour
  {

    namespace DecisionNets
    {

			public class DecisionNet{
				
			}

      public class DecisionNetDNA<T> : IDNA<DecisionNet>, IControllerExpressable<T, DecisionNet>, ICloneable<DecisionNetDNA<T>>
      {
        int m_id;
				DDecisionNetInputFactory<T>[] m_inputs;
				DDecisionNetOutputFactory<T>[] m_outputs;
				Matrix<float> m_weights;


				public DecisionNetDNA(int p_id, DDecisionNetInputFactory<T>[] p_inputs, DDecisionNetOutputFactory<T>[] p_outputs, Matrix<float> p_weights){

					if (!MatrixCalc.isSize(p_weights, p_inputs.Length, p_outputs.Length)) Debug.LogError("DecisionNetDNA requires Matrix input size inputs by outputs");

					m_id = p_id;
					m_inputs = ArrayCalc.shallowClone(p_inputs);
					m_outputs = ArrayCalc.shallowClone(p_outputs);
					m_weights = MatrixCalc.shallowClone(p_weights);
				}	

				public DecisionNetDNA<T> Clone()
        {
          throw new System.NotImplementedException();
        }

        public override DecisionNet crossover(DecisionNet p_crossover_object)
        {
          throw new System.NotImplementedException();
        }

        public DecisionNet express(T p_controller)
        {
          throw new System.NotImplementedException();
        }

        public override IDNA<DecisionNet> getIDNA(DecisionNet p_dnaify)
        {
          throw new System.NotImplementedException();
        }

        public override DecisionNet getSelf()
        {
          throw new System.NotImplementedException();
        }

        public override DecisionNet mutate()
        {
          throw new System.NotImplementedException();
        }
      }

      public class DecisionNetSpecies<T> : ISpecies<DecisionNetDNA<T>>
      {
        private int m_id;
				private DDecisionNetInputFactory<T>[] m_inputs;
				private DDecisionNetOutputFactory<T>[] m_outputs;

				private Range<float> weight_range = new Range<float>(0,1);

        public DecisionNetSpecies(int p_id, DDecisionNetInputFactory<T>[] p_inputs, DDecisionNetOutputFactory<T>[] p_outputs)
        {	
					m_id = p_id;
					m_inputs = p_inputs;
					m_outputs = p_outputs;
        }

        public int ID { get { return m_id; } }

        public DecisionNetDNA<T> randomInstance()
        {
          return new DecisionNetDNA<T>(m_id, m_inputs, m_outputs, Matrix<float>.Build.Dense(m_inputs.Length, m_outputs.Length, (i,j) => { return RandomCalc.Rand(weight_range);}));
        }
      }


      public delegate float DDecisionNetInput();

			public delegate DDecisionNetInput DDecisionNetInputFactory<T>(T p_controller);

			public delegate void DDecisionNetOutput(float p_value);

			public delegate DDecisionNetInput DDecisionNetOutputFactory<T>(T p_controller);




    }

  }

}
