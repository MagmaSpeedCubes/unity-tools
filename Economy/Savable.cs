namespace MagmaLabs.Economy{
    public interface Savable
    {
        string Serialize();
        void LoadFromSerialized(string serialized);
    }
}