using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;
using Calc.Vector;


namespace GeneticBehaviourTrees
{


  public static class RandomGen
  {

    public static VTreeNode<IBehaviourGenoType> BehaviourDNARoot()
    {
      return genStepper(null, 100);
    }

    private static VTreeNode<IBehaviourGenoType> genStepper(VTreeNode<IBehaviourGenoType> p_parent, int p_chance)
    {

      /*switch (p_chance){
        case 100: Debug.Log(1); break;
        case 75: Debug.Log(2); break;
        case 50: Debug.Log(3); break;
        case 25: Debug.Log(4); break;
        case 0: Debug.Log(5); break;	
        }*/

      if (Random.Range(0, 100) < p_chance)
      {
        VTreeNode<IBehaviourGenoType> node = VTreeNodeGenoType(p_parent);

        for (int i = 0; i < node.numChildren(); i++)
        {
          node.addChild(genStepper(node, p_chance - 25), i);
        }

        return node;
      }

      return null;
    }

    private static VTreeNode<IBehaviourGenoType> VTreeNodeGenoType(VTreeNode<IBehaviourGenoType> p_parent)
    {
      if (Random.Range(0, 2) == 1)
      {
        return Detector(p_parent);
      }
      else
      {
        return ActionSequence(p_parent);
      }
    }

    private static VTreeNode<IBehaviourGenoType> Detector(VTreeNode<IBehaviourGenoType> p_parent)
    {

      switch (Random.Range(0, 4))
      {
        case 0:
          DirectionDetectorGenoType det0 = new DirectionDetectorGenoType(0, Vector2.zero, 0, 0, p_parent);
          det0.randomize();
          return det0;
        case 1:
          DirectionDetectorGenoType det1 = new DirectionDetectorGenoType(0, Vector2.zero, 0, 0, p_parent);
          det1.randomize();
          return det1;
        // PointingAtDetectorGenoType det1 = new PointingAtDetectorGenoType(0, 0, 0, p_parent);
        // det1.randomize();
        // return det1;
        case 2:
          PointingAtDetectorGenoType det2 = new PointingAtDetectorGenoType(0, 0, 0, p_parent);
          det2.randomize();
          return det2;
        case 3:
          ProximityDetectorGenoType det3 = new ProximityDetectorGenoType(0, 0, 0, p_parent);
          det3.randomize();
          return det3;
      }

      return null;
    }

    private static VTreeNode<IBehaviourGenoType> ActionSequence(VTreeNode<IBehaviourGenoType> p_parent)
    {
      ActionSequenceGeno sequence = new ActionSequenceGeno(p_parent);

      int count = 10;

      while (count > 0)
      {
        sequence.addAction(Action(sequence));
        count -= Random.Range(0, 10);
      }

      return sequence;
    }

    private static IActionGenoType Action(ActionSequenceGeno p_parent)
    {
      MoveActionGenoType geno = new MoveActionGenoType(0, 0, false, Direction());
      geno.randomize();
      return geno;
    }

    private static IDirectionGenoType Direction()
    {
      switch (Random.Range(0, 2))
      {
        case 0:
          RelativeDirectionGenoType to_create = new RelativeDirectionGenoType(0, 0, 0);
          to_create.randomize();
          return to_create;
        // return new AbsoluteDirectionGenoType(VectorCalc.randomDirection());
        case 1:
          return new AbsoluteDirectionGenoType(Vector2Calc.randomDirection());
      }

      return null;

    }

  }

}

