namespace Homework
{
    public interface IArea
    {
        public int Capacity { get; }
        public int CurrentCount { get; }
        public int FreePlaces => Capacity - CurrentCount;

        public bool AddResources(int count);
        public bool CanAddResources(int count);
        public bool RemoveResources(int count);
        public bool CanRemoveResources(int count);
    }
}