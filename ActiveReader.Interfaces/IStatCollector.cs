namespace ActiveReader.Interfaces
{
    public interface IStatCollector
    {
        void Collect(string text, int articleID);
    }
}