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
      public class ABTDNATree<T> : Tree<ABTDNANode> where T:AController
      {
        public DInputFactory<T>[] m_input_factories;
        public DOutputFactory<T>[] m_output_factories;
        public ABTDNATree(DInputFactory<T>[] p_input_factories, DOutputFactory<T>[] p_output_factories, ATreeNode<ABTDNANode> p_root) : base(p_root)
        {
          m_input_factories = ArrayCalc.shallowClone(p_input_factories);
          m_output_factories = ArrayCalc.shallowClone(p_output_factories);
        }

        //generate a random ABTDNATree based on input info
        public static ABTDNATree<T> random(Range<float> p_weight_values, Range<float> p_threshold_range, DInputFactory<T>[] p_input_factories, DOutputFactory<T>[] p_output_factories, ATreeNode<ABTDNANode> p_root){

          ABTDNANode node = new ABTDNANode(
            EBTNodeTypes.DETECTOR, 
            ArrayCalc.functionInitialize(p_output_factories.Length, ()=> { return RandomCalc.Rand(p_weight_values); } ),
            ArrayCalc.randomIndex(p_input_factories),
            RandomCalc.Rand(p_threshold_range),
            null          
          );

          node.addChild( ABTDNANode.randomActionNode( p_output_factories.Length, p_weight_values, node), 0 );
          node.addChild( ABTDNANode.randomActionNode( p_output_factories.Length, p_weight_values, node), 1 );

          ABTDNATree<T> tree = new ABTDNATree<T>(p_input_factories, p_output_factories, node);

          randomPopulate(node.getChild(0).m_self, p_output_factories.Length, p_weight_values, p_input_factories.Length-1, p_threshold_range);
          randomPopulate(node.getChild(1).m_self, p_output_factories.Length, p_weight_values, p_input_factories.Length-1, p_threshold_range);

          return tree;
        } 

        private static void randomPopulate(ABTDNANode node, int p_out_value_number, Range<float> p_out_value_range, 
          int p_max_input_index, Range<float> p_threshold_range){

          if(node == null) return;

          node.addChild(ABTDNANode.randomNode(p_out_value_number, p_out_value_range, p_max_input_index, p_threshold_range, node), 0);
          randomPopulate(node.getChild(0).m_self, p_out_value_number, p_out_value_range, p_max_input_index, p_threshold_range);

          if(node.Type == EBTNodeTypes.DETECTOR){
            node.addChild(ABTDNANode.randomNode(p_out_value_number, p_out_value_range, p_max_input_index, p_threshold_range, node), 1);
            randomPopulate(node.getChild(1).m_self, p_out_value_number, p_out_value_range, p_max_input_index, p_threshold_range);
          }
        } 

        //Express the dna tree as a concrete tree
        public BTree expressConcrete(T p_controller){

          //THE NULL THING HERE MIGHT ERROR
          BTree tree = new BTree( 
            ArrayCalc.map(m_input_factories, ( DInputFactory<T> fact ) => { return fact(p_controller); } ),
            ArrayCalc.map(m_output_factories, ( DOutputFactory<T> fact ) => { return fact(p_controller); } ),
            null
          );

          ABTNode node = m_root.m_self.generateNode(tree, null);
          tree.setRoot(node);

          recursiveNodeGeneration(m_root.m_self, node, p_controller, tree);

          return tree;
        }

        private void recursiveNodeGeneration(ABTDNANode p_dna_node, ABTNode p_concrete_node, T p_controller, BTree p_tree){
          if(p_dna_node == null) return;

          for(int i = 0; i<p_dna_node.numChildren; i++){

            if(!p_dna_node.existsChild(i)) continue;
            p_concrete_node.addChild(p_dna_node.getChild(i).m_self.generateNode(p_tree, p_concrete_node) ,i);

            recursiveNodeGeneration(p_dna_node.getChild(i).m_self, p_concrete_node.getChild(i).m_self, p_controller, p_tree);
          }
        }

        //Clone the tree recursively
        public ABTDNATree<T> Clone(){
          return new ABTDNATree<T>(ArrayCalc.shallowClone(m_input_factories), ArrayCalc.shallowClone(m_output_factories), recClone(m_root, null) );
        }

        private ATreeNode<ABTDNANode> recClone(ATreeNode<ABTDNANode> p_cloning, ATreeNode<ABTDNANode> p_parent){
          ABTDNANode node = p_cloning.m_self.Clone( p_parent );

          for(int i = 0; i<p_cloning.numChildren; i++){
            if(p_cloning.existsChild(i)) continue;

            node.addChild( recClone(p_cloning.getChild(i), node).m_self ); 
          }

          return node;
        }


        //Clone Trees, choose random slice, give master tree the slice of other tree as child 
        public ABTDNATree<T> crossover(ABTDNATree<T> p_crossover_object){

          ABTDNATree<T> master_tree = Clone();
          ABTDNATree<T> slave_tree = p_crossover_object.Clone();       

          ABTDNANode master_node = randomNode(master_tree.m_root.m_self, master_tree.m_root.m_self);
          ABTDNANode slave_node = randomNode(slave_tree.m_root.m_self, slave_tree.m_root.m_self);

          int index = RandomCalc.Rand(new Range<int>(0, master_node.numChildren) );
          
          master_node.addChild(slave_node ,index);
          slave_node.setParent(master_node);

          return master_tree;
        }

        private ABTDNANode randomNode(ABTDNANode p_cur_node, ABTDNANode p_root_node){
          if(p_cur_node == null) return randomNode(p_root_node, p_root_node);

          bool test = BoolCalc.random();

          if(test){
            return p_cur_node;
          } else {
            return randomNode( p_cur_node.getChild( RandomCalc.Rand(new Range<int>(0, p_cur_node.numChildren))).m_self, p_root_node );
          }
        }

        //Clone tree, mutate all nodes, return
        public ABTDNATree<T> mutate(Range<float> p_mutation_range){
          ABTDNATree<T> tree = Clone();
          tree.mutateRecurse(tree.m_root.m_self, p_mutation_range);
          return tree;
        }

        private void mutateRecurse(ABTDNANode p_node, Range<float> p_mutation_range){
          p_node.mutate(p_mutation_range);

          for(int i = 0; i<p_node.numChildren; i++){
            if(!p_node.existsChild(i)) continue;

            mutateRecurse( p_node.getChild(i).m_self, p_mutation_range); 
          }
        }

      }

      public class ABTDNANode : ATreeNode<ABTDNANode>
      {
        //DNA nodes can represent 2 kinds of nodes
        private EBTNodeTypes m_type; 
        public EBTNodeTypes Type { get{ return m_type; } }
        private float[] m_out_values; 
        private int m_input_index; 
        private float m_threshold;

        //Only some parameters have meaning
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

        public override void addChild(ABTDNANode p_child)
        {
          m_children.Add(p_child);
        }

        public override void addChild(ABTDNANode p_child, int p_index)
        {
          m_children[p_index] = p_child;
        }

        protected override ABTDNANode setSelf()
        {
          return this;
        }

        public void setParent(ABTDNANode p_parent){
          m_parent = p_parent;
        }

        //generates the concrete version of the node
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

        //returns a completely random node
        public static ABTDNANode randomNode(int p_out_value_number, Range<float> p_out_value_range, int p_max_input_index, Range<float> p_threshold_range, ABTDNANode p_parent){
          int rand = RandomCalc.Rand(new Range<int>(0,2));

          switch(rand){
            case 0:
              return null; 
            case 1: 
              return randomActionNode(p_out_value_number, p_out_value_range, p_parent);
            case 2: 
              return new ABTDNANode(
                EBTNodeTypes.DETECTOR,
                null,
                RandomCalc.Rand(new Range<int>(0, p_max_input_index)),
                RandomCalc.Rand(p_threshold_range),
                p_parent
              );
          }

          return null;
        }

        //returns a random action node
        public static ABTDNANode randomActionNode(int p_out_value_number, Range<float> p_out_value_range, ABTDNANode p_parent){
          return new ABTDNANode(
            EBTNodeTypes.ACTION, 
            ArrayCalc.functionInitialize( p_out_value_number, ()=>{ return RandomCalc.Rand(p_out_value_range); } ),
            0,
            0,
            p_parent
          );
        }

        //Mutates the node
        public void mutate(Range<float> p_mutation_range){
          if(m_type == EBTNodeTypes.DETECTOR )m_threshold *= RandomCalc.Rand(p_mutation_range);

          if(m_type == EBTNodeTypes.ACTION){
            for(int i = 0; i<m_out_values.Length; i++){
              m_out_values[i] *= RandomCalc.Rand(p_mutation_range);
            }
          }
        }

        //CLones the node giving it parent passed in
        public ABTDNANode Clone( ATreeNode<ABTDNANode> p_parent){
          return new ABTDNANode(m_type, ArrayCalc.shallowClone(m_out_values), m_input_index, m_threshold, p_parent);
        }
      }

      public enum EBTNodeTypes { DETECTOR, ACTION };







      //CONCRETE BT TREE IMPLMENTATION
      public class BTree : Tree<ABTNode>
      {
        //Concrete inputs
        public DInput[] m_inputs;
        public DOutput[] m_outputs;

        //Construct with root
        public BTree(DInput[] p_inputs,  DOutput[] p_outputs, ATreeNode<ABTNode> p_root) : base(p_root)
        {
          m_inputs = p_inputs;
          m_outputs = p_outputs;
        }

        //Need to set root sometimes after the fact. Because all nodes have ref to tree
        public void setRoot(ATreeNode<ABTNode> p_root){
          m_root = p_root;
        }

        ///<summary>Index means nothing in this case Maybe need to redesign the inheritance</summary>
        public override void traverse(int index){
          traverse();
        }

        //iterate does something and returns a node with will be activated next iteration
        public void traverse(){
          ATreeNode<ABTNode> m_current_node = CurrentNode.m_self.iterate(); //Should never return null
        }
      }

      public class BTActionNode : ABTNode
      {
        //Ref to Tree and out values
        BTree m_tree;
        float[] m_out_values;

        public BTActionNode(float[] p_out_values, BTree p_tree, ATreeNode<ABTNode> p_parent) : base(p_parent)
        {
          m_tree = p_tree;
          m_out_values = ArrayCalc.shallowClone(p_out_values);
          addChild(null);
        }

        //iteration gives out values to controller with outputs lined up with index
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

        public override void addChild(ABTNode p_child)
        {
          m_children.Add(p_child);
        }

        public override void addChild(ABTNode p_child, int p_index)
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
        //Tree, detector and threshold for detection
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

        //Detector iteration continues onwards
        public override ATreeNode<ABTNode> iterate()
        {
          //NEEDS FIXING
          bool rand = m_tree.m_inputs[m_detector_index]() > m_threshold;
          int index = rand ? 1 : 0;

          return getChild(index) == null ? Root : getChild(index).m_self.iterate();
        }

        public override void addChild(ABTNode p_child)
        {
          m_children.Add(p_child);
        }

        public override void addChild(ABTNode p_child, int p_index)
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
        
        //There are two types which need to be in same tree. This is a wrapper
        protected ABTNode(ATreeNode<ABTNode> p_parent) : base(p_parent) { }

        //iterate is how we traverse and make behaviour happen
        public abstract ATreeNode<ABTNode> iterate();
      } 
    }

  }

}
