using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsPlayerGenoType : IDirectionGenoType
{
  public void mutate()
  {
    return;
  }

  public IDirection phenotype(VSequenceAction p_parent)
  {
    return new TowardsPlayerDirection(p_parent);
  }

  public void randomize()
  {
    return;
  }
}
