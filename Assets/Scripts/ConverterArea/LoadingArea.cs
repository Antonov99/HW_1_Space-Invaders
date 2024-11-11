namespace Homework
{
    public class LoadingArea:BaseArea
    {
        public LoadingArea(int capacity, Resource targetResourceType) : base(capacity, targetResourceType)
        {
        }

        public LoadingArea(int capacity, Resource targetResourceType, int currentCount) : base(capacity, targetResourceType, currentCount)
        {
        }
    }
}