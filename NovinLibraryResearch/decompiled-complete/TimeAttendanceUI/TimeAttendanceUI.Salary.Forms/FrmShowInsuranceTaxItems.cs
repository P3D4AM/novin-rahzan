using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmShowInsuranceTaxItems : CacheBaseForm
{
	private IContainer components;

	private BindingSource HesabDetBindingSource;

	internal DataGridViewX DGrid;

	internal PanelEx PanelEx1;

	private LabelX labelX37;

	private TextBoxFa TXTKolMande;

	private LabelX labelX36;

	private TextBoxFa TXTKolKosorat;

	private LabelX labelX35;

	private TextBoxFa TXTKolMazaya;

	private LabelX labelX1;

	private DataGridViewTextBoxColumn descDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn creditDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn debitDataGridViewTextBoxColumn;

	private TextBox TbxFactor;

	public List<HesabDet> HesabDets { get; set; }

	public decimal Factor { get; set; }

	public FrmShowInsuranceTaxItems(string contextGuid, List<HesabDet> hesabDets, decimal factor, string frmTitle)
		: base(contextGuid)
	{
		InitializeComponent();
		((Control)this).Text = frmTitle;
		HesabDets = hesabDets;
		Factor = factor;
	}

	private void FrmShowInsuranceTaxItems_Load(object sender, EventArgs e)
	{
		if (HesabDets != null)
		{
			HesabDetBindingSource.DataSource = HesabDets;
			decimal? num = HesabDets?.Sum((HesabDet p) => p.debit);
			decimal? num2 = HesabDets?.Sum((HesabDet p) => p.credit);
			((DataGridView)DGrid).DataSource = HesabDetBindingSource;
			((Control)TXTKolKosorat).Text = num?.ToString("N0");
			((Control)TXTKolMazaya).Text = num2?.ToString("N0");
			((Control)TXTKolMande).Text = (num2 - num)?.ToString("N0");
			decimal? num3 = num2 - num;
			if ((num3.GetValueOrDefault() < default(decimal)) & num3.HasValue)
			{
				((Control)TXTKolMande).ForeColor = Color.Red;
			}
			((Control)TbxFactor).Text = Factor.ToString();
		}
	}

	private async void DGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((Form)this).Close();
			}
			else if ((int)e.KeyCode == 123)
			{
				FrmCalc s = new FrmCalc("");
				await ((CacheBaseForm)s).ShowAsync((IWin32Window)(object)this);
				((Component)(object)s).Dispose();
			}
			else if ((int)e.KeyCode == 67)
			{
				Process.Start("calc.exe");
			}
		}
		catch (Exception)
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((CacheBaseForm)this).Dispose(disposing);
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
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Expected O, but got Unknown
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Expected O, but got Unknown
		//IL_09a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baa: Expected O, but got Unknown
		//IL_0c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Expected O, but got Unknown
		//IL_0eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Expected O, but got Unknown
		//IL_0ebd: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		descDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		creditDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		debitDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		HesabDetBindingSource = new BindingSource(components);
		PanelEx1 = new PanelEx();
		TbxFactor = new TextBox();
		labelX1 = new LabelX();
		labelX37 = new LabelX();
		TXTKolMande = new TextBoxFa();
		labelX36 = new LabelX();
		TXTKolKosorat = new TextBoxFa();
		labelX35 = new LabelX();
		TXTKolMazaya = new TextBoxFa();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)HesabDetBindingSource).BeginInit();
		((Control)PanelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToOrderColumns = true;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).BackgroundColor = Color.White;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)descDataGridViewTextBoxColumn,
			(DataGridViewColumn)creditDataGridViewTextBoxColumn,
			(DataGridViewColumn)debitDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = HesabDetBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8f, (FontStyle)1);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(4, 4);
		((Control)DGrid).Margin = new Padding(4);
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(653, 397);
		((Control)DGrid).TabIndex = 7;
		((Control)DGrid).PreviewKeyDown += new PreviewKeyDownEventHandler(DGrid_PreviewKeyDown);
		((DataGridViewColumn)descDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).DataPropertyName = "desc";
		val2.NullValue = null;
		((DataGridViewBand)descDataGridViewTextBoxColumn).DefaultCellStyle = val2;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).HeaderText = "عنوان ";
		((DataGridViewColumn)descDataGridViewTextBoxColumn).Name = "descDataGridViewTextBoxColumn";
		((DataGridViewBand)descDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)creditDataGridViewTextBoxColumn).DataPropertyName = "credit";
		val3.ForeColor = Color.Green;
		val3.Format = "N0";
		((DataGridViewBand)creditDataGridViewTextBoxColumn).DefaultCellStyle = val3;
		((DataGridViewColumn)creditDataGridViewTextBoxColumn).HeaderText = "اضافه";
		((DataGridViewColumn)creditDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)creditDataGridViewTextBoxColumn).Name = "creditDataGridViewTextBoxColumn";
		((DataGridViewBand)creditDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)debitDataGridViewTextBoxColumn).DataPropertyName = "debit";
		val4.ForeColor = Color.Red;
		val4.Format = "N0";
		((DataGridViewBand)debitDataGridViewTextBoxColumn).DefaultCellStyle = val4;
		((DataGridViewColumn)debitDataGridViewTextBoxColumn).HeaderText = "کسر";
		((DataGridViewColumn)debitDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)debitDataGridViewTextBoxColumn).Name = "debitDataGridViewTextBoxColumn";
		((DataGridViewBand)debitDataGridViewTextBoxColumn).ReadOnly = true;
		HesabDetBindingSource.DataSource = typeof(HesabDet);
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)TbxFactor);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX1);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX37);
		((Control)PanelEx1).Controls.Add((Control)(object)TXTKolMande);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX36);
		((Control)PanelEx1).Controls.Add((Control)(object)TXTKolKosorat);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX35);
		((Control)PanelEx1).Controls.Add((Control)(object)TXTKolMazaya);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(660, 464);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 4;
		((Control)TbxFactor).Location = new Point(503, 407);
		((Control)TbxFactor).Name = "TbxFactor";
		((Control)TbxFactor).Size = new Size(62, 20);
		((Control)TbxFactor).TabIndex = 67;
		((Control)labelX1).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(572, 407);
		((Control)labelX1).Margin = new Padding(4, 2, 4, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(82, 21);
		((Control)labelX1).TabIndex = 66;
		((Control)labelX1).Text = "ضریب محاسبه:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX37).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX37).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX37).Location = new Point(155, 433);
		((Control)labelX37).Margin = new Padding(4, 2, 4, 2);
		((Control)labelX37).Name = "labelX37";
		((Control)labelX37).RightToLeft = (RightToLeft)1;
		((Control)labelX37).Size = new Size(91, 21);
		((Control)labelX37).TabIndex = 60;
		((Control)labelX37).Text = "جمع کل:";
		labelX37.TextAlignment = (StringAlignment)2;
		TXTKolMande.AllowEmpty = true;
		((Control)TXTKolMande).Anchor = (AnchorStyles)6;
		((Control)TXTKolMande).BackColor = Color.White;
		TXTKolMande.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKolMande.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKolMande.BackcolorTooltip = Color.Red;
		TXTKolMande.CurrencyFormat = "";
		((Control)TXTKolMande).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TXTKolMande).ForeColor = Color.Navy;
		TXTKolMande.ForeColorTooltip = Color.Black;
		TXTKolMande.GetColoresFromSettingDLL = true;
		TXTKolMande.HandleKeydown = true;
		((Control)TXTKolMande).Location = new Point(5, 433);
		((Control)TXTKolMande).Margin = new Padding(4);
		((TextBoxBase)TXTKolMande).MaxLength = 19;
		((Control)TXTKolMande).Name = "TXTKolMande";
		((TextBoxBase)TXTKolMande).ReadOnly = true;
		((Control)TXTKolMande).RightToLeft = (RightToLeft)1;
		TXTKolMande.SendShiftTabAfterDown = true;
		TXTKolMande.SendShiftTabAfterEnter = true;
		TXTKolMande.SendTabAfterEnter = true;
		TXTKolMande.SendTabAfterUP = true;
		TXTKolMande.ShowHorofi = (ENShowHorofi)3;
		TXTKolMande.ShowToolTipText = true;
		((Control)TXTKolMande).Size = new Size(143, 22);
		((Control)TXTKolMande).TabIndex = 65;
		((Control)TXTKolMande).TabStop = false;
		((Control)TXTKolMande).Text = "0";
		TXTKolMande.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKolMande.ToolTipPostFix = "";
		TXTKolMande.TootTipDelay = 1000;
		TXTKolMande.Value = new decimal(new int[4]);
		((Control)labelX36).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX36).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX36).Location = new Point(155, 407);
		((Control)labelX36).Margin = new Padding(4, 2, 4, 2);
		((Control)labelX36).Name = "labelX36";
		((Control)labelX36).RightToLeft = (RightToLeft)1;
		((Control)labelX36).Size = new Size(91, 21);
		((Control)labelX36).TabIndex = 61;
		((Control)labelX36).Text = "مجموع کسورات:";
		labelX36.TextAlignment = (StringAlignment)2;
		TXTKolKosorat.AllowEmpty = true;
		((Control)TXTKolKosorat).Anchor = (AnchorStyles)6;
		((Control)TXTKolKosorat).BackColor = Color.White;
		TXTKolKosorat.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKolKosorat.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKolKosorat.BackcolorTooltip = Color.Red;
		TXTKolKosorat.CurrencyFormat = "";
		((Control)TXTKolKosorat).ForeColor = Color.Red;
		TXTKolKosorat.ForeColorTooltip = Color.Black;
		TXTKolKosorat.GetColoresFromSettingDLL = false;
		TXTKolKosorat.HandleKeydown = true;
		((Control)TXTKolKosorat).Location = new Point(5, 407);
		((Control)TXTKolKosorat).Margin = new Padding(4);
		((TextBoxBase)TXTKolKosorat).MaxLength = 19;
		((Control)TXTKolKosorat).Name = "TXTKolKosorat";
		((TextBoxBase)TXTKolKosorat).ReadOnly = true;
		((Control)TXTKolKosorat).RightToLeft = (RightToLeft)1;
		TXTKolKosorat.SendShiftTabAfterDown = true;
		TXTKolKosorat.SendShiftTabAfterEnter = true;
		TXTKolKosorat.SendTabAfterEnter = true;
		TXTKolKosorat.SendTabAfterUP = true;
		TXTKolKosorat.ShowHorofi = (ENShowHorofi)3;
		TXTKolKosorat.ShowToolTipText = true;
		((Control)TXTKolKosorat).Size = new Size(143, 20);
		((Control)TXTKolKosorat).TabIndex = 64;
		((Control)TXTKolKosorat).TabStop = false;
		((Control)TXTKolKosorat).Text = "0";
		TXTKolKosorat.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKolKosorat.ToolTipPostFix = "";
		TXTKolKosorat.TootTipDelay = 1000;
		TXTKolKosorat.Value = new decimal(new int[4]);
		((Control)labelX35).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX35).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX35).Location = new Point(404, 407);
		((Control)labelX35).Margin = new Padding(4, 2, 4, 2);
		((Control)labelX35).Name = "labelX35";
		((Control)labelX35).RightToLeft = (RightToLeft)1;
		((Control)labelX35).Size = new Size(91, 21);
		((Control)labelX35).TabIndex = 62;
		((Control)labelX35).Text = "مجموع  اضافات:";
		labelX35.TextAlignment = (StringAlignment)2;
		TXTKolMazaya.AllowEmpty = true;
		((Control)TXTKolMazaya).Anchor = (AnchorStyles)6;
		((Control)TXTKolMazaya).BackColor = Color.White;
		TXTKolMazaya.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTKolMazaya.BackColorTextBoxWhenLostFocus = Color.White;
		TXTKolMazaya.BackcolorTooltip = Color.Red;
		TXTKolMazaya.CurrencyFormat = "";
		((Control)TXTKolMazaya).ForeColor = Color.Green;
		TXTKolMazaya.ForeColorTooltip = Color.Black;
		TXTKolMazaya.GetColoresFromSettingDLL = true;
		TXTKolMazaya.HandleKeydown = true;
		((Control)TXTKolMazaya).Location = new Point(253, 407);
		((Control)TXTKolMazaya).Margin = new Padding(4);
		((TextBoxBase)TXTKolMazaya).MaxLength = 19;
		((Control)TXTKolMazaya).Name = "TXTKolMazaya";
		((TextBoxBase)TXTKolMazaya).ReadOnly = true;
		((Control)TXTKolMazaya).RightToLeft = (RightToLeft)1;
		TXTKolMazaya.SendShiftTabAfterDown = true;
		TXTKolMazaya.SendShiftTabAfterEnter = true;
		TXTKolMazaya.SendTabAfterEnter = true;
		TXTKolMazaya.SendTabAfterUP = true;
		TXTKolMazaya.ShowHorofi = (ENShowHorofi)3;
		TXTKolMazaya.ShowToolTipText = true;
		((Control)TXTKolMazaya).Size = new Size(143, 20);
		((Control)TXTKolMazaya).TabIndex = 63;
		((Control)TXTKolMazaya).TabStop = false;
		((Control)TXTKolMazaya).Text = "0";
		TXTKolMazaya.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TXTKolMazaya.ToolTipPostFix = "";
		TXTKolMazaya.TootTipDelay = 1000;
		TXTKolMazaya.Value = new decimal(new int[4]);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(660, 464);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Form)this).Margin = new Padding(4);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmShowInsuranceTaxItems";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "فرم محاسبه مبلغ مشمول بیمه/مالیات";
		((Form)this).Load += FrmShowInsuranceTaxItems_Load;
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)HesabDetBindingSource).EndInit();
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
