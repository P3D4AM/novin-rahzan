using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Viewer;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Dashboard.Viewer;

public class StiDashboardViewerForm : Form, IStiViewerForm
{
	private Container components;

	private StiReport report;

	public StiDashboardViewerControl ViewerControl { get; set; }

	public List<StiDashboard> Dashboards => StiDashboardViewerHelper.GetDashboardList(Report);

	[Browsable(false)]
	public virtual StiReport Report
	{
		get
		{
			return report;
		}
		set
		{
			if (value == report)
			{
				return;
			}
			report = value;
			if (value == null)
			{
				return;
			}
			string text = StiOptions.Viewer.GetViewerTitleWithMask((value.ReportAlias.Length == 0) ? null : value.ReportAlias);
			StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
			bool flag = !StiLicenseKeyValidator.IsValid(StiProductIdent.DbsWin, licenseKey);
			if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
			{
				flag = true;
			}
			if (!flag)
			{
				try
				{
					using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
					using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
					rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
					flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
				}
				catch (Exception)
				{
					flag = true;
				}
			}
			if (flag)
			{
				text += ", Trial Version";
			}
			((Control)this).Text = text;
			Build();
		}
	}

	[Browsable(false)]
	public object ViewerOwner
	{
		get
		{
			return ((Form)this).MdiParent;
		}
		set
		{
			((Form)this).MdiParent = (Form)((value is Form) ? value : null);
		}
	}

	public StiDashboardViewerForm()
		: this(null)
	{
	}

	public StiDashboardViewerForm(StiReport report)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		((Form)(object)this).InitializeNetCoreFont();
		InitializeComponent();
		((Form)this).WindowState = StiOptions.Viewer.Windows.ViewerWindowState;
		((Form)this).ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
		((Control)this).MinimumSize = StiOptions.Viewer.Windows.MinimumSize;
		((Form)this).Icon = StiOptions.Viewer.Windows.ViewerIcon ?? StiAppIcons.GetIcon();
		Report = report;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	public object GetOwner(object form)
	{
		Form val = (Form)((form is Form) ? form : null);
		if (val == null)
		{
			return null;
		}
		return val.MdiParent;
	}

	public void ShowViewer()
	{
		((Control)this).Show();
	}

	public void ShowDialogViewer()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).ShowDialog();
	}

	public void ShowViewer(object win32Window)
	{
		((Form)this).Show((IWin32Window)((win32Window is IWin32Window) ? win32Window : null));
	}

	public void ShowDialogViewer(object win32Window)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).ShowDialog((IWin32Window)((win32Window is IWin32Window) ? win32Window : null));
	}

	private void InitializeComponent()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((Form)this).ClientSize = new Size(938, 641);
		((Form)this).KeyPreview = true;
		((Control)this).Name = "StiDashboardViewerForm";
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)this).ResumeLayout(false);
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		IStiViewerAccessor viewerControl = ViewerControl;
		if (viewerControl.BlockFormClose)
		{
			viewerControl.BlockFormClose = false;
			((CancelEventArgs)(object)e).Cancel = true;
		}
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (ViewerControl != null)
		{
			ViewerControl.CleanUp();
			StiCacheCleaner.Clean(ViewerControl.Dashboard?.GetDictionary());
		}
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			if (ViewerControl != null && ViewerControl.ElementFullScreen.IsActivated)
			{
				IStiActionButtons stiActionButtons = ViewerControl.ElementFullScreen.CurrentControl as IStiActionButtons;
				stiActionButtons.CloseFullScreenButton.InvokeClick();
			}
			else
			{
				((Form)this).Close();
			}
		}
	}

	public void Build()
	{
		if (report != null && Dashboards.Any())
		{
			ViewerControl = StiDashboardViewerHelper.CreateViewerControl(Report);
			((Control)ViewerControl).Dock = (DockStyle)5;
			((Control)this).Controls.Add((Control)(object)ViewerControl);
		}
	}

	bool IStiViewerForm.get_ShowInTaskbar()
	{
		return ((Form)this).ShowInTaskbar;
	}

	void IStiViewerForm.set_ShowInTaskbar(bool value)
	{
		((Form)this).ShowInTaskbar = value;
	}

	bool IStiViewerForm.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void IStiViewerForm.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}
}
