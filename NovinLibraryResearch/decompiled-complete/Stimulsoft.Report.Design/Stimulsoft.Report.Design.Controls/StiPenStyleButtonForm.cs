using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

public class StiPenStyleButtonForm : StiPopupForm
{
	private IContainer components;

	private ListBox lbStyles;

	public StiPenStyleButtonForm(Control parentControl)
		: base(parentControl)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			InitStyles();
			((Form)this).ShowIcon = false;
		}
	}

	private void InitStyles()
	{
		StiPenStyleButton stiPenStyleButton = base.ParentControl as StiPenStyleButton;
		Array values = Enum.GetValues(typeof(StiPenStyle));
		((Control)this).Height = lbStyles.ItemHeight * values.Length + 2;
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			for (int i = 0; i < values.Length; i++)
			{
				lbStyles.Items.Add((object)((StiPenStyle)values.GetValue(i)).ToString());
				if ((StiPenStyle)values.GetValue(i) == stiPenStyleButton.PenStyle)
				{
					((ListControl)lbStyles).SelectedIndex = i;
				}
			}
			((Control)this).Width = StiPenStyleHelper.GetMaxWidthOfPenStyles(val, ((Control)this).Font);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void lbStyles_DrawItem(object sender, DrawItemEventArgs e)
	{
		StiPenStyleHelper.DrawPenStyle(e);
	}

	private void lbStyles_MouseUp(object sender, MouseEventArgs e)
	{
		StiPenStyleButton stiPenStyleButton = base.ParentControl as StiPenStyleButton;
		Array values = Enum.GetValues(typeof(StiPenStyle));
		stiPenStyleButton.PenStyle = (StiPenStyle)values.GetValue(((ListControl)lbStyles).SelectedIndex);
		ClosePopupForm();
	}

	private void StiPenStyleButtonForm_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
	}

	private void lbStyles_MouseMove(object sender, MouseEventArgs e)
	{
		Point pt = ((Control)lbStyles).PointToClient(Cursor.Position);
		for (int i = 0; i < lbStyles.Items.Count; i++)
		{
			if (lbStyles.GetItemRectangle(i).Contains(pt))
			{
				((ListControl)lbStyles).SelectedIndex = i;
				break;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		lbStyles = new ListBox();
		((Control)this).SuspendLayout();
		((Control)lbStyles).BackColor = SystemColors.Window;
		lbStyles.BorderStyle = (BorderStyle)0;
		((Control)lbStyles).Dock = (DockStyle)5;
		lbStyles.DrawMode = (DrawMode)2;
		lbStyles.IntegralHeight = false;
		lbStyles.ItemHeight = 19;
		((Control)lbStyles).Location = new Point(1, 1);
		((Control)lbStyles).Name = "lbStyles";
		((Control)lbStyles).Size = new Size(238, 230);
		((Control)lbStyles).TabIndex = 0;
		((Control)lbStyles).MouseUp += new MouseEventHandler(lbStyles_MouseUp);
		((Control)lbStyles).MouseMove += new MouseEventHandler(lbStyles_MouseMove);
		lbStyles.DrawItem += new DrawItemEventHandler(lbStyles_DrawItem);
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(240, 232);
		((Control)this).Controls.AddRange((Control[])(object)new Control[1] { (Control)lbStyles });
		((ScrollableControl)this).DockPadding.All = 1;
		((Control)this).Name = "StiPenStyleButtonForm";
		((Control)this).Text = "PenStyleButtonForm";
		((Control)this).Paint += new PaintEventHandler(StiPenStyleButtonForm_Paint);
		((Control)this).ResumeLayout(false);
	}
}
