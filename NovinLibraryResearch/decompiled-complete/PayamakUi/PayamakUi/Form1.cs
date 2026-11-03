using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FormCustom.Cache;
using PayamakUi.Classes;

namespace PayamakUi;

public class Form1 : CacheBaseForm
{
	private CancellationTokenSource token = new CancellationTokenSource(60000);

	private IContainer components;

	public Form1(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
	}

	private void ucSend1_Load(object sender, EventArgs e)
	{
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		Process.StopEngine();
		Process.ExitPayamak();
		token?.Cancel();
		token.Dispose();
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((Form)new FrmPayamakMain("", FrmPayamakMain.Enuc.OutBox)).ShowDialog();
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1376, 749);
		((Control)this).Name = "Form1";
		((Control)this).Text = "Form1";
		((Form)this).FormClosing += new FormClosingEventHandler(Form1_FormClosing);
		((Form)this).Load += Form1_Load;
		((Control)this).ResumeLayout(false);
	}
}
