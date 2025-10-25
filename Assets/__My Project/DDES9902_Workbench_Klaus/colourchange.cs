using UnityEngine;

public class ClayColor : MonoBehaviour
{
    [Header("Has it been dyed?")]
    private bool isColored = false;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        rend.material = new Material(rend.material);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isColored && collision.gameObject.CompareTag("ColorBlock"))
        {
            Renderer blockRend = collision.gameObject.GetComponent<Renderer>();
            if (blockRend != null)
            {
                Color blockColor = blockRend.material.color;

                rend.material.color = blockColor;
                isColored = true;

                Debug.Log("土坯染色成功！颜色：" + blockColor);
            }
        }
    }
}
