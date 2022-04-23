using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpriteAnimation", menuName = "Game/SpriteAnimationPreset")]
public class SpriteAnimationPreset : ScriptableObject
{
    public string animationID;
    public Sprite[] sprites;

    public SpriteAnimationSettings settings;

    public bool loop = true;
    public SimpleSpriteAnimator.Mode mode = SimpleSpriteAnimator.Mode.SingleRow;
    public int gridRowLength = 8;
}
