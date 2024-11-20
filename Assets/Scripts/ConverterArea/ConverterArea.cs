using System;

namespace Homework
{
    public class ConverterArea : IArea
    {
        public event Action<int> OnAdded;
        public event Action<int> OnRemoved;

        public ConverterArea(int capacity)
        {
            if (capacity <= 0) throw new ArgumentException();

            Capacity = capacity;
        }

        public ConverterArea(int capacity, int currentCount) : this(capacity)
        {
            if (currentCount < 0) throw new ArgumentException();
            CurrentCount = currentCount;
        }

        public int Capacity { get; private set; }
        public int CurrentCount { get; private set; }
        public int FreePlaces => Capacity - CurrentCount;


        public bool AddResources(int count)
        {
            if (!CanAddResources(count)) return false;
            CurrentCount += count;

            OnAdded?.Invoke(count);
            return true;
        }

        public bool CanAddResources(int count)
        {
            if (FreePlaces <= 0) return false;
            if (count > FreePlaces) return false;

            return true;
        }

        public bool RemoveResources(int count)
        {
            if (!CanRemoveResources(count)) return false;
            CurrentCount -= count;
            OnRemoved?.Invoke(count);
            return true;
        }

        public bool CanRemoveResources(int count)
        {
            if (count > Capacity) return false;
            if (count > CurrentCount) return false;
            return true;
        }
    }
}