using UnityEngine;
using System.Collections;
using Pathfinding; // i'm using a* pathfinding for the enemy

public class NoiseListener : MonoBehaviour
{
    /// <summary>
    /// This script goes on the enemy so it can listen to noises made by objects falling
    /// 
    /// You can hook it up to the enemy sight script or anything related to its behavior to make 
    /// the listening more complex 
    /// </summary>
    
    [Header("Speed Adjustments")]
    public float normalSpeed = 8f;   // Default movement speed
    public float alertedSpeed = 12f;  // Speed when hearing noise

    [Header("Misc")]
    [SerializeField] private bool debugOn = false;

    // private variables
    private Coroutine resetSpeedCoroutine;
    private Patrol patrolComp;
    private AIDestinationSetter destinationSetter;
    private AIPath pathmaker;

    private void Start()
    {
        patrolComp        = gameObject.GetComponent<Patrol>();
        destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        pathmaker         = gameObject.GetComponent<AIPath>();
    }

    private void OnEnable()
    {
        NoiseEventManager.OnNoiseMade += HearNoise;
    }

    private void OnDisable()
    {
        NoiseEventManager.OnNoiseMade -= HearNoise;
    }

    private void HearNoise(Transform noisePosition)
    {
        ReactToNoise(noisePosition);

        // Reset previous speed reset timer if a new noise appears
        if (resetSpeedCoroutine != null)
        {
            StopCoroutine(resetSpeedCoroutine);
        }

        // Start a new timer to reset speed when the noise disappears
        resetSpeedCoroutine = StartCoroutine(ResetSpeedAfterDelay(20f));
    }

    private void ReactToNoise(Transform noisePosition)
    {
        if(debugOn)
            Debug.LogWarning($"{gameObject.name} heard noise at {noisePosition.position}!");

        // react to noise here ↓
        patrolComp.enabled        = false;
        destinationSetter.enabled = true;
        destinationSetter.target  = noisePosition;
        pathmaker.maxSpeed        = alertedSpeed;
    }

    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if(debugOn)
            Debug.LogWarning($"{gameObject.name} has forgotten about the noise");
        
        // set values from reaction back to normal here ↓ 
        patrolComp.enabled        = true;
        destinationSetter.enabled = false;
        pathmaker.maxSpeed        = normalSpeed;
    }
}
