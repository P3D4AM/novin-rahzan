using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Win;

public sealed class StiProgressInformation : IStiProgressInformation
{
	private bool isMarquee;

	internal IStiProgressForm ProgressForm { get; set; }

	private Form Form
	{
		get
		{
			IStiProgressForm progressForm = ProgressForm;
			return (Form)((progressForm is Form) ? progressForm : null);
		}
	}

	internal Form OwnerForm { get; set; }

	public bool AllowUseDoEvents { get; set; } = true;


	public bool IsBreaked
	{
		get
		{
			if (ProgressForm != null)
			{
				return ProgressForm.IsBreaked;
			}
			return false;
		}
		set
		{
			if (ProgressForm != null)
			{
				ProgressForm.IsBreaked = value;
			}
		}
	}

	public bool IsMarquee
	{
		get
		{
			return isMarquee;
		}
		set
		{
			if (isMarquee == value)
			{
				return;
			}
			isMarquee = value;
			if (ProgressForm == null)
			{
				return;
			}
			if (IsMarquee)
			{
				if (((Control)Form).InvokeRequired)
				{
					((Control)Form).Invoke((Delegate)(EventHandler)delegate
					{
						ProgressForm.ProgressBarStyle = (ProgressBarStyle)2;
					});
				}
				else
				{
					ProgressForm.ProgressBarStyle = (ProgressBarStyle)2;
				}
			}
			else if (((Control)Form).InvokeRequired)
			{
				((Control)Form).Invoke((Delegate)(EventHandler)delegate
				{
					ProgressForm.ProgressBarStyle = (ProgressBarStyle)1;
				});
			}
			else
			{
				ProgressForm.ProgressBarStyle = (ProgressBarStyle)1;
			}
		}
	}

	public bool IsDialog { get; set; }

	public bool IsVisible
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ProgressForm != null)
			{
				return ((Control)(Form)ProgressForm).Visible;
			}
			return false;
		}
	}

	public void Start(string title)
	{
		Start(title, -1);
	}

	public void Start(string title, int progressMaximum)
	{
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		if (ProgressForm != null)
		{
			return;
		}
		if (StiOptions.Windows.UseSkinableReportRenderingWindow)
		{
			ProgressForm = new StiProgressRibbonForm();
		}
		else
		{
			ProgressForm = new StiProgressForm();
		}
		ProgressForm.AllowUseDoEvents = AllowUseDoEvents;
		if (OwnerForm == null && ((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0)
		{
			Form val = Application.OpenForms[((ReadOnlyCollectionBase)(object)Application.OpenForms).Count - 1];
			while ((((Control)val).IsDisposed || (!((double)val.Size.Width > 250.0 * StiScale.X) && !((double)val.Size.Height > 200.0 * StiScale.Y))) && val.Owner != null)
			{
				val = val.Owner;
			}
			if (val != null && !((Control)val).IsDisposed && ((double)val.Size.Width > 250.0 * StiScale.X || (double)val.Size.Height > 200.0 * StiScale.Y))
			{
				OwnerForm = val;
			}
		}
		if (OwnerForm != null)
		{
			OwnerForm.AddOwnedForm((Form)ProgressForm);
		}
		((Control)Form).Text = title;
		Form.TopMost = StiOptions.Engine.ForceTopMostPropertyOfProgressForm;
		ProgressForm.ProgressText = title;
		if (progressMaximum != -1)
		{
			ProgressForm.Maximum = progressMaximum;
		}
		ProgressForm.Minimum = 0;
		Show();
	}

	public void SetProgressBar(int value, int maximum)
	{
		if (ProgressForm != null)
		{
			ProgressForm.Maximum = maximum;
			ProgressForm.Value = value;
		}
	}

	public void Show()
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		CheckOwnerForm();
		if (ProgressForm == null)
		{
			return;
		}
		if (IsMarquee)
		{
			if (((Control)Form).InvokeRequired)
			{
				((Control)Form).Invoke((Delegate)(EventHandler)delegate
				{
					ProgressForm.ProgressBarStyle = (ProgressBarStyle)2;
				});
			}
			else
			{
				ProgressForm.ProgressBarStyle = (ProgressBarStyle)2;
			}
		}
		else if (((Control)Form).InvokeRequired)
		{
			((Control)Form).Invoke((Delegate)(EventHandler)delegate
			{
				ProgressForm.ProgressBarStyle = (ProgressBarStyle)1;
			});
		}
		else
		{
			ProgressForm.ProgressBarStyle = (ProgressBarStyle)1;
		}
		if (IsDialog)
		{
			if (((Control)Form).InvokeRequired)
			{
				((Control)Form).Invoke((Delegate)(EventHandler)delegate
				{
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					Form.Activate();
					ProgressForm.CancelButtonFocus();
					Form.ShowDialog();
				});
			}
			else
			{
				Form.Activate();
				ProgressForm.CancelButtonFocus();
				Form.ShowDialog();
			}
		}
		else if (((Control)Form).InvokeRequired)
		{
			((Control)Form).Invoke((Delegate)(EventHandler)delegate
			{
				((Control)Form).Show();
				Form.Activate();
				ProgressForm.CancelButtonFocus();
			});
		}
		else
		{
			((Control)Form).Show();
			Form.Activate();
			ProgressForm.CancelButtonFocus();
		}
	}

	public void Hide()
	{
		if (ProgressForm == null)
		{
			return;
		}
		if (((Control)Form).InvokeRequired)
		{
			((Control)Form).Invoke((Delegate)(EventHandler)delegate
			{
				((Control)Form).Visible = false;
			});
		}
		else
		{
			((Control)Form).Visible = false;
		}
	}

	public void HideProgressBar()
	{
		if (ProgressForm != null)
		{
			ProgressForm.ProgressBarVisible = false;
		}
	}

	public void ShowProgressBar()
	{
		if (ProgressForm != null)
		{
			ProgressForm.ProgressBarVisible = true;
		}
	}

	public void Update(string value)
	{
		Update(value, -1);
	}

	public void Update(string value, int progressValue)
	{
		CheckOwnerForm();
		if (ProgressForm != null)
		{
			ProgressForm.ProgressText = value;
			if (progressValue != -1)
			{
				ProgressForm.Value = progressValue;
			}
		}
	}

	public void Close()
	{
		CheckOwnerForm();
		if (ProgressForm == null)
		{
			return;
		}
		if (((Control)Form).InvokeRequired)
		{
			((Control)Form).Invoke((Delegate)(EventHandler)delegate
			{
				Form.DialogResult = (DialogResult)1;
				Form.Close();
				if (OwnerForm != null)
				{
					OwnerForm.RemoveOwnedForm(Form);
				}
				((Component)(object)Form).Dispose();
				ProgressForm = null;
			});
		}
		else
		{
			Form.DialogResult = (DialogResult)1;
			Form.Close();
			if (OwnerForm != null)
			{
				OwnerForm.RemoveOwnedForm(Form);
			}
			((Component)(object)Form).Dispose();
			ProgressForm = null;
		}
	}

	public void SetAllowClose(bool allows)
	{
		if (ProgressForm != null)
		{
			ProgressForm.CancelButtonEnabled = allows;
		}
	}

	private void CheckOwnerForm()
	{
		if (OwnerForm != null && ((Control)OwnerForm).IsDisposed)
		{
			ProgressForm = null;
			OwnerForm = null;
		}
	}

	public StiProgressInformation()
		: this(null)
	{
	}

	public StiProgressInformation(object ownerForm)
	{
		if (ownerForm is Form)
		{
			OwnerForm = (Form)((ownerForm is Form) ? ownerForm : null);
		}
	}
}
