using System;

namespace LtdPresentation
{
    public interface IMainView
    {
        event EventHandler MainViewLoadedEventRaised;
        event EventHandler HelpAboutMenuClickEventRaised;
        void ShowMainView();
    }
}