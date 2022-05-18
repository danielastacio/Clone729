using UnityEngine;
public class PlayerAudio : MonoBehaviour
{
    #region Player Audio

    [Header("Player Audio")]
    public AK.Wwise.Event healing;
    public AK.Wwise.Event attack;
    public void PlayHealing()
    { 
        healing.Post(gameObject); 
        print("healed"); 
    }

    public void PlayAttack() => attack.Post(gameObject);

    void OnEnable()
    {
        Player.PlayerHealed += PlayHealing;
    }
    private void OnDisable()
    {
        Player.PlayerHealed -= PlayHealing;
    }
    #endregion
}
