using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveCube : MonoBehaviour
{

    #region Attributes
    
    private Transform _cube;
    private float _goalRot = (Mathf.PI / 4.0f) * Mathf.Rad2Deg;
    private bool _isRunning = false;

    [SerializeField] private float rotTime = 1.0f;
    
    #endregion
    
    #region Methods
    
    private void Start()
    {
        _cube = gameObject.transform;
    }
    
    private void Update()
    {
        if (!_isRunning)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                Move();
            }
        }

    }

    private void Move()
    {
        _isRunning = true;
        StartCoroutine(SmoothRotation());
    }

    private IEnumerator SmoothRotation()
    {
        int randdir = Random.Range(0, 3);
        Vector3 aRot = new Vector3(randdir == 0 ? 1 : 0, randdir == 1 ? 1 : 0, randdir == 2 ? 1 : 0) * _goalRot;
        Quaternion startRotation = _cube.rotation;
        Quaternion goalRotation = startRotation * Quaternion.Euler(aRot);

        float countTime = 0;
        while (countTime <= rotTime)
        {
            countTime += Time.deltaTime;
            _cube.rotation = Quaternion.Slerp(startRotation, goalRotation, countTime / rotTime);
            yield return null;
        }

        _isRunning = false;
    }

    #endregion
}
