using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NOVeloY : INeuralOutput {

    EnemyControllerNeural m_controller;

    public NOVeloY(EnemyControllerNeural p_controller){
        m_controller = p_controller;
    }

    public void output(float p_value)
    {
        NumberTester.log(p_value);

        Vector2 vec = new Vector2(0, p_value);
        m_controller.changeVelocity(vec);
    }

    ENeuralOutput INeuralOutput.dnaify()
    {
        return ENeuralOutput.NOVeloY;
    }
}