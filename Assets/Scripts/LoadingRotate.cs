using UnityEngine;
using UnityEngine.UI;

public class LoadingRotate : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]

    public void Update()
    {
        float zRot = transform.rotation.z + rotationSpeed;
        transform.Rotate(new Vector3(0f, 0f, zRot) * Time.deltaTime);
    }
}
