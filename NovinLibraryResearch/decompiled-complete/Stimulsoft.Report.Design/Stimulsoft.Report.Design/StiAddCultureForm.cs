using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design;

public class StiAddCultureForm : Form
{
	private class CultureWrapper
	{
		public CultureInfo Culture { get; }

		public override string ToString()
		{
			return Culture.DisplayName + "  (" + Culture.Name + ")";
		}

		public CultureWrapper(CultureInfo culture)
		{
			Culture = culture;
		}
	}

	private IContainer components;

	private StiGroupLine groupLineButtons;

	private Button buttonOk;

	private Button buttonCancel;

	private Label labelName;

	public ComboBox comboBoxCultures;

	private ErrorProvider errorProvider;

	internal string SelectedCultureName => ((Control)comboBoxCultures).Text;

	private string SelectedCultureWrapperName
	{
		get
		{
			if (!(comboBoxCultures.SelectedItem is CultureWrapper cultureWrapper))
			{
				return null;
			}
			return cultureWrapper.Culture.Name;
		}
	}

	public StiAddCultureForm(string currentCulture = null)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			comboBoxCultures.Items.Clear();
			BuildCultures();
			if (currentCulture != null)
			{
				((Control)comboBoxCultures).Text = currentCulture;
			}
			((Form)this).ShowIcon = false;
			Localize();
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormGlobalizationEditor", "title");
		((Control)labelName).Text = Loc.GetCleaned("PropertyMain", "Name");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
		((Control)labelName).Height = ((Control)comboBoxCultures).Height;
	}

	private void BuildCultures()
	{
		CultureWrapper[] array = (from c in GetCultures()
			select new CultureWrapper(c)).ToArray();
		ObjectCollection items = comboBoxCultures.Items;
		object[] array2 = array;
		items.AddRange(array2);
	}

	private static CultureInfo[] GetCultures()
	{
		CultureInfo[] source = StiOptions.Designer.InvokeGetCulturesListForGlobalizationEditor() ?? CultureInfo.GetCultures(CultureTypes.SpecificCultures);
		return source.Where((CultureInfo c) => !string.IsNullOrWhiteSpace(c.Name)).ToArray();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (((Control)comboBoxCultures).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)comboBoxCultures, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Name")));
			((Control)comboBoxCultures).Focus();
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void ComboBoxCultures_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		if (((ListControl)comboBoxCultures).SelectedIndex != -1)
		{
			string name = SelectedCultureWrapperName;
			if (!((Control)this).IsHandleCreated)
			{
				((Control)this).CreateHandle();
			}
			((Control)this).BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				((Control)comboBoxCultures).Text = name;
			});
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Expected O, but got Unknown
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiAddCultureForm));
		groupLineButtons = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		labelName = new Label();
		comboBoxCultures = new ComboBox();
		errorProvider = new ErrorProvider(components);
		((Control)groupLineButtons).SuspendLayout();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)this).SuspendLayout();
		((Control)groupLineButtons).BackColor = Color.White;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 47);
		((Control)groupLineButtons).Margin = new Padding(2, 2, 2, 2);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(330, 46);
		((Control)groupLineButtons).TabIndex = 2;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(164, 12);
		((Control)buttonOk).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(243, 12);
		((Control)buttonCancel).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Margin = new Padding(2, 0, 2, 0);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(105, 21);
		((Control)labelName).TabIndex = 1;
		((Control)labelName).Text = "Name";
		labelName.TextAlign = (ContentAlignment)16;
		((Control)comboBoxCultures).BackColor = SystemColors.Window;
		comboBoxCultures.DropDownWidth = 250;
		((Control)comboBoxCultures).ForeColor = SystemColors.ControlText;
		((Control)comboBoxCultures).Location = new Point(120, 12);
		((Control)comboBoxCultures).Margin = new Padding(2, 2, 2, 2);
		comboBoxCultures.MaxDropDownItems = 20;
		((Control)comboBoxCultures).Name = "comboBoxCultures";
		((Control)comboBoxCultures).Size = new Size(169, 21);
		comboBoxCultures.Sorted = true;
		((Control)comboBoxCultures).TabIndex = 0;
		comboBoxCultures.SelectedIndexChanged += ComboBoxCultures_SelectedIndexChanged;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(330, 93);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)comboBoxCultures);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(2, 2, 2, 2);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiAddCultureForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Add Culture";
		((Control)groupLineButtons).ResumeLayout(false);
		((ISupportInitialize)errorProvider).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
