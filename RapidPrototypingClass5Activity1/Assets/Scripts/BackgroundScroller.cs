using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private bool scrollDown = true; // true = down, false = up

    [Header("Reset Position")]
    [SerializeField] private float resetDistance = 10f; // Distance before reset
    [SerializeField] private Vector3 startPosition;

    void Start()
    {
        // Store the starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the background
        float moveDirection = scrollDown ? -1f : 1f;
        transform.Translate(Vector3.up * moveDirection * scrollSpeed * Time.deltaTime);

        // Reset position when it moves too far (for seamless looping)
        if (scrollDown && transform.position.y <= startPosition.y - resetDistance)
        {
            transform.position = startPosition;
        }
        else if (!scrollDown && transform.position.y >= startPosition.y + resetDistance)
        {
            transform.position = startPosition;
        }
    }
}