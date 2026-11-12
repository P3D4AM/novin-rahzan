using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Dashboard;

public class StiDashboardNotSupportedForm : Form
{
	private IContainer components;

	private Button btOk;

	private Label lbMessage;

	private StiGroupLine glButtons;

	private Label labelHyperlink;

	public StiDashboardNotSupportedForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			Localize();
		}
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)lbMessage).Text = StiLocalization.Get("Dashboard", "CannotLoadDashboard");
		((Control)labelHyperlink).Text = StiLocalization.Get("Buttons", "ShowMore");
		((Control)this).Text = StiLocalization.Get("Errors", "Error");
	}

	private void ButtonOk_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void EmptyDataHyperlink_MouseEnter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Label val = (Label)sender;
		((Control)val).ForeColor = Color.Red;
		((Control)val).Invalidate();
	}

	private void EmptyDataHyperlink_MouseLeave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Label val = (Label)sender;
		((Control)val).ForeColor = Color.FromArgb(255, 43, 86, 154);
		((Control)val).Invalidate();
	}

	private void LabelHyperlink_Click(object sender, EventArgs e)
	{
		try
		{
			StiProcess.Start("https://www.stimulsoft.com/en/documentation/online/programming-manual/reports_net.htm");
		}
		catch
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		lbMessage = new Label();
		btOk = new Button();
		glButtons = new StiGroupLine();
		labelHyperlink = new Label();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lbMessage).Location = new Point(12, 23);
		((Control)lbMessage).Margin = new Padding(2, 0, 2, 0);
		((Control)lbMessage).Name = "lbMessage";
		((Control)lbMessage).Size = new Size(415, 19);
		((Control)lbMessage).TabIndex = 2;
		((Control)lbMessage).Text = "You cannot load a report with dashboards.";
		lbMessage.TextAlign = (ContentAlignment)16;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)2;
		((Control)btOk).Location = new Point(351, 12);
		((Control)btOk).Margin = new Padding(2);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += ButtonOk_Click;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)labelHyperlink);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 67);
		((Control)glButtons).Margin = new Padding(2);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(437, 46);
		((Control)glButtons).TabIndex = 1;
		((Control)labelHyperlink).Anchor = (AnchorStyles)13;
		((Control)labelHyperlink).Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelHyperlink).ForeColor = Color.FromArgb(43, 86, 154);
		((Control)labelHyperlink).Location = new Point(12, 12);
		((Control)labelHyperlink).Name = "labelHyperlink";
		((Control)labelHyperlink).Size = new Size(192, 25);
		((Control)labelHyperlink).TabIndex = 4;
		((Control)labelHyperlink).Text = "Show More";
		labelHyperlink.TextAlign = (ContentAlignment)16;
		((Control)labelHyperlink).Click += LabelHyperlink_Click;
		((Control)labelHyperlink).MouseEnter += EmptyDataHyperlink_MouseEnter;
		((Control)labelHyperlink).MouseLeave += EmptyDataHyperlink_MouseLeave;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btOk;
		((Form)this).ClientSize = new Size(437, 113);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)lbMessage);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(2);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDashboardNotSupportedForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiDashboardNotSupportedForm";
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
