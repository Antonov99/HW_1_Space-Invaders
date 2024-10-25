using ShootEmUp;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    private EnemyPool enemyPool;
    
    [SerializeField]
    private BulletPool bulletPool;

    [SerializeField]
    private BulletConfig bulletConfig;

    [SerializeField]
    private LevelBounds levelBounds;
    
    [SerializeField]
    private GameObject player;
    
    public override void InstallBindings()
    {
        Container.Bind<BulletPool>().FromInstance(bulletPool).AsSingle();
        Container.Bind<BulletConfig>().FromInstance(bulletConfig).AsSingle();
        Container.Bind<LevelBounds>().FromInstance(levelBounds).AsSingle();
        
        Container.Bind<PlayerService>().AsSingle().WithArguments(player);
        
        Container.BindInterfacesAndSelfTo<PlayerDeathObserver>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMoveController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAttackController>().AsSingle();

        Container.Bind<EnemyPool>().FromInstance(enemyPool).AsSingle();
        Container.Bind<EnemyManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemyCooldownSpawner>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<BulletSystem>().AsSingle();
        

    }
}