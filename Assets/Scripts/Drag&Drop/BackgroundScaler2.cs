using UnityEngine;

public class BackgroundScaler2 : MonoBehaviour
{
    bool scaled = false;

    void LateUpdate()
    {
        if (scaled) return;
        scaled = true;

        Camera cam = Camera.main;
        if (cam == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth  = screenHeight * cam.aspect;

        float spriteHeight = sr.sprite.bounds.size.y;
        float spriteWidth  = sr.sprite.bounds.size.x;

        transform.localScale = new Vector3(
            screenWidth  / spriteWidth,
            screenHeight / spriteHeight,
            1f
        );

        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 1f);
    }
}