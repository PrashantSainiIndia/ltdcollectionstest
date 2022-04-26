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
    public partial class ErrorMessageView : Form, IErrorMessageView
    {
        public ErrorMessageView()
        {
            InitializeComponent();
        }

        public void ShowErrorMessageView(string windowTitle, string errorMessage)
        {
            this.Text = windowTitle;
            this.txtMessage.Text = errorMessage;
            this.ShowDialog();
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text != "")
            {
                System.Windows.Forms.Clipboard.SetText(txtMessage.Text);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
