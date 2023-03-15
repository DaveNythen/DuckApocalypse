using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            ShootMouse();

        ShootMobile();
    }

    void ShootMobile()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches) //Handle more than 1 finger
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 11f);

                    SoundManager.PlaySound(SoundManager.soundList.shoot);

                    if (hit.collider != null)
                    {
                        Bird bird = hit.transform.GetComponent<Bird>();

                        if (bird != null)
                        {
                            if (bird.isAlive)
                            {
                                bird.Hit();
                                ShowHitEffect(hit.point);
                            }
                        }
                    }
                }
            }
        }
    }

    void ShootMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 11f);

        SoundManager.PlaySound(SoundManager.soundList.shoot);

        if (hit.collider != null)
        {
            Bird bird = hit.transform.GetComponent<Bird>();

            if (bird != null)
            {
                if (bird.isAlive)
                {
                    bird.Hit();
                    ShowHitEffect(hit.point);
                }
            }             
        }
    }

    private void ShowHitEffect(Vector2 hitPoint)
    {
        GameObject hitEffectInstance = Instantiate(hitEffect, hitPoint, Quaternion.identity);

        Destroy(hitEffectInstance, 0.15f);
    }
}
