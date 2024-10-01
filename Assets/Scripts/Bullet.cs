using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;
    public GameObject ExplodeVFX;
    public float VFXDuration = 2.5f;
    protected Collider shooterCollider;

    public GunController gunController;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Method to set the shooter to ignore collision with it
    public void SetShooter(Collider shooter)
    {
        shooterCollider = shooter;
        Physics.IgnoreCollision(GetComponent<Collider>(), shooterCollider);
    }

    protected void Explode(Vector3 position, Quaternion rotation)
    {
        GameObject vfx = Instantiate(ExplodeVFX, position, rotation);
        Destroy(vfx, VFXDuration); // Destroy VFX after duration
    }

    // Abstract method for handling collision, to be implemented by subclasses
    protected abstract void HandleCollision(Collider other);

    void OnTriggerEnter(Collider other)
    {
        // Ignore collision with the shooter
        if (other == shooterCollider) return;

        HandleCollision(other);
        Destroy(gameObject); // Destroy the bullet after hitting something
    }
}
