using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Genetic.Base;
using Genetic.Composite;
using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using JTools.Calc.ActiavationFunctions;
using JTools.Calc.Base;
using JTools.Calc.Lines;
using JTools.Calc.Vectors;
using JTools.Events;
using JTools.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

//T1 is concrete creature, T2 is the gamecontroller, T3 is the MindBodyDNA, T4 is the Concrete Mindbody, T5 is the AI datastructure
public abstract class ALineFollowingGameController<T1, T2, T3, T4, T5> : MonoBehaviour
  where T1 : ALineFollowingCreature<T1, T2, T3, T4, T5>
  where T2 : ALineFollowingGameController<T1, T2, T3, T4, T5>
  where T3 : ADNA<T3>, IControllerExpressable<T1, T4>, ICloneable<T3>
  where T4 : AMindBody<T5>, IBrain
  where T5 : IBrain
{

  public GameObject m_prefab;

  public DNABasedEvolutionManager<T3> m_evolution;

  IntervalEventManager m_interval;

  // Normal Line
  // Line2D m_goalLine = new Line2D(new Vector2(-7, -7), new Vector2(1, -0.5f));

  //Center Line
  Line2D m_goalLine = new Line2D(new Vector2(0, 0), new Vector2(1, 1));

	public Line2D GoalLine { get { return m_goalLine;} }

  public float m_time;

  [Range(1, 50)]
  public float m_time_scale;

  public int trial;


  protected abstract void setUpData();
  protected abstract void setUpAI();

  void Start()
  {
    Time.fixedDeltaTime = 0.04f;

    trial = DataCollector.Trials;

    setUpAI();
    
    setUpData();

    Task t1 = Task.Factory.StartNew( ()=> {
			m_evolution = createManager();			

			for (int i = 0; i < m_evolution.Size; i++)				
			{
				m_evolution.addRandom();
			}
		} );

    m_interval = new IntervalEventManager();

		m_interval.addListener(0.2f, () =>	
    {
      Task t2 = Task.Factory.StartNew( () =>  {
				m_goalLine.rotate(1f);
			});
    });


		t1.Wait();
    m_interval.addListener(lineFollowCONFIG.ControllerSpawnLatency, () =>
    {
      for (int i = 0; i < lineFollowCONFIG.ControllerCreatureSpawnAmount; i++)
      {
        spawn();
      }
    });
  }

  protected abstract DNABasedEvolutionManager<T3> createManager();

  // Update is called once per frame
  void FixedUpdate()
  {
    Time.timeScale = m_time_scale;
		float deltaTime = Time.fixedDeltaTime;
		
		m_time += deltaTime;
    m_interval.tick(deltaTime);
    m_evolution.tick(deltaTime);

		//Draw the line we're following
		Debug.DrawLine(m_goalLine.Point, m_goalLine.Point + m_goalLine.Direction * 100f, Color.green, deltaTime);
		Debug.DrawLine(m_goalLine.Point, m_goalLine.Point + m_goalLine.Direction * -100f, Color.green, deltaTime);

    if(m_time >= lineFollowCONFIG.timePerTrial){
      m_time = -10000;
      DataCollector.Trials = DataCollector.Trials+1;
      DataCollector.newRow();
      lineFollowCONFIG.nextAiKey();
      lineFollowCONFIG.loadScene();
    }
  }

  protected abstract T2 getSelf();

  void spawn()
  {
    GameObject obj = Instantiate(m_prefab, Vector3.zero, /*Quaternion.identity*/ Quaternion.Euler(0, 0, Random.Range(0, 360)));
    T1 cre = obj.GetComponent<T1>();
    cre.Initialize(m_evolution.birth(), getSelf());
  }





  private List<float> data = new List<float>();

  public void logDNA(T3 dna, float fitness)
  {
    m_evolution.addDNA(dna, fitness);
    
    
    data.Add(fitness);

    float sum =0;

    if(data.Count == 20){
      foreach(float x in data){
        sum += x;
      }

      DataCollector.recordData(""+ (sum/50f));
      data = new List<float>();
    }

    
  }
}



