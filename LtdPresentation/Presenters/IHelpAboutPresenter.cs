using LtdPresentation.Views;
using System;

namespace LtdPresentation.Presenters
{
    public interface IHelpAboutPresenter
    {
        IHelpAboutView GetHelpAboutView();
        void OnHelpAboutViewLoadEventRaised(object sender, EventArgs e);
    }
}