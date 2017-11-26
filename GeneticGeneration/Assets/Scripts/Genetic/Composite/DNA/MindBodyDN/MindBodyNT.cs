using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using JTools.Interfaces;

using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using Genetic.Behaviour.DecisionTrees;

namespace Genetic{

  namespace Composite{

    public class MindBodyDT : AMindBody<DecisionTreeWrapper>, IBrain
    {
      public MindBodyDT(Dictionary<string, float> p_body, DecisionTreeWrapper p_mind){
        m_body = p_body;
        m_mind = p_mind;
      }

      public void brainAction()
      {
        m_mind.brainAction();
      }
    }

    public class MindBodyDTDNA<T> : ADNA<MindBodyDTDNA<T>>, IControllerExpressable<T, MindBodyDT>, ICloneable<MindBodyDTDNA<T>> where T : AController
    {
      int m_species_id;

      TraitGenesDNA m_body;

      DecisionTreeDNAWrapper<T> m_mind;

      public MindBodyDTDNA(int p_species_id, TraitGenesDNA p_body, DecisionTreeDNAWrapper<T> p_mind){
        m_species_id = p_species_id;
        m_body = p_body.Clone();
        m_mind = p_mind.Clone();
      }

      public MindBodyDTDNA<T> Clone()
      {
        return new MindBodyDTDNA<T>(m_species_id, m_body.Clone(), m_mind.Clone());
      }

      public override MindBodyDTDNA<T> crossover(MindBodyDTDNA<T> p_crossover_object)
      {
        return new MindBodyDTDNA<T>(m_species_id, m_body.crossover(p_crossover_object.m_body), m_mind.crossover(p_crossover_object.m_mind));
      }

      public MindBodyDT express(T p_controller)
      {
        return new MindBodyDT(m_body.express(), m_mind.express(p_controller));
      }

      public override MindBodyDTDNA<T> getSelf()
      {
        return this;
      }

      public override MindBodyDTDNA<T> mutate()
      {
        return new MindBodyDTDNA<T>(m_species_id, m_body.mutate(), m_mind.mutate());
      }
    }

    public class MindBodySpeciesDT<T> : ISpecies<ADNA<MindBodyDTDNA<T>>> where T : AController
    {
      private int m_id;

      private TraitGenesSpecies m_body;

      private DecisionTreeSpecies<T> m_mind;

      public MindBodySpeciesDT(int p_id, TraitGenesSpecies p_body, DecisionTreeSpecies<T> p_mind){
        m_id = p_id;
        m_body = p_body;
        m_mind = p_mind;
      }
      
      public int ID { get { return m_id;} }

      public ADNA<MindBodyDTDNA<T>> randomInstance()
      {
        return new MindBodyDTDNA<T>(m_id, m_body.randomInstance().getSelf(), m_mind.randomInstance().getSelf());
      }
    }

  }

}