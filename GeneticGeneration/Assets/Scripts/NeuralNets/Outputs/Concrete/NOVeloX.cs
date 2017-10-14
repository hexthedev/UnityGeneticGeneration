using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NOVeloX : INeuralOutput {

    EnemyControllerNeural m_controller;

    public NOVeloX(EnemyControllerNeural p_controller){
        m_controller = p_controller;
    }

    public void output(float p_value)
    {
        Vector2 vec = new Vector2(p_value, 0);
        m_controller.changeVelocity(vec);
    }

    ENeuralOutput INeuralOutput.dnaify()
    {
        return ENeuralOutput.NOVeloX;
    }
}
