using Common;
using UnityEngine;
using System.Collections;
using AudioManager;

public class PigComponent : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float hoverTime = 0.3f;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private float rotationAngle = 10f;
    [SerializeField] private float scaleFactor = 1.1f;
    [SerializeField] private float startDelay = 1f;

    [Header("Movement Settings")]
    [SerializeField] private float glideSmooth = 0.1f;
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;

    private float targetX;
    private bool shouldMove = false;
    private bool isPaused = false;
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        originalScale = transform.localScale;
        targetX = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(StartWithDelay());
    }

    private void Update()
    {
        if (shouldMove)
        {
            targetX = Mathf.Clamp(targetX, minX, maxX);
            float newX = Mathf.Lerp(transform.position.x, targetX, glideSmooth);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
                shouldMove = false;
        }
    }

    public void OnRapidClick(float deltaX)
    {
        targetX -= deltaX;
        shouldMove = true;
    }

    public void StopMovement()
    {
        shouldMove = false;
    }

    public void PauseAnimation()
    {
        isPaused = true;
    }

    public void ResumeAnimation()
    {
        isPaused = false;
    }

    private IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(JumpLoop());
    }

    private IEnumerator JumpLoop()
    {
        while (true)
        {
            if (isPaused) { yield return null; continue; }

            float baseY = transform.position.y;
            float targetY = baseY + jumpHeight;

            while (transform.position.y < targetY)
            {
                if (isPaused) yield return null;
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + jumpSpeed * Time.deltaTime, transform.position.z);
                    float tilt = Mathf.Lerp(0f, rotationAngle, (transform.position.y - baseY) / jumpHeight);
                    transform.rotation = Quaternion.Euler(0, 0, tilt);
                    yield return null;
                }
            }

            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
            yield return new WaitForSeconds(hoverTime);

            while (transform.position.y > baseY)
            {
                if (isPaused) yield return null;
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - jumpSpeed * Time.deltaTime, transform.position.z);
                    float tilt = Mathf.Lerp(rotationAngle, 0f, (targetY - transform.position.y) / jumpHeight);
                    transform.rotation = Quaternion.Euler(0, 0, tilt);

                    if (transform.position.y < baseY)
                        transform.position = new Vector3(transform.position.x, baseY, transform.position.z);

                    yield return null;
                }
            }

            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(originalScale.x * scaleFactor, originalScale.y / scaleFactor, originalScale.z);
            yield return new WaitForSeconds(0.1f);
            transform.localScale = originalScale;
            yield return new WaitForSeconds(waitTime);
            AudioManager.AudioManager.Instance.PlaySFX(SFX_Type.JUMP_CLIP, 0.3f);
        }
    }

    public IEnumerator BlinkAndPause()
    {
        PauseAnimation();
        if (spriteRenderer != null)
        {
            Color original = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(1f);
            spriteRenderer.color = original;
        }
    }
}
