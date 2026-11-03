using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser.Entities;
using SmartAssembly.Attributes;
using Softgroup.NetCalculator;

namespace FormCustom.Forms;

[Serializable]
[DoNotPruneType]
public class FrmCalc : CacheBaseForm
{
	private bool _isDisposed;

	private IContainer components;

	private NetCalculator netCalculator1;

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

	public FrmCalc(string context)
		: base(context)
	{
		try
		{
			InitializeComponent();
			License.LicenseName = "TEAM Lz0";
			License.LicenseUser = "Lz0@Softgroup.net";
			License.LicenseKey = "S9AKAIL96LBZEYA9ROJMROLAB";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmCalc.cs", 22);
		}
	}

	private void netCalculator1_CloseClick()
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "netCalculator1_CloseClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmCalc.cs", 32);
		}
	}

	private void FRMCalc_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode == 27)
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMCalc_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmCalc.cs", 55);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmCalc.cs", 70);
		}
	}

	private void FrmCalc_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		netCalculator1 = new NetCalculator();
		((Control)this).SuspendLayout();
		((Control)netCalculator1).BackColor = Color.SkyBlue;
		((Control)netCalculator1).Location = new Point(0, 0);
		((Control)netCalculator1).Name = "netCalculator1";
		netCalculator1.ResultBackColor = Color.White;
		((Control)netCalculator1).Size = new Size(236, 316);
		((Control)netCalculator1).TabIndex = 0;
		netCalculator1.CloseClick += new CloseClickEventHandler(netCalculator1_CloseClick);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(236, 316);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)netCalculator1);
		((Form)this).KeyPreview = true;
		((Control)this).MaximumSize = new Size(252, 332);
		((Control)this).MinimumSize = new Size(252, 332);
		((Control)this).Name = "FrmCalc";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(FrmCalc_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(FRMCalc_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
