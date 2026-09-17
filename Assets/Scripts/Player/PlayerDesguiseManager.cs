using UnityEngine;
public enum DesguiseType
{
    None,
    Desguise,
    Degrading,
    Dead
}
[RequireComponent(typeof(MeshRenderer))]
public class PlayerDesguiseManager : MonoBehaviour
{
    public PlayerDesguiseManager instance;
    MeshRenderer playerRenderer;
    public DesguiseType currentDesguise = DesguiseType.None;
    float maxDesguiseDuration = 5f;
    float desguiseDuration;

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
        currentDesguise = DesguiseType.Desguise;
        desguiseDuration = maxDesguiseDuration;

    }
    public void SetDead()
    {
        currentDesguise = DesguiseType.Dead;
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
                    playerRenderer.material.color = Color.cyan;
                }
                break;
            case DesguiseType.Degrading:
                desguiseDuration -= Time.deltaTime;
                if (desguiseDuration <= 0)
                {
                    currentDesguise = DesguiseType.None;
                    playerRenderer.material.color = Color.white;
                }
                break;
            case DesguiseType.Dead:
                playerRenderer.material.color = Color.red;
                this.GetComponent<PlayerController>().enabled = false;
                break;
        }
    }

}
