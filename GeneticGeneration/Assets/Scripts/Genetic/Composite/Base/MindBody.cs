using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using JTools.Interfaces;

using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;

namespace Genetic{

  namespace Base{
    public abstract class AMindBody<T>{

      public Dictionary<string, float> m_body;
      public T m_mind;
    }

  }

}