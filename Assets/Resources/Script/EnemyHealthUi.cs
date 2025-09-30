using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUi : MonoBehaviour
{
    public GameObject targetObj;
    public Slider slider;

    public Enemy enemyScript;


    void Awake()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        transform.SetParent(canvasObject.transform);

        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (targetObj == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Camera.main.WorldToScreenPoint(targetObj.transform.position + Vector3.up * 2);
        }
    }

    private void LateUpdate()
    {
        slider.value = enemyScript.normalizedHP;
    }
}
