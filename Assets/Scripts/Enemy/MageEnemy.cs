using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackRate = 2f;
    [SerializeField] private float projectileSpeed = 3f;
    [SerializeField] private float projectileLifetime = 3f;

    private float nextAttackTime;

    [System.Serializable]
    private class ProjectileData : MonoBehaviour
    {
        public float damage;
        public float speed;
        public Vector2 direction;
        public float lifetime;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, lifetime);
        }

        void FixedUpdate()
        {
            if (rb) rb.velocity = direction * speed;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Player.Instance.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }

    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(30f, 0.8f, 15);
    }

    void Update()
    {
        bool targetFound = FindTarget();
        if (targetFound)
        {
            path.canMove = true;
            path.destination = playerPos.position;
            distanceToTarget = Vector2.Distance(transform.position, playerPos.position);

            Flip((playerPos.position - transform.position).normalized);

            if (distanceToTarget <= stoppingThreshold)
            {
                path.canMove = false;
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    anim.SetTrigger("Attack");
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
        else
        {
            path.canMove = false;
        }
    }

    public override void Attack()
    {

        if (!playerPos || !projectilePrefab) return;

        Vector2 direction = (playerPos.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        ProjectileData projectileScript = projectile.AddComponent<ProjectileData>();
        projectileScript.damage = damage;
        projectileScript.speed = projectileSpeed;
        projectileScript.direction = direction;
        projectileScript.lifetime = projectileLifetime;

        if (!projectile.GetComponent<Rigidbody2D>())
            projectile.AddComponent<Rigidbody2D>().gravityScale = 0;

        if (!projectile.GetComponent<Collider2D>())
            projectile.AddComponent<CircleCollider2D>().isTrigger = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}