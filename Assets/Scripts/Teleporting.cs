using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Teleporting
{
    public static void TeleportToPoint(Transform tf, Vector3 point, NavMeshAgent Agent)
    {
        int mask = LayerMask.NameToLayer("NavMesh");
        NavMesh.SamplePosition(point, out var hit, 5f, mask);
        if (hit.hit)
        {
            bool agentWasEnable = false;
            if(Agent.enabled) agentWasEnable = true;

            if(agentWasEnable) Agent.enabled = false;

            tf.position = point;
            
            if(agentWasEnable) Agent.enabled = true;
        }
    }
}
