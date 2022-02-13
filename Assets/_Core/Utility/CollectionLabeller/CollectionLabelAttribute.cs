using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class CollectionLabelAttribute : PropertyAttribute
    {
        public string elementName;
        public CollectionLabelAttribute(string elementName)
        {
            this.elementName = elementName;
        }
    }

}

