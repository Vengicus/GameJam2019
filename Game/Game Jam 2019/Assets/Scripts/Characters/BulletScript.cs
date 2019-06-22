using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speedX = 0f;
    public float speedY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        position.x += speedX;
        position.y += speedY;
        transform.position = position;
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}
