using UnityEngine;

namespace Farm
{
    public static class AnimationClipExtension
    {

        public static int LengthMillisec(this AnimationClip clip)
        {
            return Mathf.RoundToInt(clip.length * 1000);
        }
    }
}