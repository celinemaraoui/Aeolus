using UnityEngine;

public class HologramPulse : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float floatAmplitude = 0.05f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Gentle vertical floating motion
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Slow rotation for a sci-fi effect
        transform.Rotate(Vector3.up, 20f * Time.deltaTime);
    }
}