using System;
using System.Timers;
using Codice.Client.Common;

namespace Homework
{
    /**
       Конвертер представляет собой преобразователь ресурсов, который берет ресурсы
       из зоны погрузки (справа) и через несколько секунд преобразовывает его в
       ресурсы другого типа (слева).

       Конвертер работает автоматически. Когда заканчивается цикл переработки
       ресурсов, то конвертер берет следующую партию и начинает цикл по новой, пока
       можно брать ресурсы из зоны загрузки или пока есть место для ресурсов выгрузки.

       Также конвертер можно выключать. Если конвертер во время работы был
       выключен, то он возвращает обратно ресурсы в зону загрузки. Если в это время
       были добавлены еще ресурсы, то при переполнении возвращаемые ресурсы
       «сгорают».

       Характеристики конвертера:
       - Зона погрузки: вместимость бревен
       - Зона выгрузки: вместимость досок
       - Кол-во ресурсов, которое берется с зоны погрузки
       - Кол-во ресурсов, которое поставляется в зону выгрузки
       - Время преобразования ресурсов
       - Состояние: вкл/выкл
     */
    public sealed class Converter<TResource>
    {
        public IArea LoadingArea { get; private set; }
        public IArea UnloadingArea { get; private set; }
        public int InCountToConvert { get; private set; }
        public int OutCountAfterConvert { get; private set; }
        public float TimeToConvert { get; private set; }
        public bool IsActive { get; private set; }
        public float CurrentTime { get; private set; }
        public TResource ResourceToConvert { get; private set; }
        public TResource ResourceAfterConvert { get; private set; }

        public event Action<TResource, int> OnPut;
        public event Action<TResource, int> OnTake;
        public event Action<TResource, int> OnConvert;

        public Converter(
            IArea loadingArea,
            IArea unloadingArea,
            int inCountToConvert,
            int outCountAfterConvert,
            float timeToConvert,
            TResource resourceToConvert,
            TResource resourceAfterConvert)
        {
            if (loadingArea is null || unloadingArea is null) throw new NullReferenceException();
            if (inCountToConvert <= 0 || outCountAfterConvert <= 0 || timeToConvert <= 0) throw new ArgumentException();

            LoadingArea = loadingArea;
            UnloadingArea = unloadingArea;
            InCountToConvert = inCountToConvert;
            OutCountAfterConvert = outCountAfterConvert;
            TimeToConvert = timeToConvert;
            ResourceToConvert = resourceToConvert;
            ResourceAfterConvert = resourceAfterConvert;
        }

        public Converter(
            IArea loadingArea,
            IArea unloadingArea,
            int inCountToConvert,
            int outCountAfterConvert,
            float timeToConvert,
            TResource resourceToConvert,
            TResource resourceAfterConvert,
            bool isActive) : this(loadingArea, unloadingArea, inCountToConvert, outCountAfterConvert, timeToConvert,
            resourceToConvert, resourceAfterConvert)
        {
            IsActive = isActive;
        }

        public bool CanPutResources(TResource resource, int count)
        {
            if (resource is null) throw new NullReferenceException();
            if (count <= 0) throw new ArgumentException();
            if (!resource.Equals(ResourceToConvert)) return false;
            if (!LoadingArea.CanAddResources(count)) return false;
            return true;
        }

        public bool CanTakeResources(TResource resource, int count)
        {
            if (resource is null) throw new NullReferenceException();
            if (count <= 0) throw new ArgumentException();
            if (!UnloadingArea.CanRemoveResources(count)) return false;
            return true;
        }

        public bool PutResources(TResource resource, int count)
        {
            if (!CanPutResources(resource, count)) return false;
            LoadingArea.AddResources(count, out int change);

            OnPut?.Invoke(resource, count);
            return true;
        }

        public bool TakeResources(TResource resource, int count)
        {
            if (!CanTakeResources(resource, count)) return false;
            UnloadingArea.RemoveResources(count);

            OnTake?.Invoke(resource, count);
            return true;
        }

        public bool StartConvert()
        {
            if (IsActive) return false;

            if (!CanConvert()) return false;

            IsActive = true;

            return true;
        }

        public bool StopConvert()
        {
            if (!IsActive) return false;

            IsActive = false;
            if(CurrentTime>0)
                ReturnResources();
            
            return true;
        }

        public void Update(float deltaTime)
        {
            if (!IsActive)
                return;
            
            if (!CanConvert()) return;

            if (CurrentTime == 0 && CanConvert())
                LoadingArea.RemoveResources(InCountToConvert);

            Tick(deltaTime);
            if (CurrentTime < TimeToConvert) return;
            
            Convert();
            CurrentTime = 0;
        }

        public bool CanConvert()
        {
            if (LoadingArea.CanRemoveResources(InCountToConvert) &&
                UnloadingArea.CanAddResources(OutCountAfterConvert))
                return true;

            return false;
        }

        public void Convert()
        {
            if (UnloadingArea.CanAddResources(OutCountAfterConvert))
            {
                UnloadingArea.AddResources(OutCountAfterConvert, out int change);
                OnConvert?.Invoke(ResourceAfterConvert, OutCountAfterConvert);
            }
            else
            {
                ReturnResources();
            }
        }

        private void ReturnResources()
        {
            LoadingArea.AddResources(InCountToConvert, out int change);
            SetActive(false);
            CurrentTime = 0;
        }

        public void SetActive(bool value)
        {
            IsActive = value;
        }

        public void Tick(float deltaTime)
        {
            CurrentTime += deltaTime;
        }
    }
}