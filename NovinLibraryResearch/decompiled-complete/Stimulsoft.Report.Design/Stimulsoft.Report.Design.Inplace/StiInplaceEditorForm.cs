using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Inplace;

public class StiInplaceEditorForm : StiFormUX
{
	private bool skipEditorDeactivation;

	protected static object FormClosingObject = new object();

	private IContainer components;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	protected bool IsFormClosed { get; set; }

	protected int PaddingCorrection
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Padding padding = ((Control)this).Padding;
			int top = ((Padding)(ref padding)).Top;
			padding = ((Control)this).Padding;
			return top + ((Padding)(ref padding)).Bottom + StiScale.I4;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockInplaceEditorClosing { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockRefresh { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowProcessEscapeKey { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockConditionRefresh { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool IsAnyPresent { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool AllowResize { get; } = true;


	public StiInplaceEditorForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		InitializeComponent();
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
	}

	public virtual void Localize()
	{
	}

	protected virtual void AttachEvents()
	{
		StiInplaceEvents.Changed += OnGlobalElementChanged;
	}

	public virtual void DeactivateForm()
	{
		skipEditorDeactivation = true;
		((Form)this).Close();
	}

	public virtual void RefreshUiPanel()
	{
	}

	public virtual void CalculateWindowPosition()
	{
		if (((Control)this).Bottom > Screen.PrimaryScreen.WorkingArea.Bottom)
		{
			((Control)this).Top = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)this).Height;
		}
	}

	protected virtual void CalculateWindowHeight()
	{
		if (IsAnyPresent)
		{
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, PaddingCorrection + (from Control c in (IEnumerable)((Control)this).Controls
				where c.Visible
				select c.Height).Sum());
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		((Form)this).OnLoad(e);
		AttachEvents();
		RefreshUiPanel();
		if (AllowResize)
		{
			StiFormSettings.Load((Form)(object)this, skipLocation: true);
		}
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		((Form)this).OnFormClosed(e);
		if (AllowResize)
		{
			StiFormSettings.Save((Form)(object)this);
		}
		StiInplaceEvents.Changed -= OnGlobalElementChanged;
		if (!skipEditorDeactivation && !LockInplaceEditorClosing)
		{
			StiInplaceDesigner.ApplyStyle = null;
			StiInplaceDesigner.Close(closeAllDesigners: false, this);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 27 && AllowProcessEscapeKey)
		{
			((Form)this).Close();
			return true;
		}
		return ((Form)this).ProcessCmdKey(ref msg, keyData);
	}

	protected virtual void OnGlobalElementChanged(object sender, EventArgs e)
	{
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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(800, 450);
		((Control)this).DoubleBuffered = true;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiInplaceEditorForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "StiInplaceEditorForm";
		((Control)this).ResumeLayout(false);
	}
}
