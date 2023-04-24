using UnityEngine;

/// <summary>
/// Class that keeps its object rotation static
/// </summary>
public class StaticUIHolder : MonoBehaviour
{

    private Quaternion _rotation;
    private Transform _transform;
    
    void Start()
    {
        _transform = transform;
        _rotation = _transform.rotation;
    }
    
    void Update()
    {
        _transform.rotation = _rotation;
    }
}
