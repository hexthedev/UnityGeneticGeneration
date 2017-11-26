using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Events;

namespace Genetic{

  namespace Base{

    ///<summary>Controller Factories hold all possible inputs and outputs for a certain kind of controller without requiring an instance. T is the Controller type. The child needs to choose the inputs it wants to expose in getInputs</summary>
    public abstract class AControllerFactory<T> where T:AController{

      ///<summary>Implement by returning inputs from the protected input functions in this parent class</summary>
      public abstract DInputFactory<T>[] getInputs();

      ///<summary>Implement by returning output from the protected output functions in this parent class</summary>
      public abstract DOutputFactory<T>[] getOutputs();
    }

    ///<summary>Instance of a Controller. Consists of the Act action, a cooldown Timeout manager and a Fixed update call to act (ALWAYS CALL BASE FOR START AND UPDATE OVERRIDES)</summary>
    public abstract class AController: MonoBehaviour{
      protected abstract void setBehaviours();

    }

    public delegate float DInput();

    public delegate DInput DInputFactory<T>(T p_controller) where T:AController;

    public delegate void DOutput(float p_value);

    public delegate DOutput DOutputFactory<T>(T p_controller)  where T:AController;

  }

}
