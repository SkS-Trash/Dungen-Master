public static class SerializableDictionary
{
    public class Storage<T> : SerializableDictionaryBase.Storage
    {
        public T data;
    }
}