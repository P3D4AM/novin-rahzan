using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Stimulsoft.Base;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Win.Dictionary;

public class StiQuickBooksOAuthForm : Form, IStiQuickBooksOAuthForm
{
	private WebBrowser webBrowser;

	private string authorizationUrl;

	private string redirectUrl;

	public string AuthorizationCode { get; set; }

	public string RealmId { get; set; }

	public string Error { get; set; }

	public StiQuickBooksOAuthForm(string authorizationUrl, string redirectUrl)
	{
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		this.authorizationUrl = authorizationUrl;
		this.redirectUrl = redirectUrl;
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		webBrowser = new WebBrowser();
		((Control)this).SuspendLayout();
		webBrowser.AllowWebBrowserDrop = false;
		((Control)webBrowser).Dock = (DockStyle)5;
		((Control)webBrowser).Location = new Point(0, 0);
		((Control)webBrowser).MinimumSize = new Size(40, 38);
		((Control)webBrowser).Name = "webBrowser";
		webBrowser.ScriptErrorsSuppressed = true;
		((Control)webBrowser).Size = new Size(400, 500);
		((Control)webBrowser).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(400, 500);
		((Control)this).Controls.Add((Control)(object)webBrowser);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiQuickBooksOAuthForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "quick.books";
		((Form)this).Load += This_Load;
		((Form)this).FormClosed += new FormClosedEventHandler(StiQuickBooksOAuthForm_FormClosed);
		((Control)this).ResumeLayout(false);
	}

	private void CheckEmulationWebBrowserIEVersion()
	{
		try
		{
			int major = webBrowser.Version.Major;
			int num = ((major >= 11) ? 11001 : (major switch
			{
				10 => 10001, 
				9 => 9999, 
				8 => 8888, 
				_ => 7000, 
			}));
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
			if (registryKey.GetValue(Process.GetCurrentProcess().ProcessName + ".exe") == null)
			{
				registryKey.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", num, RegistryValueKind.DWord);
			}
		}
		catch
		{
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		CheckEmulationWebBrowserIEVersion();
		webBrowser.Navigating += (WebBrowserNavigatingEventHandler)delegate(object sender2, WebBrowserNavigatingEventArgs e2)
		{
			Regex regex = new Regex("^(?:https?:\\/\\/)?(?:www\\.)?", RegexOptions.IgnoreCase);
			string text = regex.Replace(e2.Url?.ToString(), "");
			string value = regex.Replace(redirectUrl, "");
			if (text.StartsWith(value, StringComparison.InvariantCultureIgnoreCase))
			{
				Task.Run(() => HandleResponseAsync(e2.Url));
			}
		};
		webBrowser.Navigate(authorizationUrl);
	}

	private void StiQuickBooksOAuthForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Control)webBrowser).Invoke((Delegate)(Action)delegate
		{
			webBrowser.Stop();
			((Control)webBrowser).Visible = false;
		});
	}

	private async Task HandleResponseAsync(Uri url)
	{
		try
		{
			((Control)webBrowser).Invoke((Delegate)(Action)delegate
			{
				webBrowser.Stop();
				((Control)webBrowser).Visible = false;
			});
			await Task.Delay(100);
			NameValueCollection nameValueCollection = new NameValueCollection();
			string[] array = url.Query.Split(new char[1] { '&' });
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(new char[1] { '=' });
				if (array3.Length != 0)
				{
					string name = array3[0].Trim('?', ' ');
					string value = array3[1].Trim();
					nameValueCollection.Add(name, value);
				}
			}
			AuthorizationCode = nameValueCollection["code"];
			RealmId = nameValueCollection["realmId"];
			Error = nameValueCollection["error"];
			_ = nameValueCollection["state"];
			if (!string.IsNullOrEmpty(Error) || string.IsNullOrEmpty(AuthorizationCode) || string.IsNullOrEmpty(RealmId))
			{
				Console.Error.WriteLine(Error);
				Environment.ExitCode = -1;
				Application.Exit();
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
			}
		}
		catch
		{
			((Form)this).DialogResult = (DialogResult)2;
		}
	}

	DialogResult IStiQuickBooksOAuthForm.ShowDialog()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((Form)this).ShowDialog();
	}
}
