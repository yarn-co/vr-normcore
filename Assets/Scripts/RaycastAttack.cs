using Fusion;
using UnityEngine;

public class RaycastAttack : NetworkBehaviour
{
    public int Damage;

    public PlayerMovement PlayerMovement;

    void Update()
    {        
        if (HasStateAuthority == false)
        {
            return;
        }

        Vector3 forward = PlayerMovement.Camera.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(PlayerMovement.Camera.transform.position, forward, Color.green);

        Ray ray = PlayerMovement.Camera.ScreenPointToRay(Input.mousePosition);
        ray.origin += PlayerMovement.Camera.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {   
            Debug.Log("Mouse1 Down");
            Debug.Log(ray);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);

            if (Runner.GetPhysicsScene().Raycast(ray.origin,ray.direction, out var hit))
            {               
                Debug.Log("Hit " + hit);

                if (hit.transform.TryGetComponent<Health>(out var health))
                {
                    Debug.Log("Has Health");
                    health.DealDamageRpc(Damage);
                }
            }
        }
    }
}