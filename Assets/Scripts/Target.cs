using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody rigid;
    public float rotateSpeed = 100f;
    public MeshRenderer meshRenderer;
    public ParticleSystem explosionEffect;
    private GameManager gameManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetColor(Material mat)
    {
        meshRenderer.material = mat;
        explosionEffect.GetComponent<ParticleSystemRenderer>().material = mat;
    }

    public Material ColorMaterial
    {
        get
        {
            return meshRenderer.sharedMaterial;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rigid.AddTorque(Vector3.one * rotateSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)
            return;
        if(player.ColorMaterial == ColorMaterial)
        {
            explosionEffect.transform.SetParent(null);
            explosionEffect.Play();
            Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
            Destroy(gameObject);
            Camera.main.DOShakePosition(0.1f, 0.25f, 200);
            gameManager.AddScore(1);
        }
        else
        {
            player.Explosion();
        }
    }

    public void Disable()
    {
        rigid.constraints = RigidbodyConstraints.FreezePositionY;
    }
}
