using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Animator animator;
    public Collider weaponCol;
    void Start()
    {
        weaponCol = GetComponent<Collider>();
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        WeaponColFalse();
    }
    public void WeaponColFalse()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f && CombatController.IsCurAttackAnime)
        {
            weaponCol.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Monster foundEnemy;
        if (other.gameObject.TryGetComponent(out foundEnemy))
        {
            SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.hitSound, other.gameObject.transform);

            weaponCol.enabled = false;
            foundEnemy.Hp -= GameManager.instance.player.atk;
            StartCoroutine(PopDamage(foundEnemy.transform));
            Debug.LogWarning("몬스터 남은 체력 : " + foundEnemy.Hp);
        }
    }

    IEnumerator PopDamage(Transform targetTransform)
    {
        TextType tempType = GameManager.instance.textObjectPool.damageTextPrefab.GetComponent<TextPopUpUI>().type;
        GameObject temp = GameManager.instance.textObjectPool.PopObj(tempType, targetTransform);//여기
        temp.GetComponent<TextPopUpUI>().popupText.text = GameManager.instance.player.atk.ToString();
        float yprev = temp.transform.position.y;//기존좌표
        while (temp.activeSelf)
        {
            temp.transform.LookAt(GameManager.instance.mainCamera.transform.position);
            while (temp.transform.position.y <= yprev + 3f)//이미지 천천히 올라가는것
            {
                temp.transform.position = new Vector3(temp.transform.position.x, Mathf.Lerp(temp.transform.position.y, temp.transform.position.y + 3f, Time.deltaTime), temp.transform.position.z);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            GameManager.instance.textObjectPool.ReturnPool(tempType, temp);
        }
    }
}
