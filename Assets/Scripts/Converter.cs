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
    public sealed class Converter
    {
        public IArea LoadingArea { get; private set; }
        public IArea UnloadingArea { get; private set; }
        public int InCountToConvert { get; private set; }
        public int OutCountAfterConvert { get; private set; }
        public float TimeToConvert { get; private set; }
        public bool IsActive { get; private set; }
        public float CurrentTime { get; private set; }
        public Resource ResourceToConvert { get; private set; }
        public Resource ResourceAfterConvert { get; private set; }


        public event Action<Resource, int> OnPut;
        public event Action<Resource, int> OnTake;
        public event Action<Resource, int> OnConvert;

        public Converter(
            IArea loadingArea,
            IArea unloadingArea,
            int inCountToConvert,
            int outCountAfterConvert,
            float timeToConvert,
            Resource resourceToConvert,
            Resource resourceAfterConvert)
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
            Resource resourceToConvert,
            Resource resourceAfterConvert,
            bool isActive) : this(loadingArea, unloadingArea, inCountToConvert, outCountAfterConvert, timeToConvert, resourceToConvert, resourceAfterConvert)
        {
            IsActive = isActive;
        }

        public bool CanPutResources(Resource resource, int count)
        {
            if (resource is null) throw new NullReferenceException();
            if (count <= 0) throw new ArgumentException();
            if (resource.Name != ResourceToConvert.Name) return false;
            if (!LoadingArea.CanAddResources(count)) return false;
            return true;
        }

        public bool CanTakeResources(Resource resource, int count)
        {
            if (resource is null) throw new NullReferenceException();
            if (count <= 0) throw new ArgumentException();
            if (!UnloadingArea.CanRemoveResources(count)) return false;
            return true;
        }

        public bool PutResources(Resource resource, int count)
        {
            if (!CanPutResources(resource, count)) return false;
            LoadingArea.AddResources(count);

            OnPut?.Invoke(resource, count);
            return true;
        }

        public bool TakeResources(Resource resource, int count)
        {
            if (!CanTakeResources(resource, count)) return false;
            UnloadingArea.RemoveResources(count);

            OnTake?.Invoke(resource, count);
            return true;
        }

        public bool StartConvert()
        {
            if (IsActive) return false;

            if (CanConvert())
            {
                LoadingArea.RemoveResources(InCountToConvert);
                IsActive = true;
                StartCooldownConverting();
            }

            return true;
        }

        private void StartCooldownConverting()
        {
            while (IsActive)
            {
                Tick(TimeToConvert);
                if (CurrentTime >= TimeToConvert)
                {
                    Convert();
                    CurrentTime = 0;
                    if (CanConvert())
                        LoadingArea.RemoveResources(InCountToConvert);
                    else
                        SetActive(false);
                }
            }
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
            if (!IsActive)
            {
                ReturnResources();
                return;
            }

            if (UnloadingArea.CanAddResources(OutCountAfterConvert))
            {
                UnloadingArea.AddResources(OutCountAfterConvert);
                OnConvert?.Invoke(ResourceAfterConvert, OutCountAfterConvert);
            }
            else
            {
                ReturnResources();
                SetActive(false);
            }
        }

        private void ReturnResources()
        {
            LoadingArea.AddResources(InCountToConvert);
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