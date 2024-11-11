namespace Homework
{
    public interface IArea
    {
        public int Capacity { get;  }
        public Resource TargetResource { get;  }
        public int CurrentCount { get;  }
        public int FreePlaces => Capacity - CurrentCount;
        
        public bool AddResources(Resource resource, int count);
        public bool CanAddResources(Resource resource, int count);
        public bool RemoveResources(Resource resource, int count);
        public bool CanRemoveResources(Resource resource, int count);
    }
}