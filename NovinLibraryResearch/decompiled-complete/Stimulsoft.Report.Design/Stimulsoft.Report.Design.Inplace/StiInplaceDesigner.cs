using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Design.Inplace;

public abstract class StiInplaceDesigner : StiComponentDesigner
{
	private static object lockObject = new object();

	public static bool IsEditorActivated
	{
		get
		{
			return StiInplaceEditor.IsEditorActivated;
		}
		set
		{
			StiInplaceEditor.IsEditorActivated = value;
		}
	}

	public static Action ApplyStyle { get; set; }

	public static StiComponent EditableComponent { get; set; }

	public static StiComponent OriginalComponent { get; set; }

	public static StiInplaceEditorForm EditorForm { get; set; }

	public static List<StiInplaceEditorForm> AllForms { get; set; } = new List<StiInplaceEditorForm>();


	public static void AddEditor(StiInplaceEditorForm form, StiComponent comp = null)
	{
		EditorForm = form;
		AllForms.Add(form);
		if (comp != null)
		{
			Activate(comp);
		}
		IsEditorActivated = true;
	}

	public static void Activate(StiComponent component)
	{
		lock (lockObject)
		{
			EditableComponent = component;
			IsEditorActivated = true;
		}
	}

	public static void Localize()
	{
		AllForms.Where((StiInplaceEditorForm f) => ((Control)f).Visible).ToList().ForEach(delegate(StiInplaceEditorForm f)
		{
			f.Localize();
		});
	}

	public static void Close(bool closeAllDesigners = true, StiInplaceEditorForm form = null)
	{
		lock (lockObject)
		{
			try
			{
				IsEditorActivated = false;
				EditableComponent = null;
				OriginalComponent = null;
				lock (AllForms)
				{
					if (form != null)
					{
						AllForms.Remove(form);
						Form owner = ((Form)form).Owner;
						if (owner != null)
						{
							owner.Activate();
						}
					}
					if (!closeAllDesigners)
					{
						return;
					}
					while (AllForms.Count > 0)
					{
						StiInplaceEditorForm stiInplaceEditorForm = AllForms.FirstOrDefault();
						if (stiInplaceEditorForm != null)
						{
							Form owner2 = ((Form)stiInplaceEditorForm).Owner;
							if (owner2 != null)
							{
								owner2.Activate();
							}
						}
						stiInplaceEditorForm?.DeactivateForm();
						((Component)(object)stiInplaceEditorForm)?.Dispose();
						AllForms.Remove(stiInplaceEditorForm);
					}
				}
			}
			catch
			{
			}
		}
	}

	public static void InvokeStyleApplied()
	{
		ApplyStyle?.Invoke();
	}

	protected abstract StiInplaceEditorForm CreateEditorForm(object component);

	public override DialogResult Design(StiComponent component)
	{
		DesignComponent(component);
		return (DialogResult)1;
	}

	public virtual void DesignComponent(object component)
	{
		try
		{
			StiInplaceEditorForm stiInplaceEditorForm = CreateEditorForm(component);
			if (IsEditorActivated && ((object)stiInplaceEditorForm).GetType() == ((object)EditorForm).GetType() && component == EditableComponent)
			{
				Close();
				return;
			}
			if (IsEditorActivated)
			{
				Close();
				component = FindComponent(component as StiComponent);
				if (component == null)
				{
					return;
				}
			}
			StiComponent stiComponent = component as StiComponent;
			EditorForm = stiInplaceEditorForm;
			if (EditorForm != null)
			{
				Activate(stiComponent);
				if (IsAllowCenterWindow(component))
				{
					((Form)EditorForm).StartPosition = (FormStartPosition)1;
				}
				else
				{
					Size formSize = StiFormSettings.LoadSize((Form)(object)EditorForm);
					if (stiComponent != null)
					{
						bool isLeft = StiInplaceEditor.IsLeftLocation(stiComponent);
						Point screenLeftPosition = GetScreenLeftPosition(stiComponent, formSize);
						Point screenRightPosition = GetScreenRightPosition(stiComponent);
						screenRightPosition.X += StiScale.I2;
						screenLeftPosition.Y -= StiScale.I2;
						screenRightPosition.Y -= StiScale.I2;
						SetFormPosition(screenLeftPosition, screenRightPosition, formSize, isLeft);
					}
				}
				((Form)EditorForm).Show((IWin32Window)(object)((Control)(base.Designer.DesignerControl as StiDesignerControl)).FindForm());
				AllForms.Add(EditorForm);
				FixFormLocation();
			}
			else
			{
				Close();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private StiComponent FindComponent(StiComponent component)
	{
		if (component == null)
		{
			return null;
		}
		return ((base.Designer as StiDesignerControl)?.PageView?.GetCurrentPageView()?.Page)?.GetComponents().ToList().FirstOrDefault((StiComponent c) => c.Name == component.Name && c.ClientRectangle == component.ClientRectangle);
	}

	private static void FixFormLocation()
	{
		if (((Control)EditorForm).Left < StiScale.I4)
		{
			((Control)EditorForm).Left = StiScale.I4;
		}
		if (((Control)EditorForm).Top < StiScale.I4)
		{
			((Control)EditorForm).Top = StiScale.I4;
		}
		if (((Control)EditorForm).Height < Screen.PrimaryScreen.WorkingArea.Height && ((Control)EditorForm).Bottom > Screen.PrimaryScreen.WorkingArea.Bottom)
		{
			((Control)EditorForm).Top = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)EditorForm).Height;
		}
	}

	private static Screen GetScreen(int x, int y)
	{
		Screen[] allScreens = Screen.AllScreens;
		foreach (Screen val in allScreens)
		{
			if (val.Bounds.Contains(x, y))
			{
				return val;
			}
		}
		return Screen.PrimaryScreen;
	}

	private static bool IsGoodHorizonalFormLocation(Size formSize)
	{
		Screen screen = GetScreen(((Control)EditorForm).Left, ((Control)EditorForm).Top);
		if (((Control)EditorForm).Left > 0)
		{
			return ((Control)EditorForm).Left + formSize.Width < screen.Bounds.Right;
		}
		return false;
	}

	public static Point GetScreenPosition(StiComponent component, Size formSize)
	{
		if (StiInplaceEditor.IsLeftLocation(component))
		{
			return GetScreenLeftPosition(component, formSize);
		}
		return GetScreenRightPosition(component);
	}

	private static Point GetScreenLeftPosition(StiComponent component, Size formSize)
	{
		StiDesignerControl stiDesignerControl = component?.Report?.Designer?.DesignerControl as StiDesignerControl;
		IStiMargin stiMargin = component as IStiMargin;
		int num = StiScale.XXI(stiMargin?.Margin.Left ?? 0.0);
		int num2 = StiScale.YYI(stiMargin?.Margin.Top ?? 0.0);
		RectangleD displayRectangle = component.GetDisplayRectangle();
		RectangleD rectangle = new RectangleD(displayRectangle.X + (double)num, displayRectangle.Y + (double)num2, 0.0, 0.0);
		Rectangle rectangle2 = StiConvert.RectangleToScreen(stiDesignerControl, rectangle);
		rectangle2.X -= StiScale.XXI(StiInplaceEditor.ButtonMargin);
		StiViewControl stiViewControl = stiDesignerControl.PageView.GetCurrentPageView()?.View;
		Point result = ((stiViewControl != null) ? ((Control)stiViewControl).RectangleToScreen(rectangle2).Location : ((Control)stiDesignerControl).RectangleToScreen(rectangle2).Location);
		result.X -= formSize.Width;
		return result;
	}

	private static Point GetScreenRightPosition(StiComponent component)
	{
		StiDesignerControl stiDesignerControl = component?.Report?.Designer?.DesignerControl as StiDesignerControl;
		int num = StiScale.YYI((component is IStiMargin stiMargin) ? stiMargin.Margin.Top : 0.0);
		RectangleD displayRectangle = component.GetDisplayRectangle();
		RectangleD rectangle = new RectangleD(displayRectangle.Right, displayRectangle.Y + (double)num, 0.0, 0.0);
		Rectangle rectangle2 = StiConvert.RectangleToScreen(stiDesignerControl, rectangle);
		rectangle2.X += StiScale.XXI(StiInplaceEditor.ButtonMargin / 2);
		StiViewControl stiViewControl = stiDesignerControl.PageView.GetCurrentPageView()?.View;
		if (stiViewControl != null)
		{
			return ((Control)stiViewControl).RectangleToScreen(rectangle2).Location;
		}
		return ((Control)stiDesignerControl).RectangleToScreen(rectangle2).Location;
	}

	private void SetFormPosition(Point posLeft, Point posRight, Size formSize, bool isLeft)
	{
		if (isLeft)
		{
			((Form)EditorForm).Location = new Point(posLeft.X + StiScale.I2, posLeft.Y + StiScale.I2);
			if (!IsGoodHorizonalFormLocation(formSize))
			{
				SetFormPosition(posLeft, posRight, formSize, isLeft: false);
			}
			return;
		}
		((Form)EditorForm).Location = new Point(posRight.X - StiScale.I2, posRight.Y + StiScale.I2);
		if (!IsGoodHorizonalFormLocation(formSize))
		{
			((Form)EditorForm).Location = new Point(posLeft.X + StiScale.I2, posLeft.Y + StiScale.I2);
			if (!IsGoodHorizonalFormLocation(formSize))
			{
				((Form)EditorForm).Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - formSize.Width, posRight.Y + StiScale.I2);
			}
		}
	}

	public virtual bool IsAllowCenterWindow(object component)
	{
		return false;
	}

	public StiInplaceDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
