using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using JTools.Interfaces;

using Genetic.Numerical.Base;
using Genetic.Numerical.TraitGenes;
using Genetic.Behaviour.DecisionNets;

namespace Genetic{

  namespace Composite{

    public struct MindBody : IBrain
    {
      public MindBody(Dictionary<ETrait, float> p_body, DecisionNet p_mind){
        m_body = p_body;
        m_mind = p_mind;
      }

      public Dictionary<ETrait, float> m_body;
      
      public DecisionNet m_mind;

      public void brainAction()
      {
        m_mind.brainAction();
      }
    }

    public class MindBodyDNA<T> : ADNA<MindBodyDNA<T>>, IControllerExpressable<T, MindBody>, ICloneable<MindBodyDNA<T>> where T : Controller
    {
      int m_species_id;

      TraitGenesDNA m_body;

      DecisionNetDNA<T> m_mind;

      public MindBodyDNA(int p_species_id, TraitGenesDNA p_body, DecisionNetDNA<T> p_mind){
        m_species_id = p_species_id;
        m_body = p_body.Clone();
        m_mind = p_mind.Clone();
      }

      public MindBodyDNA<T> Clone()
      {
        return new MindBodyDNA<T>(m_species_id, m_body.Clone(), m_mind.Clone());
      }

      public override MindBodyDNA<T> crossover(MindBodyDNA<T> p_crossover_object)
      {
        return new MindBodyDNA<T>(m_species_id, m_body.crossover(p_crossover_object.m_body), m_mind.crossover(p_crossover_object.m_mind));
      }

      public MindBody express(T p_controller)
      {
        return new MindBody(m_body.express(), m_mind.express(p_controller));
      }

      public override MindBodyDNA<T> getSelf()
      {
        return this;
      }

      public override MindBodyDNA<T> mutate()
      {
        return new MindBodyDNA<T>(m_species_id, m_body.mutate(), m_mind.mutate());
      }
    }

    public class MindBodySpecies<T> : ISpecies<ADNA<MindBodyDNA<T>>> where T : Controller
    {
      private int m_id;

      private TraitGenesSpecies m_body;

      private DecisionNetSpecies<T> m_mind;

      public MindBodySpecies(int p_id, TraitGenesSpecies p_body, DecisionNetSpecies<T> p_mind){
        m_id = p_id;
        m_body = p_body;
        m_mind = p_mind;
      }
      
      public int ID { get { return m_id;} }

      public ADNA<MindBodyDNA<T>> randomInstance()
      {
        return new MindBodyDNA<T>(m_id, m_body.randomInstance().getSelf(), m_mind.randomInstance().getSelf());
      }
    }

  }

}