using UnityEngine;
public enum DesguiseType
{
    None,
    Desguise,
    Degrading
}
[RequireComponent(typeof(MeshRenderer))]
public class PlayerDesguiseManager : MonoBehaviour
{
    public PlayerDesguiseManager instance;
    MeshRenderer playerRenderer;
    private DesguiseType currentDesguise = DesguiseType.None;
    public DesguiseType CurrentDesguise => currentDesguise;
    float maxDesguiseDuration = 5f;
    float desguiseDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        playerRenderer = GetComponent<MeshRenderer>();
    }
    public void SetDesguiseOn()
    {
        Debug.Log("Desguise set to Desguise");
        currentDesguise = DesguiseType.Desguise;
        desguiseDuration = maxDesguiseDuration;
        playerRenderer.material.color = Color.blue; // Change color to green when desguise is active

    }
    void Update()
    {
        switch (currentDesguise)
        {
            case DesguiseType.None:
                playerRenderer.material.color = Color.white;
                break;
            case DesguiseType.Desguise:
                playerRenderer.material.color = Color.blue;
                desguiseDuration -= Time.deltaTime;
                if (desguiseDuration <= maxDesguiseDuration / 2)
                {
                    currentDesguise = DesguiseType.Degrading;
                    playerRenderer.material.color = Color.cyan; // Change color to yellow when desguise is about to end
                }
                break;
            case DesguiseType.Degrading:
                desguiseDuration -= Time.deltaTime;
                if (desguiseDuration <= 0)
                {
                    currentDesguise = DesguiseType.None;
                    playerRenderer.material.color = Color.white; // Change color back to white when desguise ends
                }
                break;
        }
    }

}
