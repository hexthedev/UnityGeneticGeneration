using Genetic.Base;
using JTools.Interfaces;

namespace Genetic{

  namespace Composite{

    public struct MindBody : IBrain
    {
      public void brainAction()
      {
        throw new System.NotImplementedException();
      }
    }

    public class MindBodyDNA<T> : IDNA<MindBodyDNA<T>>, IControllerExpressable<T, MindBody>, ICloneable<MindBodyDNA<T>> where T : Controller
    {
      public MindBodyDNA<T> Clone()
      {
        throw new System.NotImplementedException();
      }

      public override MindBodyDNA<T> crossover(MindBodyDNA<T> p_crossover_object)
      {
        throw new System.NotImplementedException();
      }

      public MindBody express(T p_controller)
      {
        throw new System.NotImplementedException();
      }

      public override MindBodyDNA<T> getSelf()
      {
        throw new System.NotImplementedException();
      }

      public override MindBodyDNA<T> mutate()
      {
        throw new System.NotImplementedException();
      }
    }

    public class MindBodySpecies<T> : ISpecies<IDNA<MindBodyDNA<T>>> where T : Controller
    {
      public int ID => throw new System.NotImplementedException();

      public IDNA<MindBodyDNA<T>> randomInstance()
      {
        throw new System.NotImplementedException();
      }
    }


  }

}