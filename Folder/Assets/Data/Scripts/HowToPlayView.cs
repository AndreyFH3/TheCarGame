using UnityEngine;

public class HowToPlayView : MonoBehaviour
{

    public void Close()
    {
        Game.Player.isShowedHowTo = true;
        Destroy(gameObject);
    }
}
