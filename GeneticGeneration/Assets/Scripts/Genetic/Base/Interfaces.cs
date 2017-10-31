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
    public interface IBirthable<T>{
      T birth();
    }

  }

}