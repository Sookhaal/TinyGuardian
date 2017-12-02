using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
    public FloatData HP;
    public GameEvent Damage;
    public GameEvent Death;
    public GameEvent StartDialog;

    private void OnTriggerEnter(Collider collider) {
        Damage.Raise();

        if (HP.Value <= 0f){
            Death.Raise();
        }

        StartDialog.Raise();
    }
}