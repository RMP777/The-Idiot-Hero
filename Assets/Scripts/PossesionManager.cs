using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PossesionManager : MonoBehaviour
{
    FreeCam cam;
    Rigidbody rb;
    Collider col;
    bool possesing;
    private PlayerMover mover;
    public float onMeshThreshold = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<FreeCam>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out RaycastHit HitInfo, 100.0f))
        {
            PlayerMover m = HitInfo.transform.GetComponent<PlayerMover>();

            if(m != null)
            {
                if (possesing)
                {
                    UnPossess();
                }

                mover = m;

                Possess();
            }
        }

        if (possesing && mover != null && Input.GetKeyDown(KeyCode.Q))
        {
            UnPossess();
        }
    }

    public void Possess()
    {
        Quaternion rot = transform.rotation;

        mover.enabled = true;
        Enemy enemy = mover.GetComponent<Enemy>();

        enemy.enabled = false;
        enemy.agent.enabled = false;

        cam.enabled = false;
        transform.SetParent(mover.transform);
        transform.rotation = rot;
        rb.isKinematic = true;
        col.enabled = false;

        StartCoroutine(LerpLocalPosition(mover.cameraLocalPosition, 0.5f));

        possesing = true;
    }

    public void UnPossess()
    {
        mover.enabled = false;

        if (IsAgentOnNavMesh(mover.gameObject))
        {
            Enemy enemy = mover.GetComponent<Enemy>();

            enemy.enabled = true;
            enemy.agent.enabled = true;
        }

        cam.enabled = true;
        transform.SetParent(null);
        mover.rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        col.enabled = true;
    }

    IEnumerator LerpLocalPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;

        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;
    }

    public bool IsAgentOnNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }
}
