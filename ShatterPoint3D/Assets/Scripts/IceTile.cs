using UnityEngine;

public class IceTile : MonoBehaviour
{

    public int durability = 3; //Solid, Cracked, Very Cracked

    private Renderer rend;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateColor();

    }

    public void StepOn()
    {
        durability--;
        UpdateColor();
        if (durability <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StepOn();
        }
    }

    // Update is called once per frame
    void UpdateColor()
    {
        if (durability == 3)
        {
            rend.material.color = Color.cyan;
        }
        else if (durability == 2)
        {
            rend.material.color = Color.blue;
        }
        else if (durability == 1)
        {
            rend.material.color = Color.gray;
        }

    }
}
