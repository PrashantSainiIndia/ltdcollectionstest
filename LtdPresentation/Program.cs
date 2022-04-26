using LtdDomain.Models.UserProfile;
using LtdInfrastructure.DataAccess.Repositories.UserProfile;
using LtdPresentation.Presenters;
using LtdPresentation.Views;
using LtdService.CommonServices;
using LtdService.Services.UserProfileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace LtdPresentation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IUnityContainer UnityC;

            string _connectionString = "Data Source= ...";

            UnityC =
                new UnityContainer()
                .RegisterType<IMainView, MainView>(new ContainerControlledLifetimeManager())
                .RegisterType<IMainPresenter, MainPresenter>(new ContainerControlledLifetimeManager())
                .RegisterType<IErrorMessageView, ErrorMessageView>(new ContainerControlledLifetimeManager())
                .RegisterType<IHelpAboutPresenter, HelpAboutPresenter>(new ContainerControlledLifetimeManager())
                .RegisterType<IHelpAboutView, HelpAboutView>(new ContainerControlledLifetimeManager())
                .RegisterType<IUserProfileModel, UserProfileModel>(new ContainerControlledLifetimeManager())

                //BasePresenter : IBasePresenter
                .RegisterType<IUserProfileRepository, UserProfileRepository>(new InjectionConstructor(_connectionString))
                .RegisterType<IUserProfileService, UserProfileServices>(new ContainerControlledLifetimeManager())
                .RegisterType<IModelDataAnnotationCheck, ModelDataAnnotationCheck>(new ContainerControlledLifetimeManager());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IMainPresenter mainPresenter = UnityC.Resolve<MainPresenter>();

            IMainView mainView = mainPresenter.GetMainView();

            Application.Run((MainView)mainView);
        }
    }
}
