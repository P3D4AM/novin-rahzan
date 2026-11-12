using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Design.Controls;

public class StiExpressionForm : StiPopupForm
{
	private string expressionString = string.Empty;

	private IContainer components;

	public StiDictionaryControl DictControl;

	public string ExpressionString => expressionString;

	public StiExpressionForm(StiReport report, Control parentControl)
		: base(parentControl)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (report != null)
			{
				DictControl.NodesMemory = report.Designer.NodesMemory;
			}
			if (report != null)
			{
				((TreeView)DictControl.tvData).BorderStyle = (BorderStyle)0;
				DictControl.Build(report);
				((Control)DictControl.tvData).DoubleClick += DictControl_DoubleClick;
			}
			else
			{
				((Control)DictControl).Visible = false;
			}
			((Form)this).ShowIcon = false;
		}
	}

	private void ThisPaint(object sender, PaintEventArgs e)
	{
		e.Graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
	}

	private void DictControl_DoubleClick(object sender, EventArgs e)
	{
		((StiExpressionButton)(object)base.ParentControl).ExpressionString = StiCodeEditor.GetTextFromDictionaryTree(DictControl, null);
		ClosePopupForm();
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
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		DictControl = new StiDictionaryControl();
		((Control)this).SuspendLayout();
		((Control)DictControl).Anchor = (AnchorStyles)15;
		((Control)DictControl).Location = new Point(2, 2);
		((Control)DictControl).Name = "DictControl";
		((Control)DictControl).Size = new Size(316, 396);
		((Control)DictControl).TabIndex = 1;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Control)this).BackColor = SystemColors.Window;
		((Form)this).ClientSize = new Size(320, 400);
		((Control)this).Controls.AddRange((Control[])(object)new Control[1] { (Control)DictControl });
		((Control)this).Name = "StiExpressionForm";
		((Control)this).Text = "StiBrushButtonForm";
		((Control)this).Paint += new PaintEventHandler(ThisPaint);
		((Control)this).ResumeLayout(false);
	}
}
