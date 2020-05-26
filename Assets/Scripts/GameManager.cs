using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private Player playerInstance;
    public Material[] colorMats;
    public float twoTargetRate = 0.3f;
    public float threeTargetRate = 0.2f;
    public float spawnInterval = 0.5f;
    public float waveInterval = 3f;
    public Target targetPrefab;
    public Transform spawnPoint;
    public int curScore;
    public Text scoreText;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return Starting();
        yield return Playing();
        yield return Ending();
    }

    private IEnumerator Starting()
    {
        playerInstance = FindObjectOfType<Player>();
        playerInstance.Setup(colorMats);
        yield return null;
    }

    private IEnumerator Playing()
    {
        StartCoroutine("SpawnTarget");
        while (playerInstance)
        {
            yield return null;
        }
        StopCoroutine("SpawnTarget");
    }

    private IEnumerator Ending()
    {
        foreach (var target in FindObjectsOfType<Target>())
        {
            target.Disable();
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator SpawnTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(waveInterval);
            var random = Random.value;
            if (random <= threeTargetRate)
            {
                yield return InstantiateTarget(3);
            }
            else if (random <= twoTargetRate)
            {
                yield return InstantiateTarget(2);
            }
            else
            {
                yield return InstantiateTarget(1);
            }
        }
    }

    private IEnumerator InstantiateTarget(int count)
    {
        int randomColorIndex = Random.Range(0, colorMats.Length);
        for (int i = 0; i < count; i++)
        {
            int repeatIndex = (int)Mathf.Repeat(randomColorIndex + i, colorMats.Length);
            var target = Instantiate(targetPrefab, spawnPoint.position, Random.rotationUniform);
            target.SetColor(colorMats[repeatIndex]);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void AddScore(int score)
    {
        curScore += score;
        scoreText.text = curScore.ToString("000");
    }
}
