using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;
using PayamakSettings;
using PayamakUi.Classes.Entities;
using PayamakUi.Forms.FRMOprate;
using PayamakUi.Mapper;
using PayamakUi.Properties;
using PayamakUi.UC.Others;

namespace PayamakUi.UC.Templates;

public class UCTemplateList : UserControl, IHasContext, INpDisposable, IDisposable
{
	public delegate void OnSelectEventHandler(string SelectedItem);

	private List<SMSTemplate> lstTemplate;

	private CancellationTokenSource TokenLoad = new CancellationTokenSource();

	private CancellationTokenSource TokenDelete = new CancellationTokenSource();

	private PayamakClientHubInstances _instance;

	private IContainer components;

	internal GroupPanel GroupPanel1;

	internal TabControl TabControl2;

	internal TabControlPanel TabControlPanel4;

	internal UCParametr UcParametr1;

	internal TabItem TabItem2;

	internal TabControlPanel TabControlPanel3;

	internal GroupPanel GroupPanel2;

	internal TextBoxX TXTSearch;

	internal Label LblSearch_Inbox;

	internal TabItem TabItem1;

	internal Panel Panel1;

	internal DataGridViewX Dgrid;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal BindingSource SMSTemplateBindingSource1;

	internal DataGridViewTextBoxColumn DgId;

	internal DataGridViewTextBoxColumn DGContent;

	internal DataGridViewTextBoxColumn Title;

	public int ID
	{
		get
		{
			try
			{
				if (((DataGridView)Dgrid).RowCount == 0)
				{
					return 0;
				}
				return int.Parse(((DataGridView)Dgrid)[((DataGridViewBand)DgId).Index, ((DataGridViewBand)((DataGridView)Dgrid).CurrentRow).Index].Value.ToString());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ID", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 39);
				return 0;
			}
		}
	}

	public string ContextGuid { get; set; }

	public event OnSelectEventHandler OnSelect;

	public async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			if (string.IsNullOrEmpty(ContextGuid))
			{
				return;
			}
			if (_instance == null)
			{
				_instance = PayamakClientHubInstances.GetInstance(ContextGuid);
			}
			Font font = await Font.GetFontProgramAsync(default(CancellationToken));
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			ReturnedSaveFuncInfoWithValue<List<BaseSmsTemplate>> returnedSaveFuncInfoWithValue = await _instance.SmsTemplate.GetAllAsync(token);
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			lstTemplate = await SmsTemplateMapper.Instance.MapListAsync(ContextGuid, returnedSaveFuncInfoWithValue.value);
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
					((Control)Dgrid).Font = font;
					((Control)Dgrid).Focus();
					SMSTemplateBindingSource1.DataSource = lstTemplate;
					Search();
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 73);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 76);
		}
	}

	private void RaiseEvent_Select()
	{
		try
		{
			if (((DataGridView)Dgrid).RowCount > 0)
			{
				this.OnSelect?.Invoke(((DataGridView)Dgrid)[((DataGridViewBand)DGContent).Index, ((DataGridViewBand)((DataGridView)Dgrid).CurrentRow).Index].Value.ToString());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RaiseEvent_Select", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 85);
		}
	}

	private void Search()
	{
		try
		{
			List<SMSTemplate> source = lstTemplate;
			((Control)TXTSearch).Text.FixString().Trim();
			SMSTemplateBindingSource1.DataSource = source.Where((SMSTemplate p) => p.Title.Contains(((Control)TXTSearch).Text.Trim())).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 95);
		}
	}

	public UCTemplateList()
	{
		InitializeComponent();
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		try
		{
			TokenLoad?.Cancel();
			TokenLoad = new CancellationTokenSource(60000);
			CancellationToken tkn = TokenLoad.Token;
			if (((Control)BtnAdd).Enabled && ((Control)BtnAdd).Visible && (int)((Form)new FRMTemplate(ContextGuid)).ShowDialog((IWin32Window)(object)this) == 1)
			{
				Task.Run(() => LoadDataAsync(tkn));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 112);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			TokenLoad?.Cancel();
			TokenLoad = new CancellationTokenSource(60000);
			CancellationToken tkn = TokenLoad.Token;
			if (!((Control)BtnEdit).Enabled || !((Control)BtnEdit).Visible)
			{
				return;
			}
			((Control)Dgrid).Focus();
			if (ID <= 0)
			{
				return;
			}
			ReturnedSaveFuncInfoWithValue<BaseSmsTemplate> returnedSaveFuncInfoWithValue = await _instance.SmsTemplate.GetAsync(ID, default(CancellationToken));
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			string contextGuid = ContextGuid;
			if ((int)((Form)new FRMTemplate(contextGuid, await SmsTemplateMapper.Instance.MapAsync(ContextGuid, returnedSaveFuncInfoWithValue.value))).ShowDialog((IWin32Window)(object)this) == 1)
			{
				Task.Run(() => LoadDataAsync(tkn));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 132);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				((Control)Dgrid).Focus();
				if (!((Control)BtnDelete).Enabled || !((Control)BtnDelete).Visible || ID <= 0)
				{
					goto end_IL_0061;
				}
				(await _instance.SmsTemplate.GetAsync(ID, default(CancellationToken))).throwExceptionIfError();
				if ((int)MessageBox.Show("مایل به حذف الگوی انتخاب شده هستید ؟".GetTranslate(), "هشدار".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)16, (MessageBoxDefaultButton)256, (MessageBoxOptions)524288) != 6)
				{
					goto end_IL_0061;
				}
				TokenDelete?.Cancel();
				TokenDelete = new CancellationTokenSource();
				CancellationToken token = TokenDelete.Token;
				await _instance.SmsTemplate.DeleteAsync(ID, token);
				goto end_IL_004f;
				end_IL_0061:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 153);
				res.AddReturnedValue(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs", 154);
				goto end_IL_004f;
			}
			end_IL_004f:;
		}
		finally
		{
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoForUserControlAsync(res, "پیغام سیستم", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Templates\\UCTemplateList.cs");
			}
			else
			{
				TokenLoad?.Cancel();
				TokenLoad = new CancellationTokenSource(60000);
				CancellationToken tkn = TokenLoad.Token;
				Task.Run(() => LoadDataAsync(tkn));
			}
		}
	}

	private void TXTSearch_TextChanged(object sender, EventArgs e)
	{
		Search();
	}

	private void Dgrid_DoubleClick(object sender, EventArgs e)
	{
		RaiseEvent_Select();
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Expected O, but got Unknown
		//IL_0eaf: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		GroupPanel1 = new GroupPanel();
		TabControl2 = new TabControl();
		TabControlPanel3 = new TabControlPanel();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		Panel1 = new Panel();
		Dgrid = new DataGridViewX();
		DgId = new DataGridViewTextBoxColumn();
		DGContent = new DataGridViewTextBoxColumn();
		Title = new DataGridViewTextBoxColumn();
		SMSTemplateBindingSource1 = new BindingSource(components);
		GroupPanel2 = new GroupPanel();
		TXTSearch = new TextBoxX();
		LblSearch_Inbox = new Label();
		BtnAdd = new ButtonX();
		TabItem1 = new TabItem(components);
		TabControlPanel4 = new TabControlPanel();
		UcParametr1 = new UCParametr();
		TabItem2 = new TabItem(components);
		((Control)GroupPanel1).SuspendLayout();
		((ISupportInitialize)TabControl2).BeginInit();
		((Control)TabControl2).SuspendLayout();
		((Control)TabControlPanel3).SuspendLayout();
		((Control)Panel1).SuspendLayout();
		((ISupportInitialize)Dgrid).BeginInit();
		((ISupportInitialize)SMSTemplateBindingSource1).BeginInit();
		((Control)GroupPanel2).SuspendLayout();
		((Control)TabControlPanel4).SuspendLayout();
		((Control)this).SuspendLayout();
		GroupPanel1.CanvasColor = SystemColors.Control;
		GroupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel1).Controls.Add((Control)(object)TabControl2);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Dock = (DockStyle)5;
		((Control)GroupPanel1).Location = new Point(0, 0);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(242, 276);
		GroupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel1.Style.BackColorGradientAngle = 90;
		GroupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderBottomWidth = 1;
		GroupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderLeftWidth = 1;
		GroupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderRightWidth = 1;
		GroupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderTopWidth = 1;
		GroupPanel1.Style.CornerDiameter = 4;
		GroupPanel1.Style.CornerType = eCornerType.Rounded;
		GroupPanel1.Style.TextAlignment = eStyleTextAlignment.Center;
		GroupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)TabControl2).BackColor = Color.Transparent;
		TabControl2.CanReorderTabs = true;
		((Control)TabControl2).Controls.Add((Control)(object)TabControlPanel4);
		((Control)TabControl2).Controls.Add((Control)(object)TabControlPanel3);
		((Control)TabControl2).Dock = (DockStyle)5;
		((Control)TabControl2).Location = new Point(0, 0);
		((Control)TabControl2).Name = "TabControl2";
		((Control)TabControl2).RightToLeft = (RightToLeft)1;
		TabControl2.SelectedTabFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1);
		TabControl2.SelectedTabIndex = 0;
		((Control)TabControl2).Size = new Size(236, 270);
		((Control)TabControl2).TabIndex = 35;
		TabControl2.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		TabControl2.Tabs.Add(TabItem1);
		TabControl2.Tabs.Add(TabItem2);
		((Control)TabControl2).Text = "افزودن پارامتر";
		((Control)TabControlPanel3).Controls.Add((Control)(object)BtnDelete);
		((Control)TabControlPanel3).Controls.Add((Control)(object)BtnEdit);
		((Control)TabControlPanel3).Controls.Add((Control)(object)Panel1);
		((Control)TabControlPanel3).Controls.Add((Control)(object)GroupPanel2);
		((Control)TabControlPanel3).Controls.Add((Control)(object)BtnAdd);
		TabControlPanel3.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel3).Dock = (DockStyle)5;
		((Control)TabControlPanel3).Location = new Point(0, 26);
		((Control)TabControlPanel3).Name = "TabControlPanel3";
		((Control)TabControlPanel3).Padding = new Padding(1);
		((Control)TabControlPanel3).Size = new Size(236, 244);
		TabControlPanel3.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel3.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel3.Style.Border = eBorderType.SingleLine;
		TabControlPanel3.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel3.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel3.Style.GradientAngle = 90;
		((Control)TabControlPanel3).TabIndex = 1;
		TabControlPanel3.TabItem = TabItem1;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDelete).Anchor = (AnchorStyles)10;
		BtnDelete.ColorTable = eButtonColor.Office2007WithBackground;
		BtnDelete.Image = (Image)(object)Resources.delete1;
		((Control)BtnDelete).Location = new Point(121, 210);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(33, 30);
		BtnDelete.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDelete).TabIndex = 17;
		BtnDelete.Tooltip = "حذف الگو";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		((Control)BtnEdit).Anchor = (AnchorStyles)10;
		BtnEdit.ColorTable = eButtonColor.Office2007WithBackground;
		BtnEdit.Image = (Image)(object)Resources.pencil1;
		((Control)BtnEdit).Location = new Point(160, 210);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).Size = new Size(33, 30);
		BtnEdit.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnEdit).TabIndex = 16;
		BtnEdit.Tooltip = "ویرایش الگو";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)Panel1).Anchor = (AnchorStyles)15;
		((ScrollableControl)Panel1).AutoScroll = true;
		((Control)Panel1).Controls.Add((Control)(object)Dgrid);
		((Control)Panel1).Location = new Point(4, 68);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(228, 136);
		((Control)Panel1).TabIndex = 14;
		((DataGridView)Dgrid).AllowUserToAddRows = false;
		((DataGridView)Dgrid).AllowUserToDeleteRows = false;
		((DataGridView)Dgrid).AllowUserToResizeRows = false;
		((DataGridView)Dgrid).AutoGenerateColumns = false;
		((DataGridView)Dgrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)Dgrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)DgId,
			(DataGridViewColumn)DGContent,
			(DataGridViewColumn)Title
		});
		((DataGridView)Dgrid).DataSource = SMSTemplateBindingSource1;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)Dgrid).DefaultCellStyle = val;
		((Control)Dgrid).Dock = (DockStyle)5;
		((DataGridView)Dgrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)Dgrid).Location = new Point(0, 0);
		((Control)Dgrid).Name = "Dgrid";
		((DataGridView)Dgrid).ReadOnly = true;
		((DataGridView)Dgrid).RowHeadersVisible = false;
		((DataGridView)Dgrid).ScrollBars = (ScrollBars)2;
		((DataGridView)Dgrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)Dgrid).Size = new Size(228, 136);
		((Control)Dgrid).TabIndex = 1;
		((Control)Dgrid).DoubleClick += Dgrid_DoubleClick;
		((DataGridViewColumn)DgId).DataPropertyName = "Id";
		((DataGridViewColumn)DgId).HeaderText = "Id";
		((DataGridViewColumn)DgId).Name = "DgId";
		((DataGridViewBand)DgId).ReadOnly = true;
		((DataGridViewBand)DgId).Visible = false;
		((DataGridViewColumn)DGContent).DataPropertyName = "Content";
		((DataGridViewColumn)DGContent).HeaderText = "Content";
		((DataGridViewColumn)DGContent).Name = "DGContent";
		((DataGridViewBand)DGContent).ReadOnly = true;
		((DataGridViewBand)DGContent).Visible = false;
		((DataGridViewColumn)Title).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)Title).DataPropertyName = "Title";
		((DataGridViewColumn)Title).HeaderText = "عنوان";
		((DataGridViewColumn)Title).Name = "Title";
		((DataGridViewBand)Title).ReadOnly = true;
		SMSTemplateBindingSource1.DataSource = typeof(SMSTemplate);
		((Control)GroupPanel2).Anchor = (AnchorStyles)13;
		((Control)GroupPanel2).BackColor = Color.Transparent;
		GroupPanel2.CanvasColor = SystemColors.Control;
		GroupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel2).Controls.Add((Control)(object)TXTSearch);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblSearch_Inbox);
		GroupPanel2.DisabledBackColor = Color.Empty;
		((Control)GroupPanel2).Location = new Point(4, 2);
		((Control)GroupPanel2).Name = "GroupPanel2";
		((Control)GroupPanel2).Size = new Size(228, 60);
		GroupPanel2.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel2.Style.BackColorGradientAngle = 90;
		GroupPanel2.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel2.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderBottomWidth = 1;
		GroupPanel2.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel2.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderLeftWidth = 1;
		GroupPanel2.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderRightWidth = 1;
		GroupPanel2.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderTopWidth = 1;
		GroupPanel2.Style.CornerDiameter = 4;
		GroupPanel2.Style.CornerType = eCornerType.Rounded;
		GroupPanel2.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel2.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel2.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel2.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel2.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel2).TabIndex = 6;
		((Control)GroupPanel2).Text = "جستجو در الگوها";
		((Control)TXTSearch).Anchor = (AnchorStyles)13;
		TXTSearch.Border.Class = "TextBoxBorder";
		TXTSearch.Border.CornerType = eCornerType.Square;
		((Control)TXTSearch).Location = new Point(4, 7);
		((Control)TXTSearch).Name = "TXTSearch";
		((Control)TXTSearch).Size = new Size(131, 20);
		((Control)TXTSearch).TabIndex = 12;
		((Control)TXTSearch).TextChanged += TXTSearch_TextChanged;
		((Control)LblSearch_Inbox).Anchor = (AnchorStyles)9;
		((Control)LblSearch_Inbox).BackColor = Color.Transparent;
		((Control)LblSearch_Inbox).Location = new Point(119, 9);
		((Control)LblSearch_Inbox).Name = "LblSearch_Inbox";
		((Control)LblSearch_Inbox).Size = new Size(101, 18);
		((Control)LblSearch_Inbox).TabIndex = 13;
		((Control)LblSearch_Inbox).Text = "جستجو بر اساس :";
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAdd).Anchor = (AnchorStyles)10;
		BtnAdd.ColorTable = eButtonColor.Office2007WithBackground;
		BtnAdd.Image = (Image)(object)Resources.add;
		((Control)BtnAdd).Location = new Point(199, 210);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(33, 30);
		BtnAdd.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnAdd).TabIndex = 15;
		BtnAdd.Tooltip = "افزودن الگوی جدید";
		((Control)BtnAdd).Click += BtnAdd_Click;
		TabItem1.AttachedControl = (Control)(object)TabControlPanel3;
		TabItem1.Name = "TabItem1";
		TabItem1.Text = "استفاده از الگو ها";
		((Control)TabControlPanel4).Controls.Add((Control)(object)UcParametr1);
		TabControlPanel4.DisabledBackColor = Color.Empty;
		((Control)TabControlPanel4).Dock = (DockStyle)5;
		((Control)TabControlPanel4).Location = new Point(0, 26);
		((Control)TabControlPanel4).Name = "TabControlPanel4";
		((Control)TabControlPanel4).Padding = new Padding(1);
		((Control)TabControlPanel4).Size = new Size(236, 244);
		TabControlPanel4.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabControlPanel4.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabControlPanel4.Style.Border = eBorderType.SingleLine;
		TabControlPanel4.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabControlPanel4.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabControlPanel4.Style.GradientAngle = 90;
		((Control)TabControlPanel4).TabIndex = 2;
		TabControlPanel4.TabItem = TabItem2;
		((Control)UcParametr1).Anchor = (AnchorStyles)15;
		((Control)UcParametr1).BackColor = Color.Transparent;
		((Control)UcParametr1).Location = new Point(11, 5);
		((Control)UcParametr1).Name = "UcParametr1";
		((Control)UcParametr1).Size = new Size(215, 235);
		((Control)UcParametr1).TabIndex = 4;
		TabItem2.AttachedControl = (Control)(object)TabControlPanel4;
		TabItem2.Name = "TabItem2";
		TabItem2.Text = "افزودن پارامتر";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)GroupPanel1);
		((Control)this).Name = "UCTemplateList";
		((Control)this).Size = new Size(242, 276);
		((Control)GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)TabControl2).EndInit();
		TabControl2.ResumeLayout(performLayout: false);
		((Control)TabControlPanel3).ResumeLayout(false);
		((Control)Panel1).ResumeLayout(false);
		((ISupportInitialize)Dgrid).EndInit();
		((ISupportInitialize)SMSTemplateBindingSource1).EndInit();
		((Control)GroupPanel2).ResumeLayout(false);
		((Control)TabControlPanel4).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	bool INpDisposable.get_IsDisposed()
	{
		return ((Control)this).IsDisposed;
	}
}
