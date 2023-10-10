using System.Collections;
using System.Collections.Generic;
using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SartenBehavior : GOAction
{
    [InParam("controller")]
    public SartenController sartenController;


    /// <summary>Update method of DoneShoot.</summary>
    /// <returns>Return Running task.</returns>
    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if (sartenController.bothInside)
        {
            return TaskStatus.RUNNING;
        }
        
        base.OnUpdate();
        return TaskStatus.COMPLETED;

    } // OnUpdate
}
