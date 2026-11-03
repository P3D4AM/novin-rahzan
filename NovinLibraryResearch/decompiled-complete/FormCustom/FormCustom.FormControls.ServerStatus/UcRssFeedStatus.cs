using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace FormCustom.FormControls.ServerStatus;

public class UcRssFeedStatus : UserControl, IDisposable
{
	private IContainer components;

	private ButtonX buttonX1;

	private string Link { get; set; }

	public UcRssFeedStatus(bool pIsup, string pLink, string Message)
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
		StringBuilder stringBuilder = new StringBuilder();
		if (pIsup)
		{
			buttonX1.TextColor = Color.Green;
		}
		stringBuilder.AppendLine(pLink);
		stringBuilder.AppendLine(Message);
		Link = pLink;
		((Control)buttonX1).Click += ButtonX1_Click;
		((Control)buttonX1).Text = stringBuilder.ToString();
	}

	private void ButtonX1_Click(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(Link))
		{
			Process.Start(Link);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		buttonX1 = new ButtonX();
		((Control)this).SuspendLayout();
		((Control)buttonX1).AccessibleRole = (AccessibleRole)43;
		buttonX1.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonX1).Dock = (DockStyle)5;
		((Control)buttonX1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)buttonX1).Location = new Point(0, 0);
		((Control)buttonX1).Name = "buttonX1";
		((Control)buttonX1).Size = new Size(683, 64);
		buttonX1.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonX1).TabIndex = 0;
		buttonX1.TextColor = Color.Red;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((UserControl)this).BorderStyle = (BorderStyle)2;
		((Control)this).Controls.Add((Control)(object)buttonX1);
		((Control)this).Name = "UcRssFeedStatus";
		((Control)this).Size = new Size(683, 64);
		((Control)this).ResumeLayout(false);
	}
}
