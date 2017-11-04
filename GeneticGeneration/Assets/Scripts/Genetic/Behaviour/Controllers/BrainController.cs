using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;

namespace Genetic{

namespace Behaviour{

namespace Controllers{

      public class BrainControllerFactory : ControllerFactory<BrainController>
      {
        public override DInputFactory<BrainController>[] getInputs()
        {
          return new DInputFactory<BrainController>[] {half, dub};
        }

        public override DOutputFactory<BrainController>[] getOutputs()
        {
          return new DOutputFactory<BrainController>[] {oot, oo2t};
        }


				//INPUTS
				DInputFactory<BrainController> half = ( BrainController p_controller ) => {
					return () => {
						GameObject ob = p_controller.gameObject;
						return 1f;
					};
				};

				DInputFactory<BrainController> dub = ( BrainController p_controller ) => {
					return () => {
						GameObject ob = p_controller.gameObject;
						return -1f;
					};
				};

				DOutputFactory<BrainController> oot = ( BrainController p_controller ) => {
					return (float p_value) => {
						p_controller.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, p_value/40);
					};
				};

				DOutputFactory<BrainController> oo2t = ( BrainController p_controller ) => {
					return (float p_value) => {
						p_controller.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(p_value/40, 0);
					};
				};
				

      }

      public class BrainController : Controller, IBrainInit
      {
        bool m_brainReady = false;
				IBrain m_brain;

        public void InitializeBrain(IBrain p_brain)
        {
          m_brain = p_brain;
					m_brainReady = true;
        }

        protected override void act()
        {
          if(m_brainReady){
						m_brain.brainAction();
					}
        }
      }

			public interface IBrainInit{
				void InitializeBrain(IBrain p_brain);
			}


    }

}

}


