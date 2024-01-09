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
        playerLayerMask = 1 << 7;//�÷��̾�� ��Ʈ�� 7������ 7�� ���̾��̴�
        thisMonster = gameObject.GetComponent<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        // To DO LIST
        // ���Ͱ� �÷��̾ ã��
        // �����ȿ� �÷��̾ ������ 
        // �÷��̾��� ������͸� ����
        // �ٵ� ���� �߰��� ��ֹ��� ������?
        if (thisMonster.isDead == false)
        {
            Detective();//���׾�� ã��
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
            //���⺤�ͷ� �i����
            if (Physics.Raycast(transform.position + Vector3.up, (player.transform.position - transform.position).normalized, out RaycastHit hit, maxRayDistance))
            {
                isLastDect = CheckLayer(hit.collider.gameObject.layer);
                if (isLastDect)//���̿� ���� �Ȱ����� �÷��̾ ã������ �÷��̾ �i�ƿ��� ��
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
