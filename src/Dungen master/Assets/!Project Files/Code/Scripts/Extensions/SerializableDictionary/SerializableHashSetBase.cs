#if NET_4_6 || NET_STANDARD_2_0
using System.Collections.Generic;
using System.Runtime.Serialization;

public abstract class SerializableHashSetBase
{
    public abstract class Storage
    {
    }

    protected class HashSet<TValue> : System.Collections.Generic.HashSet<TValue>
    {
        public HashSet()
        {
        }

        public HashSet(ISet<TValue> set) : base(set)
        {
        }

        public HashSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

#endif