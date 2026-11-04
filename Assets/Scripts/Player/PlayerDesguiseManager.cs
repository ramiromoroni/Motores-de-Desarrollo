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
    //MeshRenderer playerRenderer;
    public DesguiseType currentDesguise = DesguiseType.None;

    [Header("Game Over")]
    [SerializeField] private GameOverController gameOverController;

    [SerializeField] private GameObject desguiseChassis;
    [SerializeField] private SkinnedMeshRenderer desguiseRenderer;
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

    }
    public void SetDesguiseOn()
    {
        currentDesguise = DesguiseType.Desguise;
        desguiseDuration = maxDesguiseDuration;
    }

    public void SetDead()
    {
        if (currentDesguise == DesguiseType.Dead)
            return;

        currentDesguise = DesguiseType.Dead;

        if (gameOverController != null)
        {
            gameOverController.StartGameOver();
        }
        else
        {
            Debug.LogError("No se asignó GameOverController al PlayerDesguiseManager.");
        }
    }

    void Update()
    {
        switch (currentDesguise)
        {
            case DesguiseType.None:
                desguiseRenderer.material.color = Color.white;
                desguiseChassis.SetActive(false);
                break;
            case DesguiseType.Desguise:
                desguiseChassis.SetActive(true);
                desguiseDuration -= Time.deltaTime;
                if (desguiseDuration <= maxDesguiseDuration / 2)
                {
                    desguiseRenderer.material.color = Color.grey;
                    currentDesguise = DesguiseType.Degrading;
                }
                break;
            case DesguiseType.Degrading:
                desguiseDuration -= Time.deltaTime;
                if (desguiseDuration <= 0)
                {
                    currentDesguise = DesguiseType.None;
                    desguiseRenderer.material.color = Color.white;
                }
                break;
            case DesguiseType.Dead:
                //playerRenderer.material.color = Color.red;
                this.GetComponent<PlayerController>().enabled = false;
                break;
        }
    }

}
