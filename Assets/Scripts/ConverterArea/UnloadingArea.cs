namespace Homework
{
    public class UnloadingArea:BaseArea
    {
        public UnloadingArea(int capacity, Resource targetResourceType) : base(capacity, targetResourceType)
        {
        }

        public UnloadingArea(int capacity, Resource targetResourceType, int currentCount) : base(capacity, targetResourceType, currentCount)
        {
        }
    }
}