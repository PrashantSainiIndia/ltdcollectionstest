using CommonComponents;
using LtdPresentation.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LtdPresentation.Views
{
    public partial class HelpAboutView : Form, IHelpAboutView
    {
        public HelpAboutView()
        {
            InitializeComponent();
        }

        public event EventHandler HelpAboutViewLoadEventRaised;

        public void SetAboutValues(string windowTitle, string productName, string version, string copyright, string companyName, string description)
        {
            this.Text = windowTitle;
            this.lblProduct.Text = productName;
            this.lblVersion.Text = version;
            this.lblCopyright.Text = copyright;
            this.lblCompany.Text = companyName;
            this.lblDescription.Text = description;
        }

        private void HelpAboutView_Load(object sender, EventArgs e)
        {
            FormHelper.SetDialogAppearance(this);
            EventHelpers.RaiseEvent(this, HelpAboutViewLoadEventRaised, e);
        }
        public void ShowHelpAboutView()
        {
            this.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
