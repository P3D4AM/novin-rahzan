using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Person.TypeEstekhdam;

public class FRMTypeE : CacheBaseForm, IHasContext
{
	private CancellationTokenSource _loadDataToken;

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

	public Per_Typeestekhdam TypeEstekhdam { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((CacheBaseForm)this).IsDisposed;
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
				components?.Dispose();
				((CacheBaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FRMTypeE(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
		try
		{
			TypeEstekhdam = new Per_Typeestekhdam(((CacheBaseForm)this).ContextGuid);
			((Control)this).Text = "افزودن نوع استخدامی جدید";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 54);
		}
	}

	public FRMTypeE(string contextGuid, Per_Typeestekhdam typeestekhdam)
		: base(contextGuid)
	{
		InitializeComponent();
		try
		{
			TypeEstekhdam = typeestekhdam;
			FillForm();
			((Control)this).Text = "ویرایش نوع استخدامی";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 69);
		}
	}

	private void FillForm()
	{
		try
		{
			((Control)TxtType).Text = TypeEstekhdam.Name;
			((Control)TxtDesc).Text = TypeEstekhdam.Desc;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 81);
		}
	}

	private void GetData()
	{
		try
		{
			TypeEstekhdam.Name = Text.FixString(((Control)TxtType).Text.Trim());
			TypeEstekhdam.Desc = Text.FixString(((Control)TxtDesc).Text.Trim());
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 93);
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
				Utility.MessageBoxShowError("لطفا نوع استخدامی را پر کنید", ((CacheBaseForm)this).ContextGuid);
				((Control)TxtType).Focus();
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			if (await TypeEstekhdam.SaveAsync(token))
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRegister_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 122);
		}
	}

	private void FRMTypeE_Load(object sender, EventArgs e)
	{
		((Control)TxtType).Focus();
	}

	private void FRMTypeE_KeyDown(object sender, KeyEventArgs e)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMTypeE_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 154);
		}
	}

	private void TxtType_Enter(object sender, EventArgs e)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtType_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 167);
		}
	}

	private void TxtType_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(TextBox)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtType_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 179);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Btncancela_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\TypeEstekhdam\\FRMTypeE.cs", 192);
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
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Expected O, but got Unknown
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Expected O, but got Unknown
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Expected O, but got Unknown
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		BtnRegister = new ButtonX();
		Btncancela = new ButtonX();
		PanelEx2 = new PanelEx();
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
		((Control)PanelEx1).Size = new Size(378, 158);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.SteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 3;
		((Control)BtnRegister).AccessibleRole = (AccessibleRole)43;
		((Control)BtnRegister).Anchor = (AnchorStyles)6;
		BtnRegister.ColorTable = (eButtonColor)3;
		((Control)BtnRegister).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnRegister.Image = (Image)(object)Resources.accept;
		BtnRegister.ImagePosition = (eImagePosition)1;
		((Control)BtnRegister).Location = new Point(118, 113);
		((Control)BtnRegister).Name = "BtnRegister";
		((Control)BtnRegister).Size = new Size(106, 33);
		((PopupItemControl)BtnRegister).Style = (eDotNetBarStyle)9;
		((Control)BtnRegister).TabIndex = 1;
		((Control)BtnRegister).Text = "ثبت(F5)";
		((Control)BtnRegister).Click += BtnRegister_Click;
		((Control)Btncancela).AccessibleRole = (AccessibleRole)43;
		((Control)Btncancela).Anchor = (AnchorStyles)6;
		Btncancela.ColorTable = (eButtonColor)3;
		((Control)Btncancela).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		Btncancela.Image = (Image)(object)Resources.stop1;
		Btncancela.ImagePosition = (eImagePosition)1;
		((Control)Btncancela).Location = new Point(3, 113);
		((Control)Btncancela).Name = "Btncancela";
		((Control)Btncancela).Size = new Size(106, 33);
		((PopupItemControl)Btncancela).Style = (eDotNetBarStyle)9;
		((Control)Btncancela).TabIndex = 2;
		((Control)Btncancela).TabStop = false;
		((Control)Btncancela).Text = "لغـو(Esc)";
		((Control)Btncancela).Click += Btncancela_Click;
		((Control)PanelEx2).Anchor = (AnchorStyles)15;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)TxtDesc);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtType);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX1);
		((Control)PanelEx2).Location = new Point(3, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(369, 95);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(9, 41);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(259, 40);
		((Control)TxtDesc).TabIndex = 3;
		((Control)TxtDesc).Leave += TxtType_Leave;
		((Control)TxtDesc).Enter += TxtType_Enter;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(274, 42);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(72, 23);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "توضیحات :";
		labelX2.TextAlignment = (StringAlignment)2;
		TxtType.Border.Class = "TextBoxBorder";
		TxtType.Border.CornerType = (eCornerType)1;
		((Control)TxtType).Location = new Point(130, 14);
		((Control)TxtType).Name = "TxtType";
		((Control)TxtType).Size = new Size(138, 21);
		((Control)TxtType).TabIndex = 1;
		((Control)TxtType).Leave += TxtType_Leave;
		((Control)TxtType).Enter += TxtType_Enter;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX1).Location = new Point(274, 13);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(84, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "نوع استخدام :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(378, 158);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(384, 186);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(384, 186);
		((Control)this).Name = "FRMTypeE";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMTypeE";
		((Form)this).Load += FRMTypeE_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMTypeE_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
