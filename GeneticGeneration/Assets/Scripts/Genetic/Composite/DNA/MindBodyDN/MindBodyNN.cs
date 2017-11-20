using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using JTools.Interfaces;

using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using Genetic.Behaviour.NeuralNets;

namespace Genetic{

  namespace Composite{

    public struct MindBodyNN : IBrain
    {
      public MindBodyNN(Dictionary<string, float> p_body, NeuralNet p_mind){
        m_body = p_body;
        m_mind = p_mind;
      }

      public Dictionary<string, float> m_body;
      
      public NeuralNet m_mind;

      public void brainAction()
      {
        m_mind.brainAction();
      }
    }

    public class MindBodyNNDNA<T> : ADNA<MindBodyNNDNA<T>>, IControllerExpressable<T, MindBodyNN>, ICloneable<MindBodyNNDNA<T>> where T : AController
    {
      int m_species_id;

      TraitGenesDNA m_body;

      NeuralNetDNA<T> m_mind;

      public MindBodyNNDNA(int p_species_id, TraitGenesDNA p_body, NeuralNetDNA<T> p_mind){
        m_species_id = p_species_id;
        m_body = p_body.Clone();
        m_mind = p_mind.Clone();
      }

      public MindBodyNNDNA<T> Clone()
      {
        return new MindBodyNNDNA<T>(m_species_id, m_body.Clone(), m_mind.Clone());
      }

      public override MindBodyNNDNA<T> crossover(MindBodyNNDNA<T> p_crossover_object)
      {
        return new MindBodyNNDNA<T>(m_species_id, m_body.crossover(p_crossover_object.m_body), m_mind.crossover(p_crossover_object.m_mind));
      }

      public MindBodyNN express(T p_controller)
      {
        return new MindBodyNN(m_body.express(), m_mind.express(p_controller));
      }

      public override MindBodyNNDNA<T> getSelf()
      {
        return this;
      }

      public override MindBodyNNDNA<T> mutate()
      {
        return new MindBodyNNDNA<T>(m_species_id, m_body.mutate(), m_mind.mutate());
      }
    }

    public class MindBodyNNSpecies<T> : ISpecies<ADNA<MindBodyNNDNA<T>>> where T : AController
    {
      private int m_id;

      private TraitGenesSpecies m_body;

      private NeuralNetSpecies<T> m_mind;

      public MindBodyNNSpecies(int p_id, TraitGenesSpecies p_body, NeuralNetSpecies<T> p_mind){
        m_id = p_id;
        m_body = p_body;
        m_mind = p_mind;
      }
      
      public int ID { get { return m_id;} }

      public ADNA<MindBodyNNDNA<T>> randomInstance()
      {
        return new MindBodyNNDNA<T>(m_id, m_body.randomInstance().getSelf(), m_mind.randomInstance().getSelf());
      }
    }

  }

}