using CommonComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LtdPresentation
{
    public partial class MainView : Form, IMainView
    {
        public event EventHandler MainViewLoadedEventRaised;
        public event EventHandler HelpAboutMenuClickEventRaised;
        public void ShowMainView()
        {
            this.Show();
        }
        public MainView()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void helpAboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventHelpers.RaiseEvent(this, HelpAboutMenuClickEventRaised, e);
        }
    }
}
