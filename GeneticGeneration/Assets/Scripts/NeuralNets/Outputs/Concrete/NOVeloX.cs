using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NOVeloX : INeuralOutput {

    CreatureController m_controller;

    public NOVeloX(CreatureController p_controller){
        m_controller = p_controller;
    }

    public void output(float p_value)
    {
        NumberTester.log(p_value);
        
        if(p_value != p_value) p_value = 0;

        Vector2 vec = new Vector2(p_value, 0);
        m_controller.moveForce(vec);
    }

    ENeuralOutput INeuralOutput.dnaify()
    {
        return ENeuralOutput.NOVeloX;
    }
}
