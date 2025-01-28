using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RandomImageDisplay : MonoBehaviour
{
    public Image[] images;
    public Sprite[] sprites;
    public float updateRate = 1;

    private int _currentImage;
    private int _currentSprite;
    private float _elapsed;

    private void Start()
    {
        // shuffle sprites
        for (int i = 0; i < sprites.Length; i++)
        {
            int randomIndex = Random.Range(0, sprites.Length);
            (sprites[i], sprites[randomIndex]) = (sprites[randomIndex], sprites[i]);
        }
        
        Invoke(nameof(CycleImage), updateRate);
    }

    private static IEnumerator CycleCoroutine(Image image, Sprite sprite)
    {
        image.CrossFadeAlpha(0, 1, false);
        yield return new WaitForSeconds(1);
        image.sprite = sprite;
        image.CrossFadeAlpha(1, 1, false);
        yield return new WaitForSeconds(1);
    }

    private void CycleImage()
    {
        StartCoroutine(CycleCoroutine(images[_currentImage], sprites[_currentSprite]));
        _currentSprite = (_currentSprite + 1) % sprites.Length;
        _currentImage = (_currentImage + 1) % images.Length;
        Invoke(nameof(CycleImage), updateRate);
    }
}
