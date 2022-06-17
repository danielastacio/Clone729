using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] UnityEvent unityEvent;
    // Start is called before the first frame update
    void Awake() => gameEvent.Register(this);

    // Update is called once per frame
    void OnDestroy() => gameEvent.DeRegister(this);

    public void RaiseEvent() => unityEvent.Invoke();
}
