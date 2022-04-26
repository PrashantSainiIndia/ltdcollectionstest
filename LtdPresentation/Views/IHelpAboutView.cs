using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LtdPresentation.Views
{
    public interface IHelpAboutView
    {
        event EventHandler HelpAboutViewLoadEventRaised;

        void SetAboutValues(string wnidowTitle, string productName, string version, string copyright, string companyName, string description);

        void ShowHelpAboutView();
    }
}
