using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AnimateSpriteShape : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    public Sprite[] animationSprites;
    public float frameDuration = 0.1f;
    private float frameTimer;

    int currentSpriteIndex = 0;
    private void Update()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= frameDuration)
        {
            frameTimer = 0;
            
            
            for (int i = 0; i < spriteShapeController.spriteShape.angleRanges.Count; i++)
            {
                AngleRange angleRange = spriteShapeController.spriteShape.angleRanges[i];
                angleRange.sprites = new List<Sprite>() { animationSprites[currentSpriteIndex] };
            }

            spriteShapeController.spriteShape.SetDirty();

            currentSpriteIndex = (currentSpriteIndex + 1) % animationSprites.Length;
        }
    }

    private IEnumerator Animate()
    {

        while (true)
        {
        }
    }
}




