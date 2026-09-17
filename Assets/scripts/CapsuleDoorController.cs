using UnityEngine;

public class CapsuleDoorController : MonoBehaviour
{
    [Header("Capsule")]
    public Animator capsuleAnimator;

    [Header("Player")]
    public Transform player;

    [Header("Detection Zones")]
    public BoxCollider outerZone;
    public BoxCollider insideZone;

    [Header("Front Wall")]
    public BoxCollider frontCollider;

    [Header("Animation")]
    public string animationStateName = "Take 001";
    public float animationDuration = 1.5f;

    // 0 = Closed
    // 1 = Open
    private float animationProgress = 0f;

    void Start()
    {
        // نوقف التشغيل التلقائي للـ Animator
        capsuleAnimator.speed = 0f;

        // تبدأ الكبسولة مغلقة
        animationProgress = 0f;

        capsuleAnimator.Play(
            animationStateName,
            0,
            animationProgress
        );

        // يجبر Unity على عرض أول Frame
        capsuleAnimator.Update(0f);

        // المدخل مسكر بالبداية
        frontCollider.enabled = true;
    }

    void Update()
    {
        // هل اللاعب قريب من الكبسولة؟
        bool playerNear = IsInsideZone(
            outerZone,
            player.position
        );

        // هل اللاعب دخل داخل الكبسولة؟
        bool playerInside = IsInsideZone(
            insideZone,
            player.position
        );

        // تفتح فقط إذا كان قريبًا ولكن ليس داخلها
        bool shouldOpen = playerNear && !playerInside;

        // إذا يجب أن تفتح نذهب إلى نهاية الأنميشن
        // وإذا يجب أن تسكر نرجع إلى البداية
        float targetProgress = shouldOpen ? 1f : 0f;

        animationProgress = Mathf.MoveTowards(
            animationProgress,
            targetProgress,
            Time.deltaTime / animationDuration
        );

        // نضع الأنميشن يدويًا عند النقطة المطلوبة
        capsuleAnimator.Play(
            animationStateName,
            0,
            animationProgress
        );

        // مهم جدًا:
        // يجبر الـ Animator على تحديث شكل الكبسولة
        // حتى لو كان Speed = 0
        capsuleAnimator.Update(0f);

        // إذا الكبسولة مفتوحة، نسمح بالدخول
        // إذا كانت تسكر/مسكرة، نمنع المرور
        frontCollider.enabled = !shouldOpen;
    }

    private bool IsInsideZone(Collider zone, Vector3 point)
    {
        Vector3 closestPoint = zone.ClosestPoint(point);

        return (closestPoint - point).sqrMagnitude < 0.0001f;
    }
}