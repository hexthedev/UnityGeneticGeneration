using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using JTools.Interfaces;

using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using Genetic.Behaviour.BehaviourTrees;

namespace Genetic{

  namespace Composite{

    public class MindBodyBT : AMindBody<BehaviourTree>, IBrain
    {

      public MindBodyBT(Dictionary<string, float> p_body, BehaviourTree p_mind){
        m_body = p_body;
        m_mind = p_mind;
      }

      public void brainAction()
      {
        m_mind.brainAction();
      }
    }

    public class MindBodyBTDNA<T> : ADNA<MindBodyBTDNA<T>>, IControllerExpressable<T, MindBodyBT>, ICloneable<MindBodyBTDNA<T>> where T : AController
    {
      int m_species_id;

      TraitGenesDNA m_body;

      BehaviourTreeDNA<T> m_mind;

      public MindBodyBTDNA(int p_species_id, TraitGenesDNA p_body, BehaviourTreeDNA<T> p_mind){
        m_species_id = p_species_id;
        m_body = p_body.Clone();
        m_mind = p_mind.Clone();
      }

      public MindBodyBTDNA<T> Clone()
      {
        return new MindBodyBTDNA<T>(m_species_id, m_body.Clone(), m_mind.Clone());
      }

      public override MindBodyBTDNA<T> crossover(MindBodyBTDNA<T> p_crossover_object)
      {
        return new MindBodyBTDNA<T>(m_species_id, m_body.crossover(p_crossover_object.m_body), m_mind.crossover(p_crossover_object.m_mind));
      }

      public MindBodyBT express(T p_controller)
      {
        return new MindBodyBT(m_body.express(), m_mind.express(p_controller));
      }

      public override MindBodyBTDNA<T> getSelf()
      {
        return this;
      }

      public override MindBodyBTDNA<T> mutate()
      {
        return new MindBodyBTDNA<T>(m_species_id, m_body.mutate(), m_mind.mutate());
      }
    }

    public class MindBodyBTSpecies<T> : ISpecies<ADNA<MindBodyBTDNA<T>>> where T : AController
    {
      private int m_id;

      private TraitGenesSpecies m_body;

      private BehaviourTreeSpecies<T> m_mind;

      public MindBodyBTSpecies(int p_id, TraitGenesSpecies p_body, BehaviourTreeSpecies<T> p_mind){
        m_id = p_id;
        m_body = p_body;
        m_mind = p_mind;
      }
      
      public int ID { get { return m_id;} }

      public ADNA<MindBodyBTDNA<T>> randomInstance()
      {
        return new MindBodyBTDNA<T>(m_id, m_body.randomInstance().getSelf(), m_mind.randomInstance().getSelf());
      }
    }

  }

}