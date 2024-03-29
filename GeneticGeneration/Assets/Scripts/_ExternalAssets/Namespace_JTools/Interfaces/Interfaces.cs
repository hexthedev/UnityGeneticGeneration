
namespace JTools{

  namespace Interfaces{

		///<summary>Return object as it's specific type.!-- Useful for cing objects own type maps to a generic type</summary>
		public interface ISelf<T>{
			///<summary>Should always implement as return this;</summary>
			T getSelf();
		}


		public interface ICloneable<T>{
			T Clone();
		}

		public interface IDamagable {

			void damage(float p_damage);

		}

		public interface IResource {

			float collect(float p_collect_power);

		}

  }

}