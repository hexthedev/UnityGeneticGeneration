using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.DataStructures.BinaryTrees;
using Genetic.Base;
using JTools.Calc.Base;
using JTools.Calc.Array;
using JTools.Calc.Rand;
using JTools.Calc.Bool;
using JTools.Interfaces;


namespace Genetic
{
  namespace Behaviour
  {
    namespace DecisionTrees
    {

      public class DecisionTreeWrapper : IBrain
      {
        DecisionTree m_tree;

        public DecisionTreeWrapper(DecisionTree p_tree)
        {
          m_tree = p_tree;
        }

        public void brainAction()
        {
          m_tree.decisionIteration();
        }


        public override string ToString()
        {
          return "TODO";
        }
      }


      public class DecisionTreeDNAWrapper<T> : ADNA<DecisionTreeDNAWrapper<T>>, IControllerExpressable<T, DecisionTreeWrapper>, ICloneable<DecisionTreeDNAWrapper<T>> where T : AController
      {
        int m_id;
        DecisionTreeDNA<T> m_tree;


        public DecisionTreeDNAWrapper(int p_id, DecisionTreeDNA<T> p_tree)
        {
          m_id = p_id;
          m_tree = p_tree;
        }

        public DecisionTreeDNAWrapper<T> Clone()
        {
          return new DecisionTreeDNAWrapper<T>(m_id, m_tree.clone());
        }

        public override DecisionTreeDNAWrapper<T> crossover(DecisionTreeDNAWrapper<T> p_crossover_object)
        {
          DecisionTreeDNA<T> crossovered = m_tree.crossover(p_crossover_object.m_tree);
          return new DecisionTreeDNAWrapper<T>(m_id, crossovered);
        }

        public override DecisionTreeDNAWrapper<T> getSelf()
        {
          return this;
        }

        public override DecisionTreeDNAWrapper<T> mutate()
        {
          DecisionTreeDNA<T> mutated = m_tree.mutate();
          return new DecisionTreeDNAWrapper<T>(m_id, mutated);
        }

        public DecisionTreeWrapper express(T p_controller)
        {
          DecisionTree expressed = m_tree.express(p_controller);
          return new DecisionTreeWrapper(expressed);
        }

        public override string ToString()
        {
          return "ToDo";
        }

      }


      public class DecisionTreeSpecies<T> : ISpecies<ADNA<DecisionTreeDNAWrapper<T>>> where T : AController
      {
        private int m_id;
        private DInputFactory<T>[] m_inputs;
        private DOutputFactory<T>[] m_outputs;

        private Range<float> m_output_range;
        private Range<float> m_threshold_range;

        private Range<float> m_mutation_multiplier;

        public DecisionTreeSpecies(int p_id, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, Range<float> p_mutation_multiplier, Range<float> p_output_range, Range<float> p_threshold_range)
        {
          m_id = p_id;
          m_inputs = p_inputs;
          m_outputs = p_outputs;
          m_mutation_multiplier = p_mutation_multiplier;
          m_output_range = p_output_range;
          m_threshold_range = p_threshold_range;
        }


        public int ID { get { return m_id; } }

        public override string ToString()
        {
          return "TODO";
        }

        public ADNA<DecisionTreeDNAWrapper<T>> randomInstance()
        {
          return new DecisionTreeDNAWrapper<T>(
            m_id,
            new DecisionTreeDNA<T>(
              new DecisionTreeDNANode(m_inputs.Length, m_threshold_range, m_outputs.Length, m_output_range),
              m_inputs,
              m_outputs,
              m_mutation_multiplier,
              m_threshold_range,
              m_output_range,
              5
            )
          );
        }
      }



      //DNA TREE
      public class DecisionTreeDNA<T> : BinaryTree<DecisionTreeDNANode> where T : AController
      {

        DInputFactory<T>[] m_inputs;
        DOutputFactory<T>[] m_outputs;
        Range<float> m_mutation_mult;

        //Assumes Tree is constructed
        public DecisionTreeDNA(DecisionTreeDNANode p_root, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, Range<float> p_mutation_mult) : base(p_root)
        {
          m_inputs = p_inputs;
          m_outputs = p_outputs;
          m_mutation_mult = p_mutation_mult;
        }

        //Assumes want random tree construction
        public DecisionTreeDNA(DecisionTreeDNANode p_root, DInputFactory<T>[] p_inputs, DOutputFactory<T>[] p_outputs, Range<float> p_mutation_mult, Range<float> p_threshold_range, Range<float> p_outputs_range, int p_max_depth) : base(p_root)
        {
          m_inputs = p_inputs;
          m_outputs = p_outputs;
          m_mutation_mult = p_mutation_mult;

          recRandom(Root, 0, p_max_depth, p_threshold_range, p_outputs_range);
        }

        private void recRandom(DecisionTreeDNANode p_current_node, int p_current_depth, int p_max_depth, Range<float> p_threshold_range, Range<float> p_outputs_range)
        {

          if (p_current_depth == p_max_depth) return;

          if (BoolCalc.random())
          {
            p_current_node.addChild(BinaryDirection.LEFT, new DecisionTreeDNANode(m_inputs.Length, p_threshold_range, m_outputs.Length, p_outputs_range), p_current_node);
            recRandom(p_current_node.LeftChild, p_current_depth + 1, p_max_depth, p_threshold_range, p_outputs_range);
          }

          if (BoolCalc.random())
          {
            p_current_node.addChild(BinaryDirection.RIGHT, new DecisionTreeDNANode(m_inputs.Length, p_threshold_range, m_outputs.Length, p_outputs_range), p_current_node);
            recRandom(p_current_node.RightChild, p_current_depth + 1, p_max_depth, p_threshold_range, p_outputs_range);
          }
        }

        //Clone
        public DecisionTreeDNA<T> clone()
        {
          return new DecisionTreeDNA<T>(recClone(Root), ArrayCalc.shallowClone(m_inputs), ArrayCalc.shallowClone(m_outputs), m_mutation_mult);
        }

        private DecisionTreeDNANode recClone(DecisionTreeDNANode p_node)
        {

          DecisionTreeDNANode clone = p_node.clone();
          if (p_node.hasLeft) clone.addChild(BinaryDirection.LEFT, recClone(p_node.LeftChild), clone);
          if (p_node.hasRight) clone.addChild(BinaryDirection.RIGHT, recClone(p_node.RightChild), clone);

          return clone;
        }


        //Crossover
        public DecisionTreeDNA<T> crossover(DecisionTreeDNA<T> p_crossover_object)
        {
          return new DecisionTreeDNA<T>(recCrossover(Root, p_crossover_object.Root), ArrayCalc.shallowClone(m_inputs), ArrayCalc.shallowClone(m_outputs), m_mutation_mult);
        }

        private DecisionTreeDNANode recCrossover(DecisionTreeDNANode p_node1, DecisionTreeDNANode p_node2)
        {

          DecisionTreeDNANode crossedover = p_node1.crossover(p_node2);

          if (p_node1.hasLeft && p_node2.hasLeft)
          {
            crossedover.addChild(BinaryDirection.LEFT, recCrossover(p_node1.LeftChild, p_node2.LeftChild), crossedover);
          }
          else if (!p_node1.hasLeft && !p_node2.hasLeft)
          {

          }
          else
          {
            DecisionTreeDNANode only_existing = p_node1.hasLeft ? p_node1.LeftChild : p_node2.LeftChild;
            if (BoolCalc.random()) crossedover.addChild(BinaryDirection.LEFT, recClone(only_existing), crossedover);
          }

          if (p_node1.hasRight && p_node2.hasRight)
          {
            crossedover.addChild(BinaryDirection.RIGHT, recCrossover(p_node1.RightChild, p_node2.RightChild), crossedover);
          }
          else if (!p_node1.hasRight && !p_node2.hasRight)
          {

          }
          else
          {
            DecisionTreeDNANode only_existing = p_node1.hasRight ? p_node1.RightChild : p_node2.RightChild;
            if (BoolCalc.random()) crossedover.addChild(BinaryDirection.RIGHT, recClone(only_existing), crossedover);
          }

          return crossedover;
        }

        //Mutation
        public DecisionTreeDNA<T> mutate()
        {
          return new DecisionTreeDNA<T>(recMutate(Root), ArrayCalc.shallowClone(m_inputs), ArrayCalc.shallowClone(m_outputs), m_mutation_mult);
        }

        private DecisionTreeDNANode recMutate(DecisionTreeDNANode p_mutate)
        {
          DecisionTreeDNANode mutated = p_mutate.mutate(m_inputs.Length, m_mutation_mult);

          if (p_mutate.hasLeft) mutated.addChild(BinaryDirection.LEFT, recMutate(p_mutate.LeftChild), mutated);
          if (p_mutate.hasRight) mutated.addChild(BinaryDirection.RIGHT, recMutate(p_mutate.RightChild), mutated);

          return mutated;
        }


        //Express
        public DecisionTree express(T controller)
        {
          return new DecisionTree(
            recExpress(Root),
            ArrayCalc.map(m_inputs, (DInputFactory<T> p_fact) => { return p_fact(controller); }),
            ArrayCalc.map(m_outputs, (DOutputFactory<T> p_fact) => { return p_fact(controller); })
          );
        }

        private DecisionTreeNode recExpress(DecisionTreeDNANode p_express)
        {
          DecisionTreeNode node = p_express.express();

          if (p_express.hasLeft) node.addChild(BinaryDirection.LEFT, recExpress(p_express.LeftChild), node);
          if (p_express.hasRight) node.addChild(BinaryDirection.RIGHT, recExpress(p_express.RightChild), node);

          return node;
        }

      }



      public class DecisionTreeDNANode : BinaryNode<DecisionTreeDNANode>
      {

        //Members
        private int m_input_index;
        private float m_min_input;
        private float m_max_input;
        private float[] m_output_values;

        //Constructor
        public DecisionTreeDNANode(int p_input_index, float p_min_input, float p_max_input, float[] p_output_values)
        {
          m_input_index = p_input_index;
          m_min_input = p_min_input;
          m_max_input = p_max_input > p_min_input ? p_max_input : p_min_input;
          m_output_values = ArrayCalc.shallowClone(p_output_values);
        }

        public DecisionTreeDNANode(int p_num_inputs, Range<float> p_threshold_range, int p_num_outputs, Range<float> p_outputs_range)
        {
          m_input_index = RandomCalc.Rand(new Range<int>(0, p_num_inputs-1));
          m_min_input = RandomCalc.Rand(p_threshold_range);
          m_max_input = RandomCalc.Rand(new Range<float>(m_min_input, p_threshold_range.Max));
          m_output_values = ArrayCalc.functionInitialize(p_num_outputs, () => { return RandomCalc.Rand(p_outputs_range); });
        }

        public DecisionTreeDNANode crossover(DecisionTreeDNANode p_crossover_object)
        {

          int cross_input_index = BoolCalc.random() ? m_input_index : p_crossover_object.m_input_index;

          float cross_min_input = BoolCalc.random() ? m_min_input : p_crossover_object.m_min_input;
          float cross_max_input = BoolCalc.random() ? m_max_input : p_crossover_object.m_max_input;
          if (cross_max_input < cross_min_input) cross_max_input = cross_min_input;

          float[] cross_output_values = ArrayCalc.crossover(m_output_values, p_crossover_object.m_output_values);

          return new DecisionTreeDNANode(cross_input_index, cross_min_input, cross_max_input, cross_output_values);
        }

        public DecisionTreeDNANode mutate(int p_num_inputs, Range<float> p_mutation_mult)
        {
          int mut_input_index = RandomCalc.Rand(new Range<int>(0, p_num_inputs-1));

          float mut_min_input = m_min_input * RandomCalc.Rand(p_mutation_mult);
          float mut_max_input = m_max_input * RandomCalc.Rand(p_mutation_mult);
          if (mut_max_input < mut_min_input) mut_max_input = mut_min_input;

          float[] mut_output_values = ArrayCalc.shallowClone(m_output_values);
          for (int i = 0; i < mut_output_values.Length; i++)
          {
            mut_output_values[i] *= RandomCalc.Rand(p_mutation_mult);
          }

          return new DecisionTreeDNANode(mut_input_index, mut_min_input, mut_max_input, mut_output_values);
        }

        //Cloning does not clone parents and children. Dosen't make sense to
        public DecisionTreeDNANode clone()
        {
          return new DecisionTreeDNANode(m_input_index, m_min_input, m_max_input, m_output_values);
        }

        public DecisionTreeNode express()
        {
          return new DecisionTreeNode(m_input_index, m_min_input, m_max_input, m_output_values);
        }

      }






      //CONCRETE TREE
      public class DecisionTree : BinaryTree<DecisionTreeNode>
      {

        DInput[] m_inputs;
        DOutput[] m_outputs;

        public DecisionTree(DecisionTreeNode p_root, DInput[] p_inputs, DOutput[] p_outputs) : base(p_root)
        {
          m_inputs = p_inputs;
          m_outputs = p_outputs;
        }

        public override bool moveDown(BinaryDirection p_direction)
        {
          if (!base.moveDown(p_direction)) m_current_node = Root;
          return true;
        }

        public void decisionIteration()
        {
          int decision = CurrentNode.testDecision(m_inputs);

          if (decision < 0)
          {
            moveDown(BinaryDirection.LEFT);
          }
          else if (decision > 0)
          {
            moveDown(BinaryDirection.RIGHT);
          }

          CurrentNode.makeDecision(m_outputs);
        }

      }

      public class DecisionTreeNode : BinaryNode<DecisionTreeNode>
      {

        //Members
        private int m_input_index;
        private float m_min_input;
        private float m_max_input;
        private float[] m_output_values;

        //Constructor
        public DecisionTreeNode(int p_input_index, float p_min_input, float p_max_input, float[] p_output_values)
        {
          m_input_index = p_input_index;
          m_min_input = p_min_input;
          m_max_input = p_max_input > p_min_input ? p_max_input : p_min_input;
          m_output_values = ArrayCalc.shallowClone(p_output_values);
        }

        public int testDecision(DInput[] p_inputs)
        {

          float value = p_inputs[m_input_index]();

          if (value < m_min_input)
          {
            return -1;
          }
          else if (value > m_max_input)
          {
            return 1;
          }
          else
          {
            return 0;
          }
        }

        public void makeDecision(DOutput[] p_outputs)
        {
          if (p_outputs.Length != m_output_values.Length) Debug.LogError("Must have same number of outputs as output values");

          for (int i = 0; i < m_output_values.Length; i++)
          {
            p_outputs[i](m_output_values[i]);
          }
        }


      }


    }
  }
}
