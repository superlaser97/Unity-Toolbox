using UnityEngine;
using UnityEngine.Events;

public class TestUnityActions : MonoBehaviour 
{
    private bool coolDown;
    private int skillAmmo;
    private float range;
    private float currentCD;
    private float coolDownTimer;

    public void Activate()
    {
        if (!coolDown && skillAmmo>0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, range);

            foreach(Collider c in hitColliders)
            {
                if (c != null && c is IDamagable)
                {
                    IDamagable hitObj = c.gameObject.GetComponent<IDamagable>();
                    Vector3 pushForce = (c.transform.position - gameObject.transform.position) * 5f;

                    hitObj.TakeDamage(2f);
                    hitObj.ApplyPushForce(pushForce);
                }
                Destroy(gameObject);
            }

            coolDown = true;
            currentCD = coolDownTimer;
        }
    }
}

public interface IDamagable 
{
    void TakeDamage(float dmg);
    void ApplyPushForce(Vector3 force);
}