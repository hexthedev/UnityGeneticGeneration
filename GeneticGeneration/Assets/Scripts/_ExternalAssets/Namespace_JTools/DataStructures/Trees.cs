using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Interfaces;

namespace JTools
{

  namespace DataStructures
  {

    namespace Trees
    {

      public class Tree<T> where T:ATreeNode<T>, ICloneable<T>{

        protected ATreeNode<T> m_root;
        protected ATreeNode<T> m_current_node;

        public Tree(ATreeNode<T> p_root){
          m_root = p_root;
          m_current_node = p_root;
        }

        public int CurrentChildren { get { return m_current_node.numChildren;} }

        public ATreeNode<T> CurrentNode { get { return m_current_node; } set { m_current_node = value; } }

        public virtual void traverse( int p_index ){
          if(p_index >= CurrentChildren ) Debug.LogError("Tree Traversal index does not exist");
          
          ATreeNode<T> child = m_current_node.getChild(p_index);

          if(child == null){
            m_current_node = m_root;
          } else {
            m_current_node = child;
          }
        }

      }


      public abstract class ATreeNode<T> : ICloneable<T> where T:ATreeNode<T>, ICloneable<T>{

        protected ATreeNode<T> m_parent;
        public T m_self;
        protected List<ATreeNode<T>> m_children;


        public ATreeNode( ATreeNode<T> p_parent ){
          m_parent = p_parent;
          m_self = setSelf();
          m_children = new List<ATreeNode<T>>();
        }

        ///<summary>Used to get the concrrete class. Should just return this<summary>
        protected abstract T setSelf();

        public T Root{
          get { 
            if(m_parent == null) { return m_self; }
            else { return m_parent.Root; }  
            }
        }

        public bool existsChild(int p_index)
        {
          if( p_index >= m_children.Count || p_index < 0) return false;
          return m_children[p_index] != null;
        }

        public int numChildren{ get { return m_children.Count; } }

        public abstract void addChild(T p_child);
        
        public abstract void addChild(T p_child, int p_index);

        public ATreeNode<T> getChild(int p_index){
          if(p_index >= numChildren ) Debug.LogError("Tree Node getChild index does not exist");

          return m_children[p_index];
        }

        public virtual T Clone()
        {
          return m_self.Clone();
        }
      }


    }

  }

}