using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Person.Work;

public class FrmWorkT : BaseForm
{
	private string frm_Text = "[افزودن عنوان شغلی جدید]";

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnRegister;

	internal ButtonX Btncancela;

	internal PanelEx PanelEx2;

	internal TextBoxX TxtDesc;

	internal LabelX labelX2;

	internal TextBoxX TxtType;

	internal LabelX LabelX1;

	internal TextBoxX TxtCode;

	internal LabelX labelX3;

	public Per_WorkTitle WorkTitle { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
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
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmWorkT(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		try
		{
			WorkTitle = new Per_WorkTitle(((CacheBaseForm)this).ContextGuid);
			((Control)this).Text = "افزودن عنوان شغلی جدید";
			frm_Text = "[افزودن عنوان شغلی جدید]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 62);
		}
	}

	public FrmWorkT(string contextGuid, Per_WorkTitle workTitle)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		try
		{
			WorkTitle = workTitle;
			FillForm();
			((Control)this).Text = "ویرایش عنوان شغلی";
			frm_Text = "[ویرایش عنوان شغلی]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 78);
		}
	}

	private void FillForm()
	{
		try
		{
			((Control)TxtType).Text = WorkTitle.Name;
			((Control)TxtDesc).Text = WorkTitle.Desc;
			((Control)TxtCode).Text = WorkTitle.Code;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 91);
		}
	}

	private void GetData()
	{
		try
		{
			WorkTitle.Name = Text.FixString(((Control)TxtType).Text.Trim());
			WorkTitle.Desc = Text.FixString(((Control)TxtDesc).Text.Trim());
			WorkTitle.Code = Text.FixString(((Control)TxtCode).Text.Trim());
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 104);
		}
	}

	private bool CheckValid()
	{
		return ((Control)TxtType).Text != "";
	}

	private async void BtnRegister_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnRegister).Visible || !((Control)BtnRegister).Enabled)
		{
			return;
		}
		try
		{
			GetData();
			if (!CheckValid())
			{
				Utility.MessageBoxShowError("[لطفا عنوان شغل را پر کنید]", ((CacheBaseForm)this).ContextGuid);
				((Control)TxtType).Focus();
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			if (await WorkTitle.SaveAsync(token))
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRegister_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 134);
		}
	}

	private void FrmWorkT_Load(object sender, EventArgs e)
	{
		((Control)TxtCode).Focus();
		try
		{
			frm_Text = Utilities.GetTranslate(frm_Text);
			((Control)this).Text = frm_Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmWorkT_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 148);
		}
	}

	private void FrmWorkT_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 116)
					{
						e.Handled = true;
						((PopupItemControl)BtnRegister).PerformClick();
					}
				}
				else
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmWorkT_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 175);
		}
	}

	private void Btncancela_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Btncancela_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 188);
		}
	}

	private void TxtCode_Enter_1(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((TextBoxBase)(TextBox)sender).SelectAll();
			((Control)(TextBox)sender).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtCode_Enter_1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 201);
		}
	}

	private void TxtCode_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(TextBox)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtCode_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Work\\FrmWorkT.cs", 213);
		}
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
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Expected O, but got Unknown
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Expected O, but got Unknown
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Expected O, but got Unknown
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		BtnRegister = new ButtonX();
		Btncancela = new ButtonX();
		PanelEx2 = new PanelEx();
		TxtCode = new TextBoxX();
		labelX3 = new LabelX();
		TxtDesc = new TextBoxX();
		labelX2 = new LabelX();
		TxtType = new TextBoxX();
		LabelX1 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)PanelEx2).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnRegister);
		((Control)PanelEx1).Controls.Add((Control)(object)Btncancela);
		((Control)PanelEx1).Controls.Add((Control)(object)PanelEx2);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(384, 213);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.SteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BtnRegister).AccessibleRole = (AccessibleRole)43;
		((Control)BtnRegister).Anchor = (AnchorStyles)6;
		BtnRegister.ColorTable = (eButtonColor)3;
		((Control)BtnRegister).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnRegister.Image = (Image)(object)Resources.accept;
		BtnRegister.ImagePosition = (eImagePosition)1;
		((Control)BtnRegister).Location = new Point(118, 168);
		((Control)BtnRegister).Name = "BtnRegister";
		((Control)BtnRegister).Size = new Size(106, 33);
		((PopupItemControl)BtnRegister).Style = (eDotNetBarStyle)9;
		((Control)BtnRegister).TabIndex = 1;
		((Control)BtnRegister).Text = "[ثبت] (F5)";
		((Control)BtnRegister).Click += BtnRegister_Click;
		((Control)Btncancela).AccessibleRole = (AccessibleRole)43;
		((Control)Btncancela).Anchor = (AnchorStyles)6;
		Btncancela.ColorTable = (eButtonColor)3;
		((Control)Btncancela).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		Btncancela.Image = (Image)(object)Resources.stop1;
		Btncancela.ImagePosition = (eImagePosition)1;
		((Control)Btncancela).Location = new Point(3, 168);
		((Control)Btncancela).Name = "Btncancela";
		((Control)Btncancela).Size = new Size(106, 33);
		((PopupItemControl)Btncancela).Style = (eDotNetBarStyle)9;
		((Control)Btncancela).TabIndex = 2;
		((Control)Btncancela).TabStop = false;
		((Control)Btncancela).Text = "[لغو] (Esc)";
		((Control)Btncancela).Click += Btncancela_Click;
		((Control)PanelEx2).Anchor = (AnchorStyles)15;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)TxtCode);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX3);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtDesc);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtType);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX1);
		((Control)PanelEx2).Location = new Point(3, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(375, 150);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		TxtCode.Border.Class = "TextBoxBorder";
		TxtCode.Border.CornerType = (eCornerType)1;
		((Control)TxtCode).Location = new Point(136, 14);
		((Control)TxtCode).Name = "TxtCode";
		((Control)TxtCode).Size = new Size(150, 21);
		((Control)TxtCode).TabIndex = 1;
		((Control)TxtCode).Leave += TxtCode_Leave;
		((Control)TxtCode).Enter += TxtCode_Enter_1;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(292, 13);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(72, 23);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "[کد شغل] :";
		labelX3.TextAlignment = (StringAlignment)2;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(15, 68);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(271, 69);
		((Control)TxtDesc).TabIndex = 5;
		((Control)TxtDesc).Leave += TxtCode_Leave;
		((Control)TxtDesc).Enter += TxtCode_Enter_1;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(292, 69);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(72, 23);
		((Control)labelX2).TabIndex = 4;
		((Control)labelX2).Text = "[شرح] :";
		labelX2.TextAlignment = (StringAlignment)2;
		TxtType.Border.Class = "TextBoxBorder";
		TxtType.Border.CornerType = (eCornerType)1;
		((Control)TxtType).Location = new Point(136, 41);
		((Control)TxtType).Name = "TxtType";
		((Control)TxtType).Size = new Size(150, 21);
		((Control)TxtType).TabIndex = 3;
		((Control)TxtType).Leave += TxtCode_Leave;
		((Control)TxtType).Enter += TxtCode_Enter_1;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX1).Location = new Point(292, 40);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(72, 23);
		((Control)LabelX1).TabIndex = 2;
		((Control)LabelX1).Text = "[عنوان شغل] :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(384, 213);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmWorkT";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).Load += FrmWorkT_Load;
		((Control)this).KeyDown += new KeyEventHandler(FrmWorkT_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
