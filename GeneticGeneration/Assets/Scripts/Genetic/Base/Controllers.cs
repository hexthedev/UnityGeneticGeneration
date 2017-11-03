using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Events;

namespace Genetic{

  namespace Base{

    ///<summary>Controller Factories hold all possible inputs and outputs for a certain kind of controller without requiring an instance.!-- T is Controller</summary>
    public abstract class ControllerFactory<T> where T:Controller{

      public abstract DInputFactory<T>[] getInputs();

      public abstract DOutputFactory<T>[] getOutputs();
    
    }

    ///<summary>Instance of a Controller. Consists of the Act action, a cooldown Timeout manager and a Fixed update call to act (ALWAYS CALL BASE FOR START AND UPDATE OVERRIDES)</summary>
    public abstract class Controller: MonoBehaviour{
      TimeoutEventManager m_cooldowns;

      protected virtual void Start(){
        m_cooldowns = new TimeoutEventManager();
      }

      protected virtual void FixedUpdate(){
        m_cooldowns.tick(Time.fixedDeltaTime);
        act();
      }

      protected void logCooldown(float p_time, DTimeoutListener p_listener){
        m_cooldowns.addTimeout(p_time, p_listener);
      }

      protected abstract void act();

    }

    public delegate float DInput();

    public delegate DInput DInputFactory<T>(T p_controller) where T:Controller;

    public delegate void DOutput(float p_value);

    public delegate DOutput DOutputFactory<T>(T p_controller)  where T:Controller;

  }

}
