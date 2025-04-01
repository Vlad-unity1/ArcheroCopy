using Zenject;

namespace Project.Scripts.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ZenjectSceneLoader>().AsSingle();
         //   Container.BindInterfacesAndSelfTo<Bootstrap>().AsSingle().NonLazy();
        }
    }
}