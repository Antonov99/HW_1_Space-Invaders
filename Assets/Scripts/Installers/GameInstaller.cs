using ShootEmUp;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject player;
    
    public override void InstallBindings()
    {
        Container.Bind<PlayerService>().AsSingle().WithArguments(player);
        Container.BindInterfacesAndSelfTo<PlayerDeathObserver>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMoveController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAttackController>().AsSingle();
    }
}