using UnityEngine;

public class AutoDestroyParticles : MonoBehaviour
{
    //THis is to clean the instanstiated particle also i can use here object pooling but there are 3 effects
    private void OnEnable()
    {
        Destroy(gameObject, 2f);
    }


}