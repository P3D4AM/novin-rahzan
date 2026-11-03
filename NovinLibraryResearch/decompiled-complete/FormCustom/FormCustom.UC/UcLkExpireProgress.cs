using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache.Business.LkService;
using NPResourceManagement;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace FormCustom.UC;

public class UcLkExpireProgress : UserControl
{
	private CancellationTokenSource _loadDataToken;

	private LKInfo _lkObjects;

	private IContainer components;

	private PanelEx pnlHeader;

	private ProgressBar progressBar1;

	private Label lblTitle;

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
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpireProgress.cs", 35);
		}
		return Task.CompletedTask;
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
			ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue = await LkActivatorBaseBusiness.GetLkInfoAsync(ContextGuid, token);
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
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpireProgress.cs", 51);
		}
	}

	private async Task FillLkExpireDate(CancellationToken token)
	{
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
							desc = $"تاریخ اتمام پشتیبانی: {_lkObjects.ExpireDateSh} - {dayLeft} روز مانده";
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
						desc = $"تاریخ اتمام پشتیبانی: {_lkObjects.ExpireDateSh} - {dayLeft} روز مانده";
						Color = Color.FromArgb(0, 112, 37);
					}
				}
			}
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
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
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					((Control)lblTitle).ForeColor = Color;
					((Control)lblTitle).Text = desc;
					if (dayLeft <= 365)
					{
						progressBar1.Value = dayLeft;
					}
					else
					{
						progressBar1.Value = 365;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpireProgress.cs", 97);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillLkExpireDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpireProgress.cs", 100);
		}
	}

	public UcLkExpireProgress()
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
			WebErrorLog.Instance.StartLog(ex, "", "lblTitle_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcLkExpireProgress.cs", 115);
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		lblTitle = new Label();
		progressBar1 = new ProgressBar();
		((Control)pnlHeader).SuspendLayout();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)lblTitle);
		((Control)pnlHeader).Controls.Add((Control)(object)progressBar1);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(307, 45);
		pnlHeader.Style.BackColor1.Color = Color.FromArgb(241, 245, 253);
		pnlHeader.Style.BackColor2.Color = Color.FromArgb(241, 245, 253);
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		((Control)pnlHeader).TabIndex = 143;
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).BackColor = Color.Transparent;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).Location = new Point(3, 1);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).RightToLeft = (RightToLeft)1;
		((Control)lblTitle).Size = new Size(301, 23);
		((Control)lblTitle).TabIndex = 6;
		((Control)lblTitle).Text = "---";
		((Control)lblTitle).Click += lblTitle_Click;
		((Control)progressBar1).Anchor = (AnchorStyles)14;
		((Control)progressBar1).Location = new Point(0, 26);
		progressBar1.Maximum = 365;
		((Control)progressBar1).Name = "progressBar1";
		((Control)progressBar1).Size = new Size(307, 18);
		((Control)progressBar1).TabIndex = 5;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcLkExpireProgress";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(307, 45);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
