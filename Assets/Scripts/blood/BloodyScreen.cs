using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodyScreen : MonoBehaviour
{
    [SerializeField] private List<Image> offImagesList;
    [SerializeField] private List<Image> onImagesList;

    [SerializeField] private int bloodyScreenChance;
    [SerializeField] private float fadeDuration = 5f;
    [SerializeField] private int howManyBloodToFade = 3;

    private GameEvents events;
    private bool isFading = false;

    void Awake()
    {
        events = FindObjectOfType<GameEvents>();

        
        foreach (Image img in offImagesList)
        {
            Color newColor = img.color;
            newColor.a = 0f;
            img.color = newColor;
        }
    }

    void Update()
    {
       
        if (Input.GetKeyDown("l") && !isFading && onImagesList.Count > 0)
        {
            StartCoroutine(FadeMany());
        }
    }

    private IEnumerator FadeMany()
    {
        isFading = true;

        int count = Mathf.Min(howManyBloodToFade, onImagesList.Count);
        List<Image> imagesToFade = onImagesList.GetRange(0, count);
        List<Coroutine> coroutines = new List<Coroutine>();
        foreach (Image img in imagesToFade)
        {
            coroutines.Add(StartCoroutine(FadeOne(img)));
        }
        foreach (Coroutine c in coroutines)
        {
            yield return c;
        }

        isFading = false;
    }

    private IEnumerator FadeOne(Image img)
    {
        float startAlpha = img.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);

            Color color = img.color;
            color.a = alpha;
            img.color = color;

            yield return null; 
        }

        
        Color finalColor = img.color;
        finalColor.a = 0f;
        img.color = finalColor;

        offImagesList.Add(img);
        onImagesList.Remove(img);
    }

    private void OnEnable()
    {
        events.OnEnemyHit += addBloodOnScreen;
    }

    private void OnDisable()
    {
        events.OnEnemyHit -= addBloodOnScreen;
    }

    private void addBloodOnScreen()
    {
        int randChance = Random.Range(1, 101);
        if (randChance <= bloodyScreenChance)
        {
            randomBlood();
        }
    }

    private void randomBlood()
    {
        if (offImagesList.Count == 0) return;

        int randIndex = Random.Range(0, offImagesList.Count);
        Image img = offImagesList[randIndex];

        Color newColor = img.color;
        newColor.a = 1f;
        img.color = newColor;

        onImagesList.Add(img);
        offImagesList.Remove(img);
    }
}
