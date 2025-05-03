#if NET_4_6 || NET_STANDARD_2_0
using System;

[Serializable]
public class StringHashSet : SerializableHashSet<string>
{
}
#endif