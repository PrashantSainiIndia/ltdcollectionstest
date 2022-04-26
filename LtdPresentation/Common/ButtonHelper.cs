using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LtdPresentation.Common
{
    static public class ButtonHelper
    {
        static public void SetToBorderLess(Button button)
        {
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.MouseOverBackColor = button.BackColor;
            button.FlatAppearance.MouseDownBackColor = button.BackColor;

            button.MouseMove += new MouseEventHandler(OnBorderlessMouseMoveEventRaised);
            button.MouseLeave += new EventHandler(OnBorderlessMouseLeaveEventRaised);
        }

        static public void SetGroupToBorderLess(List<Button> buttons)
        {
            foreach(Button btn in buttons)
            {
                SetToBorderLess(btn);
            }
        }

        static public void OnBorderlessMouseMoveEventRaised(Object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        static public void OnBorderlessMouseLeaveEventRaised(Object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }

        static public void SetVisibilityOfButons(List<Button> buttons, bool visibility, Label underlineLabel)
        {
            foreach(Button btn in buttons)
            {
                btn.Visible = visibility;
            }
            if(underlineLabel != null)
            {
                underlineLabel.Visible = visibility;
            }
        }

        static public void SetUnderlinePosition(Button button, Label underlineLabel)
        {
            underlineLabel.Width = button.Bounds.Width - (int)(button.Bounds.Width * .15); ;
            underlineLabel.Left = button.Bounds.Left - (int)(button.Bounds.Width * .08); ;
            underlineLabel.Top = button.Top + button.Height;
        }
    }
}
