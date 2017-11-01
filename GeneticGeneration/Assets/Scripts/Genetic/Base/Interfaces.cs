namespace Genetic{

  namespace Base{

    ///<summary>Return mutated version of object</summary>
    public interface IMutatable<T>{
      T mutate();
    }

    ///<summary>Return object after applying crossover with input object</summary>
    public interface ICrossoverable<T>
    {
      T crossover(T p_crossover_object);
    }

    ///<summary>Transform object from DNA state to birthed useable state</summary>
    public interface IExpressable<T>{
      T express();
    }

    ///<summary>Requires a controller to be exressed T1 controller, T2 expression</summary>
    public interface IControllerExpressable<T1, T2>{
      T2 express(T1 p_controller);
    }
  }

}