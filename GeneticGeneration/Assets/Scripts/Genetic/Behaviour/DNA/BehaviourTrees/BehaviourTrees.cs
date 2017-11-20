using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Interfaces;
using JTools.Calc.Matrices;
using JTools.Calc.Array;
using JTools.Calc.Base;
using JTools.Calc.Rand;
using JTools.Calc.Bool;
using JTools.DataStructures.Trees;

using MathNet.Numerics.LinearAlgebra;

using Genetic.Base;

namespace Genetic
{
  namespace Behaviour
  {
    namespace BehaviourTrees
    {
			public class BehaviourTree : IBrain{
				DInput[] m_inputs;
        DOutput[] m_outputs;
        Matrix<float> m_weights;

        public BehaviourTree(DInput[] p_inputs, DOutput[] p_outputs, Matrix<float> p_weights){
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
      public class BehaviourTreeDNA<T> : ADNA<BehaviourTreeDNA<T>>, IControllerExpressable<T, BehaviourTree>, ICloneable<BehaviourTreeDNA<T>> where T:AController
      {
        int m_id;
				DInputFactory<T>[] m_inputs;
				DOutputFactory<T>[] m_outputs;
        Range<float> m_mutation_multiplier;

				public BehaviourTreeDNA(int p_id, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, Range<float> p_mutation_multiplier){
					m_id = p_id;
					m_inputs = ArrayCalc.shallowClone(p_inputs);
					m_outputs = ArrayCalc.shallowClone(p_outputs);
          m_mutation_multiplier = p_mutation_multiplier;
				}	

				public BehaviourTreeDNA<T> Clone()
        {
          return new BehaviourTreeDNA<T>(m_id, m_inputs, m_outputs, m_mutation_multiplier);
        }

        public override BehaviourTreeDNA<T> crossover(BehaviourTreeDNA<T> p_crossover_object)
        {
          BehaviourTreeDNA<T> crossovered = Clone();
          crossovered.m_weights = MatrixCalc.crossover(m_weights, p_crossover_object.m_weights);
          return crossovered;
        }

        public override BehaviourTreeDNA<T> getSelf()
        {
          return this;
        }

        public override BehaviourTreeDNA<T> mutate()
        {
          BehaviourTreeDNA<T> mutated = Clone();
          mutated.m_weights = MatrixCalc.elementwiseRandomMultiply(m_weights, m_mutation_multiplier);
          mutated.m_weights = MatrixCalc.columnNormalize(mutated.m_weights);
          return mutated;
        }

        public BehaviourTree express(T p_controller)
        {
          DInput[] inputs = new DInput[m_inputs.Length];

          for(int i = 0; i<m_inputs.Length; i++){
            inputs[i] = m_inputs[i](p_controller);
          }

          DOutput[] outputs = new DOutput[m_outputs.Length];

          for(int i = 0; i<outputs.Length; i++){
            outputs[i] = m_outputs[i](p_controller);
          }
          
          return new BehaviourTree(inputs, outputs, m_weights);
        }

        public override string ToString(){
          return "BEHAVIOUR NET DNA:: ID: " + m_id + " - Inputs: " + m_inputs.Length + " - Outputs: " + m_outputs.Length + " - Weights: " + MatrixCalc.sum(m_weights) + " - Mutation Rate: " + m_mutation_multiplier.Min + " to " + m_mutation_multiplier.Max;
        }
        
      }



      ///<summary> In this case T is the controller </summary>
      public class BehaviourTreeSpecies<T> : ISpecies<ADNA<BehaviourTreeDNA<T>>> where T:AController
      {
        private int m_id;
				private DInputFactory<T>[] m_inputs;
				private DOutputFactory<T>[] m_outputs;

				private Range<float> m_threshold_range = new Range<float>(0.1f, 0.9f);
        private Range<float> m_mutation_multiplier;

        public BehaviourTreeSpecies(int p_id, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, Range<float> p_mutation_multiplier)
        {	
					m_id = p_id;
					m_inputs = p_inputs;
					m_outputs = p_outputs;
          m_mutation_multiplier = p_mutation_multiplier;
        }

        public int ID { get { return m_id; } }

        public override string ToString(){
          return "BEHAVIOUR TREE SPECIES:: ID: " + m_id + " - Inputs: " + m_inputs.Length + " - Ouputs: " + m_outputs.Length + " - Mutation Range: " + m_mutation_multiplier.Min + " to " + m_mutation_multiplier.Max;
        }

        public ADNA<BehaviourTreeDNA<T>> randomInstance()
        {
          return new BehaviourTreeDNA<T>(m_id, m_inputs, m_outputs, m_mutation_multiplier);
        }
      }







      //BT DNA IMPLEMENTATION


      //NEED TO WRITE THE ABTDNATREE which will be randomly generated and do mutation and crossover opertations. 
      //Last part of the puzzle then should be able to test


      public class ABTDNANode : ATreeNode<ABTDNANode>
      {
        EBTNodeTypes m_type; 
        float[] m_out_values; 
        int m_input_index; 
        float m_threshold;


        public ABTDNANode(EBTNodeTypes p_type, float[] p_outvalues, int p_input_index, float p_threshold, ATreeNode<ABTDNANode> p_parent) : base(p_parent)
        {
          m_type = p_type;

          if(p_type == EBTNodeTypes.ACTION){
            m_out_values = ArrayCalc.shallowClone(p_outvalues);
            addChild(null);
          } else if(p_type == EBTNodeTypes.ACTION){
            m_input_index = p_input_index;
            m_threshold = p_threshold;
            addChild(null);
            addChild(null);
          } else {
            Debug.LogError("Bad constructor argument ABTDNA Node");
          }
        }

        protected override void addChild(ABTDNANode p_child)
        {
          m_children.Add(p_child);
        }

        protected override void addChild(ABTDNANode p_child, int p_index)
        {
          m_children[p_index] = p_child;
        }

        protected override ABTDNANode setSelf()
        {
          return this;
        }

        public ABTNode generateNode(BTree p_tree, ATreeNode<ABTNode> p_parent){
          if(m_type == EBTNodeTypes.ACTION){
            return new BTActionNode(m_out_values, p_tree, p_parent);
          } else if(m_type == EBTNodeTypes.DETECTOR){
            return new BTDetectorNode(m_input_index, m_threshold, p_tree, p_parent);
          } else {
            Debug.LogError("Bad node generation, no Type");
            return null;
          }
        }
      }

      public enum EBTNodeTypes { DETECTOR, ACTION };







      //CONCRETE BT TREE IMPLMENTATION
      public class BTree : Tree<ABTNode>
      {
        public DInput[] m_inputs;
        public DOutput[] m_outputs;

        public BTree(DInput[] p_inputs,  DOutput[] p_outputs, ATreeNode<ABTNode> p_root) : base(p_root)
        {
          m_inputs = p_inputs;
          m_outputs = p_outputs;
        }

        ///<summary>Index means nothing in this case Maybe need to redesign the inheritance</summary>
        public override void traverse(int index){
          traverse();
        }

        public void traverse(){
          ATreeNode<ABTNode> m_current_node = CurrentNode.m_self.iterate(); //Should never return null
        }
      }

      public class BTActionNode : ABTNode
      {
        BTree m_tree;
        float[] m_out_values;

        public BTActionNode(float[] p_out_values, BTree p_tree, ATreeNode<ABTNode> p_parent) : base(p_parent)
        {
          m_tree = p_tree;
          m_out_values = ArrayCalc.shallowClone(p_out_values);
          addChild(null);
        }

        public override ATreeNode<ABTNode> iterate()
        {
          for(int i = 0; i<m_out_values.Length; i++){
            m_tree.m_outputs[i](m_out_values[i]);
          }

          if(m_children[0] == null){
            return Root;
          } else {
            return m_children[0];
          }
        }

        protected override void addChild(ABTNode p_child)
        {
          m_children.Add(p_child);
        }

        protected override void addChild(ABTNode p_child, int p_index)
        {
          m_children[p_index] = p_child;
        }

        protected override ABTNode setSelf()
        {
          return this;
        }
      }

      public class BTDetectorNode : ABTNode
      {
        BTree m_tree;
        int m_detector_index;
        float m_threshold;

        public BTDetectorNode(int p_detector, float p_threshold, BTree p_tree, ATreeNode<ABTNode> p_parent) : base(p_parent)
        {
          m_tree = p_tree;
          m_detector_index = p_detector;
          m_threshold = p_threshold;

          addChild(null);
          addChild(null);
        }

        public override ATreeNode<ABTNode> iterate()
        {
          //NEEDS FIXING
          bool rand = m_tree.m_inputs[m_detector_index]() > m_threshold;
          int index = rand ? 1 : 0;

          return getChild(index) == null ? Root.iterate() : getChild(index).m_self.iterate();
        }

        protected override void addChild(ABTNode p_child)
        {
          m_children.Add(p_child);
        }

        protected override void addChild(ABTNode p_child, int p_index)
        {
          if(m_children.Count <= p_index) Debug.LogError("Index out of range adding children Detector Node");
          m_children[p_index] = p_child;
        }

        protected override ABTNode setSelf()
        {
          return this;
        }
      }

      //T ATreeNode of type controller
      public abstract class ABTNode : ATreeNode<ABTNode> {
        protected ABTNode(ATreeNode<ABTNode> p_parent) : base(p_parent)
        {
        }

        public abstract ATreeNode<ABTNode> iterate();
      } 
    }

  }

}
