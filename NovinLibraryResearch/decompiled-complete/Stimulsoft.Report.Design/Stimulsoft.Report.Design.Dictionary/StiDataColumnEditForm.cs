using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataColumnEditForm : StiFormUX
{
	private string oldValue;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	public StiTextBox tbName;

	public StiTextBox tbAlias;

	private StiLabelUX lbName;

	private StiLabelUX lbAlias;

	public StiComboBox cbType;

	private StiLabelUX lbType;

	private ErrorProvider errorProvider;

	private StiButtonPanelUX glButtons;

	private StiLabelUX labelNameInSource;

	public StiTextBox textBoxNameInSource;

	private StiButtonUX btSaveCopy;

	private StiLabelUX lbExpression;

	public StiTextBox tbExpression;

	private ImageList imageListTypes;

	public bool CopyModeActivated { get; private set; }

	public string Expression
	{
		get
		{
			return ((Control)tbExpression).Text;
		}
		set
		{
			((Control)tbExpression).Text = value;
			((Control)lbExpression).Visible = true;
			((Control)tbExpression).Visible = true;
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, ((Control)glButtons).Height + ((Control)tbExpression).Bottom + StiScale.I12);
		}
	}

	public StiDataColumnEditForm(bool isEditMode)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			DoScaling();
			((Control)btSaveCopy).Visible = isEditMode;
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, ((Control)glButtons).Height + ((Control)tbExpression).Top);
			((Control)lbExpression).Visible = false;
			((Control)tbExpression).Visible = false;
		}
	}

	private void DoScaling()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		double step = StiScale.Step;
		if (step >= 1.5)
		{
			string stepName = StiScale.StepName;
			imageListTypes = new ImageList();
			imageListTypes.TransparentColor = Color.Transparent;
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableString" + stepName + ".png");
			imageListTypes.ImageSize = ((Image)image).Size;
			imageListTypes.Images.Add("String", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableString" + stepName + ".png"));
			imageListTypes.Images.Add("Binary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableBinary" + stepName + ".png"));
			imageListTypes.Images.Add("Bool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableBool" + stepName + ".png"));
			imageListTypes.Images.Add("Char", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableChar" + stepName + ".png"));
			imageListTypes.Images.Add("DateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableDateTime" + stepName + ".png"));
			imageListTypes.Images.Add("Decimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableDecimal" + stepName + ".png"));
			imageListTypes.Images.Add("Float", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableFloat" + stepName + ".png"));
			imageListTypes.Images.Add("Image", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableImage" + stepName + ".png"));
			imageListTypes.Images.Add("Int", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableInt" + stepName + ".png"));
		}
		((ComboBox)cbType).ItemHeight = (int)Math.Ceiling(16.0 * StiScale.Y);
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void Localize()
	{
		((Control)labelNameInSource).Text = Loc.GetCleaned("Report", "LabelNameInSource");
		((Control)lbName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)lbAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)lbExpression).Text = Loc.GetCleaned("Report", "LabelExpression");
		((Control)lbType).Text = Loc.GetCleaned("Report", "LabelType");
		((Control)btSaveCopy).Text = Loc.Get("Buttons", "SaveCopy");
		((Control)btOk).Text = Loc.Get("Buttons", "Ok");
		((Control)btCancel).Text = Loc.Get("Buttons", "Cancel");
		StiButtonSizeHelper.CheckSizeOfButton((Button)(object)btSaveCopy);
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		bool flag = true;
		if (((Control)cbType).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)cbType, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Type")));
			flag = false;
			((Control)cbType).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)cbType, "");
		}
		if (((Control)tbAlias).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)tbAlias, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Alias")));
			flag = false;
			((Control)tbAlias).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbAlias, "");
		}
		if (((Control)tbName).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Name")));
			flag = false;
			((Control)tbName).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbName, "");
		}
		if (((Control)textBoxNameInSource).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)textBoxNameInSource, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "NameInSource")));
			flag = false;
			((Control)textBoxNameInSource).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)textBoxNameInSource, "");
		}
		if (flag)
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/data_data_dictionary_datasources_creating_and_editing_data_columns.htm");
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_SaveCopyClick(object sender, EventArgs e)
	{
		CopyModeActivated = true;
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void TextBoxName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)tbName).Text;
	}

	private void TextBoxName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)tbAlias).Text)
		{
			((Control)tbAlias).Text = ((Control)tbName).Text;
		}
		if (oldValue == ((Control)textBoxNameInSource).Text)
		{
			((Control)textBoxNameInSource).Text = ((Control)tbName).Text;
		}
	}

	private void ComboBoxType_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Invalid comparison between Unknown and I4
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		if (e.Index == -1)
		{
			return;
		}
		string text = null;
		StiTypeWrapper stiTypeWrapper = ((ComboBox)cbType).Items[e.Index] as StiTypeWrapper;
		Type type = stiTypeWrapper.Type;
		if (Nullable.GetUnderlyingType(type) != null)
		{
			type = Nullable.GetUnderlyingType(type);
		}
		text = ((type == typeof(float)) ? "Float" : ((type == typeof(double)) ? "Float" : ((type == typeof(decimal)) ? "Decimal" : ((type == typeof(DateTime)) ? "DateTime" : ((type == typeof(TimeSpan)) ? "DateTime" : ((type == typeof(bool)) ? "Bool" : ((type == typeof(sbyte)) ? "Int" : ((type == typeof(byte)) ? "Int" : ((type == typeof(short)) ? "Int" : ((type == typeof(ushort)) ? "Int" : ((type == typeof(int)) ? "Int" : ((type == typeof(uint)) ? "Int" : ((type == typeof(long)) ? "Int" : ((type == typeof(ulong)) ? "Int" : ((type == typeof(Image)) ? "Image" : ((!(type == typeof(char))) ? "String" : "Char"))))))))))))))));
		Graphics graphics = e.Graphics;
		Color color = StiUX.ItemBackground;
		Color color2 = StiUX.ItemForeground;
		if ((e.State & 0x10) > 0)
		{
			color = StiUX.ItemBackgroundSelected;
			color2 = StiUX.ItemForegroundSelected;
		}
		StiDrawing.FillRectangle(graphics, color, e.Bounds);
		if ((e.State & 0x10) == 0 && e.Bounds.Top > 10 && (type == typeof(float) || type == typeof(DateTime) || type == typeof(bool) || type == typeof(sbyte) || type == typeof(Image)))
		{
			Pen val = new Pen(StiUX.ItemSeparator);
			try
			{
				graphics.DrawLine(val, e.Bounds.X, e.Bounds.Y, e.Bounds.Right, e.Bounds.Y);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		double step = StiScale.Step;
		Image val2 = imageListTypes.Images[text];
		if (val2 != null)
		{
			Rectangle rectangle = new Rectangle(e.Bounds.Left + (int)Math.Ceiling(2.0 * step), e.Bounds.Top + (e.Bounds.Height - (int)(16.0 * step)) / 2, (int)Math.Ceiling(16.0 * step), (int)Math.Ceiling(16.0 * step));
			graphics.DrawImage(val2, rectangle);
		}
		Rectangle rectangle2 = new Rectangle(e.Bounds.Left + (int)Math.Ceiling(22.0 * step), e.Bounds.Top, e.Bounds.Width - (int)Math.Ceiling(22.0 * step), e.Bounds.Height - 2);
		StringFormat val3 = new StringFormat();
		try
		{
			SolidBrush val4 = new SolidBrush(color2);
			try
			{
				val3.LineAlignment = (StringAlignment)1;
				val3.Alignment = (StringAlignment)0;
				graphics.DrawString(stiTypeWrapper.ToString(), cbType.DrawingFont, (Brush)(object)val4, (RectangleF)rectangle2, val3);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataColumnEditForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		tbName = new StiTextBox();
		lbName = new StiLabelUX();
		lbAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		lbType = new StiLabelUX();
		cbType = new StiComboBox();
		errorProvider = new ErrorProvider(components);
		glButtons = new StiButtonPanelUX();
		btSaveCopy = new StiButtonUX();
		labelNameInSource = new StiLabelUX();
		textBoxNameInSource = new StiTextBox();
		lbExpression = new StiLabelUX();
		tbExpression = new StiTextBox();
		imageListTypes = new ImageList(components);
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(210, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(291, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).BackColor = SystemColors.Window;
		((Control)tbName).Location = new Point(129, 45);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(235, 21);
		((Control)tbName).TabIndex = 1;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)lbName).ForeColor = Color.DimGray;
		((Control)lbName).Location = new Point(12, 45);
		((Control)lbName).Name = "lbName";
		((Control)lbName).Size = new Size(115, 20);
		((Control)lbName).TabIndex = 6;
		((Control)lbName).Text = "Name";
		((Label)lbName).TextAlign = (ContentAlignment)16;
		((Control)lbAlias).ForeColor = Color.DimGray;
		((Control)lbAlias).Location = new Point(12, 78);
		((Control)lbAlias).Name = "lbAlias";
		((Control)lbAlias).Size = new Size(115, 20);
		((Control)lbAlias).TabIndex = 7;
		((Control)lbAlias).Text = "Alias";
		((Label)lbAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).BackColor = SystemColors.Window;
		((Control)tbAlias).Location = new Point(129, 78);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(235, 21);
		((Control)tbAlias).TabIndex = 2;
		((Control)lbType).ForeColor = Color.DimGray;
		((Control)lbType).Location = new Point(12, 111);
		((Control)lbType).Name = "lbType";
		((Control)lbType).Size = new Size(115, 20);
		((Control)lbType).TabIndex = 8;
		((Control)lbType).Text = "Type";
		((Label)lbType).TextAlign = (ContentAlignment)16;
		((Control)cbType).Anchor = (AnchorStyles)13;
		((Control)cbType).BackColor = SystemColors.Window;
		cbType.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbType).IntegralHeight = false;
		((Control)cbType).Location = new Point(129, 111);
		((ComboBox)cbType).MaxDropDownItems = 30;
		((Control)cbType).Name = "cbType";
		((Control)cbType).Size = new Size(159, 21);
		((Control)cbType).TabIndex = 3;
		((ComboBox)cbType).DrawItem += new DrawItemEventHandler(ComboBoxType_DrawItem);
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btSaveCopy);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 179);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(378, 46);
		((Control)glButtons).TabIndex = 5;
		((Control)btSaveCopy).Anchor = (AnchorStyles)6;
		((Control)btSaveCopy).Location = new Point(12, 12);
		((Control)btSaveCopy).Name = "btSaveCopy";
		((Control)btSaveCopy).Size = new Size(93, 24);
		((Control)btSaveCopy).TabIndex = 0;
		((Control)btSaveCopy).Text = "Save a Copy";
		((ButtonBase)btSaveCopy).UseVisualStyleBackColor = true;
		((Control)btSaveCopy).Visible = false;
		((Control)btSaveCopy).Click += This_SaveCopyClick;
		((Control)labelNameInSource).ForeColor = Color.DimGray;
		((Control)labelNameInSource).Location = new Point(12, 12);
		((Control)labelNameInSource).Name = "labelNameInSource";
		((Control)labelNameInSource).Size = new Size(115, 20);
		((Control)labelNameInSource).TabIndex = 5;
		((Control)labelNameInSource).Text = "Name in Source";
		((Label)labelNameInSource).TextAlign = (ContentAlignment)16;
		((Control)textBoxNameInSource).Anchor = (AnchorStyles)13;
		((Control)textBoxNameInSource).BackColor = SystemColors.Window;
		((Control)textBoxNameInSource).Location = new Point(129, 12);
		((Control)textBoxNameInSource).Name = "textBoxNameInSource";
		((Control)textBoxNameInSource).Size = new Size(235, 21);
		((Control)textBoxNameInSource).TabIndex = 0;
		((Control)lbExpression).ForeColor = Color.DimGray;
		((Control)lbExpression).Location = new Point(12, 144);
		((Control)lbExpression).Name = "lbExpression";
		((Control)lbExpression).Size = new Size(115, 21);
		((Control)lbExpression).TabIndex = 10;
		((Control)lbExpression).Text = "Expression";
		((Label)lbExpression).TextAlign = (ContentAlignment)16;
		((Control)lbExpression).Visible = false;
		((Control)tbExpression).Anchor = (AnchorStyles)13;
		((Control)tbExpression).BackColor = SystemColors.Window;
		((Control)tbExpression).Location = new Point(129, 144);
		((Control)tbExpression).Name = "tbExpression";
		((Control)tbExpression).Size = new Size(235, 21);
		((Control)tbExpression).TabIndex = 4;
		((Control)tbExpression).Visible = false;
		imageListTypes.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListTypes.ImageStream");
		imageListTypes.TransparentColor = Color.Magenta;
		imageListTypes.Images.SetKeyName(0, "String");
		imageListTypes.Images.SetKeyName(1, "Binary");
		imageListTypes.Images.SetKeyName(2, "Bool");
		imageListTypes.Images.SetKeyName(3, "Char");
		imageListTypes.Images.SetKeyName(4, "DateTime");
		imageListTypes.Images.SetKeyName(5, "Decimal");
		imageListTypes.Images.SetKeyName(6, "Float");
		imageListTypes.Images.SetKeyName(7, "Image");
		imageListTypes.Images.SetKeyName(8, "Int");
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(378, 225);
		((Control)this).Controls.Add((Control)(object)lbExpression);
		((Control)this).Controls.Add((Control)(object)tbExpression);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)cbType);
		((Control)this).Controls.Add((Control)(object)lbType);
		((Control)this).Controls.Add((Control)(object)lbAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)lbName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)labelNameInSource);
		((Control)this).Controls.Add((Control)(object)textBoxNameInSource);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataColumnEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit column";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
