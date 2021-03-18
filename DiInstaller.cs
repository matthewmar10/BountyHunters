using System;
using Zenject;

public class DiInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameSessionFirst>().AsSingle().NonLazy();
        Container.Bind<RealTimeClient>().AsSingle().NonLazy();
    }
}
