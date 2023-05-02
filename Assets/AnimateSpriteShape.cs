using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AnimateSpriteShape : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    public Sprite[] animationSprites;
    public float frameDuration = 0.1f;

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int currentSpriteIndex = 0;

        while (true)
        {
            for (int i = 0; i < spriteShapeController.spriteShape.angleRanges.Count; i++)
            {
                AngleRange angleRange = spriteShapeController.spriteShape.angleRanges[i];
                angleRange.sprites = new List<Sprite>() { animationSprites[currentSpriteIndex] };
            }

            spriteShapeController.spriteShape.SetDirty();

            currentSpriteIndex = (currentSpriteIndex + 1) % animationSprites.Length;
            yield return new WaitForSeconds(frameDuration);
        }
    }
}




