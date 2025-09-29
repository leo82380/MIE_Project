using UnityEngine;

namespace MIE.Attribute.Conditional
{
    public class ConditionalAttribute : PropertyAttribute
    {
        public string BoolFieldName;

        public ConditionalAttribute(string boolFieldName)
        {
            BoolFieldName = boolFieldName;
        }
    }
}