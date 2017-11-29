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
				BTree m_tree;

        public BehaviourTree(BTree p_tree){
          m_tree = p_tree;
        }

        public void brainAction()
        {
          m_tree.traverse();
        }

        public override string ToString(){
          return m_tree.size();
        }

      }

      ///<summary> In this case T is the controller </summary>
      public class BehaviourTreeDNA<T> : ADNA<BehaviourTreeDNA<T>>, IControllerExpressable<T, BehaviourTree>, ICloneable<BehaviourTreeDNA<T>> where T:AController
      {
        int m_id;
        BTDNATree<T> m_tree;
        Range<float> m_mutation_range;

				public BehaviourTreeDNA(int p_id, BTDNATree<T> p_tree,  Range<float> p_mutation_range){
					m_id = p_id;
          m_tree = p_tree;
          m_mutation_range = p_mutation_range;
				}	

				public BehaviourTreeDNA<T> Clone()
        {
          return new BehaviourTreeDNA<T>(m_id, m_tree.Clone(), m_mutation_range);
        }

        public override BehaviourTreeDNA<T> crossover(BehaviourTreeDNA<T> p_crossover_object)
        {
          return new BehaviourTreeDNA<T>(m_id, m_tree.crossover(p_crossover_object.m_tree), m_mutation_range );
        }

        public override BehaviourTreeDNA<T> getSelf()
        {
          return this;
        }

        public override BehaviourTreeDNA<T> mutate()
        {
          BehaviourTreeDNA<T> mutator = Clone();
          mutator.m_tree = mutator.m_tree.mutate(m_mutation_range);
          return mutator;

        }

        public BehaviourTree express(T p_controller)
        {
          return new BehaviourTree(m_tree.expressConcrete(p_controller));
        }

        public override string ToString(){
          return m_tree.size();
        }
        
      }



      ///<summary> In this case T is the controller </summary>
      public class BehaviourTreeSpecies<T> : ISpecies<ADNA<BehaviourTreeDNA<T>>> where T:AController
      {
        private int m_id;
				private DInputFactory<T>[] m_inputs;
				private DOutputFactory<T>[] m_outputs;

        private Range<float> m_weight_values = new Range<float>(0, 1f);
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
          return "BEHAVIOUR TREE SPECIES:: ID: ";
        }

        public ADNA<BehaviourTreeDNA<T>> randomInstance()
        {
          return new BehaviourTreeDNA<T>( m_id, BTDNATree<T>.random(m_weight_values, m_threshold_range, m_inputs, m_outputs), m_mutation_multiplier );
        }
      }







      //BT DNA IMPLEMENTATION
      public class BTDNATree<T> : Tree<ABTDNANode> where T:AController
      {
        public DInputFactory<T>[] m_input_factories;
        public DOutputFactory<T>[] m_output_factories;
        public BTDNATree(DInputFactory<T>[] p_input_factories, DOutputFactory<T>[] p_output_factories, ATreeNode<ABTDNANode> p_root) : base(p_root)
        {
          m_input_factories = ArrayCalc.shallowClone(p_input_factories);
          m_output_factories = ArrayCalc.shallowClone(p_output_factories);
        }

        //generate a random ABTDNATree based on input info
        public static BTDNATree<T> random(Range<float> p_weight_values, Range<float> p_threshold_range, DInputFactory<T>[] p_input_factories, DOutputFactory<T>[] p_output_factories){

          ABTDNANode node = new ABTDNANode(
            EBTNodeTypes.DETECTOR, 
            ArrayCalc.functionInitialize(p_output_factories.Length, ()=> { return RandomCalc.Rand(p_weight_values); } ),
            ArrayCalc.randomIndex(p_input_factories),
            RandomCalc.Rand(p_threshold_range),
            null          
          );      

          node.addChild( ABTDNANode.randomActionNode( p_output_factories.Length, p_weight_values, node), 0 );
          node.addChild( ABTDNANode.randomActionNode( p_output_factories.Length, p_weight_values, node), 1 );

          BTDNATree<T> tree = new BTDNATree<T>(p_input_factories, p_output_factories, node);

          randomPopulate(node.getChild(0).m_self, p_output_factories.Length, p_weight_values, p_input_factories.Length-1, p_threshold_range, 0);
          randomPopulate(node.getChild(1).m_self, p_output_factories.Length, p_weight_values, p_input_factories.Length-1, p_threshold_range, 0);         

          return tree;
        } 

        private static void randomPopulate(ABTDNANode p_node, int p_out_value_number, Range<float> p_out_value_range, 
          int p_max_input_index, Range<float> p_threshold_range, int p_depth){

          if(p_node == null || p_depth == 10) return;

          p_node.addChild(ABTDNANode.randomNode(p_out_value_number, p_out_value_range, p_max_input_index, p_threshold_range, p_node), 0);            
          if(!p_node.existsChild(0)) return;
          randomPopulate(p_node.getChild(0).m_self, p_out_value_number, p_out_value_range, p_max_input_index, p_threshold_range, p_depth+1);

          if(p_node.Type == EBTNodeTypes.DETECTOR){
            p_node.addChild(ABTDNANode.randomNode(p_out_value_number, p_out_value_range, p_max_input_index, p_threshold_range, p_node), 1);
            if(!p_node.existsChild(1)) return;
            randomPopulate(p_node.getChild(1).m_self, p_out_value_number, p_out_value_range, p_max_input_index, p_threshold_range, p_depth+1);
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
        public BTDNATree<T> Clone(){
          return new BTDNATree<T>(ArrayCalc.shallowClone(m_input_factories), ArrayCalc.shallowClone(m_output_factories), recClone(m_root, null) );
        }

        private ATreeNode<ABTDNANode> recClone(ATreeNode<ABTDNANode> p_cloning, ATreeNode<ABTDNANode> p_parent){
          ABTDNANode node = p_cloning.m_self.Clone( p_parent );

          for(int i = 0; i<p_cloning.numChildren; i++){
            if(!p_cloning.existsChild(i)) continue;                       

            node.addChild( recClone(p_cloning.getChild(i), node).m_self, i ); 
          }

          return node;
        }


        //Clone Trees, choose random slice, give master tree the slice of other tree as child 
        public BTDNATree<T> crossover(BTDNATree<T> p_crossover_object){

          BTDNATree<T> master_tree = Clone();
          BTDNATree<T> slave_tree = p_crossover_object.Clone();       

          ABTDNANode master_node = randomNode(master_tree.m_root.m_self, master_tree.m_root.m_self);
          ABTDNANode slave_node = randomNode(slave_tree.m_root.m_self, slave_tree.m_root.m_self);

          int index = RandomCalc.Rand(new Range<int>(0, master_node.numChildren-1) );
          
          master_node.addChild(slave_node ,index);
          slave_node.setParent(master_node);

          recDepthDelete(master_tree.m_root.m_self, 0, 10);

          return master_tree;
        }

        private void recDepthDelete(ABTDNANode p_node, int p_depth, int p_depth_limit){

          if(p_depth == p_depth_limit){
            for(int i = 0; i<p_node.numChildren; i++){
              p_node.addChild(null, i);
            }
          } else {
            for(int i = 0; i<p_node.numChildren; i++){
              if(p_node.existsChild(i)) recDepthDelete( p_node.getChild(i).m_self, p_depth+1, p_depth_limit );
            }
          }

        }

        private ABTDNANode randomNode(ABTDNANode p_cur_node, ABTDNANode p_root_node){
          if(p_cur_node == null) return randomNode(p_root_node, p_root_node);

          bool test = BoolCalc.random();

          if(test){
            return p_cur_node;
          } else {
            ATreeNode<ABTDNANode> node = p_cur_node.getChild( RandomCalc.Rand(new Range<int>(0, p_cur_node.numChildren-1)));
            ABTDNANode actual_node = node == null? null : node.m_self;           
            return randomNode( actual_node, p_root_node );
          }
        }

        //Clone tree, mutate all nodes, return
        public BTDNATree<T> mutate(Range<float> p_mutation_range){
          BTDNATree<T> tree = Clone();
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


        //Size
        public string size(){
          return m_root.m_self.size(0);
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
          } else if(p_type == EBTNodeTypes.DETECTOR){
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
          float[] values = m_out_values== null ? null : ArrayCalc.shallowClone(m_out_values);
          
          return new ABTDNANode(m_type, values, m_input_index, m_threshold, p_parent);
        }




        public string size(int depth){
          string x = depth + "-" + m_type.ToString() + " ";

          for(int i = 0; i<numChildren;i++){
            if(existsChild(i)) x += getChild(i).m_self.size(depth+1);
          }

          return x;
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
          m_current_node = p_root;
        }

        ///<summary>Index means nothing in this case Maybe need to redesign the inheritance</summary>
        public override void traverse(int index){
          traverse();
        }

        //iterate does something and returns a node with will be activated next iteration
        public void traverse(){
          // Debug.Log("Traverse");
          CurrentNode = CurrentNode.m_self.iterate(); //Should never return null
          // Debug.Log(CurrentNode.m_self.size(0));
        }

        public string size(){
          return m_root.m_self.size(0);
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

          if(!existsChild(0)){
            return Root;
          } else {
            //Debug.Log(getChild(0).m_self.size(0));
            return getChild(0);
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

        public override string size(int depth){
          string x = depth + "-" + EBTNodeTypes.ACTION.ToString() + "(" + m_out_values[0] +") ";
          if(existsChild(0)) x += getChild(0).m_self.size(depth+1);
          return x;
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
          bool dec = m_tree.m_inputs[m_detector_index]() > m_threshold;
          int index = dec ? 1 : 0;        

          return !existsChild(index) ? Root : getChild(index).m_self;
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

        public override string size(int depth){
          string x = depth + "-" + EBTNodeTypes.DETECTOR.ToString() + "( " + m_threshold + ") ";

          if(existsChild(0)) x += getChild(0).m_self.size(depth+1);
          if(existsChild(1)) x += getChild(1).m_self.size(depth+1);
          
          return x;
        }
      }

      //T ATreeNode of type controller
      public abstract class ABTNode : ATreeNode<ABTNode> {
        
        //There are two types which need to be in same tree. This is a wrapper
        protected ABTNode(ATreeNode<ABTNode> p_parent) : base(p_parent) { }

        //iterate is how we traverse and make behaviour happen
        public abstract ATreeNode<ABTNode> iterate();

        public abstract string size(int depth);
      } 
    }

  }

}
