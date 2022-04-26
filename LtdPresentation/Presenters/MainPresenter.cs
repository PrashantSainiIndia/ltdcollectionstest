using LtdPresentation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LtdPresentation.Presenters
{
    public class MainPresenter : BasePresenter, IMainPresenter
    {
        public event EventHandler UserProfileDetailedViewBindingDoneEventRaised;
        IMainView _mainView;
        IHelpAboutPresenter _helpAboutPresenter;

        public IMainView GetMainView() { return _mainView; }

        public MainPresenter(IMainView mainView, IErrorMessageView errorMessageView, IHelpAboutPresenter helpAboutPresenter) : base (errorMessageView)
        {
            _mainView = mainView;
            _helpAboutPresenter = helpAboutPresenter;
            SubscribeToEventsSetup();
        }

        private void SubscribeToEventsSetup()
        {
            _mainView.HelpAboutMenuClickEventRaised += new EventHandler(OnHelpAboutMenuClickEventRaised);
        }

        public void OnHelpAboutMenuClickEventRaised(object sender, EventArgs e)
        {
            _helpAboutPresenter.GetHelpAboutView().ShowHelpAboutView();
        }
    }
}
