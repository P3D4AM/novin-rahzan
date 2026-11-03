using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using FormCustom.Properties;
using FormCustom.UC;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.UrlBuilder;

public class FrmUrlBuilder : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _checkUrlToken;

	private List<UrlView> _orginalList;

	private List<string> defaultUrls = new List<string>(5) { "http://novinrank.ir/hubCore", "https://novinrank.ir/HubCore", "http://novinrank.ir:8040/HubCore", "https://webservice.NpMail.ir/HubCore", "http://webservice.NpMail.ir/HubCore" };

	private IContainer components;

	private UcGroupBox panel1;

	private Label label1;

	private ComboBox CmbUrls;

	private UcGroupBox panel3;

	private ComboBoxEx comboBoxEx1;

	private PictureBox pictestUrl;

	private BindingSource urlViewBindingSource;

	private DataGridView GrdAddress;

	private DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn ordinalDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn StatusConects;

	private PictureBox picUp;

	private PictureBox picDown;

	private PictureBox picAdd;

	private PictureBox picDelete;

	private UcButtons btnCancel;

	private UcButtons btnOk;

	public IEnumerable<string> InputStringList { get; private set; }

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

	public UrlView SelectedUrl => ((UrlView)urlViewBindingSource.Current) ?? new UrlView();

	public List<string> GetList => _orginalList?.OrderBy((UrlView x) => x.Ordinal)?.Select((UrlView x) => x.Url)?.ToList();

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			SortableBindingList<UrlView> listData = _orginalList?.OrderByDescending((UrlView p) => p.Ordinal)?.ToSortableBindingList(token);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						urlViewBindingSource.DataSource = listData?.ToSortableBindingList(token);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 55);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 58);
		}
	}

	private async Task CheckUrlAsync(string url, CancellationToken token)
	{
		try
		{
			ReturnedSaveFuncInfo validUrlStatus = new ReturnedSaveFuncInfo();
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = validUrlStatus;
			returnedSaveFuncInfo.AddReturnedValue(await UrlView.CheckUrlValidationAsync(url, token), ReturnedState.None, "CheckUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 66);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						if (validUrlStatus.HasError)
						{
							pictestUrl.Image = pictestUrl.ErrorImage;
						}
						else
						{
							pictestUrl.Image = pictestUrl.InitialImage;
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "CheckUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 85);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 88);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_checkUrlToken?.Cancel();
				_checkUrlToken?.Dispose();
				_checkUrlToken = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 108);
		}
	}

	public FrmUrlBuilder(string ContextGuid, IEnumerable<string> inputStringList, Font font = null)
		: base(ContextGuid)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			if (font != null)
			{
				GrdAddress.Initialize(font);
			}
			_orginalList = new List<UrlView>();
			InputStringList = inputStringList?.Select((string p) => p.FixString())?.Distinct();
			ToolTip val = new ToolTip();
			val.SetToolTip((Control)(object)picDelete, "حذف".GetTranslate());
			val.SetToolTip((Control)(object)picAdd, "افزودن".GetTranslate());
			val.SetToolTip((Control)(object)picUp, "انتقال به بالا".GetTranslate());
			val.SetToolTip((Control)(object)picDown, "انتقال به پایین".GetTranslate());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 127);
		}
	}

	private void CmbUrls_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (!IsDisposed)
			{
				_checkUrlToken?.Cancel();
				_checkUrlToken = new CancellationTokenSource(60000);
				CancellationToken token = _checkUrlToken.Token;
				string str = ((Control)CmbUrls).Text;
				Task.Run(() => CheckUrlAsync(str, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CmbUrls_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 141);
		}
	}

	private void CmbUrls_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (!IsDisposed)
			{
				_checkUrlToken?.Cancel();
				_checkUrlToken = new CancellationTokenSource(60000);
				CancellationToken token = _checkUrlToken.Token;
				string str = ((Control)CmbUrls).Text;
				Task.Run(() => CheckUrlAsync(str, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CmbUrls_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 154);
		}
	}

	private void pictestUrl_Click(object sender, EventArgs e)
	{
		try
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			ComboBox cmbUrls = CmbUrls;
			processStartInfo.FileName = ((cmbUrls != null) ? ((Control)cmbUrls).Text : null) ?? "";
			processStartInfo.UseShellExecute = true;
			Process.Start(processStartInfo);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictestUrl_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 166);
		}
	}

	private async void FrmUrlBuilder_Load(object sender, EventArgs e)
	{
		try
		{
			if (!(InputStringList?.Any() ?? false))
			{
				InputStringList = defaultUrls?.ToList();
			}
			short ordinal = 1;
			foreach (string inputString in InputStringList)
			{
				UrlView urlView = new UrlView
				{
					Ordinal = ordinal
				};
				await urlView.SetUrlAsync(inputString);
				_orginalList.Add(urlView);
				ordinal++;
			}
			CmbUrls.DataSource = defaultUrls;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, Task.Run(() => LoadDataAsync(token)), freezeForm: false, EnWaiterType.Circle, null, "FrmUrlBuilder_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FrmUrlBuilder_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 188);
		}
	}

	private async void picAdd_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			try
			{
				((Control)GrdAddress).Focus();
				if (IsDisposed || !((Control)picAdd).Enabled)
				{
					goto end_IL_0066;
				}
				((Control)picAdd).Enabled = false;
				string url = ((Control)CmbUrls).Text;
				if (_orginalList.Any((UrlView p) => p.Url.ToLower() == url.ToLower()))
				{
					await this.ShowWarningAsync("آدرس وارد شده (" + url + ") تکراری می باشد");
					goto end_IL_0066;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await UrlView.CheckUrlValidationAsync(url, default(CancellationToken)), ReturnedState.None, "picAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 204);
				if (ret.HasError)
				{
					goto end_IL_0066;
				}
				int num = ((_orginalList?.Any() ?? false) ? _orginalList.Max((UrlView p) => p.Ordinal) : 0);
				if (string.IsNullOrEmpty(((Control)CmbUrls).Text))
				{
					goto end_IL_0066;
				}
				UrlView urlView = new UrlView
				{
					Ordinal = num + 1
				};
				await urlView.SetUrlAsync(((Control)CmbUrls).Text);
				_orginalList.Add(urlView);
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource(60000);
				CancellationToken token = _loadDataToken.Token;
				new Waiter("بارگذاری داده", (Form)(object)this, Task.Run(() => LoadDataAsync(token)), freezeForm: false, EnWaiterType.Circle, null, "picAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs");
				goto end_IL_004f;
				end_IL_0066:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "picAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 221);
				ret.AddReturnedValue(ex, "", "picAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 222);
				goto end_IL_004f;
			}
			end_IL_004f:;
		}
		finally
		{
			if (!IsDisposed)
			{
				((Control)picAdd).Enabled = true;
				if (ret.HasError)
				{
					await this.ShowReturnedSavedFuncInfoAsync(ret, "خطا در اضافه کردن آدرس".GetTranslate(), "picAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs");
				}
			}
		}
	}

	private void picDelete_Click(object sender, EventArgs e)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Invalid comparison between I4 and Unknown
		try
		{
			((Control)GrdAddress).Focus();
			if (IsDisposed || urlViewBindingSource.Current == null || !((Control)picDelete).Enabled)
			{
				return;
			}
			((Control)picDelete).Enabled = false;
			if (6 == (int)MessageBox.Show("آیا از حذف آیتم مطمئن هستید".GetTranslate(), "هشدار".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)48, (MessageBoxDefaultButton)0))
			{
				_orginalList.Remove(SelectedUrl);
			}
			int num = 0;
			foreach (UrlView orginal in _orginalList)
			{
				orginal.Ordinal = num;
				num++;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, Task.Run(() => LoadDataAsync(token)), freezeForm: false, EnWaiterType.Circle, null, "picDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 256);
		}
		finally
		{
			((Control)picDelete).Enabled = true;
		}
	}

	private void picUp_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)GrdAddress).Focus();
			if (IsDisposed || urlViewBindingSource.Current == null || !((Control)picUp).Enabled)
			{
				return;
			}
			((Control)picUp).Enabled = false;
			int num = SelectedUrl.Ordinal + 1;
			foreach (UrlView orginal in _orginalList)
			{
				if (num == orginal.Ordinal)
				{
					orginal.Ordinal--;
					SelectedUrl.Ordinal++;
				}
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, Task.Run(() => LoadDataAsync(token)), freezeForm: false, EnWaiterType.Circle, null, "picUp_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picUp_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 280);
		}
		finally
		{
			((Control)picUp).Enabled = true;
		}
	}

	private void picDown_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)GrdAddress).Focus();
			if (IsDisposed || urlViewBindingSource.Current == null || !((Control)picDown).Enabled)
			{
				return;
			}
			((Control)picDown).Enabled = false;
			int num = SelectedUrl.Ordinal - 1;
			foreach (UrlView orginal in _orginalList)
			{
				if (num == orginal.Ordinal)
				{
					orginal.Ordinal++;
					SelectedUrl.Ordinal--;
				}
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, Task.Run(() => LoadDataAsync(token)), freezeForm: false, EnWaiterType.Circle, null, "picDown_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picDown_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 304);
		}
		finally
		{
			((Control)picDown).Enabled = true;
		}
	}

	private void btnCancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 314);
		}
	}

	private void FrmUrlBuilder_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void FrmUrlBuilder_KeyDown(object sender, KeyEventArgs e)
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
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					btnOk.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FrmUrlBuilder_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 327);
		}
	}

	private void btnOk_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\FrmUrlBuilder.cs", 336);
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Expected O, but got Unknown
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_0b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Expected O, but got Unknown
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c41: Expected O, but got Unknown
		//IL_0cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdc: Expected O, but got Unknown
		//IL_0d1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d89: Expected O, but got Unknown
		//IL_0eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec1: Expected O, but got Unknown
		//IL_0ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ede: Expected O, but got Unknown
		//IL_0ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f35: Expected O, but got Unknown
		//IL_0f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f59: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmUrlBuilder));
		panel1 = new UcGroupBox();
		pictestUrl = new PictureBox();
		label1 = new Label();
		CmbUrls = new ComboBox();
		panel3 = new UcGroupBox();
		GrdAddress = new DataGridView();
		urlDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		ordinalDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		StatusConects = new DataGridViewTextBoxColumn();
		urlViewBindingSource = new BindingSource(components);
		comboBoxEx1 = new ComboBoxEx();
		picUp = new PictureBox();
		picDown = new PictureBox();
		picAdd = new PictureBox();
		picDelete = new PictureBox();
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		((ISupportInitialize)pictestUrl).BeginInit();
		((ISupportInitialize)GrdAddress).BeginInit();
		((ISupportInitialize)urlViewBindingSource).BeginInit();
		((ISupportInitialize)picUp).BeginInit();
		((ISupportInitialize)picDown).BeginInit();
		((ISupportInitialize)picAdd).BeginInit();
		((ISupportInitialize)picDelete).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panel1).BackColor = SystemColors.ActiveCaption;
		((UserControl)panel1).BorderStyle = (BorderStyle)2;
		panel1.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		panel1.HeadHeight = 40;
		panel1.IsShowHeader = false;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Margin = new Padding(4);
		((Control)panel1).Name = "panel1";
		((Control)panel1).RightToLeft = (RightToLeft)1;
		((Control)panel1).Size = new Size(661, 56);
		((Control)panel1).TabIndex = 0;
		panel1.Title = "[عنوان]";
		((Control)pictestUrl).BackColor = Color.Lime;
		((Control)pictestUrl).Enabled = false;
		pictestUrl.ErrorImage = (Image)(object)FormCustom.Properties.Resources.Catch_Disconnect;
		pictestUrl.Image = (Image)(object)FormCustom.Properties.Resources.Catch_ServerAccessblock;
		pictestUrl.InitialImage = (Image)(object)FormCustom.Properties.Resources.Catch_ConnectSucceess;
		((Control)pictestUrl).Location = new Point(19, 12);
		((Control)pictestUrl).Name = "pictestUrl";
		((Control)pictestUrl).Size = new Size(28, 28);
		pictestUrl.SizeMode = (PictureBoxSizeMode)1;
		pictestUrl.TabIndex = 2;
		pictestUrl.TabStop = false;
		((Control)pictestUrl).Click += pictestUrl_Click;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label1).Location = new Point(601, 15);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(46, 20);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "[آدرس]:";
		label1.TextAlign = (ContentAlignment)64;
		CmbUrls.AutoCompleteMode = (AutoCompleteMode)1;
		CmbUrls.AutoCompleteSource = (AutoCompleteSource)256;
		((ListControl)CmbUrls).DisplayMember = "StringData";
		((Control)CmbUrls).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)CmbUrls).Location = new Point(54, 12);
		((Control)CmbUrls).Margin = new Padding(4);
		((Control)CmbUrls).Name = "CmbUrls";
		((Control)CmbUrls).RightToLeft = (RightToLeft)0;
		((Control)CmbUrls).Size = new Size(540, 28);
		((Control)CmbUrls).TabIndex = 0;
		((ListControl)CmbUrls).ValueMember = "StringData";
		CmbUrls.SelectedIndexChanged += CmbUrls_SelectedIndexChanged;
		((Control)CmbUrls).TextChanged += CmbUrls_TextChanged;
		((Control)panel3).BackColor = SystemColors.ActiveCaption;
		panel3.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)panel3).Dock = (DockStyle)5;
		((Control)panel3).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		panel3.HeadHeight = 40;
		panel3.IsShowHeader = false;
		((Control)panel3).Location = new Point(0, 56);
		((Control)panel3).Margin = new Padding(4);
		((Control)panel3).Name = "panel3";
		((Control)panel3).RightToLeft = (RightToLeft)1;
		((Control)panel3).Size = new Size(661, 369);
		((Control)panel3).TabIndex = 2;
		panel3.Title = "[عنوان]";
		GrdAddress.AllowUserToAddRows = false;
		GrdAddress.AllowUserToDeleteRows = false;
		GrdAddress.AutoGenerateColumns = false;
		GrdAddress.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		GrdAddress.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)urlDataGridViewTextBoxColumn,
			(DataGridViewColumn)ordinalDataGridViewTextBoxColumn,
			(DataGridViewColumn)StatusConects
		});
		GrdAddress.DataSource = urlViewBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		GrdAddress.DefaultCellStyle = val;
		GrdAddress.GridColor = Color.FromArgb(208, 215, 229);
		((Control)GrdAddress).Location = new Point(8, 102);
		((Control)GrdAddress).Name = "GrdAddress";
		GrdAddress.ReadOnly = true;
		((Control)GrdAddress).Size = new Size(646, 278);
		((Control)GrdAddress).TabIndex = 8;
		((DataGridViewColumn)urlDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)urlDataGridViewTextBoxColumn).DataPropertyName = "Url";
		((DataGridViewColumn)urlDataGridViewTextBoxColumn).HeaderText = "[آدرس]";
		((DataGridViewColumn)urlDataGridViewTextBoxColumn).Name = "urlDataGridViewTextBoxColumn";
		((DataGridViewBand)urlDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)ordinalDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)2;
		((DataGridViewColumn)ordinalDataGridViewTextBoxColumn).DataPropertyName = "Ordinal";
		((DataGridViewColumn)ordinalDataGridViewTextBoxColumn).HeaderText = "[ترتیب]";
		((DataGridViewColumn)ordinalDataGridViewTextBoxColumn).Name = "ordinalDataGridViewTextBoxColumn";
		((DataGridViewBand)ordinalDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)ordinalDataGridViewTextBoxColumn).Width = 59;
		((DataGridViewColumn)StatusConects).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)StatusConects).DataPropertyName = "StatusConects";
		((DataGridViewColumn)StatusConects).HeaderText = "[وضعیت]";
		((DataGridViewColumn)StatusConects).Name = "StatusConects";
		((DataGridViewBand)StatusConects).ReadOnly = true;
		((DataGridViewColumn)StatusConects).Width = 67;
		urlViewBindingSource.DataSource = typeof(UrlView);
		((Control)comboBoxEx1).Location = new Point(492, 44);
		((Control)comboBoxEx1).Name = "comboBoxEx1";
		((Control)comboBoxEx1).Size = new Size(121, 21);
		((Control)comboBoxEx1).TabIndex = 0;
		((Control)comboBoxEx1).Text = "comboBoxEx1";
		((Control)picUp).Anchor = (AnchorStyles)6;
		((Control)picUp).BackColor = Color.FromArgb(249, 250, 255);
		((Control)picUp).Cursor = Cursors.Hand;
		picUp.Image = (Image)(object)FormCustom.Properties.Resources.down_1;
		((Control)picUp).Location = new Point(98, 67);
		((Control)picUp).Name = "picUp";
		((Control)picUp).Size = new Size(30, 29);
		picUp.SizeMode = (PictureBoxSizeMode)4;
		picUp.TabIndex = 53;
		picUp.TabStop = false;
		((Control)picUp).Click += picUp_Click;
		((Control)picDown).Anchor = (AnchorStyles)6;
		((Control)picDown).BackColor = Color.FromArgb(249, 250, 255);
		((Control)picDown).Cursor = Cursors.Hand;
		picDown.Image = (Image)(object)FormCustom.Properties.Resources.up_1;
		((Control)picDown).Location = new Point(68, 67);
		((Control)picDown).Name = "picDown";
		((Control)picDown).Size = new Size(30, 29);
		picDown.SizeMode = (PictureBoxSizeMode)4;
		picDown.TabIndex = 54;
		picDown.TabStop = false;
		((Control)picDown).Click += picDown_Click;
		((Control)picAdd).Anchor = (AnchorStyles)6;
		((Control)picAdd).BackColor = Color.FromArgb(249, 250, 255);
		((Control)picAdd).Cursor = Cursors.Hand;
		picAdd.Image = (Image)(object)FormCustom.Properties.Resources.addGreen;
		((Control)picAdd).Location = new Point(38, 67);
		((Control)picAdd).Name = "picAdd";
		((Control)picAdd).Size = new Size(30, 29);
		picAdd.SizeMode = (PictureBoxSizeMode)4;
		picAdd.TabIndex = 55;
		picAdd.TabStop = false;
		((Control)picAdd).Click += picAdd_Click;
		((Control)picDelete).Anchor = (AnchorStyles)6;
		((Control)picDelete).BackColor = Color.FromArgb(249, 250, 255);
		((Control)picDelete).Cursor = Cursors.Hand;
		picDelete.Image = (Image)(object)FormCustom.Properties.Resources.trashRed;
		((Control)picDelete).Location = new Point(8, 67);
		((Control)picDelete).Name = "picDelete";
		((Control)picDelete).Size = new Size(30, 29);
		picDelete.SizeMode = (PictureBoxSizeMode)4;
		picDelete.TabIndex = 56;
		picDelete.TabStop = false;
		((Control)picDelete).Click += picDelete_Click;
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).BackColor = Color.FromArgb(249, 250, 255);
		btnCancel.CornerType = EnButtonCornerType.Round;
		((Control)btnCancel).Cursor = Cursors.Hand;
		btnCancel.DisplayText = "[انصراف](Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(11, 384);
		((Control)btnCancel).Margin = new Padding(0);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(121, 35);
		((Control)btnCancel).TabIndex = 58;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btnCancel_OnClick;
		((Control)btnOk).Anchor = (AnchorStyles)6;
		((Control)btnOk).BackColor = Color.FromArgb(249, 250, 255);
		btnOk.CornerType = EnButtonCornerType.Round;
		((Control)btnOk).Cursor = Cursors.Hand;
		btnOk.DisplayText = "[تایید](F5)";
		((Control)btnOk).Font = new Font("B Yekan", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(132, 384);
		((Control)btnOk).Margin = new Padding(0);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(220, 35);
		((Control)btnOk).TabIndex = 57;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = EnButtonType.Bold;
		btnOk.OnClick += btnOk_OnClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(661, 425);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Controls.Add((Control)(object)picUp);
		((Control)this).Controls.Add((Control)(object)picDown);
		((Control)this).Controls.Add((Control)(object)picAdd);
		((Control)this).Controls.Add((Control)(object)picDelete);
		((Control)this).Controls.Add((Control)(object)GrdAddress);
		((Control)this).Controls.Add((Control)(object)pictestUrl);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)CmbUrls);
		((Control)this).Controls.Add((Control)(object)panel3);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmUrlBuilder";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[انتخاب آدرس]";
		((Form)this).FormClosed += new FormClosedEventHandler(FrmUrlBuilder_FormClosed);
		((Form)this).Load += FrmUrlBuilder_Load;
		((Control)this).KeyDown += new KeyEventHandler(FrmUrlBuilder_KeyDown);
		((ISupportInitialize)pictestUrl).EndInit();
		((ISupportInitialize)GrdAddress).EndInit();
		((ISupportInitialize)urlViewBindingSource).EndInit();
		((ISupportInitialize)picUp).EndInit();
		((ISupportInitialize)picDown).EndInit();
		((ISupportInitialize)picAdd).EndInit();
		((ISupportInitialize)picDelete).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
