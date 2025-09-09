using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraOrder : MonoBehaviour
{
    public GameObject Player;
    void Start()
    {
        CameraMovement.TargetTracking(Player.transform,new Vector3(-1,5,0));
        CameraMovement.RotationTracking(Player.transform, Vector3.zero);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
