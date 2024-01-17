using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    public float spawnCoolDown = 2;

    public Queue<Color> monsterQueue = new();
    public GameObject background;
    public GameObject monster;
    private float spawnTimer = 0;

    private float currentNoisePosition = 0;

    private Color targetColor;

    private SpriteRenderer backgroundSprite;


    void Start()
    {
        currentNoisePosition = UnityEngine.Random.Range(0f, 10f);
        backgroundSprite = background.GetComponent<SpriteRenderer>();
        targetColor = backgroundSprite.color;
        GenerateNextMonster();
        GenerateNextMonster();
        GenerateNextMonster();
        GenerateNextMonster();
        GenerateNextMonster();
        GenerateNextMonster();
        UpdateTargetColor();
    }


    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnCoolDown)
        {
            Instantiate(monster, this.transform.position, new Quaternion(0, 0, 0, 0));
            monster.GetComponent<SpriteRenderer>().color = monsterQueue.Dequeue();
            GenerateNextMonster();
            UpdateTargetColor();
            spawnTimer = 0;
            Debug.Log("monster" + monster.GetComponent<SpriteRenderer>().color);
        }
        backgroundSprite.color = Color.Lerp(backgroundSprite.color, targetColor, Time.deltaTime * 0.1f);
        Debug.Log("lerp " + backgroundSprite.color + " " + targetColor);
    }

    void GenerateNextMonster()
    {
        float linearNoise = Mathf.PerlinNoise1D(currentNoisePosition);
        float colorValue = Mathf.Clamp(linearNoise * 1.2f - 0.1f, 0, 1);
        currentNoisePosition += 0.1f;
        monsterQueue.Enqueue(new Color(colorValue, colorValue, colorValue, 1));
    }

    void UpdateTargetColor()
    {
        float colorSum = 0;
        foreach (var color in monsterQueue.ToArray())
        {
            colorSum += color.r;
        }
        float colorValue = colorSum / monsterQueue.Count;
        targetColor = new Color(colorValue, colorValue, colorValue, 1);
        Debug.Log("next: " + targetColor);
    }
}
