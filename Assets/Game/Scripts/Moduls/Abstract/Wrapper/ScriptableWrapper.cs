using UnityEngine;

namespace Company.Module
{
    public abstract class ScriptableWrapper< T > : ScriptableObject
    {
        public T Value;
    }
}