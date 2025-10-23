using Common;
using UnityEngine;
using System.Collections;

public class BirdComponent : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float hoverTime = 0.3f;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private float rotationAngle = 15f;
    [SerializeField] private float scaleFactor = 1.2f;
    #endregion Inspector Variables

    #region Private Variables
    private Vector3 startPos;
    private Vector3 originalScale;
    #endregion Private Variables

    #region Monobehaviour Methods
    private void Start()
    {
        startPos = transform.position;
        originalScale = transform.localScale;
        StartCoroutine(JumpLoop());
    }
    #endregion Monobehaviour Methods

    #region Private Methods
    private IEnumerator JumpLoop()
    {
        while (true)
        {
            Vector3 targetPos = startPos + Vector3.up * jumpHeight;

            // Jump up
            while (transform.position.y < targetPos.y)
            {
                float step = jumpSpeed * Time.deltaTime;
                transform.position += Vector3.up * step;

                float tilt = Mathf.Lerp(0f, rotationAngle, (transform.position.y - startPos.y) / jumpHeight);
                transform.rotation = Quaternion.Euler(0, 0, tilt);

                yield return null;
            }

            transform.position = targetPos;
            yield return new WaitForSeconds(hoverTime);

            // Fall down
            while (transform.position.y > startPos.y)
            {
                float step = jumpSpeed * Time.deltaTime;
                transform.position -= Vector3.up * step;

                float tilt = Mathf.Lerp(rotationAngle, 0f, (targetPos.y - transform.position.y) / jumpHeight);
                transform.rotation = Quaternion.Euler(0, 0, tilt);

                if (transform.position.y < startPos.y)
                    transform.position = startPos;

                yield return null;
            }

            // Reset rotation
            transform.rotation = Quaternion.identity;

            // Apply landing scale effect
            transform.localScale = new Vector3(originalScale.x * scaleFactor, originalScale.y / scaleFactor, originalScale.z);
            yield return new WaitForSeconds(0.1f); // Short "landing squash" effect
            transform.localScale = originalScale;

            yield return new WaitForSeconds(waitTime);
        }
    }

    #endregion Private Methods
}
