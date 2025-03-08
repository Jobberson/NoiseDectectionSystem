using Unity.Mathematics;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    /// <summary>
    /// This script goes on objects you want to make noise when they fall on the ground
    /// 
    /// It essentially just takes a prefab object and instantiates 
    /// The script takes the position of the noise and sends it to the enemy
    /// 
    /// There is still room for improvement, this is a very basic system as of right now
    /// </summary>

    public GameObject noisePointPrefab;
    private GameObject noiseInstance;

    void OnCollisionEnter(Collision collision)
    {
        // Optional: Create a visual noise marker in the scene
        if (noiseInstance == null)
        {
            noiseInstance = Instantiate(noisePointPrefab, transform.position, quaternion.identity);
        }
        else
        {
            noiseInstance.transform.position = transform.position;
        }

        // Notify all listeners that noise was made
        NoiseEventManager.MakeNoise(noiseInstance.transform);

        Destroy(noiseInstance, 20);
    }
}
