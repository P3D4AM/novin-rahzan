using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Cache;
using NPResourceManagement;
using PacketParser.Entities;

namespace FormCustom.Forms;

public class FrmRunAsAdmin : CacheBaseForm
{
	private bool _isDisposed;

	private IContainer components;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public FrmRunAsAdmin(string context)
		: base(context)
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmRunAsAdmin.cs", 33);
		}
	}

	private void FrmRunAsAdmin_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void FrmRunAsAdmin_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)ImageResourceManagement.RunAdmin;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(800, 450);
		((Control)this).Name = "FrmRunAsAdmin";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "اجرا با دسترسی Administrator";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosing += new FormClosingEventHandler(FrmRunAsAdmin_FormClosing);
		((Form)this).FormClosed += new FormClosedEventHandler(FrmRunAsAdmin_FormClosed);
		((Control)this).ResumeLayout(false);
	}
}
