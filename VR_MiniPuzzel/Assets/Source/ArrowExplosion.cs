using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ArrowExplosion : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float time;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Arrow arrow;

    private ParticleSystem explosionVFX;
    private Vector3 impactPosition;
    private static int counter; 
    // Start is called before the first frame update
    void Start()
    {
        explosionVFX = Instantiate(explosion).GetComponent<ParticleSystem>();
        explosionVFX.Stop();
        explosionVFX.gameObject.SetActive(false);
        arrow.arrowHit.AddListener(Explode);
    }

    public void Explode()
    {
        Debug.Log(counter);
        if (!CheckIfExplosionShot())
        {
            arrow.arrowHit.RemoveListener(Explode);
            return;
        }

        int hitCount;
        RaycastHit[] hits = new RaycastHit[50];

        hitCount = Physics.SphereCastNonAlloc(transform.position, radius, Vector3.up, hits, 0.0f, 8);

        StartCoroutine(ExplosionVisuals());

        if (hitCount == 0)
        {
            Debug.Log("Nothing was hit");
            return;
        }

        foreach (var hit in hits)
        {
            Debug.Log(hit.collider.gameObject.name);
        }

        for(int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.tag == "Enemy")
            {
                Destroy(hits[i].collider.gameObject);
            }
        }
        arrow.arrowHit.RemoveListener(Explode);
    }

    private static bool CheckIfExplosionShot()
    {
        if(counter >= 2)
        {
            counter = 0;
            return true;
        }
        counter++;
        return false;
    }

    private IEnumerator ExplosionVisuals()
    {
        SetScaleOfExplosion();
        explosionVFX.transform.position = impactPosition;
        explosionVFX.transform.position = transform.position;
        explosionVFX.gameObject.SetActive(true);
        explosionVFX.Play();
        yield return new WaitForSeconds(0.5f);
        explosionVFX.gameObject.SetActive(false);
    } 

    private void SetScaleOfExplosion()
    {
        Vector3 newScale = explosionVFX.transform.localScale * radius * 0.4f;
        explosionVFX.transform.localScale = newScale;
        explosionVFX.transform.GetChild(0).localScale = newScale;
        explosionVFX.transform.GetChild(1).localScale = newScale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
    
}
