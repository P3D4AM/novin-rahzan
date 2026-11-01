using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Waiting;
using PacketParser.Entities;
using Serilog;
using TimeAttendance.ValidatorAttribute.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmFormula : FormExtention
{
	private GharardadNew _gharardad;

	private FormulaList _lstFormulaList;

	private int _lastPrivilage;

	private CancellationTokenSource _loadDataToken;

	private IContainer components;

	private PanelEx panelEx1;

	private TextBoxX TXTTitle;

	private LabelX labelX2;

	private LabelX labelX4;

	private LabelX labelX3;

	internal ComboBoxEx CMBType;

	private TextBoxX TXTDescription;

	private LabelX labelX5;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private ComboItem comboItem1;

	private ComboItem comboItem2;

	private LabelX labelX6;

	private ComboItem comboItem3;

	private NumericUpDown numHesabdariCode;

	private NumericUpDown NumCodeTafsil;

	private Button btnCloseParenthes;

	private Button btnSubtract;

	private Button btnSum;

	private Button btnDivision;

	private Button btnProduct;

	private Button button2;

	private Button btnOpenParenthes;

	private TextBoxX txtFormula;

	private LabelX labelX1;

	private ListBox listBoxFormula;

	private GroupPanel groupPanel1;

	private Button btnCama;

	private Button btnIIF;

	public GharardadDetNew GharardadDet { get; private set; }

	public FrmFormula(string contextGuid, GharardadNew gharardad)
		: base(contextGuid, true, true)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			_gharardad = gharardad;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(null, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs", 42);
		}
	}

	public FrmFormula(string contextGuid, GharardadNew gharardad, GharardadDetNew pGharardadDet)
		: base(contextGuid, true, true)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		_gharardad = gharardad;
		InitializeComponent();
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(pGharardadDet, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs");
			((Control)this).Text = "ویرایش فرمول حساب در قرارداد خاص";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs", 61);
		}
	}

	private async Task LoadDataAsync(GharardadDetNew gharardadDet, CancellationToken token)
	{
		try
		{
			if (gharardadDet != null)
			{
				GharardadDet = new GharardadDetNew(((CacheBaseForm)this).ContextGuid, gharardadDet);
				if (GharardadDet != null)
				{
					FishHesab fishHesab = await GharardadDet.GetFishHesabAsync(token);
					while (!((Control)this).IsHandleCreated)
					{
						await Task.Delay(100);
						if (((FormExtention)this).IsDisposed || token.IsCancellationRequested)
						{
							return;
						}
					}
					((Control)this).BeginInvoke((Delegate)(Action)delegate
					{
						try
						{
							if (!((FormExtention)this).IsDisposed && !token.IsCancellationRequested)
							{
								if (GharardadDet.TypeAccount >= 1)
								{
									((ListControl)CMBType).SelectedIndex = GharardadDet.TypeAccount - 1;
								}
								((Control)TXTTitle).Text = GharardadDet.Name;
								((Control)TXTDescription).Text = GharardadDet.Desc;
								((Control)NumCodeTafsil).Text = fishHesab.CodeTafsil.ToString();
								((Control)numHesabdariCode).Text = fishHesab.CodeMoein.ToString();
								((Control)txtFormula).Text = GharardadDet.Formula;
							}
						}
						catch (OperationCanceledException)
						{
						}
						catch (Exception ex3)
						{
							Log.Error(ex3, "");
						}
					});
				}
			}
			base.objsCanEmpty.Add((Control)(object)numHesabdariCode);
			base.objsCanEmpty.Add((Control)(object)NumCodeTafsil);
			((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
			_lstFormulaList = await FormulaList.GetAsync(((CacheBaseForm)this).ContextGuid, null, _gharardad, token);
			foreach (GharardadDetNew item2 in _gharardad.ListGharardadDet.Where((GharardadDetNew q) => q.Privilege >= 1))
			{
				FormulaClass item = new FormulaClass(item2.Formula, item2.Privilege, item2.Desc);
				_lstFormulaList.FormulaClasses.Add(item);
			}
			((ListControl)listBoxFormula).DataSource = _lstFormulaList.FormulaClasses;
			((ListControl)listBoxFormula).DisplayMember = "Name";
			((ListControl)listBoxFormula).ValueMember = "Formula";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs", 114);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
	}

	private void GetData()
	{
		try
		{
			if (GharardadDet == null)
			{
				GharardadDet = new GharardadDetNew(((CacheBaseForm)this).ContextGuid, (short)(((ListControl)CMBType).SelectedIndex + 1), _lastPrivilage, ((Control)txtFormula).Text, ((Control)TXTTitle).Text, ((Control)TXTDescription).Text)
				{
					nValue = -1m,
					Account = -1m,
					HesabId = -1,
					IsDecrease = false
				};
			}
			else
			{
				GharardadDet.Desc = ((Control)TXTDescription).Text;
				GharardadDet.Name = ((Control)TXTTitle).Text;
				GharardadDet.Formula = ((Control)txtFormula).Text;
				GharardadDet.TypeAccount = (short)(((ListControl)CMBType).SelectedIndex + 1);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs", 143);
		}
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		if (!((Control)BtnAccept).Visible || !((Control)BtnAccept).Enabled)
		{
			return;
		}
		try
		{
			GetData();
			DialogResult val = GharardadDetCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, GharardadDet, _lastPrivilage);
			if ((int)val != 2)
			{
				if ((int)val == 6)
				{
					((Form)this).DialogResult = (DialogResult)1;
					((FormExtention)this).ResetControlChanged();
					((Form)this).Close();
				}
			}
			else
			{
				((FormExtention)this).ResetControlChanged();
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs", 169);
		}
	}

	private void listBoxFormula_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			FormulaClass formulaClass = (FormulaClass)listBoxFormula.SelectedItem;
			if (_lastPrivilage < formulaClass.Privilege)
			{
				_lastPrivilage = formulaClass.Privilege;
			}
			TextBoxX obj = txtFormula;
			((Control)obj).Text = ((Control)obj).Text + " [" + formulaClass.Name + "] ";
			if (formulaClass.Value == 0m)
			{
				Utility.MessageBoxShowError("مقدار حساب انتخاب شده صفر است ممکن است در محاسبات شما اشکال ایجاد کند", ((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "listBoxFormula_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs", 186);
		}
	}

	private void btnOpenParenthes_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		try
		{
			Control val = (Control)sender;
			TextBoxX obj = txtFormula;
			((Control)obj).Text = ((Control)obj).Text + " " + val.Text + " ";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnOpenParenthes_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmFormula.cs", 199);
		}
	}

	private void labelX5_Click(object sender, EventArgs e)
	{
	}

	private void BtnCancel_Click_1(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7c: Expected O, but got Unknown
		//IL_1068: Unknown result type (might be due to invalid IL or missing references)
		//IL_1072: Expected O, but got Unknown
		//IL_121d: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1523: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1700: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bc: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		listBoxFormula = new ListBox();
		groupPanel1 = new GroupPanel();
		txtFormula = new TextBoxX();
		btnCama = new Button();
		btnIIF = new Button();
		btnCloseParenthes = new Button();
		btnSubtract = new Button();
		btnSum = new Button();
		btnDivision = new Button();
		btnProduct = new Button();
		button2 = new Button();
		btnOpenParenthes = new Button();
		NumCodeTafsil = new NumericUpDown();
		numHesabdariCode = new NumericUpDown();
		labelX6 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		TXTDescription = new TextBoxX();
		labelX1 = new LabelX();
		labelX5 = new LabelX();
		CMBType = new ComboBoxEx();
		comboItem1 = new ComboItem();
		comboItem2 = new ComboItem();
		comboItem3 = new ComboItem();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		TXTTitle = new TextBoxX();
		labelX2 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel1).SuspendLayout();
		((ISupportInitialize)NumCodeTafsil).BeginInit();
		((ISupportInitialize)numHesabdariCode).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)listBoxFormula);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)btnCama);
		((Control)panelEx1).Controls.Add((Control)(object)btnIIF);
		((Control)panelEx1).Controls.Add((Control)(object)btnCloseParenthes);
		((Control)panelEx1).Controls.Add((Control)(object)btnSubtract);
		((Control)panelEx1).Controls.Add((Control)(object)btnSum);
		((Control)panelEx1).Controls.Add((Control)(object)btnDivision);
		((Control)panelEx1).Controls.Add((Control)(object)btnProduct);
		((Control)panelEx1).Controls.Add((Control)(object)button2);
		((Control)panelEx1).Controls.Add((Control)(object)btnOpenParenthes);
		((Control)panelEx1).Controls.Add((Control)(object)NumCodeTafsil);
		((Control)panelEx1).Controls.Add((Control)(object)numHesabdariCode);
		((Control)panelEx1).Controls.Add((Control)(object)labelX6);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)TXTDescription);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)CMBType);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)TXTTitle);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(784, 340);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((ListControl)listBoxFormula).FormattingEnabled = true;
		((Control)listBoxFormula).Location = new Point(401, 162);
		((Control)listBoxFormula).Name = "listBoxFormula";
		((Control)listBoxFormula).Size = new Size(298, 121);
		((Control)listBoxFormula).TabIndex = 11;
		((Control)listBoxFormula).DoubleClick += listBoxFormula_DoubleClick;
		((Control)groupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)txtFormula);
		((Control)groupPanel1).Location = new Point(12, 12);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(383, 275);
		((PanelControl)groupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel1).Style.Class = "";
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.Class = "";
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.Class = "";
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 20;
		((Control)groupPanel1).Text = "فرمول";
		((Control)txtFormula).Anchor = (AnchorStyles)13;
		txtFormula.Border.Class = "TextBoxBorder";
		txtFormula.Border.CornerType = (eCornerType)1;
		((Control)txtFormula).Location = new Point(3, 3);
		((TextBoxBase)txtFormula).Multiline = true;
		((Control)txtFormula).Name = "txtFormula";
		((Control)txtFormula).RightToLeft = (RightToLeft)1;
		((Control)txtFormula).Size = new Size(362, 247);
		((Control)txtFormula).TabIndex = 0;
		((Control)btnCama).Anchor = (AnchorStyles)9;
		((Control)btnCama).Location = new Point(433, 293);
		((Control)btnCama).Name = "btnCama";
		((Control)btnCama).RightToLeft = (RightToLeft)1;
		((Control)btnCama).Size = new Size(26, 23);
		((Control)btnCama).TabIndex = 12;
		((Control)btnCama).Text = ",";
		((ButtonBase)btnCama).UseVisualStyleBackColor = true;
		((Control)btnCama).Click += btnOpenParenthes_Click;
		((Control)btnIIF).Anchor = (AnchorStyles)9;
		((Control)btnIIF).Location = new Point(369, 293);
		((Control)btnIIF).Name = "btnIIF";
		((Control)btnIIF).RightToLeft = (RightToLeft)1;
		((Control)btnIIF).Size = new Size(58, 23);
		((Control)btnIIF).TabIndex = 12;
		((Control)btnIIF).Text = "شرط";
		((ButtonBase)btnIIF).UseVisualStyleBackColor = true;
		((Control)btnIIF).Click += btnOpenParenthes_Click;
		((Control)btnCloseParenthes).Anchor = (AnchorStyles)9;
		((Control)btnCloseParenthes).Location = new Point(625, 293);
		((Control)btnCloseParenthes).Name = "btnCloseParenthes";
		((Control)btnCloseParenthes).RightToLeft = (RightToLeft)1;
		((Control)btnCloseParenthes).Size = new Size(26, 23);
		((Control)btnCloseParenthes).TabIndex = 12;
		((Control)btnCloseParenthes).Text = "(";
		((ButtonBase)btnCloseParenthes).UseVisualStyleBackColor = true;
		((Control)btnCloseParenthes).Click += btnOpenParenthes_Click;
		((Control)btnSubtract).Anchor = (AnchorStyles)9;
		((Control)btnSubtract).Location = new Point(593, 293);
		((Control)btnSubtract).Name = "btnSubtract";
		((Control)btnSubtract).RightToLeft = (RightToLeft)1;
		((Control)btnSubtract).Size = new Size(26, 23);
		((Control)btnSubtract).TabIndex = 13;
		((Control)btnSubtract).Text = "-";
		((ButtonBase)btnSubtract).UseVisualStyleBackColor = true;
		((Control)btnSubtract).Click += btnOpenParenthes_Click;
		((Control)btnSum).Anchor = (AnchorStyles)9;
		((Control)btnSum).Location = new Point(561, 293);
		((Control)btnSum).Name = "btnSum";
		((Control)btnSum).RightToLeft = (RightToLeft)1;
		((Control)btnSum).Size = new Size(26, 23);
		((Control)btnSum).TabIndex = 14;
		((Control)btnSum).Text = "+";
		((ButtonBase)btnSum).UseVisualStyleBackColor = true;
		((Control)btnSum).Click += btnOpenParenthes_Click;
		((Control)btnDivision).Anchor = (AnchorStyles)9;
		((Control)btnDivision).Location = new Point(529, 293);
		((Control)btnDivision).Name = "btnDivision";
		((Control)btnDivision).RightToLeft = (RightToLeft)1;
		((Control)btnDivision).Size = new Size(26, 23);
		((Control)btnDivision).TabIndex = 15;
		((Control)btnDivision).Text = "/";
		((ButtonBase)btnDivision).UseVisualStyleBackColor = true;
		((Control)btnDivision).Click += btnOpenParenthes_Click;
		((Control)btnProduct).Anchor = (AnchorStyles)9;
		((Control)btnProduct).Location = new Point(497, 293);
		((Control)btnProduct).Name = "btnProduct";
		((Control)btnProduct).RightToLeft = (RightToLeft)1;
		((Control)btnProduct).Size = new Size(26, 23);
		((Control)btnProduct).TabIndex = 16;
		((Control)btnProduct).Text = "*";
		((ButtonBase)btnProduct).UseVisualStyleBackColor = true;
		((Control)btnProduct).Click += btnOpenParenthes_Click;
		((Control)button2).Anchor = (AnchorStyles)9;
		((Control)button2).Location = new Point(497, 293);
		((Control)button2).Name = "button2";
		((Control)button2).RightToLeft = (RightToLeft)1;
		((Control)button2).Size = new Size(26, 23);
		((Control)button2).TabIndex = 16;
		((Control)button2).Text = ")";
		((ButtonBase)button2).UseVisualStyleBackColor = true;
		((Control)btnOpenParenthes).Anchor = (AnchorStyles)9;
		((Control)btnOpenParenthes).Location = new Point(465, 293);
		((Control)btnOpenParenthes).Name = "btnOpenParenthes";
		((Control)btnOpenParenthes).RightToLeft = (RightToLeft)1;
		((Control)btnOpenParenthes).Size = new Size(26, 23);
		((Control)btnOpenParenthes).TabIndex = 17;
		((Control)btnOpenParenthes).Text = ")";
		((ButtonBase)btnOpenParenthes).UseVisualStyleBackColor = true;
		((Control)btnOpenParenthes).Click += btnOpenParenthes_Click;
		((Control)NumCodeTafsil).Anchor = (AnchorStyles)9;
		((Control)NumCodeTafsil).Enabled = false;
		((Control)NumCodeTafsil).Location = new Point(401, 21);
		NumCodeTafsil.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		NumCodeTafsil.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)NumCodeTafsil).Name = "NumCodeTafsil";
		((Control)NumCodeTafsil).Size = new Size(114, 21);
		((Control)NumCodeTafsil).TabIndex = 3;
		((UpDownBase)NumCodeTafsil).TextAlign = (HorizontalAlignment)2;
		NumCodeTafsil.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)NumCodeTafsil).Visible = false;
		((Control)numHesabdariCode).Anchor = (AnchorStyles)9;
		((Control)numHesabdariCode).Enabled = false;
		((Control)numHesabdariCode).Location = new Point(593, 21);
		numHesabdariCode.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numHesabdariCode.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numHesabdariCode).Name = "numHesabdariCode";
		((Control)numHesabdariCode).Size = new Size(106, 21);
		((Control)numHesabdariCode).TabIndex = 1;
		((UpDownBase)numHesabdariCode).TextAlign = (HorizontalAlignment)2;
		numHesabdariCode.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numHesabdariCode).Visible = false;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(521, 21);
		((Control)labelX6).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(60, 21);
		((Control)labelX6).TabIndex = 2;
		((Control)labelX6).Text = "[کد تفصیل]:";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX6).Visible = false;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 293);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 19;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو](Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click_1;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(137, 293);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 18;
		((Control)BtnAccept).Text = "[ذخیره] (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)TXTDescription).Anchor = (AnchorStyles)9;
		TXTDescription.Border.Class = "TextBoxBorder";
		TXTDescription.Border.CornerType = (eCornerType)1;
		((Control)TXTDescription).Location = new Point(401, 102);
		((TextBoxBase)TXTDescription).Multiline = true;
		((Control)TXTDescription).Name = "TXTDescription";
		((Control)TXTDescription).Size = new Size(298, 54);
		((Control)TXTDescription).TabIndex = 9;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(707, 162);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(65, 21);
		((Control)labelX1).TabIndex = 10;
		((Control)labelX1).Text = "[پارامتر]:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(707, 102);
		((Control)labelX5).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(65, 19);
		((Control)labelX5).TabIndex = 8;
		((Control)labelX5).Text = "[شرح]";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Click += labelX5_Click;
		((Control)CMBType).Anchor = (AnchorStyles)9;
		((ListControl)CMBType).DisplayMember = "name";
		((ComboBox)CMBType).DrawMode = (DrawMode)1;
		((ComboBox)CMBType).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMBType).FormattingEnabled = true;
		((ComboBox)CMBType).ItemHeight = 15;
		CMBType.Items.AddRange(new object[3] { comboItem1, comboItem2, comboItem3 });
		((Control)CMBType).Location = new Point(401, 75);
		((Control)CMBType).Name = "CMBType";
		((Control)CMBType).Size = new Size(298, 21);
		CMBType.Style = (eDotNetBarStyle)9;
		((Control)CMBType).TabIndex = 7;
		((Control)CMBType).TabStop = false;
		((ListControl)CMBType).ValueMember = "id";
		comboItem1.FontSize = 12f;
		comboItem1.FontStyle = (FontStyle)1;
		comboItem1.Text = "حقوق و مزایا";
		comboItem1.TextAlignment = (StringAlignment)2;
		comboItem2.FontSize = 12f;
		comboItem2.FontStyle = (FontStyle)1;
		comboItem2.Text = "کسورات";
		comboItem2.TextAlignment = (StringAlignment)2;
		comboItem3.FontSize = 12f;
		comboItem3.FontStyle = (FontStyle)1;
		comboItem3.Text = "سایر";
		comboItem3.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(707, 75);
		((Control)labelX4).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(65, 21);
		((Control)labelX4).TabIndex = 6;
		((Control)labelX4).Text = "[نوع]:";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(707, 21);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(74, 21);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "[کد معین] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Visible = false;
		((Control)TXTTitle).Anchor = (AnchorStyles)9;
		TXTTitle.Border.Class = "TextBoxBorder";
		TXTTitle.Border.CornerType = (eCornerType)1;
		((Control)TXTTitle).Location = new Point(401, 48);
		((Control)TXTTitle).Name = "TXTTitle";
		((Control)TXTTitle).Size = new Size(298, 21);
		((Control)TXTTitle).TabIndex = 5;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(707, 48);
		((Control)labelX2).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(65, 21);
		((Control)labelX2).TabIndex = 4;
		((Control)labelX2).Text = "[نام حساب] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(784, 362);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmFormula";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "افزودن مزایا و کسورات بصورت  فرمول*";
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel1).ResumeLayout(false);
		((ISupportInitialize)NumCodeTafsil).EndInit();
		((ISupportInitialize)numHesabdariCode).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
