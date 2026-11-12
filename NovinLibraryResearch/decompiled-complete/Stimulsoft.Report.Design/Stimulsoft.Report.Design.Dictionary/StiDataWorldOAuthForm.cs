using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataWorldOAuthForm : StiFormUX
{
	private string redirectUri = "https://reports.stimulsoft.com";

	private string authorizationUrl = "https://data.world/oauth/authorize";

	private string tokenUrl = "https://data.world/oauth/access_token";

	private string clientId = "7011d98eeb82b9c1";

	private string clientSecret = "secret-n0kCrYEfS8IVL5SJjBMjJHG6";

	private string responseType = "code";

	private IContainer components;

	private WebBrowser webBrowser;

	public string Token { get; set; }

	public string Error { get; set; }

	public string Message { get; set; }

	public StiDataWorldOAuthForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		InitializeComponent();
		((Form)this).HelpButton = false;
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
			RegistryKey val = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", (RegistryKeyPermissionCheck)2);
			try
			{
				if (val.GetValue(Process.GetCurrentProcess().ProcessName + ".exe") == null)
				{
					val.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", (object)num, (RegistryValueKind)4);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		CheckEmulationWebBrowserIEVersion();
		bool runOnce = true;
		webBrowser.ProgressChanged += (WebBrowserProgressChangedEventHandler)delegate(object s, WebBrowserProgressChangedEventArgs e2)
		{
			if (runOnce && e2.CurrentProgress == e2.MaximumProgress)
			{
				Uri url = webBrowser.Url;
				if ((object)url != null && url.ToString().StartsWith(redirectUri, StringComparison.InvariantCultureIgnoreCase))
				{
					runOnce = false;
					Task.Run(() => HandleResponseAsync(webBrowser.Url));
				}
			}
		};
		webBrowser.Navigate(CreateAuthorizationUrl());
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
			string text2 = nameValueCollection["code"];
			string value2 = nameValueCollection["error"];
			_ = nameValueCollection["state"];
			if (!string.IsNullOrEmpty(value2))
			{
				Console.Error.WriteLine(value2);
				Environment.ExitCode = -1;
				Application.Exit();
				return;
			}
			using WebClient webClient = GetDefaultWebClient();
			string text3 = "code=" + text2 + "&client_id=" + clientId + "&client_secret=" + clientSecret + "&grant_type=authorization_code";
			new Uri(tokenUrl + "?" + text3);
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
			string json = webClient.UploadString(tokenUrl, text3);
			dynamic val = JObject.Parse(json);
			if (val.access_token != null)
			{
				Token = val.access_token.ToString();
				((Form)this).DialogResult = (DialogResult)1;
			}
			if (val.error != null && val.message != null)
			{
				Error = val.error.ToString();
				Message = val.message.ToString();
				((Form)this).DialogResult = (DialogResult)2;
			}
		}
		catch
		{
			((Form)this).DialogResult = (DialogResult)2;
		}
	}

	private WebClient GetDefaultWebClient()
	{
		WebClient webClient = new WebClient();
		webClient.Proxy = StiProxy.GetProxy();
		webClient.Encoding = StiBaseOptions.WebClientEncoding;
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		return webClient;
	}

	private string CreateAuthorizationUrl()
	{
		UriBuilder uriBuilder = new UriBuilder(authorizationUrl);
		uriBuilder.Query = "client_id=" + clientId + "&redirect_uri=" + Uri.EscapeDataString(redirectUri) + "&response_type=" + responseType;
		return uriBuilder.Uri.ToString();
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
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		webBrowser = new WebBrowser();
		((Control)this).SuspendLayout();
		webBrowser.AllowWebBrowserDrop = false;
		((Control)webBrowser).Dock = (DockStyle)5;
		((Control)webBrowser).Location = new Point(0, 0);
		((Control)webBrowser).Margin = new Padding(6, 6, 6, 6);
		((Control)webBrowser).MinimumSize = new Size(40, 38);
		((Control)webBrowser).Name = "webBrowser";
		((Control)webBrowser).Size = new Size(484, 611);
		((Control)webBrowser).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(484, 611);
		((Control)this).Controls.Add((Control)(object)webBrowser);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataWorldOAuthForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "data.world";
		((Form)this).Load += This_Load;
		((Control)this).ResumeLayout(false);
	}
}
