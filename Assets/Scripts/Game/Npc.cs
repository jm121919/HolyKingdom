using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Npc : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination1;
    private Vector3 destination2;
    bool isDestination1;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination1 = new Vector3 (11.69f, 0.53f, 9.737f);
        destination2 = new Vector3 (119.197f, -17.167f, -21.918f);
        agent.SetDestination(destination1);
        isDestination1 = true;
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        yield return null;
        while (true)
        {
            if (agent.remainingDistance < 2f && isDestination1)
            {
                agent.SetDestination(destination2);
                yield return new WaitForSeconds(5f);
                isDestination1 = !isDestination1;
            }
            if (agent.remainingDistance < 2f && !isDestination1)
            {
                agent.SetDestination(destination1);
                yield return new WaitForSeconds(5f);
                isDestination1 = !isDestination1;
            }
            yield return null;
        }
    }
}

