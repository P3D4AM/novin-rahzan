using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using NPResourceManagement;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces;

namespace FormCustom.Forms;

public class UcLkExpireDisplay : UserControl, INpDisposable, IDisposable, IHasContext
{
	private LKInfo _lkObjects;

	private CancellationTokenSource _loadDataToken;

	private Color _lockBackColor = Color.Transparent;

	private ILockInformationManager _lockManager;

	private Package _package;

	private bool isFirstLoad = true;

	private IContainer components;

	private Button BtnDisplay;

	private Button LblDescription;

	private Panel panel1;

	public string ContextGuid { get; private set; }

	public UcLkExpireDisplay()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	public async Task SetContextGuidAsync(string contextGuid, ILockInformationManager lockManager, Package package)
	{
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				throw new Exception("contextGuid in ucExpireLKDisplay is null");
			}
			ContextGuid = contextGuid;
			_lockManager = lockManager;
			_package = package;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetContextGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcExpireLkDisplay.cs", 44);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue = await _lockManager.CheckLkWithWebServiceAsync(_package, ignoreHttpExceptions: true);
			if (!((Control)this).IsDisposed && !token.IsCancellationRequested)
			{
				_lkObjects = returnedSaveFuncInfoWithValue.value;
				await Task.Delay(1000);
				if (!((Control)this).IsDisposed && !token.IsCancellationRequested)
				{
					await FillLkExpireDate(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcExpireLkDisplay.cs", 64);
		}
	}

	private async Task FillLkExpireDate(CancellationToken token)
	{
		string errorMessage = "";
		if (_lkObjects == null)
		{
			errorMessage += "_lkObjects is null";
		}
		if (!(_lkObjects?.LKSerial).HasValue)
		{
			errorMessage += "LKSerial is null";
		}
		if ((_lkObjects?.LKSerial).HasValue)
		{
			errorMessage += $"LKSerial ={_lkObjects?.LKSerial}";
		}
		try
		{
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			string btnDisplayText = "?";
			string lblDescriptionText = "قفل تشخیص داده نشد.";
			Color lblDescriptionForeColor = Color.Red;
			_lockBackColor = Color.Yellow;
			if (_lkObjects != null && (_lkObjects?.LKSerial ?? 0) > 0)
			{
				if (_lkObjects.IsFailed)
				{
					btnDisplayText = "X";
					lblDescriptionText = "قفل مسدود شده با پشتیبانی تماس بگیرید";
					lblDescriptionForeColor = Color.Red;
					_lockBackColor = Color.Red;
				}
				else
				{
					int num = 0;
					if (_lkObjects.ExpireDate > DateTime.Now)
					{
						num = (int)Math.Floor((_lkObjects.ExpireDate - DateTime.Now).TotalDays);
					}
					if (num <= 5)
					{
						if (num > 0)
						{
							btnDisplayText = "!";
							lblDescriptionText = $"{num} روز تا پایان دوره پشتیبانی سرویس";
							lblDescriptionForeColor = Color.DarkOrange;
							_lockBackColor = Color.DarkOrange;
							Process.Start($"{TextResourceManagement.TextResource.CRMBaseURL}/extend/lk/{_lkObjects?.LKSerial}");
						}
						else
						{
							btnDisplayText = "X";
							lblDescriptionText = "دوره پشتیبانی شما به اتمام رسیده است";
							lblDescriptionForeColor = Color.Red;
							_lockBackColor = Color.PaleVioletRed;
							Process.Start($"{TextResourceManagement.TextResource.CRMBaseURL}/extend/lk/{_lkObjects?.LKSerial}");
						}
					}
					else
					{
						btnDisplayText = "";
						lblDescriptionText = $"{num} روز تا پایان پشتیبانی سرویس";
						lblDescriptionForeColor = Color.Green;
						_lockBackColor = Color.LawnGreen;
						if (!isFirstLoad)
						{
							Process.Start($"{TextResourceManagement.TextResource.CRMBaseURL}/extend/lk/{_lkObjects?.LKSerial}");
						}
						isFirstLoad = false;
					}
				}
			}
			errorMessage += " \n enter to ( while (!IsHandleCreated) ) statement in fillLkExpireDate";
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					errorMessage += $"line 124: IsDisposed = {((Control)this).IsDisposed} and token is cancelled = {token.IsCancellationRequested}";
					WebErrorLog.Instance.StartLog(errorMessage, "", "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcExpireLkDisplay.cs", 131);
					return;
				}
			}
			errorMessage += " \n end of while statement in fillLkExpireDate";
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				errorMessage += $" line 132: IsDisposed = {((Control)this).IsDisposed} and token is cancelled = {token.IsCancellationRequested}";
				WebErrorLog.Instance.StartLog(errorMessage, "", "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcExpireLkDisplay.cs", 139);
				return;
			}
			errorMessage += " \n enter to beginInvoke() statement in fillLkExpireDate";
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					((Control)BtnDisplay).Text = btnDisplayText;
					((Control)LblDescription).Text = lblDescriptionText;
					((Control)LblDescription).ForeColor = lblDescriptionForeColor;
					((Control)BtnDisplay).BackColor = _lockBackColor;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, errorMessage, "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcExpireLkDisplay.cs", 152);
				}
			});
			errorMessage += " \n end of beginInvoke statement in fillLkExpireDate";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, errorMessage, "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcExpireLkDisplay.cs", 156);
		}
	}

	private void BtnDisplay_Click(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)this).IsDisposed)
			{
				((Control)BtnDisplay).Enabled = false;
				_lockBackColor = Color.DarkGray;
				((Control)BtnDisplay).BackColor = _lockBackColor;
				((Control)LblDescription).Enabled = false;
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource(60000);
				CancellationToken token = _loadDataToken.Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnDisplay_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\UcExpireLkDisplay.cs", 173);
		}
		finally
		{
			((Control)BtnDisplay).Enabled = true;
			((Control)LblDescription).Enabled = true;
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
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
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
		((Control)LblDescription).Size = new Size(252, 28);
		((Control)LblDescription).TabIndex = 2;
		((Control)LblDescription).Text = "در حال بررسی قفل سخت افزاری";
		((ButtonBase)LblDescription).TextAlign = (ContentAlignment)16;
		((ButtonBase)LblDescription).UseVisualStyleBackColor = false;
		((Control)LblDescription).Click += BtnDisplay_Click;
		((Control)BtnDisplay).BackColor = Color.Transparent;
		((Control)BtnDisplay).Dock = (DockStyle)4;
		((ButtonBase)BtnDisplay).FlatStyle = (FlatStyle)1;
		((Control)BtnDisplay).Font = new Font("B Titr", 11f, (FontStyle)1);
		((Control)BtnDisplay).ForeColor = Color.Red;
		((ButtonBase)BtnDisplay).Image = (Image)(object)FormCustom.Properties.Resources._lock;
		((ButtonBase)BtnDisplay).ImageAlign = (ContentAlignment)512;
		((Control)BtnDisplay).Location = new Point(252, 0);
		((Control)BtnDisplay).Name = "BtnDisplay";
		((Control)BtnDisplay).RightToLeft = (RightToLeft)1;
		((Control)BtnDisplay).Size = new Size(46, 28);
		((Control)BtnDisplay).TabIndex = 1;
		((Control)BtnDisplay).Text = "X";
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
		((Control)this).Name = "UcLkExpireDisplay";
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
