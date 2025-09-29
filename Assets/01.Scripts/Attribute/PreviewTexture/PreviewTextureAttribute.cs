using UnityEditor;
using UnityEngine;

namespace MIE.Attribute.PreviewTexture
{
    public class PreviewTextureAttribute : PropertyAttribute
    {
        public float size;
        public PreviewTextureAttribute(float size = 64f)
        {
            this.size = size;
        }
    }

}
