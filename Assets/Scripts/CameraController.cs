using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Réglage Largeur")]
    public float xMin = -10f;
    public float xMax = 10f;

    public float zMin = -9f;
    public float zMax = 9.3f;

    [Header("Occupation écran")]
    [Range(0.1f, 1f)]
    public float arenaScreenPercent = 0.65f;

    [Header("Vitesse de déplacement")]
    public float scrollSpeed = 20f;

    void Update()
    {
        AdjustWidth();
        HandleMovement();
    }

    void AdjustWidth()
    {
        Camera cam = GetComponent<Camera>();

        float arenaWidth = xMax - xMin;

        // Largeur totale visible par la caméra
        // Exemple :
        // si l'arène doit occuper 70% de l'écran,
        // alors l'écran doit être plus large.
        float totalVisibleWidth = arenaWidth / arenaScreenPercent;

        // Taille orthographique nécessaire
        cam.orthographicSize = totalVisibleWidth / (2f * cam.aspect);

        // Bord gauche de l'arène
        float leftArenaEdge = xMin;

        // Largeur visible totale
        float visibleWidth = cam.orthographicSize * 2f * cam.aspect;

        // Centre caméra pour que :
        // - le bord gauche écran = xMin
        // - l'arène reste dans les 70% de gauche
        float cameraCenterX = leftArenaEdge + (visibleWidth / 2f);

        transform.position = new Vector3(
            cameraCenterX,
            transform.position.y,
            transform.position.z
        );
    }


    void HandleMovement()
    {
        float moveZ = Input.GetAxis("Vertical");

        if (moveZ != 0)
        {
            transform.Translate(
                Vector3.forward * moveZ * scrollSpeed * Time.deltaTime,
                Space.World
            );
        }

        // 🔒 Clamp Z après déplacement
        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z, zMin, zMax);
        transform.position = pos;
    }
}