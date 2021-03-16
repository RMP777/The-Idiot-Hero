using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform moveTarget;
    public NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(moveTarget.position);
        Debug.Log("help");

        /*if (Vector3.Distance(moveTarget.position, transform.position) < agent.stoppingDistance)
        {
            LerpRotationTo(moveTarget);

            //Add attack code here in future 
        }
        else
        {
            agent.SetDestination(moveTarget.position)
        }*/ 
    }

    void LerpRotationTo(Transform target)
    {
        //Lerps this to face the player
        var qTo = Quaternion.LookRotation(target.position - transform.position);
        qTo = Quaternion.Lerp(transform.rotation, qTo, 2f * Time.deltaTime);
        Quaternion qToY = Quaternion.Euler(transform.rotation.eulerAngles.x, qTo.eulerAngles.y, transform.rotation.eulerAngles.z);

        //Since it's a locked rotation rigidbody, we have to just set the transform.rotation and can't use MoveRotation
        transform.rotation = qToY;
    }

}