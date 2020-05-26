using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    public MeshRenderer[] renderers;
    public ParticleSystem explosionEffect;
    private Material[] colorMats;
    private int curColorIndex;

    public void Setup(Material[] colorMats)
    {
        this.colorMats = colorMats;
        RefreshColor(0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RefreshColor((int)Mathf.Repeat(curColorIndex + 1, colorMats.Length));
        }
    }

    private void RefreshColor(int index)
    {
        curColorIndex = index;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = colorMats[(int)Mathf.Repeat(curColorIndex + i, colorMats.Length)];
        }
        explosionEffect.GetComponent<ParticleSystemRenderer>().material = ColorMaterial;
    }

    public Material ColorMaterial
    {
        get
        {
            return renderers[0].sharedMaterial;
        }
    }

    public void Explosion()
    {
        explosionEffect.transform.SetParent(null);
        explosionEffect.Play();
        Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
        Destroy(gameObject);
    }
}
