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
			public class DecisionNet : IBrain{
				DInput[] m_inputs;
        DOutput[] m_outputs;
        Matrix<float> m_weights;

        public DecisionNet(DInput[] p_inputs, DOutput[] p_outputs, Matrix<float> p_weights){
          if(!MatrixCalc.isSize(p_weights, p_inputs.Length,  p_outputs.Length)) Debug.LogError("Constructing DecisionNet with wrong sized matrix");

          m_inputs = p_inputs;
          m_outputs = p_outputs;
          m_weights = MatrixCalc.shallowClone(p_weights);

        }

        public void brainAction()
        {
          performOutputs(getInputValueMatrix()*m_weights);
        }

        private void performOutputs(Matrix<float> p_output_values){
          if(!MatrixCalc.isSize(p_output_values, 1, m_outputs.Length)) Debug.LogError("Trying to perform outputs in Decision net with malformed output_values matrix");

          for(int i = 0; i<m_outputs.Length; i++){
            m_outputs[i](p_output_values[0,i]);
          }          
        }

        private Matrix<float> getInputValueMatrix(){
          Matrix<float> input_values = Matrix<float>.Build.Dense(1, m_inputs.Length);
          
          for(int i = 0; i<m_inputs.Length; i++){
            input_values[0, i] = m_inputs[i]();
          }

          return input_values;
        }

        public override string ToString(){
          return "DECISION NET :: Inputs: " + m_inputs.Length + " - Outputs: " + m_outputs.Length + " - Weight Sum: " + MatrixCalc.sum(m_weights);
        }

        //MAYBE ADD LEARNING TO THIS MODEL EVENTUALLY
      }

      ///<summary> In this case T is the controller </summary>
      public class DecisionNetDNA<T> : ADNA<DecisionNetDNA<T>>, IControllerExpressable<T, DecisionNet>, ICloneable<DecisionNetDNA<T>> where T:AController
      {
        int m_id;
				DInputFactory<T>[] m_inputs;
				DOutputFactory<T>[] m_outputs;
				Matrix<float> m_weights;
        Range<float> m_mutation_multiplier;

				public DecisionNetDNA(int p_id, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, Matrix<float> p_weights, Range<float> p_mutation_multiplier){

					if (!MatrixCalc.isSize(p_weights, p_inputs.Length, p_outputs.Length)) Debug.LogError("DecisionNetDNA requires Matrix input size inputs by outputs");

					m_id = p_id;
					m_inputs = ArrayCalc.shallowClone(p_inputs);
					m_outputs = ArrayCalc.shallowClone(p_outputs);
					m_weights = MatrixCalc.shallowClone( MatrixCalc.columnNormalize(p_weights));
          m_mutation_multiplier = p_mutation_multiplier;
				}	

				public DecisionNetDNA<T> Clone()
        {
          return new DecisionNetDNA<T>(m_id, m_inputs, m_outputs, m_weights, m_mutation_multiplier);
        }

        public override DecisionNetDNA<T> crossover(DecisionNetDNA<T> p_crossover_object)
        {
          DecisionNetDNA<T> crossovered = Clone();
          crossovered.m_weights = MatrixCalc.crossover(m_weights, p_crossover_object.m_weights);
          return crossovered;
        }

        public override DecisionNetDNA<T> getSelf()
        {
          return this;
        }

        public override DecisionNetDNA<T> mutate()
        {
          DecisionNetDNA<T> mutated = Clone();
          mutated.m_weights = MatrixCalc.elementwiseRandomMultiply(m_weights, m_mutation_multiplier);
          mutated.m_weights = MatrixCalc.columnNormalize(mutated.m_weights);
          return mutated;
        }

        public DecisionNet express(T p_controller)
        {
          DInput[] inputs = new DInput[m_inputs.Length];

          for(int i = 0; i<m_inputs.Length; i++){
            inputs[i] = m_inputs[i](p_controller);
          }

          DOutput[] outputs = new DOutput[m_outputs.Length];

          for(int i = 0; i<outputs.Length; i++){
            outputs[i] = m_outputs[i](p_controller);
          }
          
          return new DecisionNet(inputs, outputs, m_weights);
        }

        public void receiveLearnedMatrix(Matrix<float> p_matrix){
          if(!MatrixCalc.isSameSize(p_matrix, m_weights)) Debug.LogError("Learned matrix must be same size as non-learned matrix");

          m_weights = MatrixCalc.shallowClone(p_matrix);
        }

        public override string ToString(){
          return "DECISION NET DNA:: ID: " + m_id + " - Inputs: " + m_inputs.Length + " - Outputs: " + m_outputs.Length + " - Weights: " + MatrixCalc.sum(m_weights) + " - Mutation Rate: " + m_mutation_multiplier.Min + " to " + m_mutation_multiplier.Max;
        }
        
      }

      ///<summary> In this case T is the controller </summary>
      public class DecisionNetSpecies<T> : ISpecies<ADNA<DecisionNetDNA<T>>> where T:AController
      {
        private int m_id;
				private DInputFactory<T>[] m_inputs;
				private DOutputFactory<T>[] m_outputs;

				private Range<float> m_weight_range = new Range<float>(-1f,1f);
        private Range<float> m_mutation_multiplier;

        public DecisionNetSpecies(int p_id, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, Range<float> p_mutation_multiplier)
        {	
					m_id = p_id;
					m_inputs = p_inputs;
					m_outputs = p_outputs;
          m_mutation_multiplier = p_mutation_multiplier;
        }

        public int ID { get { return m_id; } }

        public override string ToString(){
          return "DECISION NET SPECIES:: ID: " + m_id + " - Inputs: " + m_inputs.Length + " - Ouputs: " + m_outputs.Length + " - Mutation Range: " + m_mutation_multiplier.Min + " to " + m_mutation_multiplier.Max;
        }

        public ADNA<DecisionNetDNA<T>> randomInstance()
        {
          return new DecisionNetDNA<T>(m_id, m_inputs, m_outputs, Matrix<float>.Build.Dense(m_inputs.Length, m_outputs.Length, (i,j) => { return RandomCalc.Rand(m_weight_range);}), m_mutation_multiplier);
        }
      }

    }

  }

}
