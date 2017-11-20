using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using JTools.Interfaces;

using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using Genetic.Behaviour.DecisionNets;

namespace Genetic{

  namespace Composite{

    public struct MindBodyDN : IBrain
    {
      public MindBodyDN(Dictionary<string, float> p_body, DecisionNet p_mind){
        m_body = p_body;
        m_mind = p_mind;
      }

      public Dictionary<string, float> m_body;
      
      public DecisionNet m_mind;

      public void brainAction()
      {
        m_mind.brainAction();
      }
    }

    public class MindBodyDNDNA<T> : ADNA<MindBodyDNDNA<T>>, IControllerExpressable<T, MindBodyDN>, ICloneable<MindBodyDNDNA<T>> where T : AController
    {
      int m_species_id;

      TraitGenesDNA m_body;

      DecisionNetDNA<T> m_mind;

      public MindBodyDNDNA(int p_species_id, TraitGenesDNA p_body, DecisionNetDNA<T> p_mind){
        m_species_id = p_species_id;
        m_body = p_body.Clone();
        m_mind = p_mind.Clone();
      }

      public MindBodyDNDNA<T> Clone()
      {
        return new MindBodyDNDNA<T>(m_species_id, m_body.Clone(), m_mind.Clone());
      }

      public override MindBodyDNDNA<T> crossover(MindBodyDNDNA<T> p_crossover_object)
      {
        return new MindBodyDNDNA<T>(m_species_id, m_body.crossover(p_crossover_object.m_body), m_mind.crossover(p_crossover_object.m_mind));
      }

      public MindBodyDN express(T p_controller)
      {
        return new MindBodyDN(m_body.express(), m_mind.express(p_controller));
      }

      public override MindBodyDNDNA<T> getSelf()
      {
        return this;
      }

      public override MindBodyDNDNA<T> mutate()
      {
        return new MindBodyDNDNA<T>(m_species_id, m_body.mutate(), m_mind.mutate());
      }
    }

    public class MindBodySpeciesDN<T> : ISpecies<ADNA<MindBodyDNDNA<T>>> where T : AController
    {
      private int m_id;

      private TraitGenesSpecies m_body;

      private DecisionNetSpecies<T> m_mind;

      public MindBodySpeciesDN(int p_id, TraitGenesSpecies p_body, DecisionNetSpecies<T> p_mind){
        m_id = p_id;
        m_body = p_body;
        m_mind = p_mind;
      }
      
      public int ID { get { return m_id;} }

      public ADNA<MindBodyDNDNA<T>> randomInstance()
      {
        return new MindBodyDNDNA<T>(m_id, m_body.randomInstance().getSelf(), m_mind.randomInstance().getSelf());
      }
    }

  }

}