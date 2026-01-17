using UnityEngine;

public class CheckGlowWin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckGlow"))
        {
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
