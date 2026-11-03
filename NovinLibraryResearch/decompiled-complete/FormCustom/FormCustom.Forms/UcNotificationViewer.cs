using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business.Global;
using EntityCache.Mapping.Global;
using FormCustom.Properties;
using PacketParser.Entities;
using PacketParser.Entities.Global;
using PacketParser.Entities.Interfaces;
using PacketParser.Global.PushNotification;
using PacketParser.Mapping;

namespace FormCustom.Forms;

public class UcNotificationViewer : UserControl, INpDisposable, IDisposable, IHasContext
{
	private CancellationTokenSource _tokenLoadData = new CancellationTokenSource();

	private Color _lockBackColor = Color.Transparent;

	private PushNotificationBusiness _selectedNotification;

	private IContainer components;

	private Button BtnDisplay;

	private Button LblDescription;

	private Panel panel1;

	private List<PushNotificationBusiness> NotificationList { get; set; }

	public string ContextGuid { get; private set; }

	public bool NewsView { get; set; }

	public bool MonasebatView { get; set; }

	public bool NotificationView { get; set; }

	public ENSource NotificationSource { get; set; }

	public UcNotificationViewer()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	public async Task SetContextGuidAsync(string contextGuid, ENSource source)
	{
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				throw new Exception("contextGuid in ucExpireLKDisplay is null");
			}
			ContextGuid = contextGuid;
			NotificationSource = source;
			await LoadData();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetContextGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcNotificationViewer.cs", 47);
		}
	}

	private async Task LoadData()
	{
		try
		{
			if (!((Control)this).IsDisposed)
			{
				_tokenLoadData?.Cancel();
				_tokenLoadData = new CancellationTokenSource();
				CancellationToken token = _tokenLoadData.Token;
				await RefreshNotificationAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcNotificationViewer.cs", 60);
		}
	}

	private async Task SaveNewNotificationToLocal(CancellationToken token)
	{
		_ = 3;
		try
		{
			if (((Control)this).IsDisposed || token.IsCancellationRequested || !NotificationView)
			{
				return;
			}
			List<PushNotificationBusiness> list = await (await WebPushNotificationClient.GetFromApi(await PushNotificationBusiness.GetLastModifiedDateAsync(ContextGuid), NotificationSource)).value.MapListAsync<WebPushNotification, PushNotificationBusiness>(ContextGuid, PushNotificationMapper.Instance, null, "", 20);
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			foreach (PushNotificationBusiness item in list)
			{
				await item.SaveAsync(10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveNewNotificationToLocal", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcNotificationViewer.cs", 78);
		}
	}

	private async Task UpdatePushNotificationFromApi(CancellationToken token)
	{
		_ = 1;
		try
		{
			await SaveNewNotificationToLocal(token);
			NotificationList = (await PushNotificationBusiness.GetActiveNotificationAsync(ContextGuid, NotificationSource))?.ToList() ?? new List<PushNotificationBusiness>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdatePushNotificationFromApi", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcNotificationViewer.cs", 89);
		}
	}

	private void BtnDisplay_MouseHover(object sender, EventArgs e)
	{
		((Control)BtnDisplay).BackColor = Color.Aqua;
	}

	private void BtnDisplay_MouseLeave(object sender, EventArgs e)
	{
		((Control)BtnDisplay).BackColor = _lockBackColor;
	}

	private async Task RefreshNotificationAsync(CancellationToken token)
	{
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			await UpdatePushNotificationFromApi(token);
			DateTime lastUpdate = DateTime.Now;
			if (((Control)this).IsDisposed || token.IsCancellationRequested || !NotificationView || !(NotificationList?.Any() ?? false))
			{
				return;
			}
			while (!((Control)this).IsDisposed && !token.IsCancellationRequested)
			{
				if (lastUpdate.AddMinutes(5.0) < DateTime.Now)
				{
					await UpdatePushNotificationFromApi(token);
					lastUpdate = DateTime.Now;
				}
				foreach (PushNotificationBusiness notifaction in NotificationList)
				{
					if (((Control)this).IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					((Control)this).BeginInvoke((Delegate)(Action)delegate
					{
						try
						{
							if (!((Control)this).IsDisposed && !token.IsCancellationRequested)
							{
								switch (notifaction.Type)
								{
								case EnPushNotificationType.News:
									if (NewsView)
									{
										((Control)LblDescription).Text = "(" + notifaction.Title + "):  " + notifaction.Message;
										((Control)LblDescription).ForeColor = Color.Black;
										((Control)BtnDisplay).Enabled = notifaction.HelperUri != null;
									}
									break;
								case EnPushNotificationType.Bug:
									((Control)LblDescription).Text = notifaction.Message;
									((Control)LblDescription).ForeColor = Color.Red;
									((Control)BtnDisplay).Enabled = notifaction.HelperUri != null;
									break;
								case EnPushNotificationType.Warning:
									((Control)LblDescription).Text = notifaction.Message;
									((Control)LblDescription).ForeColor = Color.OrangeRed;
									((Control)BtnDisplay).Enabled = notifaction.HelperUri != null;
									break;
								case EnPushNotificationType.UpdateVersion:
									((Control)LblDescription).Text = "(" + notifaction.Title + "):  " + notifaction.Message;
									((Control)LblDescription).ForeColor = Color.Blue;
									((Control)BtnDisplay).Enabled = notifaction.HelperUri != null;
									break;
								case EnPushNotificationType.Monasbat:
									if (MonasebatView)
									{
										((Control)LblDescription).Text = "(" + notifaction.Title + "):  " + notifaction.Message;
										((Control)LblDescription).ForeColor = Color.Green;
										((Control)BtnDisplay).Enabled = notifaction.HelperUri != null;
									}
									break;
								}
							}
						}
						catch (OperationCanceledException)
						{
						}
						catch (Exception ex4)
						{
							WebErrorLog.Instance.StartLog(ex4, "", "RefreshNotificationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcNotificationViewer.cs", 167);
						}
					});
					_selectedNotification = notifaction;
					await Task.Delay(10000);
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "RefreshNotificationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcNotificationViewer.cs", 176);
		}
	}

	private async void LblDescription_Click(object sender, EventArgs e)
	{
		if (((Control)this).IsDisposed)
		{
			return;
		}
		((Control)LblDescription).Enabled = false;
		((Control)BtnDisplay).Enabled = false;
		((Control)BtnDisplay).BackColor = Color.DarkGray;
		try
		{
			_tokenLoadData?.Cancel();
			LoadData();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblDescription_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcNotificationViewer.cs", 193);
		}
		finally
		{
			((Control)LblDescription).Enabled = true;
			((Control)BtnDisplay).Enabled = true;
		}
	}

	private void BtnDisplay_Click(object sender, EventArgs e)
	{
		try
		{
			if (_selectedNotification != null && !string.IsNullOrEmpty(_selectedNotification.HelperUri))
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = _selectedNotification.HelperUri,
					UseShellExecute = true
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnDisplay_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcNotificationViewer.cs", 216);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		LblDescription = new Button();
		BtnDisplay = new Button();
		panel1 = new Panel();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)LblDescription).BackColor = Color.Transparent;
		((Control)LblDescription).Dock = (DockStyle)5;
		((ButtonBase)LblDescription).FlatStyle = (FlatStyle)1;
		((Control)LblDescription).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)LblDescription).ForeColor = Color.Black;
		((ButtonBase)LblDescription).ImageAlign = (ContentAlignment)2;
		((Control)LblDescription).Location = new Point(0, 0);
		((Control)LblDescription).Name = "LblDescription";
		((Control)LblDescription).Size = new Size(258, 28);
		((Control)LblDescription).TabIndex = 2;
		((ButtonBase)LblDescription).TextAlign = (ContentAlignment)16;
		((ButtonBase)LblDescription).UseVisualStyleBackColor = false;
		((Control)LblDescription).Click += LblDescription_Click;
		((Control)LblDescription).MouseLeave += BtnDisplay_MouseLeave;
		((Control)BtnDisplay).BackColor = Color.Transparent;
		((Control)BtnDisplay).Dock = (DockStyle)4;
		((ButtonBase)BtnDisplay).FlatStyle = (FlatStyle)1;
		((Control)BtnDisplay).Font = new Font("B Titr", 11f, (FontStyle)1);
		((Control)BtnDisplay).ForeColor = Color.Red;
		((ButtonBase)BtnDisplay).Image = (Image)(object)FormCustom.Properties.Resources.Attach1;
		((Control)BtnDisplay).Location = new Point(258, 0);
		((Control)BtnDisplay).Name = "BtnDisplay";
		((Control)BtnDisplay).RightToLeft = (RightToLeft)1;
		((Control)BtnDisplay).Size = new Size(40, 28);
		((Control)BtnDisplay).TabIndex = 1;
		((ButtonBase)BtnDisplay).TextAlign = (ContentAlignment)2;
		((ButtonBase)BtnDisplay).UseVisualStyleBackColor = false;
		((Control)BtnDisplay).Click += BtnDisplay_Click;
		((Control)BtnDisplay).MouseLeave += BtnDisplay_MouseLeave;
		((Control)BtnDisplay).MouseHover += BtnDisplay_MouseHover;
		((Control)panel1).Controls.Add((Control)(object)LblDescription);
		((Control)panel1).Controls.Add((Control)(object)BtnDisplay);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(298, 28);
		((Control)panel1).TabIndex = 4;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Name = "UcNotificationViewer";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(298, 28);
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	bool INpDisposable.get_IsDisposed()
	{
		return ((Control)this).IsDisposed;
	}
}
