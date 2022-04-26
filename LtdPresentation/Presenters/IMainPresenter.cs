using System;

namespace LtdPresentation.Presenters
{
    public interface IMainPresenter
    {
        event EventHandler UserProfileDetailedViewBindingDoneEventRaised;
        IMainView GetMainView();
    }
}