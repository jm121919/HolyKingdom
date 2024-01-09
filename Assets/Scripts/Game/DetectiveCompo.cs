using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveCompo : MonoBehaviour
{
    public Monster thisMonster;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float maxRayDistance;
    LayerMask playerLayerMask;
    bool isFirstDectect;
    bool isLastDect;


    // Start is called before the first frame update
    void Start()
    {
        radius = 5f;
        maxRayDistance = 5f;
        playerLayerMask = 1 << 7;//플레이어는 비트로 7번을민 7번 레이어이다
        thisMonster = gameObject.GetComponent<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        // To DO LIST
        // 몬스터가 플레이어를 찾음
        // 범위안에 플레이어가 들어오면 
        // 플레이어의 방향백터를 구함
        // 근데 만약 중간에 장애물이 있으면?
        if (thisMonster.isDead == false)
        {
            Detective();//안죽어야 찾음
        }
    }

    void Detective()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius, playerLayerMask);
        isFirstDectect = cols.Length > 0;
        if (isFirstDectect)
        {
            GameObject player = cols[0].gameObject;
            Debug.DrawRay(transform.position + Vector3.up, (player.transform.position - transform.position), Color.yellow);
            //방향벡터로 쐇을때
            if (Physics.Raycast(transform.position + Vector3.up, (player.transform.position - transform.position).normalized, out RaycastHit hit, maxRayDistance))
            {
                isLastDect = CheckLayer(hit.collider.gameObject.layer);
                if (isLastDect)//사이에 벽도 안가리고 플레이어를 찾았을때 플레이어를 쫒아오게 됨
                {
                    thisMonster.agent.SetDestination(player.transform.position);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    bool CheckLayer(int layerMask)
    {
        return (playerLayerMask & (1 << layerMask)) != 0;
    }
}
