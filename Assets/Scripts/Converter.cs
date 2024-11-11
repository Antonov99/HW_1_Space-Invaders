using System;
using System.Timers;

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
    public sealed class Converter
    {
        private Timer _timer;
        
        public IArea LoadingArea { get; private set; }
        public IArea UnloadingArea { get; private set; }
        public int InCountToConvert { get; private set; }
        public int OutCountAfterConvert { get; private set; }
        public float TimeToConvert { get; private set; }
        public bool IsActive { get; private set; }

        public event Action<Resource, int> OnPut;
        public event Action<Resource, int> OnTake;
        public event Action<Resource, int> OnConvert;

        public Converter(
            IArea loadingArea,
            IArea unloadingArea,
            int inCountToConvert,
            int outCountAfterConvert,
            float timeToConvert)
        {
            if (loadingArea is null || unloadingArea is null) throw new NullReferenceException();
            if (inCountToConvert <= 0 || outCountAfterConvert <= 0 || timeToConvert <= 0) throw new ArgumentException();

            LoadingArea = loadingArea;
            UnloadingArea = unloadingArea;
            InCountToConvert = inCountToConvert;
            OutCountAfterConvert = outCountAfterConvert;
            TimeToConvert = timeToConvert;
        }

        public Converter(
            IArea loadingArea,
            IArea unloadingArea,
            int inCountToConvert,
            int outCountAfterConvert,
            float timeToConvert,
            bool isActive) : this(loadingArea, unloadingArea, inCountToConvert, outCountAfterConvert, timeToConvert)
        {
            IsActive = isActive;
        }

        public bool CanPutResources(Resource resource, int count)
        {
            if (resource is null) throw new NullReferenceException();
            if (count <= 0) throw new ArgumentException();
            if (!LoadingArea.CanAddResources(resource, count)) return false;
            return true;
        }

        public bool CanTakeResources(Resource resource, int count)
        {
            if (resource is null) throw new NullReferenceException();
            if (count <= 0) throw new ArgumentException();
            if (!UnloadingArea.CanRemoveResources(resource, count)) return false;
            return true;
        }

        public bool PutResources(Resource resource, int count)
        {
            if (!CanPutResources(resource, count)) return false;
            LoadingArea.AddResources(resource, count);

            OnPut?.Invoke(resource, count);
            return true;
        }

        public bool TakeResources(Resource resource, int count)
        {
            if (!CanTakeResources(resource, count)) return false;
            UnloadingArea.RemoveResources(resource, count);

            OnTake?.Invoke(resource, count);
            return true;
        }

        public bool CanConvert()
        {
            if (LoadingArea.CanRemoveResources(LoadingArea.TargetResource, InCountToConvert) &&
                UnloadingArea.CanAddResources(UnloadingArea.TargetResource, OutCountAfterConvert))
                return true;

            return false;
        }

        public void Convert()
        {
            if (!IsActive) return;

            if (CanConvert())
            {
                LoadingArea.RemoveResources(LoadingArea.TargetResource, InCountToConvert);
                UnloadingArea.AddResources(UnloadingArea.TargetResource, OutCountAfterConvert);
                OnConvert?.Invoke(UnloadingArea.TargetResource, OutCountAfterConvert);
            }
            else
            {
                IsActive = false;
                StopTimer();
                ReturnResources();
            }
        }

        private void ReturnResources()
        {
            LoadingArea.AddResources(LoadingArea.TargetResource, InCountToConvert);
        }

        public bool StartConvert()
        {
            if (IsActive) return false;

            IsActive = true;
            StartTimer();
            return true;
        }

        public void SetActive(bool value)
        {
            IsActive = value;
            if (!value)
            {
                StopTimer();
                ReturnResources();
            }
        }

        private void StartTimer()
        {
            _timer = new Timer(TimeToConvert);
            _timer.Elapsed += (sender, e) => Convert();
            _timer.AutoReset = true;
            _timer.Enabled = true; 
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}