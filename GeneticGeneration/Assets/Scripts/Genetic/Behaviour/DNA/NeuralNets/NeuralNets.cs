using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Interfaces;
using JTools.Calc.Matrices;
using JTools.Calc.Array;
using JTools.Calc.Base;
using JTools.Calc.Rand;
using JTools.Calc.ActiavationFunctions;


using MathNet.Numerics.LinearAlgebra;

using Genetic.Base;
using JTools.Calc.Bool;

namespace Genetic
{
  namespace Behaviour
  {
    namespace NeuralNets
    {
			public class NeuralNet : IBrain{
				DInput[] m_inputs;
        DOutput[] m_outputs;
        Matrix<float>[] m_weights;

        public NeuralNet(DInput[] p_inputs, DOutput[] p_outputs, Matrix<float>[] p_weights){
          m_inputs = p_inputs;
          m_outputs = p_outputs;

          m_weights = new Matrix<float>[p_weights.Length];

          for(int i = 0; i<p_weights.Length; i++){
            m_weights[i] = MatrixCalc.shallowClone(p_weights[i]);
          }
        }

        public void brainAction()
        {
          Matrix<float> current_matrix = getInputValueMatrix()*m_weights[0];
          DActivationFunction activator = ActivationFactory.generateSigmoid(2, 2, true, false, false);
          MatrixCalc.activate(activator, current_matrix);

          for(int i = 1; i<m_weights.Length; i++){
            current_matrix = current_matrix * m_weights[i];
            MatrixCalc.activate(activator, current_matrix);
          }

          performOutputs(current_matrix);
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
          //NEEDS DOING
          return "DECISION NET :: Inputs: " + m_inputs.Length + " - Outputs: " + m_outputs.Length + " - Weight Sum: " + MatrixCalc.sum(m_weights[0]);
        }

        //MAYBE ADD LEARNING TO THIS MODEL EVENTUALLY
      }

      ///<summary> In this case T is the controller </summary>
      public class NeuralNetDNA<T> : ADNA<NeuralNetDNA<T>>, IControllerExpressable<T, NeuralNet>, ICloneable<NeuralNetDNA<T>> where T:AController
      {
        int m_id;
				DInputFactory<T>[] m_inputs;
				DOutputFactory<T>[] m_outputs;
				Matrix<float>[] m_weights;
        Range<float> m_mutation_multiplier;

				public NeuralNetDNA(int p_id, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, int p_hidden_number, int p_hidden_size,  
                            Range<float> p_weight_range, Range<float> p_mutation_multiplier){

					m_id = p_id;
					m_inputs = ArrayCalc.shallowClone(p_inputs);
					m_outputs = ArrayCalc.shallowClone(p_outputs);


          m_weights = new Matrix<float>[p_hidden_number+1];

          m_weights[0] = Matrix<float>.Build.Dense( m_inputs.Length, p_hidden_size, (i,j) => { return RandomCalc.Rand( p_weight_range ); } );

          for(int i = 1; i<m_weights.Length-1; i++){
            m_weights[i] = Matrix<float>.Build.Dense( p_hidden_size, p_hidden_size, (x,y) => { return RandomCalc.Rand( p_weight_range ); } );
          }

          m_weights[m_weights.Length-1] = Matrix<float>.Build.Dense( p_hidden_size, m_outputs.Length, (i,j) => { return RandomCalc.Rand( p_weight_range ); } );         
          
          m_mutation_multiplier = p_mutation_multiplier;
				}	

        //Private constructor for cloning
        private NeuralNetDNA(int p_id, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, Range<float> p_mutation_multiplier){
					m_id = p_id;
					m_inputs = ArrayCalc.shallowClone(p_inputs);
					m_outputs = ArrayCalc.shallowClone(p_outputs);
          
          m_mutation_multiplier = p_mutation_multiplier;
				}	

				public NeuralNetDNA<T> Clone()
        {
          NeuralNetDNA<T> clone = new NeuralNetDNA<T>(m_id, m_inputs, m_outputs, m_mutation_multiplier);

          clone.m_weights = new Matrix<float>[m_weights.Length];

          for(int i = 0; i<m_weights.Length; i++){
            clone.m_weights[i] = MatrixCalc.shallowClone(m_weights[i]);
          }

          return clone;
        }

        public override NeuralNetDNA<T> crossover(NeuralNetDNA<T> p_crossover_object)
        {
          //CAN optimize with another private function. CLone copies all weights then they get overriden
          NeuralNetDNA<T> crossovered = Clone();
         
          for(int i = 0; i<m_weights.Length; i++){
            crossovered.m_weights[i] = MatrixCalc.crossover(m_weights[i], p_crossover_object.m_weights[i]);
          }
         
          return crossovered;
        }

        public override NeuralNetDNA<T> getSelf()
        {
          return this;
        }

        public override NeuralNetDNA<T> mutate()
        {
          NeuralNetDNA<T> mutated = Clone();
          
          for(int i = 0; i<m_weights.Length; i++){
    
            bool test = BoolCalc.random();
            Range<float> mut = test ? m_mutation_multiplier : new Range<float>( -m_mutation_multiplier.Min, -m_mutation_multiplier.Max );
            mutated.m_weights[i] =  MatrixCalc.elementwiseRandomMultiply(m_weights[i], mut);
          }

          return mutated;
        }

        public NeuralNet express(T p_controller)
        {
          DInput[] inputs = new DInput[m_inputs.Length];

          for(int i = 0; i<m_inputs.Length; i++){
            inputs[i] = m_inputs[i](p_controller);
          }

          DOutput[] outputs = new DOutput[m_outputs.Length];

          for(int i = 0; i<outputs.Length; i++){
            outputs[i] = m_outputs[i](p_controller);
          }
          
          return new NeuralNet(inputs, outputs, m_weights);
        }

        public override string ToString(){
          //Needs doing
          return "DECISION NET DNA:: ID: " + m_id + " - Inputs: " + m_inputs.Length + " - Outputs: " + m_outputs.Length + " - Weights: " + MatrixCalc.sum(m_weights[0]) + " - Mutation Rate: " + m_mutation_multiplier.Min + " to " + m_mutation_multiplier.Max;
        }
        
      }

      ///<summary> In this case T is the controller </summary>
      public class NeuralNetSpecies<T> : ISpecies<ADNA<NeuralNetDNA<T>>> where T:AController
      {
        private int m_id;
				private DInputFactory<T>[] m_inputs;
				private DOutputFactory<T>[] m_outputs;
        private int m_hidden_number;
        private int m_hidden_size;
				private Range<float> m_weight_range;
        private Range<float> m_mutation_multiplier;

        public NeuralNetSpecies(int p_id, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, int p_hidden_number, int p_hidden_size, 
                                Range<float> p_weight_range,  Range<float> p_mutation_multiplier)
        {	
					m_id = p_id;
					m_inputs = p_inputs;
					m_outputs = p_outputs;
          m_hidden_number = p_hidden_number;
          m_hidden_size = p_hidden_size;
          m_weight_range = p_weight_range;
          m_mutation_multiplier = p_mutation_multiplier;
        }

        public int ID { get { return m_id; } }

        public override string ToString(){
          //NEEDS DOING
          return "NEURAL NET SPECIES:: ID: " + m_id + " - Inputs: " + m_inputs.Length + " - Ouputs: " + m_outputs.Length + " - Mutation Range: " + m_mutation_multiplier.Min + " to " + m_mutation_multiplier.Max;
        }

        public ADNA<NeuralNetDNA<T>> randomInstance()
        {
          return new NeuralNetDNA<T>(m_id, m_inputs, m_outputs, m_hidden_number, m_hidden_size, m_weight_range, m_mutation_multiplier);
        }
      }

    }

  }

}
