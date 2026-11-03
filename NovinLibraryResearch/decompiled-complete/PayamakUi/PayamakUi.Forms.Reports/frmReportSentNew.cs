using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Cache;
using FormCustom.UC;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;
using PayamakUi.Mapper;

namespace PayamakUi.Forms.Reports;

public class frmReportSentNew : CacheBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _searchToken;

	private IEnumerable<SmsSent> _listData;

	private IEnumerable<SmsSent> _filteredList;

	private IContainer components;

	internal BindingSource ViewSMSSentBindingSource;

	private UcSearchBox ucSearchBox1;

	private DataGridView DGrid;

	private DataGridViewTextBoxColumn DeliveryStatusForDisplay;

	private DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn sendDateShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn sendTimeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn refrencedIDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;

	private DataGridViewImageColumn imgDataGridViewImageColumn;

	private DataGridViewCheckBoxColumn isCheckedDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn sendDateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryStDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliverySTShortDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn deliveryRequestedDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn refrencedIdsDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn senderNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn refrenseAccountIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn refrenceGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateSHDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fixedMessageDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn sTDataGridViewCheckBoxColumn;

	internal Label lblCounter;

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

	public string SearchString { get; private set; }

	private async Task LoadDataAsync(IEnumerable<BaseSmsSent> _list)
	{
		try
		{
			_listData = await SmsSentMapper.Instance.MapListAsync(base.ContextGuid, _list);
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 39);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			_filteredList = _listData;
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			List<string> list = SearchString.SplitString();
			if (list != null && list.Count > 0)
			{
				foreach (string item in list)
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return;
					}
					if (!string.IsNullOrEmpty(item))
					{
						_filteredList = _filteredList?.Where((SmsSent x) => x.Number.StringComparisons(item) || x.RefrencedID.StringComparisons(item) || x.DeliveryStatusForDisplay.StringComparisons(item) || x.Message.StringComparisons(item));
					}
				}
			}
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
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				try
				{
					ViewSMSSentBindingSource.DataSource = _filteredList?.OrderBy((SmsSent q) => q.DateM)?.ToSortableBindingList(token);
					((Control)lblCounter).Text = $"تعداد پیامک ارسال شده: {_filteredList?.Count() ?? 0:N0}";
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 73);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 76);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 92);
		}
	}

	public frmReportSentNew(string contextGuid, IEnumerable<BaseSmsSent> _list, Font font)
		: base(contextGuid)
	{
		frmReportSentNew frmReportSentNew2 = this;
		try
		{
			InitializeComponent();
			DGrid.Initialize(font);
			Task.Run(() => frmReportSentNew2.LoadDataAsync(_list));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 103);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)ucSearchBox1).Focus();
				((Control)ucSearchBox1).Text = e.KeyChar.ToString();
				ucSearchBox1.SelectionStart = 9999;
				ucSearchBox1.SelectionLength = 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 116);
		}
	}

	private void frmReportSentNew_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				if (!string.IsNullOrEmpty(((Control)ucSearchBox1).Text))
				{
					((Control)ucSearchBox1).Text = "";
					return;
				}
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmReportSentNew_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 135);
		}
	}

	private void ucSearchBox1_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			SearchString = ((Control)ucSearchBox1).Text;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucSearchBox1_OnTextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 147);
		}
	}

	private void ucSearchBox1_OnKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 40 || (int)e.KeyCode == 13)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucSearchBox1_OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\frmReportSentNew.cs", 156);
		}
	}

	private void frmReportSentNew_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Expected O, but got Unknown
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Expected O, but got Unknown
		//IL_0d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4e: Expected O, but got Unknown
		//IL_0d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da7: Expected O, but got Unknown
		//IL_0daf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db9: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmReportSentNew));
		ViewSMSSentBindingSource = new BindingSource(components);
		ucSearchBox1 = new UcSearchBox();
		DGrid = new DataGridView();
		DeliveryStatusForDisplay = new DataGridViewTextBoxColumn();
		numberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sendDateShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sendTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		refrencedIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		messageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		imgDataGridViewImageColumn = new DataGridViewImageColumn();
		isCheckedDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		sendDateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryStDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliverySTShortDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryRequestedDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		refrencedIdsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		senderNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		refrenseAccountIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		refrenceGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		iDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateSHDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		customerIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		customerNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fixedMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sTDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		lblCounter = new Label();
		((ISupportInitialize)ViewSMSSentBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		ViewSMSSentBindingSource.DataSource = typeof(SmsSent);
		((Control)ucSearchBox1).Anchor = (AnchorStyles)13;
		((Control)ucSearchBox1).BackColor = Color.Transparent;
		((Control)ucSearchBox1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucSearchBox1.IsSaerchBox = true;
		((Control)ucSearchBox1).Location = new Point(69, 7);
		((Control)ucSearchBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucSearchBox1).Name = "ucSearchBox1";
		((Control)ucSearchBox1).RightToLeft = (RightToLeft)1;
		ucSearchBox1.SelectionLength = 0;
		ucSearchBox1.SelectionStart = 0;
		((Control)ucSearchBox1).Size = new Size(664, 44);
		((Control)ucSearchBox1).TabIndex = 168;
		ucSearchBox1.OnTextChanged += ucSearchBox1_OnTextChanged;
		ucSearchBox1.OnKeyDown += ucSearchBox1_OnKeyDown;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		DGrid.AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		DGrid.CellBorderStyle = (DataGridViewCellBorderStyle)4;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = Color.FromArgb(192, 255, 255);
		val.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.WindowText;
		val.SelectionBackColor = Color.DodgerBlue;
		val.SelectionForeColor = SystemColors.HighlightText;
		val.WrapMode = (DataGridViewTriState)1;
		DGrid.ColumnHeadersDefaultCellStyle = val;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[23]
		{
			(DataGridViewColumn)DeliveryStatusForDisplay,
			(DataGridViewColumn)numberDataGridViewTextBoxColumn,
			(DataGridViewColumn)sendDateShDataGridViewTextBoxColumn,
			(DataGridViewColumn)sendTimeDataGridViewTextBoxColumn,
			(DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn,
			(DataGridViewColumn)messageDataGridViewTextBoxColumn,
			(DataGridViewColumn)imgDataGridViewImageColumn,
			(DataGridViewColumn)isCheckedDataGridViewCheckBoxColumn,
			(DataGridViewColumn)sendDateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryStDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliverySTShortDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryRequestedDataGridViewCheckBoxColumn,
			(DataGridViewColumn)refrencedIdsDataGridViewTextBoxColumn,
			(DataGridViewColumn)senderNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)refrenseAccountIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)refrenceGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)iDDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateSHDataGridViewTextBoxColumn,
			(DataGridViewColumn)customerIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)customerNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)sTDataGridViewCheckBoxColumn
		});
		DGrid.DataSource = ViewSMSSentBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = Color.FromArgb(192, 255, 255);
		val2.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = Color.DodgerBlue;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)2;
		DGrid.DefaultCellStyle = val2;
		DGrid.GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(8, 57);
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = Color.FromArgb(192, 255, 255);
		val3.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val3.ForeColor = SystemColors.WindowText;
		val3.SelectionBackColor = Color.DodgerBlue;
		val3.SelectionForeColor = SystemColors.HighlightText;
		val3.WrapMode = (DataGridViewTriState)1;
		DGrid.RowHeadersDefaultCellStyle = val3;
		DGrid.RowHeadersVisible = false;
		val4.BackColor = Color.FromArgb(192, 255, 255);
		val4.SelectionBackColor = Color.DodgerBlue;
		DGrid.RowsDefaultCellStyle = val4;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(769, 457);
		((Control)DGrid).TabIndex = 167;
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		((DataGridViewColumn)DeliveryStatusForDisplay).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DeliveryStatusForDisplay).DataPropertyName = "DeliveryStatusForDisplay";
		((DataGridViewColumn)DeliveryStatusForDisplay).HeaderText = "وضعیت";
		((DataGridViewColumn)DeliveryStatusForDisplay).Name = "DeliveryStatusForDisplay";
		((DataGridViewBand)DeliveryStatusForDisplay).ReadOnly = true;
		((DataGridViewBand)DeliveryStatusForDisplay).Visible = false;
		((DataGridViewColumn)numberDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)numberDataGridViewTextBoxColumn).DataPropertyName = "Number";
		((DataGridViewColumn)numberDataGridViewTextBoxColumn).HeaderText = "گیرنده";
		((DataGridViewColumn)numberDataGridViewTextBoxColumn).Name = "numberDataGridViewTextBoxColumn";
		((DataGridViewColumn)numberDataGridViewTextBoxColumn).Width = 66;
		((DataGridViewColumn)sendDateShDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)sendDateShDataGridViewTextBoxColumn).DataPropertyName = "SendDateSh";
		((DataGridViewColumn)sendDateShDataGridViewTextBoxColumn).HeaderText = "تاریخ ارسال";
		((DataGridViewColumn)sendDateShDataGridViewTextBoxColumn).Name = "sendDateShDataGridViewTextBoxColumn";
		((DataGridViewBand)sendDateShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)sendDateShDataGridViewTextBoxColumn).Width = 92;
		((DataGridViewColumn)sendTimeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)sendTimeDataGridViewTextBoxColumn).DataPropertyName = "SendTime";
		((DataGridViewColumn)sendTimeDataGridViewTextBoxColumn).HeaderText = "زمان ارسال";
		((DataGridViewColumn)sendTimeDataGridViewTextBoxColumn).Name = "sendTimeDataGridViewTextBoxColumn";
		((DataGridViewBand)sendTimeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)sendTimeDataGridViewTextBoxColumn).Width = 90;
		((DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn).DataPropertyName = "RefrencedID";
		((DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn).HeaderText = "مرجع";
		((DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn).Name = "refrencedIDDataGridViewTextBoxColumn";
		((DataGridViewBand)refrencedIDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)refrencedIDDataGridViewTextBoxColumn).Width = 59;
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).DataPropertyName = "Message";
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).HeaderText = "پیام";
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).Name = "messageDataGridViewTextBoxColumn";
		((DataGridViewColumn)imgDataGridViewImageColumn).DataPropertyName = "Img";
		((DataGridViewColumn)imgDataGridViewImageColumn).HeaderText = "Img";
		((DataGridViewColumn)imgDataGridViewImageColumn).Name = "imgDataGridViewImageColumn";
		((DataGridViewBand)imgDataGridViewImageColumn).Visible = false;
		((DataGridViewColumn)isCheckedDataGridViewCheckBoxColumn).DataPropertyName = "IsChecked";
		((DataGridViewColumn)isCheckedDataGridViewCheckBoxColumn).HeaderText = "IsChecked";
		((DataGridViewColumn)isCheckedDataGridViewCheckBoxColumn).Name = "isCheckedDataGridViewCheckBoxColumn";
		((DataGridViewBand)isCheckedDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)sendDateMDataGridViewTextBoxColumn).DataPropertyName = "SendDateM";
		((DataGridViewColumn)sendDateMDataGridViewTextBoxColumn).HeaderText = "SendDateM";
		((DataGridViewColumn)sendDateMDataGridViewTextBoxColumn).Name = "sendDateMDataGridViewTextBoxColumn";
		((DataGridViewBand)sendDateMDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryStDataGridViewTextBoxColumn).DataPropertyName = "DeliverySt";
		((DataGridViewColumn)deliveryStDataGridViewTextBoxColumn).HeaderText = "DeliverySt";
		((DataGridViewColumn)deliveryStDataGridViewTextBoxColumn).Name = "deliveryStDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryStDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliverySTShortDataGridViewTextBoxColumn).DataPropertyName = "DeliverySTShort";
		((DataGridViewColumn)deliverySTShortDataGridViewTextBoxColumn).HeaderText = "DeliverySTShort";
		((DataGridViewColumn)deliverySTShortDataGridViewTextBoxColumn).Name = "deliverySTShortDataGridViewTextBoxColumn";
		((DataGridViewBand)deliverySTShortDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliverySTShortDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryRequestedDataGridViewCheckBoxColumn).DataPropertyName = "DeliveryRequested";
		((DataGridViewColumn)deliveryRequestedDataGridViewCheckBoxColumn).HeaderText = "DeliveryRequested";
		((DataGridViewColumn)deliveryRequestedDataGridViewCheckBoxColumn).Name = "deliveryRequestedDataGridViewCheckBoxColumn";
		((DataGridViewBand)deliveryRequestedDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)refrencedIdsDataGridViewTextBoxColumn).DataPropertyName = "RefrencedIds";
		((DataGridViewColumn)refrencedIdsDataGridViewTextBoxColumn).HeaderText = "RefrencedIds";
		((DataGridViewColumn)refrencedIdsDataGridViewTextBoxColumn).Name = "refrencedIdsDataGridViewTextBoxColumn";
		((DataGridViewBand)refrencedIdsDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).DataPropertyName = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).HeaderText = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).Name = "senderNumberDataGridViewTextBoxColumn";
		((DataGridViewBand)senderNumberDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)refrenseAccountIdDataGridViewTextBoxColumn).DataPropertyName = "RefrenseAccountId";
		((DataGridViewColumn)refrenseAccountIdDataGridViewTextBoxColumn).HeaderText = "RefrenseAccountId";
		((DataGridViewColumn)refrenseAccountIdDataGridViewTextBoxColumn).Name = "refrenseAccountIdDataGridViewTextBoxColumn";
		((DataGridViewBand)refrenseAccountIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)refrenceGuidDataGridViewTextBoxColumn).DataPropertyName = "RefrenceGuid";
		((DataGridViewColumn)refrenceGuidDataGridViewTextBoxColumn).HeaderText = "RefrenceGuid";
		((DataGridViewColumn)refrenceGuidDataGridViewTextBoxColumn).Name = "refrenceGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)refrenceGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)iDDataGridViewTextBoxColumn).DataPropertyName = "ID";
		((DataGridViewColumn)iDDataGridViewTextBoxColumn).HeaderText = "ID";
		((DataGridViewColumn)iDDataGridViewTextBoxColumn).Name = "iDDataGridViewTextBoxColumn";
		((DataGridViewBand)iDDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).DataPropertyName = "DateM";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).HeaderText = "DateM";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Name = "dateMDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dateSHDataGridViewTextBoxColumn).DataPropertyName = "DateSH";
		((DataGridViewColumn)dateSHDataGridViewTextBoxColumn).HeaderText = "DateSH";
		((DataGridViewColumn)dateSHDataGridViewTextBoxColumn).Name = "dateSHDataGridViewTextBoxColumn";
		((DataGridViewBand)dateSHDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)dateSHDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).DataPropertyName = "CustomerId";
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).HeaderText = "CustomerId";
		((DataGridViewColumn)customerIdDataGridViewTextBoxColumn).Name = "customerIdDataGridViewTextBoxColumn";
		((DataGridViewBand)customerIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)customerNameDataGridViewTextBoxColumn).DataPropertyName = "CustomerName";
		((DataGridViewColumn)customerNameDataGridViewTextBoxColumn).HeaderText = "CustomerName";
		((DataGridViewColumn)customerNameDataGridViewTextBoxColumn).Name = "customerNameDataGridViewTextBoxColumn";
		((DataGridViewBand)customerNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)customerNameDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).DataPropertyName = "FixedMessage";
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).HeaderText = "FixedMessage";
		((DataGridViewColumn)fixedMessageDataGridViewTextBoxColumn).Name = "fixedMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)fixedMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)fixedMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).DataPropertyName = "ST";
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).HeaderText = "ST";
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).Name = "sTDataGridViewCheckBoxColumn";
		((DataGridViewBand)sTDataGridViewCheckBoxColumn).Visible = false;
		((Control)lblCounter).Anchor = (AnchorStyles)14;
		((Control)lblCounter).BackColor = Color.Transparent;
		((Control)lblCounter).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblCounter).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)lblCounter).Location = new Point(215, 531);
		((Control)lblCounter).Name = "lblCounter";
		((Control)lblCounter).Size = new Size(562, 21);
		((Control)lblCounter).TabIndex = 182;
		((Control)lblCounter).Text = "---";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)lblCounter);
		((Control)this).Controls.Add((Control)(object)ucSearchBox1);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Control)this).MinimumSize = new Size(698, 407);
		((Control)this).Name = "frmReportSentNew";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "گزارش پیامک های ارسال شده";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosed += new FormClosedEventHandler(frmReportSentNew_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmReportSentNew_KeyDown);
		((ISupportInitialize)ViewSMSSentBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
