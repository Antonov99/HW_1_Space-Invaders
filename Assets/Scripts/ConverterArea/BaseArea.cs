using System;

namespace Homework
{
    public class BaseArea : IArea
    {
        public event Action<Resource, int> OnAdded;
        public event Action<Resource, int> OnRemoved;

        public BaseArea(int capacity, Resource targetResourceType)
        {
            if (capacity <= 0) throw new ArgumentException();
            if (targetResourceType is null) throw new NullReferenceException();

            Capacity = capacity;
            TargetResource = targetResourceType;
        }

        public BaseArea(int capacity, Resource targetResourceType, int currentCount) : this(capacity,
            targetResourceType)
        {
            if (currentCount < 0) throw new ArgumentException();
            CurrentCount = currentCount;
        }

        public int Capacity { get; private set; }
        public Resource TargetResource { get; private set; }
        public int CurrentCount { get; private set; }
        public int FreePlaces => Capacity - CurrentCount;


        public bool AddResources(Resource resource, int count)
        {
            if (!CanAddResources(resource, count)) return false;
            CurrentCount += count;

            OnAdded?.Invoke(resource, count);
            return true;
        }

        public bool CanAddResources(Resource resource, int count)
        {
            if (resource.Name != TargetResource.Name) return false;
            if (FreePlaces <= 0) return false;
            if (count > FreePlaces) return false;

            return true;
        }

        public bool RemoveResources(Resource resource, int count)
        {
            if (!CanRemoveResources(resource, count)) return false;
            CurrentCount -= count;
            OnRemoved?.Invoke(resource, count);
            return true;
        }

        public bool CanRemoveResources(Resource resource, int count)
        {
            if (resource.Name != TargetResource.Name) return false;
            if (count > Capacity) return false;
            if (count > CurrentCount) return false;
            return true;
        }
    }
}