using System.Collections;
using System.Collections.Generic;

namespace JTools
{

  namespace DataStructures
  {

    namespace BinaryTrees
    {
      public class BinaryTree<T> where T : BinaryNode<T>
      {

        //Members
        protected T m_root_node;
        protected T m_current_node;

        //Properties
        public T Root { get { return m_root_node; } }
        public T CurrentNode { get { return m_current_node; } }

        //Constructor
        public BinaryTree(T p_root){
          m_root_node = p_root;
          m_current_node = p_root;
        }

        //False if root
        public virtual bool moveUp()
        {
          if (m_current_node.isRoot) return false;
          m_current_node = m_current_node.Parent;
          return true;
        }

        public virtual bool moveDown(BinaryDirection p_direction)
        {
          switch (p_direction)
          {
            case BinaryDirection.LEFT:
              if(!m_current_node.hasLeft) return false;
							m_current_node = m_current_node.LeftChild;
							return true;
            case BinaryDirection.RIGHT:
              if(!m_current_node.hasRight) return false;
							m_current_node = m_current_node.RightChild;
							return true;
          }

					return false;
        }
      }

      public class BinaryNode<T> where T : BinaryNode<T>
      {

        //Variables
        protected T m_parent;
        protected T m_left_child;
        protected T m_right_child;


        //Properties
        public T Parent { get { return m_parent; } }
        public T LeftChild { get { return m_left_child; } }
        public T RightChild { get { return m_right_child; } }
        public bool isRoot { get { return m_parent == null; } }
        public bool hasLeft { get { return m_left_child != null; } }
        public bool hasRight { get { return m_right_child != null; } }

        //Constructors
        public BinaryNode()
        {
          m_parent = null;
          m_left_child = null;
          m_right_child = null;
        }

        public BinaryNode(T p_parent)
        {
          m_parent = p_parent;
          m_left_child = null;
          m_right_child = null;
        }

        //Manipulation Functions
        public virtual void addParent(T p_node)
        {
          m_parent = p_node;
        }

        public virtual void removeParent()
        {
          m_parent = null;
        }

        public virtual void addChild(BinaryDirection p_child, T p_node, T p_parent)
        {
          switch (p_child)
          {
            case BinaryDirection.LEFT:
              m_left_child = p_node;
              m_left_child.addParent(p_parent);
              break;
            case BinaryDirection.RIGHT:
              m_right_child = p_node;
              m_right_child.addParent(p_parent);
              break;
          }
        }

        public virtual void removeChild(BinaryDirection p_child)
        {
          switch (p_child)
          {
            case BinaryDirection.LEFT:
              m_left_child = null;
              break;
            case BinaryDirection.RIGHT:
              m_right_child = null;
              break;
          }
        }

      }

      public enum BinaryDirection { LEFT, RIGHT };

    }
  }
}