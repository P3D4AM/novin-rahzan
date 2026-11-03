using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business.LkService;
using FormCustom.Properties;
using NPResourceManagement;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace FormCustom.UC;

public class UcLkExpire : UserControl
{
	private CancellationTokenSource _loadDataToken;

	private LkActivatorBaseBusiness _lockManager;

	private Package _package;

	private LKInfo _lkObjects;

	private IContainer components;

	private PanelEx pnlHeader;

	private PictureBox picBox;

	private Label lblTitle;

	private CircularProgress progressBar1;

	private Panel panel1;

	public string ContextGuid { get; private set; }

	public Color Color { get; private set; } = Color.FromArgb(41, 204, 161);


	public Task InitAsync(string contextGuid, Package package)
	{
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				throw new Exception("contextGuid in ucExpireLKDisplay is null");
			}
			ContextGuid = contextGuid;
			_package = package;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpire.cs", 38);
		}
		return Task.CompletedTask;
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue = await LkActivatorBaseBusiness.GetLkInfoAsync(ContextGuid, token);
			if (!((Control)this).IsDisposed && !token.IsCancellationRequested && !((Control)this).IsDisposed && !token.IsCancellationRequested)
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
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpire.cs", 54);
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
			string desc = "قفل تشخیص داده نشد.";
			int dayLeft = 0;
			if (_lkObjects != null && (_lkObjects?.LKSerial ?? 0) > 0)
			{
				if (_lkObjects.IsFailed)
				{
					desc = "قفل مسدود شده با پشتیبانی تماس بگیرید";
					Color = Color.Red;
				}
				else
				{
					if (_lkObjects.ExpireDate > DateTime.Now)
					{
						dayLeft = (int)Math.Floor((_lkObjects.ExpireDate - DateTime.Now).TotalDays);
					}
					if (dayLeft <= 60)
					{
						if (dayLeft > 0)
						{
							desc = $"{dayLeft} روز تا پایان دوره پشتیبانی سرویس";
							Color = Color.FromArgb(253, 111, 0);
						}
						else
						{
							desc = "دوره پشتیبانی شما به اتمام رسیده است";
							Color = Color.FromArgb(252, 79, 132);
						}
					}
					else
					{
						desc = $"{dayLeft} روز تا پایان پشتیبانی سرویس";
						Color = Color.FromArgb(41, 204, 161);
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
					WebErrorLog.Instance.StartLog(errorMessage, "", "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpire.cs", 103);
					return;
				}
			}
			errorMessage += " \n end of while statement in fillLkExpireDate";
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				errorMessage += $" line 132: IsDisposed = {((Control)this).IsDisposed} and token is cancelled = {token.IsCancellationRequested}";
				WebErrorLog.Instance.StartLog(errorMessage, "", "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpire.cs", 111);
				return;
			}
			errorMessage += " \n enter to beginInvoke() statement in fillLkExpireDate";
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					((Control)lblTitle).ForeColor = Color;
					progressBar1.ProgressColor = Color;
					if (dayLeft <= 365)
					{
						progressBar1.Value = dayLeft;
					}
					else
					{
						progressBar1.Value = 365;
					}
					new ToolTip().SetToolTip((Control)(object)progressBar1, desc);
					if (dayLeft != 0)
					{
						progressBar1.ProgressText = $"{dayLeft}";
					}
					if (dayLeft > 60)
					{
						((Control)picBox).Visible = true;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, errorMessage, "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpire.cs", 129);
				}
			});
			errorMessage += " \n end of beginInvoke statement in fillLkExpireDate";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, errorMessage, "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpire.cs", 133);
		}
	}

	public UcLkExpire()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblTitle_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start($"{TextResourceManagement.TextResource.CRMBaseURL}/extend/lk/{_lkObjects?.LKSerial}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblTitle_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpire.cs", 148);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		lblTitle = new Label();
		progressBar1 = new CircularProgress();
		picBox = new PictureBox();
		panel1 = new Panel();
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)picBox).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)panel1);
		((Control)pnlHeader).Controls.Add((Control)(object)picBox);
		((Control)pnlHeader).Controls.Add((Control)(object)lblTitle);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(143, 182);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 143;
		((Control)lblTitle).Anchor = (AnchorStyles)9;
		((Control)lblTitle).AutoSize = true;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblTitle).Location = new Point(39, 11);
		((Control)lblTitle).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(100, 24);
		((Control)lblTitle).TabIndex = 3;
		((Control)lblTitle).Text = "[اعتبار پشتیبانی]";
		lblTitle.TextAlign = (ContentAlignment)64;
		((Control)lblTitle).Click += lblTitle_Click;
		progressBar1.BackgroundStyle.CornerDiameter = 25;
		progressBar1.BackgroundStyle.CornerType = eCornerType.Rounded;
		progressBar1.BackgroundStyle.UseMnemonic = true;
		progressBar1.BackgroundStyle.WordWrap = true;
		((Control)progressBar1).Dock = (DockStyle)5;
		((Control)progressBar1).Location = new Point(0, 0);
		progressBar1.Maximum = 365;
		((Control)progressBar1).Name = "progressBar1";
		progressBar1.ProgressBarType = eCircularProgressType.Donut;
		progressBar1.ProgressColor = Color.FromArgb(30, 196, 160);
		progressBar1.ProgressTextColor = Color.FromArgb(41, 204, 161);
		progressBar1.ProgressTextFormat = "";
		progressBar1.ProgressTextVisible = true;
		((Control)progressBar1).Size = new Size(137, 138);
		progressBar1.Style = eDotNetBarStyle.OfficeXP;
		((Control)progressBar1).TabIndex = 5;
		picBox.Image = (Image)(object)FormCustom.Properties.Resources.LkExpire;
		((Control)picBox).Location = new Point(3, 9);
		((Control)picBox).Name = "picBox";
		((Control)picBox).Size = new Size(29, 26);
		picBox.SizeMode = (PictureBoxSizeMode)1;
		picBox.TabIndex = 4;
		picBox.TabStop = false;
		((Control)picBox).Visible = false;
		((Control)panel1).Anchor = (AnchorStyles)15;
		((Control)panel1).Controls.Add((Control)(object)progressBar1);
		((Control)panel1).Location = new Point(3, 41);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(137, 138);
		((Control)panel1).TabIndex = 6;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcLkExpire";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(143, 182);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)pnlHeader).PerformLayout();
		((ISupportInitialize)picBox).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
