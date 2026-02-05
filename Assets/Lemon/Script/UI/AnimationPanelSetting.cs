using UnityEngine;

public class AnimationPanelSetting : MonoBehaviour
{
    public GameObject panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void AnimationOpen() => panel.SetActive(false);
    // Update is called once per frame
    void Update()
    {
        
    }
}
