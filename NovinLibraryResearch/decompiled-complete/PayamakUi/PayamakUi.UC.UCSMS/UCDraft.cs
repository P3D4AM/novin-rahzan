using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PayamakUi.UC.UCSMS;

public class UCDraft : UserControl, IHasContext
{
	private IContainer components;

	internal TabControl TabC2_Inbox;

	internal TabControlPanel TabCp;

	internal ButtonX BTNExit;

	internal DataGridViewX DGrid;

	internal GroupPanel GroupPanel_Search_Inbox;

	internal Label LblSearch_Inbox;

	internal TextBoxX TXTSearch;

	internal TabItem TabItem1Draft;

	internal ButtonX Btn_Twicesend;

	internal ButtonX BtnEdit;

	internal ButtonX BtnForward;

	internal ButtonX BtnDelete;

	internal ButtonX BtnSave;

	public string ContextGuid { get; set; }

	public UCDraft()
	{
		InitializeComponent();
		LoadDataAsync();
	}

	public async Task LoadDataAsync()
	{
		try
		{
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCDraft.cs", 28);
		}
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Hide();
			((Control)this).Visible = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCDraft.cs", 40);
		}
	}

	public void Search_Press(char keyChar)
	{
		try
		{
			if (!((Control)TXTSearch).Focused)
			{
				((Control)TXTSearch).Focus();
				((Control)TXTSearch).Text = keyChar.ToString();
				((TextBoxBase)TXTSearch).SelectionStart = 999;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Search_Press", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCDraft.cs", 61);
		}
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		TabC2_Inbox = new TabControl();
		TabCp = new TabControlPanel();
		BTNExit = new ButtonX();
		DGrid = new DataGridViewX();
		GroupPanel_Search_Inbox = new GroupPanel();
		LblSearch_Inbox = new Label();
		TXTSearch = new TextBoxX();
		TabItem1Draft = new TabItem(components);
		Btn_Twicesend = new ButtonX();
		BtnEdit = new ButtonX();
		BtnForward = new ButtonX();
		BtnDelete = new ButtonX();
		BtnSave = new ButtonX();
		((ISupportInitialize)TabC2_Inbox).BeginInit();
		((Control)TabC2_Inbox).SuspendLayout();
		((Control)TabCp).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)GroupPanel_Search_Inbox).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)TabC2_Inbox).Anchor = (AnchorStyles)15;
		((Control)TabC2_Inbox).BackColor = Color.Transparent;
		TabC2_Inbox.CanReorderTabs = true;
		((Control)TabC2_Inbox).Controls.Add((Control)(object)TabCp);
		((Control)TabC2_Inbox).Location = new Point(3, 3);
		((Control)TabC2_Inbox).Name = "TabC2_Inbox";
		((Control)TabC2_Inbox).RightToLeft = (RightToLeft)1;
		TabC2_Inbox.SelectedTabFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1);
		TabC2_Inbox.SelectedTabIndex = 0;
		((Control)TabC2_Inbox).Size = new Size(414, 286);
		((Control)TabC2_Inbox).TabIndex = 1;
		TabC2_Inbox.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		TabC2_Inbox.Tabs.Add(TabItem1Draft);
		((Control)TabC2_Inbox).Text = "TabControl1";
		((Control)TabCp).Controls.Add((Control)(object)Btn_Twicesend);
		((Control)TabCp).Controls.Add((Control)(object)BtnEdit);
		((Control)TabCp).Controls.Add((Control)(object)BtnForward);
		((Control)TabCp).Controls.Add((Control)(object)BtnDelete);
		((Control)TabCp).Controls.Add((Control)(object)BtnSave);
		((Control)TabCp).Controls.Add((Control)(object)BTNExit);
		((Control)TabCp).Controls.Add((Control)(object)DGrid);
		((Control)TabCp).Controls.Add((Control)(object)GroupPanel_Search_Inbox);
		TabCp.DisabledBackColor = Color.Empty;
		((Control)TabCp).Dock = (DockStyle)5;
		((Control)TabCp).Location = new Point(0, 26);
		((Control)TabCp).Name = "TabCp";
		((Control)TabCp).Padding = new Padding(1);
		((Control)TabCp).Size = new Size(414, 260);
		TabCp.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		TabCp.Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		TabCp.Style.Border = eBorderType.SingleLine;
		TabCp.Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		TabCp.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		TabCp.Style.GradientAngle = 90;
		((Control)TabCp).TabIndex = 1;
		TabCp.TabItem = TabItem1Draft;
		((Control)BTNExit).AccessibleRole = (AccessibleRole)43;
		((Control)BTNExit).Anchor = (AnchorStyles)6;
		BTNExit.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BTNExit).Location = new Point(4, 229);
		((Control)BTNExit).Name = "BTNExit";
		((Control)BTNExit).Size = new Size(30, 28);
		((Control)BTNExit).TabIndex = 12;
		((Control)BTNExit).TabStop = false;
		BTNExit.Tooltip = "خروج از صفحه";
		((Control)BTNExit).Click += BtnExit_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.MintCream;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).EditMode = (DataGridViewEditMode)4;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(4, 78);
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(407, 145);
		((Control)DGrid).TabIndex = 3;
		((Control)GroupPanel_Search_Inbox).Anchor = (AnchorStyles)13;
		((Control)GroupPanel_Search_Inbox).BackColor = Color.Transparent;
		GroupPanel_Search_Inbox.CanvasColor = SystemColors.Control;
		GroupPanel_Search_Inbox.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel_Search_Inbox).Controls.Add((Control)(object)LblSearch_Inbox);
		((Control)GroupPanel_Search_Inbox).Controls.Add((Control)(object)TXTSearch);
		GroupPanel_Search_Inbox.DisabledBackColor = Color.Empty;
		((Control)GroupPanel_Search_Inbox).Location = new Point(3, 1);
		((Control)GroupPanel_Search_Inbox).Name = "GroupPanel_Search_Inbox";
		((Control)GroupPanel_Search_Inbox).Size = new Size(408, 71);
		GroupPanel_Search_Inbox.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel_Search_Inbox.Style.BackColorGradientAngle = 90;
		GroupPanel_Search_Inbox.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel_Search_Inbox.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel_Search_Inbox.Style.BorderBottomWidth = 1;
		GroupPanel_Search_Inbox.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel_Search_Inbox.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel_Search_Inbox.Style.BorderLeftWidth = 1;
		GroupPanel_Search_Inbox.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel_Search_Inbox.Style.BorderRightWidth = 1;
		GroupPanel_Search_Inbox.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel_Search_Inbox.Style.BorderTopWidth = 1;
		GroupPanel_Search_Inbox.Style.CornerDiameter = 4;
		GroupPanel_Search_Inbox.Style.CornerType = eCornerType.Rounded;
		GroupPanel_Search_Inbox.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel_Search_Inbox.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel_Search_Inbox.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel_Search_Inbox.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel_Search_Inbox.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel_Search_Inbox).TabIndex = 2;
		((Control)GroupPanel_Search_Inbox).Text = "جستجو در پیام های پیش نویس";
		((Control)LblSearch_Inbox).Anchor = (AnchorStyles)9;
		((Control)LblSearch_Inbox).Text = "";
		((Control)LblSearch_Inbox).Location = new Point(300, 3);
		((Control)LblSearch_Inbox).Name = "LblSearch_Inbox";
		((Control)LblSearch_Inbox).Size = new Size(102, 25);
		((Control)LblSearch_Inbox).TabIndex = 11;
		((Control)LblSearch_Inbox).Text = "جستجو بر اساس :";
		((Control)TXTSearch).Anchor = (AnchorStyles)13;
		TXTSearch.Border.Class = "TextBoxBorder";
		TXTSearch.Border.CornerType = eCornerType.Square;
		((Control)TXTSearch).Location = new Point(3, 7);
		((Control)TXTSearch).Name = "TXTSearch";
		((Control)TXTSearch).Size = new Size(291, 20);
		((Control)TXTSearch).TabIndex = 1;
		TabItem1Draft.AttachedControl = (Control)(object)TabCp;
		TabItem1Draft.Name = "TabItem1Draft";
		TabItem1Draft.Text = "پیش نویس پیام";
		((Control)Btn_Twicesend).AccessibleRole = (AccessibleRole)43;
		((Control)Btn_Twicesend).Anchor = (AnchorStyles)10;
		Btn_Twicesend.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)Btn_Twicesend).Location = new Point(378, 228);
		((Control)Btn_Twicesend).Name = "Btn_Twicesend";
		((Control)Btn_Twicesend).Size = new Size(30, 28);
		((Control)Btn_Twicesend).TabIndex = 25;
		Btn_Twicesend.Tooltip = "ارسال مجدد پیام";
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		((Control)BtnEdit).Anchor = (AnchorStyles)10;
		BtnEdit.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnEdit).Location = new Point(304, 228);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).Size = new Size(30, 28);
		((Control)BtnEdit).TabIndex = 24;
		BtnEdit.Tooltip = "ویرایش پیام";
		((Control)BtnForward).AccessibleRole = (AccessibleRole)43;
		((Control)BtnForward).Anchor = (AnchorStyles)10;
		BtnForward.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnForward).Location = new Point(342, 228);
		((Control)BtnForward).Name = "BtnForward";
		((Control)BtnForward).Size = new Size(30, 28);
		((Control)BtnForward).TabIndex = 23;
		BtnForward.Tooltip = "ارسال پیام به دیگری";
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDelete).Anchor = (AnchorStyles)10;
		BtnDelete.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnDelete).Location = new Point(264, 228);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(30, 28);
		((Control)BtnDelete).TabIndex = 21;
		BtnDelete.Tooltip = "حذف پیام از پیش نویس";
		((Control)BtnSave).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSave).Anchor = (AnchorStyles)10;
		BtnSave.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnSave).Location = new Point(228, 228);
		((Control)BtnSave).Name = "BtnSave";
		((Control)BtnSave).Size = new Size(30, 28);
		((Control)BtnSave).TabIndex = 22;
		BtnSave.Tooltip = "انتقال به....";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)TabC2_Inbox);
		((Control)this).Name = "UCDraft";
		((Control)this).Size = new Size(420, 292);
		((ISupportInitialize)TabC2_Inbox).EndInit();
		TabC2_Inbox.ResumeLayout(performLayout: false);
		((Control)TabCp).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)GroupPanel_Search_Inbox).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
