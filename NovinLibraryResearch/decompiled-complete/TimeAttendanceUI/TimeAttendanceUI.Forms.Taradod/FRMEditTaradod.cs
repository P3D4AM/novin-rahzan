using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FRMEditTaradod : CacheBaseForm, IHasContext
{
	private IContainer components;

	private PanelEx panelEx1;

	private LabelX labelX5;

	private MaskedTextBox TbxNewTime;

	private LabelX labelX1;

	private ButtonX BtnCancel;

	internal ButtonX BtnDelete;

	internal ButtonX BtnChange;

	private LabelX labelX2;

	private Label LblOldTime;

	private Label LblDateNow;

	public LogOnPersonel Taradod { get; private set; }

	public FRMEditTaradod(string contextGuid, LogOnPersonel taradod)
		: base(contextGuid)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)AccessLevelClass.Instance.LevelHozor < 2)
			{
				FMessageBox.Show("جهت اضافه،حذف،ویرایش تردد بایستی نرم افزار سطح 2 به بالا داشته باشید", "اضافه،حذف،ویرایش تردد", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
				return;
			}
			if (taradod == null)
			{
				FMessageBox.Show("ترددی جهت ویرایش شناسایی نشد", "ثبت تردد", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
				((Form)this).DialogResult = (DialogResult)3;
				((Form)this).Close();
			}
			if (taradod.PersonNo <= 0)
			{
				FMessageBox.Show("به جهت اینکه کد سخت افزاری مشخص نشده است \r\n امکان ثبت تردد وجود ندارد", "ثبت تردد", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
				((Form)this).DialogResult = (DialogResult)3;
				((Form)this).Close();
			}
			Taradod = taradod;
			InitializeComponent();
			((Control)LblDateNow).Text = Calendar.MiladiToCalture(taradod.DateTimeLogM);
			((Control)LblOldTime).Text = taradod.DateTimeLogM.TimeOfDay.ToString().Substring(0, 5);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnChange_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodInsert)
			{
				FMessageBox.Show("متاسفانه شما مجوز ثبت ندارید", "ثبت تردد", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
				return;
			}
			if (!TbxNewTime.MaskCompleted)
			{
				Utility.MessageBoxShowError("ساعت تردد را کامل وارد نمائید", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			short.TryParse(((Control)TbxNewTime).Text.Substring(0, 2), out var result);
			short.TryParse(((Control)TbxNewTime).Text.Substring(3, 2), out var result2);
			if (result > 24 || result2 > 59 || (result == 24 && result2 > 0))
			{
				Utility.MessageBoxShowError("ساعت تردد وارد شده نامعتبر است", ((CacheBaseForm)this).ContextGuid);
			}
			else
			{
				if (Taradod == null)
				{
					return;
				}
				LogOnPersonel logOnPersonel = Taradod?.Clone();
				if (logOnPersonel != null)
				{
					if (Taradod.DateTimeLogM.TimeOfDay.Ticks > 0)
					{
						logOnPersonel.IsVirtual = (SavedTaradodEnumType)2;
					}
					else
					{
						logOnPersonel.IsVirtual = (SavedTaradodEnumType)5;
					}
					logOnPersonel.DateTimeLogM = Taradod.DateTimeLogM.Date.AddMinutes(result * 60 + result2);
					await logOnPersonel.SaveAsync("EditTaradod", default(CancellationToken), "BtnChange_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMEditTaradod.cs");
					if (Taradod.Id > 0)
					{
						await Taradod.DeleteAsync();
					}
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelSelf.Taradod.TaradodDelete)
			{
				FMessageBox.Show("متاسفانه شما مجوز حذف ندارید.", "حذف تردد", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
			}
			else if ((int)FMessageBox.Show("آیا مطمئن هستید؟", "حذف تردد", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Question, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				await Taradod.DeleteAsync();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FRMEditTaradod_KeyDown(object sender, KeyEventArgs e)
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
					if ((int)keyCode == 46)
					{
						e.Handled = true;
						((PopupItemControl)BtnDelete).PerformClick();
					}
				}
				else
				{
					e.Handled = true;
					((PopupItemControl)BtnCancel).PerformClick();
				}
			}
			else
			{
				e.Handled = true;
				((PopupItemControl)BtnChange).PerformClick();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void TbxNewTime_Enter(object sender, EventArgs e)
	{
		try
		{
			((TextBoxBase)TbxNewTime).Clear();
			((TextBoxBase)TbxNewTime).SelectAll();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09da: Expected O, but got Unknown
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a25: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		LblOldTime = new Label();
		LblDateNow = new Label();
		BtnChange = new ButtonX();
		BtnDelete = new ButtonX();
		BtnCancel = new ButtonX();
		labelX1 = new LabelX();
		TbxNewTime = new MaskedTextBox();
		labelX2 = new LabelX();
		labelX5 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)LblOldTime);
		((Control)panelEx1).Controls.Add((Control)(object)LblDateNow);
		((Control)panelEx1).Controls.Add((Control)(object)BtnChange);
		((Control)panelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)TbxNewTime);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(383, 142);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightBlue;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)LblOldTime).Anchor = (AnchorStyles)6;
		((Control)LblOldTime).AutoSize = true;
		((Control)LblOldTime).Location = new Point(196, 64);
		((Control)LblOldTime).Name = "LblOldTime";
		((Control)LblOldTime).Size = new Size(38, 13);
		((Control)LblOldTime).TabIndex = 6;
		((Control)LblOldTime).Text = "00:00";
		((Control)LblDateNow).Anchor = (AnchorStyles)6;
		((Control)LblDateNow).AutoSize = true;
		((Control)LblDateNow).Location = new Point(196, 109);
		((Control)LblDateNow).Name = "LblDateNow";
		((Control)LblDateNow).Size = new Size(75, 13);
		((Control)LblDateNow).TabIndex = 5;
		((Control)LblDateNow).Text = "0000/00/00";
		((Control)BtnChange).AccessibleRole = (AccessibleRole)43;
		((Control)BtnChange).Anchor = (AnchorStyles)6;
		BtnChange.ColorTable = (eButtonColor)6;
		((Control)BtnChange).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnChange.Image = (Image)(object)Resources.accept1;
		BtnChange.ImageFixedSize = new Size(30, 30);
		BtnChange.ImagePosition = (eImagePosition)1;
		BtnChange.ImageTextSpacing = 5;
		((Control)BtnChange).Location = new Point(10, 8);
		((Control)BtnChange).Margin = new Padding(5);
		((Control)BtnChange).Name = "BtnChange";
		((Control)BtnChange).Padding = new Padding(5);
		BtnChange.PopupSide = (ePopupSide)2;
		((Control)BtnChange).RightToLeft = (RightToLeft)1;
		BtnChange.Shape = (ShapeDescriptor)new RoundRectangleShapeDescriptor(10, 0, 0, 10);
		((Control)BtnChange).Size = new Size(173, 34);
		((PopupItemControl)BtnChange).Style = (eDotNetBarStyle)9;
		((Control)BtnChange).TabIndex = 4;
		((Control)BtnChange).Text = "    اصلاح تردد   (Enter)     ";
		((Control)BtnChange).Click += BtnChange_Click;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDelete).Anchor = (AnchorStyles)6;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImageFixedSize = new Size(27, 27);
		BtnDelete.ImagePosition = (eImagePosition)1;
		BtnDelete.ImageTextSpacing = 5;
		((Control)BtnDelete).ImeMode = (ImeMode)0;
		((Control)BtnDelete).Location = new Point(10, 53);
		((Control)BtnDelete).Margin = new Padding(0);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).RightToLeft = (RightToLeft)1;
		BtnDelete.Shape = (ShapeDescriptor)new RoundRectangleShapeDescriptor(10, 0, 0, 10);
		((Control)BtnDelete).Size = new Size(173, 34);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 2;
		((Control)BtnDelete).Text = "حذف تردد فعلی (Del)";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.gnome_application_exit;
		BtnCancel.ImagePosition = (eImagePosition)1;
		BtnCancel.ImageTextSpacing = 5;
		((Control)BtnCancel).Location = new Point(10, 98);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		BtnCancel.Shape = (ShapeDescriptor)new RoundRectangleShapeDescriptor(10, 0, 0, 10);
		((Control)BtnCancel).Size = new Size(173, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 3;
		((Control)BtnCancel).Text = "                خروج   (Esc)     ";
		((PopupItemControl)BtnCancel).ThemeAware = true;
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)labelX1).Anchor = (AnchorStyles)6;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(271, 104);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(58, 23);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "تاریخ روز:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)TbxNewTime).Anchor = (AnchorStyles)6;
		((Control)TbxNewTime).Font = new Font("Tahoma", 10f, (FontStyle)1);
		((TextBoxBase)TbxNewTime).HideSelection = false;
		TbxNewTime.InsertKeyMode = (InsertKeyMode)2;
		((Control)TbxNewTime).Location = new Point(196, 13);
		TbxNewTime.Mask = "00:00";
		((Control)TbxNewTime).Name = "TbxNewTime";
		((Control)TbxNewTime).RightToLeft = (RightToLeft)0;
		((Control)TbxNewTime).Size = new Size(62, 24);
		((Control)TbxNewTime).TabIndex = 1;
		((Control)TbxNewTime).Text = "0000";
		TbxNewTime.TextAlign = (HorizontalAlignment)2;
		TbxNewTime.ValidatingType = typeof(DateTime);
		((Control)TbxNewTime).Enter += TbxNewTime_Enter;
		((Control)labelX2).Anchor = (AnchorStyles)6;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(271, 59);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(104, 23);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = "ساعت تردد فعلی:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)6;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(271, 14);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(100, 23);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "اصلاح/ثبت تردد:";
		labelX5.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(383, 142);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMEditTaradod";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "ویرایش تردد";
		((Control)this).KeyDown += new KeyEventHandler(FRMEditTaradod_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
