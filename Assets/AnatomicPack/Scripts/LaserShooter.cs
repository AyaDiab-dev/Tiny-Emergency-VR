using UnityEngine;
using UnityEngine.Rendering;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class LaserShooter : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] private Camera aimCamera;

    [Header("Laser Settings")]
    [SerializeField] private float range = 100f;
    [SerializeField] private float damagePerSecond = 25f;

    [Header("Laser Appearance")]
    [SerializeField] private Color laserColor = Color.red;
    [SerializeField] private float laserWidth = 0.02f;
    [SerializeField] private float startDistance = 0.35f;
    [SerializeField] private float verticalOffset = -0.12f;

    private LineRenderer lineRenderer;
    private Material laserMaterial;

    private void Awake()
    {
        // استخدام الكاميرا الفعالة تلقائيًا إذا لم يتم تعيينها.
        if (aimCamera == null)
            aimCamera = Camera.main;

        // الحصول على Line Renderer أو إنشاؤه تلقائيًا.
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        ConfigureLineRenderer();
    }

    private void ConfigureLineRenderer()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;

        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.textureMode = LineTextureMode.Stretch;
        lineRenderer.numCapVertices = 0;
        lineRenderer.numCornerVertices = 0;

        lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;

        Shader laserShader =
            Shader.Find("Universal Render Pipeline/Unlit");

        if (laserShader == null)
            laserShader = Shader.Find("Unlit/Color");

        if (laserShader == null)
        {
            Debug.LogError("Laser shader was not found.");
            return;
        }

        laserMaterial = new Material(laserShader);
        laserMaterial.color = laserColor;

        if (laserMaterial.HasProperty("_BaseColor"))
            laserMaterial.SetColor("_BaseColor", laserColor);

        lineRenderer.material = laserMaterial;
    }

    private void Update()
    {
        // البحث مجددًا عن الكاميرا إذا لم تكن موجودة.
        if (aimCamera == null)
            aimCamera = Camera.main;

        if (IsFireHeld())
        {
            FireLaser();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private bool IsFireHeld()
    {
#if ENABLE_INPUT_SYSTEM
        bool keyboardHeld =
            Keyboard.current != null &&
            Keyboard.current.fKey.isPressed;

        bool mouseHeld =
            Mouse.current != null &&
            Mouse.current.leftButton.isPressed;

        return keyboardHeld || mouseHeld;
#else
        return Input.GetKey(KeyCode.F) ||
               Input.GetMouseButton(0);
#endif
    }

    private void FireLaser()
{
    if (aimCamera == null)
    {
        Debug.LogError("No active Main Camera was found.");
        lineRenderer.enabled = false;
        return;
    }

    Transform cameraTransform = aimCamera.transform;
    Vector3 direction = cameraTransform.forward;

    Vector3 start =
        cameraTransform.position
        + direction * startDistance
        + cameraTransform.up * verticalOffset;

    Vector3 end = start + direction * range;

    RaycastHit[] hits = Physics.RaycastAll(
        start,
        direction,
        range,
        Physics.DefaultRaycastLayers,
        QueryTriggerInteraction.Collide
    );

    bool foundValidHit = false;
    RaycastHit closestHit = default;
    float closestDistance = Mathf.Infinity;

    foreach (RaycastHit hit in hits)
    {
        Transform hitTransform = hit.collider.transform;

        // تجاهل Collider المركبة وجميع أجزائها.
        if (hitTransform == transform ||
            hitTransform.IsChildOf(transform))
        {
            continue;
        }

        if (hit.distance < closestDistance)
        {
            closestDistance = hit.distance;
            closestHit = hit;
            foundValidHit = true;
        }
    }

    if (foundValidHit)
    {
        end = closestHit.point;

        ClotHealth clot =
            closestHit.collider.GetComponentInParent<ClotHealth>();

        if (clot != null)
        {
            clot.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    lineRenderer.startWidth = laserWidth;
    lineRenderer.endWidth = laserWidth;
    lineRenderer.startColor = laserColor;
    lineRenderer.endColor = laserColor;

    lineRenderer.SetPosition(0, start);
    lineRenderer.SetPosition(1, end);
    lineRenderer.enabled = true;
}

    private void OnDestroy()
    {
        if (laserMaterial != null)
            Destroy(laserMaterial);
    }
}