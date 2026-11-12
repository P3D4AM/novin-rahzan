using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Design.DataTransformation.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.DataTransformation.Editors;

public class StiDataTransformReplaceForm : Form
{
	private List<StiDataActionRule> actionRules;

	private string columnKey;

	private string columnPath;

	private bool primaryKey;

	private List<string> items;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonOk;

	private Button buttonCancel;

	public StiToolBar toolbar;

	private StiToolButton buttonRemove;

	private StiToolButton buttonAdd;

	private StiDataTransformZoneContainer zoneValues;

	public StiDataTransformReplaceForm(List<StiDataActionRule> actionRules, string columnKey, string columnPath, bool primaryKey, List<string> items)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.actionRules = actionRules;
		this.columnKey = columnKey;
		this.columnPath = columnPath;
		this.primaryKey = primaryKey;
		this.items = items;
		Localize();
		ReadColumnActions();
		DoScaling();
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Dashboard", "ReplaceValues");
		((Control)buttonAdd).Text = Loc.Get("Buttons", "Add");
		((Control)buttonRemove).Text = Loc.Get("Buttons", "Remove");
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		((ButtonBase)buttonAdd).Image = StiImageResizer.GetImage(((ButtonBase)buttonAdd).Image);
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
	}

	private void ReadColumnActions()
	{
		actionRules.Where((StiDataActionRule a) => a.Type == StiDataActionType.Replace && (a.Key == columnKey || (a.Key == null && a.Path == columnPath))).ToList().ForEach(delegate(StiDataActionRule a)
		{
			AddValue(a.ValueFrom, a.ValueTo, a.MatchCase, a.MatchWholeWord);
		});
		if (((ArrangedElementCollection)((Control)zoneValues).Controls).Count == 0)
		{
			AddValue();
		}
	}

	private void WriteColumnActions()
	{
		actionRules.RemoveAll((StiDataActionRule a) => a.Type == StiDataActionType.Replace && (a.Key == columnKey || (a.Key == null && a.Path == columnPath)));
		foreach (StiDataTransformReplaceZoneControl item2 in (ArrangedElementCollection)((Control)zoneValues).Controls)
		{
			if (!string.IsNullOrEmpty(((Control)item2.comboBoxFrom).Text))
			{
				StiDataActionRule item = new StiDataActionRule(columnKey, columnPath, ((Control)item2.comboBoxFrom).Text, ((Control)item2.comboBoxTo).Text, item2.checkBoxMatchCase.Checked, item2.checkBoxMatchWholeWord.Checked);
				actionRules.Insert(0, item);
			}
		}
	}

	private bool CheckPrimaryKey()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (primaryKey)
		{
			MessageBox.Show((IWin32Window)(object)this, string.Format(Loc.Get("Errors", "PrimaryColumnAction"), columnPath), Loc.Get("Errors", "Error"));
			return true;
		}
		return false;
	}

	private void AddValue()
	{
		AddValue(string.Empty, string.Empty, matchCase: false, matchWholeWord: false);
	}

	private void AddValue(string valueFrom, string valueTo, bool matchCase, bool matchWholeWord)
	{
		StiDataTransformReplaceZoneControl control = new StiDataTransformReplaceZoneControl();
		items.ForEach(delegate(string i)
		{
			control.comboBoxFrom.Items.Add((object)i);
			control.comboBoxTo.Items.Add((object)i);
		});
		((Control)control.comboBoxFrom).Text = valueFrom;
		((Control)control.comboBoxTo).Text = valueTo;
		control.checkBoxMatchCase.Checked = matchCase;
		control.checkBoxMatchWholeWord.Checked = matchWholeWord;
		((Control)zoneValues).Controls.Add((Control)(object)control);
		((Control)control.comboBoxFrom).Focus();
	}

	protected override void OnShown(EventArgs e)
	{
		((Form)this).OnShown(e);
		if (((ArrangedElementCollection)((Control)zoneValues).Controls).Count > 0)
		{
			StiDataTransformReplaceZoneControl stiDataTransformReplaceZoneControl = ((Control)zoneValues).Controls[((ArrangedElementCollection)((Control)zoneValues).Controls).Count - 1] as StiDataTransformReplaceZoneControl;
			((Control)stiDataTransformReplaceZoneControl).Focus();
			((Control)stiDataTransformReplaceZoneControl.comboBoxFrom).Focus();
		}
	}

	private void ButtonAdd_Click(object sender, EventArgs e)
	{
		AddValue();
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		zoneValues.RemoveFocused();
	}

	private void ButtonOk_Click(object sender, EventArgs e)
	{
		if (!CheckPrimaryKey())
		{
			WriteColumnActions();
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataTransformReplaceForm));
		stiGroupLine1 = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		toolbar = new StiToolBar();
		buttonRemove = new StiToolButton();
		buttonAdd = new StiToolButton();
		zoneValues = new StiDataTransformZoneContainer();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)toolbar).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 360);
		((Control)stiGroupLine1).Margin = new Padding(2);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(694, 46);
		((Control)stiGroupLine1).TabIndex = 43;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(529, 12);
		((Control)buttonOk).Margin = new Padding(2);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += ButtonOk_Click;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(608, 12);
		((Control)buttonCancel).Margin = new Padding(2);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)toolbar).Controls.Add((Control)(object)buttonRemove);
		((Control)toolbar).Controls.Add((Control)(object)buttonAdd);
		toolbar.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolbar).Dock = (DockStyle)1;
		toolbar.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolbar).Location = new Point(0, 0);
		((Control)toolbar).Margin = new Padding(0);
		((Control)toolbar).Name = "toolbar";
		((Control)toolbar).Padding = new Padding(6, 3, 3, 4);
		((Control)toolbar).Size = new Size(694, 29);
		((Control)toolbar).TabIndex = 44;
		((Control)toolbar).Text = "stiToolBar1";
		((Control)buttonRemove).AutoSize = true;
		((Control)buttonRemove).Dock = (DockStyle)3;
		buttonRemove.Draw3DButton = false;
		buttonRemove.DropDownMenu = null;
		buttonRemove.HotDragDrop = false;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(55, 3);
		((Control)buttonRemove).Margin = new Padding(2);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(71, 22);
		((Control)buttonRemove).TabIndex = 1;
		((Control)buttonRemove).Text = "Remove";
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = false;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)buttonAdd).AutoSize = true;
		((Control)buttonAdd).Dock = (DockStyle)3;
		buttonAdd.Draw3DButton = false;
		buttonAdd.DropDownMenu = null;
		buttonAdd.HotDragDrop = false;
		((ButtonBase)buttonAdd).Image = (Image)componentResourceManager.GetObject("buttonAdd.Image");
		((Control)buttonAdd).Location = new Point(6, 3);
		((Control)buttonAdd).Margin = new Padding(2);
		((Control)buttonAdd).Name = "buttonAdd";
		((Control)buttonAdd).Size = new Size(49, 22);
		((Control)buttonAdd).TabIndex = 0;
		((Control)buttonAdd).Text = "Add";
		((ButtonBase)buttonAdd).UseVisualStyleBackColor = false;
		((Control)buttonAdd).Click += ButtonAdd_Click;
		((Control)zoneValues).Anchor = (AnchorStyles)15;
		((ScrollableControl)zoneValues).AutoScroll = true;
		((Control)zoneValues).BackColor = Color.White;
		((Control)zoneValues).Location = new Point(12, 41);
		((Control)zoneValues).Margin = new Padding(1);
		((Control)zoneValues).Name = "zoneValues";
		((Control)zoneValues).Padding = new Padding(2);
		((Control)zoneValues).Size = new Size(668, 303);
		((Control)zoneValues).TabIndex = 45;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(694, 406);
		((Control)this).Controls.Add((Control)(object)zoneValues);
		((Control)this).Controls.Add((Control)(object)toolbar);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataTransformReplaceForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Replace";
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)toolbar).ResumeLayout(false);
		((Control)toolbar).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
