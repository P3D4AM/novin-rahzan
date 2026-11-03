using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Viewer;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Win.Parameters;
using Stimulsoft.Report.Win.Parameters.List;
using Stimulsoft.Report.Win.Parameters.Range;
using Stimulsoft.Report.Win.Parameters.Value;

namespace Stimulsoft.Report.Viewer;

public class StiParametersAccessor : IStiParametersAccessor
{
	private StiDialogsProvider dialogsProvider;

	private Hashtable controlVariables;

	private ToolTip requestFromUserToolTip;

	private Hashtable variablesCurrentValues = new Hashtable();

	internal bool LockRequestFromUser { get; set; }

	internal IStiViewerAccessor ViewerAccessor { get; set; }

	public void CleanAllVariableStates()
	{
		variablesCurrentValues.Clear();
	}

	public void RefreshDialogs()
	{
		ClearDialogs();
		if (!(ViewerAccessor.GetCurrentReport() is StiReport stiReport))
		{
			return;
		}
		CheckRequestFromUser();
		foreach (StiPage page in stiReport.Pages)
		{
			if (!(page is StiForm { StartMode: StiFormStartMode.OnPreview } stiForm))
			{
				continue;
			}
			dialogsProvider = StiDialogsProvider.GetProvider(stiReport);
			if (dialogsProvider == null)
			{
				throw new Exception("StiDialogsProvider is not found in services or report engine can't find assembly 'Stimulsoft.Report.Dialogs.dll'!");
			}
			dialogsProvider.ButtonClick += ButtonRequestFromUserButton_Click;
			dialogsProvider.PrepareForm();
			if (!stiForm.Visible)
			{
				break;
			}
			stiReport.Dictionary.Connect();
			dialogsProvider.LoadForm(stiForm);
			if (stiForm.Visible)
			{
				Panel panelDialogs = ViewerAccessor.GetPanelDialogs();
				((Control)panelDialogs).SuspendLayout();
				((Control)panelDialogs).Controls.Clear();
				((Control)panelDialogs).Dock = (DockStyle)1;
				((Control)panelDialogs).Height = ((Control)((StiWinDialogsProvider)dialogsProvider).Form).Height - SystemInformation.CaptionHeight;
				Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)((StiWinDialogsProvider)dialogsProvider).Form).Controls).Count];
				((ArrangedElementCollection)((Control)((StiWinDialogsProvider)dialogsProvider).Form).Controls).CopyTo((Array)array, 0);
				((Control)panelDialogs).Controls.AddRange(array);
				if (!((Control)panelDialogs).ContainsFocus && ((ArrangedElementCollection)((Control)panelDialogs).Controls).Count > 0)
				{
					((Control)panelDialogs).Controls[((ArrangedElementCollection)((Control)panelDialogs).Controls).Count - 1].Focus();
				}
				ViewerAccessor.EnableParametersButton();
				LockRequestFromUser = true;
				ViewerAccessor.CheckParametersButton();
				LockRequestFromUser = false;
				((Control)panelDialogs).ResumeLayout();
			}
			stiReport.Dictionary.Disconnect();
			break;
		}
	}

	public void CheckVariables()
	{
		if (controlVariables == null)
		{
			return;
		}
		StiReport stiReport = ViewerAccessor.GetCurrentReport() as StiReport;
		foreach (string key in controlVariables.Keys)
		{
			StiVariable stiVariable = stiReport.Dictionary.Variables[key];
			FieldInfo field = stiReport.GetType().GetField(key);
			if (stiVariable.Type.BaseType == typeof(Range))
			{
				if (controlVariables[key] is IStiRangeItemControl stiRangeItemControl)
				{
					if (field != null)
					{
						field.SetValue(stiReport, stiRangeItemControl.Value);
					}
					else
					{
						stiReport[key] = stiRangeItemControl.Value;
					}
					variablesCurrentValues[key] = stiRangeItemControl.Value;
				}
			}
			else if (controlVariables[key] is StiListButtonXControl stiListButtonXControl)
			{
				object result = stiListButtonXControl.GetResult();
				if (field != null)
				{
					field.SetValue(stiReport, result);
				}
				else
				{
					stiReport[key] = result;
				}
				variablesCurrentValues[key] = result;
			}
			else if (controlVariables[key] is IStiValueItemControl stiValueItemControl)
			{
				if (field != null)
				{
					field?.SetValue(stiReport, stiValueItemControl.Value);
				}
				else
				{
					stiReport[key] = stiValueItemControl.Value;
				}
				variablesCurrentValues[key] = stiValueItemControl.Value;
			}
		}
	}

	internal void CopyVariableStatesToReport(StiReport newReport)
	{
		if (variablesCurrentValues.Count <= 0)
		{
			return;
		}
		foreach (string key in variablesCurrentValues.Keys)
		{
			newReport[key] = variablesCurrentValues[key];
		}
	}

	internal void CheckRequestFromUser()
	{
		StiReport stiReport = ViewerAccessor.GetCurrentReport() as StiReport;
		StiReport stiReport2 = ViewerAccessor.GetReport() as StiReport;
		StiReport stiReport3 = stiReport2?.CompiledReport ?? stiReport2;
		foreach (StiVariable variable in stiReport2.Dictionary.Variables)
		{
			if (variable.RequestFromUser && (stiReport3.GetType().GetField(variable.Name) != null || stiReport.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode))
			{
				ShowDialogRequestFromUserVariables();
				ViewerAccessor.EnableParametersButton();
				LockRequestFromUser = true;
				ViewerAccessor.CheckParametersButton();
				LockRequestFromUser = false;
				break;
			}
		}
	}

	internal void ShowDialogRequestFromUserVariables()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0909: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Expected O, but got Unknown
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Expected O, but got Unknown
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Expected O, but got Unknown
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Expected O, but got Unknown
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f79: Expected O, but got Unknown
		//IL_0fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd8: Expected O, but got Unknown
		Panel panelDialogs = ViewerAccessor.GetPanelDialogs();
		if (requestFromUserToolTip == null)
		{
			requestFromUserToolTip = new ToolTip(ViewerAccessor.GetIContainer());
		}
		requestFromUserToolTip.RemoveAll();
		controlVariables = new Hashtable();
		((Control)panelDialogs).Controls.Clear();
		Hashtable requestFromUserComponents = new Hashtable();
		List<Label> requestFromUserLabels = new List<Label>();
		StiReport stiReport = ViewerAccessor.GetReport() as StiReport;
		StiReport stiReport2 = stiReport.CompiledReport ?? stiReport;
		foreach (StiVariable variable in stiReport2.Dictionary.Variables)
		{
			if (!variable.RequestFromUser || variable.Type == typeof(object) || variable.Type == typeof(byte[]))
			{
				continue;
			}
			UserControl val = null;
			Label val2;
			if (variable.Type.BaseType == typeof(Range))
			{
				val2 = StiParameterHelper.CreateLabel(GetNameVariable(variable));
				if (variable.Type == typeof(DateTimeRange))
				{
					val = (UserControl)(object)new StiRangeDateTimeItemControl(variable, stiReport2);
				}
				else if (variable.Type == typeof(TimeSpanRange))
				{
					val = (UserControl)(object)new StiRangeTimeSpanItemControl(variable, stiReport2);
				}
				else if (variable.Type == typeof(CharRange))
				{
					val = (UserControl)(object)new StiRangeCharItemControl(variable, stiReport2);
				}
				else if (variable.Type == typeof(FloatRange) || variable.Type == typeof(DoubleRange) || variable.Type == typeof(DecimalRange))
				{
					val = (UserControl)(object)new StiRangeDoubleItemControl(variable, stiReport2);
				}
				else if (variable.Type == typeof(GuidRange))
				{
					val = (UserControl)(object)new StiRangeGuidItemControl(variable, stiReport2);
				}
				else if (variable.Type == typeof(StringRange))
				{
					val = (UserControl)(object)new StiRangeStringItemControl(variable, stiReport2);
				}
				else if (variable.Type == typeof(ByteRange) || variable.Type == typeof(ShortRange) || variable.Type == typeof(IntRange) || variable.Type == typeof(LongRange))
				{
					val = (UserControl)(object)new StiRangeLongItemControl(variable, stiReport2);
				}
			}
			else
			{
				bool flag = true;
				StiListType type = StiListType.DateTimeList;
				if (variable.Type == typeof(DateTimeList))
				{
					type = StiListType.DateTimeList;
				}
				else if (variable.Type == typeof(TimeSpanList))
				{
					type = StiListType.TimeSpanList;
				}
				else if (variable.Type == typeof(CharList))
				{
					type = StiListType.CharList;
				}
				else if (variable.Type == typeof(ByteList))
				{
					type = StiListType.ByteList;
				}
				else if (variable.Type == typeof(FloatList))
				{
					type = StiListType.FloatList;
				}
				else if (variable.Type == typeof(DoubleList))
				{
					type = StiListType.DoubleList;
				}
				else if (variable.Type == typeof(DecimalList))
				{
					type = StiListType.DecimalList;
				}
				else if (variable.Type == typeof(GuidList))
				{
					type = StiListType.GuidList;
				}
				else if (variable.Type == typeof(StringList))
				{
					type = StiListType.StringList;
				}
				else if (variable.Type == typeof(ShortList))
				{
					type = StiListType.ShortList;
				}
				else if (variable.Type == typeof(IntList))
				{
					type = StiListType.IntList;
				}
				else if (variable.Type == typeof(LongList))
				{
					type = StiListType.LongList;
				}
				else if (variable.Type == typeof(BoolList))
				{
					type = StiListType.BoolList;
				}
				else
				{
					flag = false;
				}
				if (flag)
				{
					val2 = StiParameterHelper.CreateLabel(GetNameVariable(variable));
					val = (UserControl)(object)new StiListButtonXControl(type, variable, stiReport);
				}
				else
				{
					val2 = StiParameterHelper.CreateLabel(GetNameVariable(variable));
					if (variable.Type == typeof(bool) || variable.Type == typeof(bool?))
					{
						val = (UserControl)(object)new StiValueBoolItemControl(variable, stiReport2, variablesCurrentValues);
						requestFromUserToolTip.SetToolTip((Control)(object)((StiValueBoolItemControl)(object)val).checkBoxNotAssigned, string.IsNullOrWhiteSpace(variable.Description) ? Loc.Get("Report", "NotAssigned") : variable.Description);
					}
					else if (variable.Type == typeof(DateTime) || variable.Type == typeof(DateTime?))
					{
						val = (UserControl)(object)new StiValueDateTimeItemControl(variable, stiReport2, variablesCurrentValues);
					}
					else if (variable.Type == typeof(TimeSpan) || variable.Type == typeof(TimeSpan?))
					{
						val = (UserControl)(object)new StiValueTimeSpanItemControl(variable, stiReport2, variablesCurrentValues);
					}
					else if (variable.Type == typeof(Image) || variable.Type == typeof(Bitmap))
					{
						val = (UserControl)(object)new StiValueImageItemControl(variable, stiReport2, variablesCurrentValues);
						requestFromUserToolTip.SetToolTip((Control)(object)((StiValueImageItemControl)(object)val).checkBoxNotAssigned, string.IsNullOrWhiteSpace(variable.Description) ? Loc.Get("Report", "NotAssigned") : variable.Description);
					}
					else if (variable.Type == typeof(char) || variable.Type == typeof(char?))
					{
						val = (UserControl)(object)new StiValueCharItemControl(variable, stiReport2, variablesCurrentValues);
						requestFromUserToolTip.SetToolTip((Control)(object)((StiValueCharItemControl)(object)val).checkBoxNotAssigned, string.IsNullOrWhiteSpace(variable.Description) ? Loc.Get("Report", "NotAssigned") : variable.Description);
					}
					else if (variable.Type == typeof(float) || variable.Type == typeof(double) || variable.Type == typeof(float?) || variable.Type == typeof(double?) || variable.Type == typeof(decimal) || variable.Type == typeof(decimal?))
					{
						val = (UserControl)(object)new StiValueDoubleItemControl(variable, stiReport2, variablesCurrentValues);
						requestFromUserToolTip.SetToolTip((Control)(object)((StiValueDoubleItemControl)(object)val).checkBoxNotAssigned, string.IsNullOrWhiteSpace(variable.Description) ? Loc.Get("Report", "NotAssigned") : variable.Description);
					}
					else if (variable.Type == typeof(Guid) || variable.Type == typeof(Guid?))
					{
						val = (UserControl)(object)new StiValueGuidItemControl(variable, stiReport2, variablesCurrentValues);
						requestFromUserToolTip.SetToolTip((Control)(object)((StiValueGuidItemControl)(object)val).checkBoxNotAssigned, string.IsNullOrWhiteSpace(variable.Description) ? Loc.Get("Report", "NotAssigned") : variable.Description);
					}
					else if (variable.Type == typeof(string))
					{
						val = (UserControl)(object)new StiValueStringItemControl(variable, stiReport2, variablesCurrentValues);
					}
					else
					{
						val = (UserControl)(object)new StiValueLongItemControl(variable, stiReport2, variablesCurrentValues);
						requestFromUserToolTip.SetToolTip((Control)(object)((StiValueLongItemControl)(object)val).checkBoxNotAssigned, string.IsNullOrWhiteSpace(variable.Description) ? Loc.Get("Report", "NotAssigned") : variable.Description);
					}
					requestFromUserToolTip.SetToolTip((Control)(object)val2, string.IsNullOrWhiteSpace(variable.Description) ? ((Control)val2).Text : variable.Description);
				}
			}
			if (val2 != null && val != null)
			{
				controlVariables.Add(variable.Name, val);
				requestFromUserLabels.Add(val2);
				requestFromUserComponents.Add(val2, val);
			}
		}
		IStiValueItemControl stiValueItemControl = null;
		foreach (UserControl value in controlVariables.Values)
		{
			UserControl val3 = value;
			IStiValueItemControl valueItemControl = val3 as IStiValueItemControl;
			if (valueItemControl == null)
			{
				continue;
			}
			StiVariable stiVariable2 = valueItemControl?.Variable;
			if (stiVariable2.DialogInfo.BindingValue && stiVariable2.DialogInfo.BindingVariable != null && controlVariables[stiVariable2.DialogInfo.BindingVariable.Name] is IStiValueItemControl stiValueItemControl2)
			{
				stiValueItemControl2.SelectedValue += delegate(object sender, EventArgs args)
				{
					valueItemControl?.FilterValues(sender);
				};
				if (stiValueItemControl == null)
				{
					stiValueItemControl = stiValueItemControl2;
				}
			}
		}
		if (StiOptions.Engine.ReconnectDataSourcesIfRequestFromUserVariableChanged)
		{
			foreach (UserControl value2 in controlVariables.Values)
			{
				UserControl val4 = value2;
				if (!(val4 is IStiValueItemControl stiValueItemControl3))
				{
					continue;
				}
				List<string> relatedVariablesList = StiVariableHelper.GetRelatedVariablesList(stiReport2, stiValueItemControl3.Variable);
				foreach (string item in relatedVariablesList)
				{
					IStiValueItemControl valueItemBindingControl = controlVariables[item] as IStiValueItemControl;
					if (valueItemBindingControl != null)
					{
						stiValueItemControl3.SelectedValue += delegate(object sender, EventArgs args)
						{
							valueItemBindingControl.FilterValues(sender);
						};
					}
				}
				if (relatedVariablesList.Count <= 0)
				{
					continue;
				}
				if (stiValueItemControl == null)
				{
					stiValueItemControl = stiValueItemControl3;
					continue;
				}
				foreach (string item2 in relatedVariablesList)
				{
					if (stiValueItemControl.Variable.Name == item2)
					{
						stiValueItemControl = stiValueItemControl3;
						break;
					}
				}
			}
		}
		stiValueItemControl?.InvokeSelectedValue();
		Panel controlsPanel = new Panel();
		((Control)panelDialogs).SuspendLayout();
		((Control)controlsPanel).SuspendLayout();
		((ScrollableControl)controlsPanel).AutoScroll = true;
		Button buttonSubmit = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "Submit"),
			Size = StiScale.SizeI(75, 23)
		};
		((Control)buttonSubmit).Click += ButtonSubmit_Click;
		Button buttonReset = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Wizards", "Reset").Replace("&", ""),
			Size = StiScale.SizeI(75, 23)
		};
		((Control)buttonReset).Click += ButtonReset_Click;
		int num = (int)Math.Max(StiReport.GlobalMeasureGraphics.MeasureString(((Control)buttonSubmit).Text, ((Control)buttonSubmit).Font).Width, StiReport.GlobalMeasureGraphics.MeasureString(((Control)buttonReset).Text, ((Control)buttonReset).Font).Width) + 10;
		if (((Control)buttonSubmit).Width < num)
		{
			((Control)buttonSubmit).Width = num;
			((Control)buttonReset).Width = num;
		}
		if (stiReport.ParametersOrientation == StiOrientation.Vertical)
		{
			((Control)controlsPanel).Dock = (DockStyle)1;
			((Control)controlsPanel).AutoSize = true;
			controlsPanel.AutoSizeMode = (AutoSizeMode)1;
		}
		else
		{
			((Control)controlsPanel).Dock = (DockStyle)5;
		}
		((Control)panelDialogs).Controls.Add((Control)(object)controlsPanel);
		if (stiReport.ParametersOrientation == StiOrientation.Horizontal)
		{
			int? requestFromUserElementsPerColumn = StiOptions.Viewer.RequestFromUserElementsPerColumn;
			if (!requestFromUserElementsPerColumn.HasValue)
			{
				if (requestFromUserComponents.Count > 3)
				{
					PlaceComponentsByColumn(controlsPanel, requestFromUserLabels, requestFromUserComponents);
				}
				else
				{
					PlaceComponentsOfOneColumn(controlsPanel, requestFromUserLabels, requestFromUserComponents);
				}
			}
			else
			{
				PlaceComponentsOfMultiColumn(controlsPanel, requestFromUserLabels, requestFromUserComponents, requestFromUserElementsPerColumn.GetValueOrDefault());
			}
			int? requestFromUserPanelMaxHeight = StiOptions.Viewer.RequestFromUserPanelMaxHeight;
			if (requestFromUserPanelMaxHeight.HasValue)
			{
				((Control)controlsPanel).MaximumSize = new Size(9999, requestFromUserPanelMaxHeight.GetValueOrDefault());
				((Control)panelDialogs).Height = requestFromUserPanelMaxHeight.GetValueOrDefault() + StiScale.YYI(55.0);
			}
			((Control)panelDialogs).Dock = (DockStyle)1;
		}
		else
		{
			PlaceComponentsOfOneColumn(controlsPanel, requestFromUserLabels, requestFromUserComponents);
			((Control)panelDialogs).Dock = (DockStyle)3;
		}
		int num2 = 0;
		int num3 = 0;
		IEnumerable<Control> source = ((IEnumerable)((Control)controlsPanel).Controls).Cast<Control>();
		if (source.Any())
		{
			num2 = source.Max((Control c) => c.Bottom) + StiScale.I8;
			num3 = source.Max((Control c) => c.Right);
		}
		((Control)panelDialogs).Width = num3 + StiScale.I12 + SystemInformation.VerticalScrollBarWidth;
		((Control)panelDialogs).Height = num2 + StiScale.I(32);
		((Control)buttonSubmit).Location = new Point(num3 - ((Control)buttonSubmit).Width, num2);
		((Control)buttonReset).Location = new Point(num3 - StiScale.I6 - ((Control)buttonSubmit).Width - ((Control)buttonReset).Width, num2);
		if (((Control)buttonReset).Location.X < 0)
		{
			((Control)buttonReset).Location = new Point(StiScale.I3, ((Control)buttonReset).Location.Y);
			((Control)buttonSubmit).Location = new Point(((Control)buttonReset).Location.X + ((Control)buttonReset).Width + StiScale.I5, ((Control)buttonSubmit).Location.Y);
		}
		ViewerAccessor.OnShowParameters();
		((Control)controlsPanel).ResumeLayout(false);
		((Control)panelDialogs).ResumeLayout(false);
		if (StiScaleUI.IsNoScaling)
		{
			((Control)controlsPanel).Controls.AddRange((Control[])(object)new Control[2]
			{
				(Control)buttonReset,
				(Control)buttonSubmit
			});
		}
		else
		{
			Timer timer = new Timer();
			timer.Interval = 250;
			timer.Start();
			timer.Tick += delegate
			{
				timer.Stop();
				((Component)(object)timer).Dispose();
				((Control)controlsPanel).Controls.AddRange((Control[])(object)new Control[2]
				{
					(Control)buttonReset,
					(Control)buttonSubmit
				});
			};
		}
		if (requestFromUserLabels.Count > 0)
		{
			Timer timerFocus = new Timer();
			timerFocus.Interval = 250;
			timerFocus.Start();
			timerFocus.Tick += delegate
			{
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				timerFocus.Stop();
				((Component)(object)timerFocus).Dispose();
				((Control)requestFromUserComponents[requestFromUserLabels[0]]).Focus();
			};
		}
	}

	private void ClearDialogs()
	{
		if (dialogsProvider != null)
		{
			dialogsProvider.ButtonClick -= ButtonRequestFromUserButton_Click;
			dialogsProvider.CloseForm();
			dialogsProvider.DisposeForm();
			dialogsProvider = null;
		}
		Panel panelDialogs = ViewerAccessor.GetPanelDialogs();
		if (((Control)panelDialogs).Visible)
		{
			((Control)panelDialogs).Visible = false;
		}
		if (((ArrangedElementCollection)((Control)panelDialogs).Controls).Count > 0)
		{
			((Control)panelDialogs).Controls.Clear();
		}
	}

	private string GetNameVariable(StiVariable variable)
	{
		if (variable.Name == variable.Alias)
		{
			return variable.Name;
		}
		if (string.IsNullOrEmpty(variable.Alias))
		{
			return variable.Name;
		}
		return variable.Alias;
	}

	private int PlaceComponentsOfOneColumn(Panel panel, List<Label> labels, Hashtable controls)
	{
		int num = StiScale.I8;
		int num2 = 0;
		foreach (Label label in labels)
		{
			((Control)panel).Controls.Add((Control)(object)label);
			if (((Control)label).Width > num2)
			{
				num2 = ((Control)label).Width;
			}
		}
		num2 += StiScale.XXI(18.0);
		foreach (Label label2 in labels)
		{
			object? obj = controls[label2];
			UserControl val = (UserControl)((obj is UserControl) ? obj : null);
			((Control)val).Location = new Point(num2, num);
			((Control)panel).Controls.Add((Control)(object)val);
			((Control)label2).Location = new Point(StiScale.I4, num + (((Control)val).Height - ((Control)label2).Height) / 2);
			num += ((Control)val).Height + StiScale.I4;
		}
		((Control)ViewerAccessor.GetPanelDialogs()).Height = num + StiScale.YYI(30.0);
		IEnumerable<Control> source = controls.Values.Cast<Control>();
		if (!source.Any())
		{
			return 0;
		}
		return source.Max((Control c) => c.Right);
	}

	private void PlaceComponentsByColumn(Panel panel, List<Label> labels, Hashtable controls)
	{
		int[] array = new int[2];
		array[0] = (array[1] = labels.Count / 2);
		if (labels.Count % 2 > 0)
		{
			array[0]++;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 2; i++)
		{
			int num3 = num2;
			int num4 = StiScale.I8;
			int num5 = num;
			int num6 = 0;
			for (int j = 0; j < array[i]; j++)
			{
				if (num >= labels.Count)
				{
					break;
				}
				Label val = labels[num];
				((Control)panel).Controls.Add((Control)(object)val);
				if (((Control)val).Width > num6)
				{
					num6 = ((Control)val).Width;
				}
				num++;
			}
			num6 += StiScale.I5;
			num = num5;
			for (int k = 0; k < array[i]; k++)
			{
				if (num >= labels.Count)
				{
					break;
				}
				Label val2 = labels[num];
				object? obj = controls[val2];
				Control val3 = (Control)((obj is Control) ? obj : null);
				((Control)val2).Location = new Point(num3, num4 + (val3.Height - ((Control)val2).Height) / 2);
				val3.Location = new Point(num3 + num6, num4);
				((Control)panel).Controls.Add(val3);
				if (val3.Right > num2)
				{
					num2 = val3.Right;
				}
				num4 += val3.Height + StiScale.I4;
				num++;
			}
			num2 += StiScale.XXI(10.0);
		}
	}

	private void PlaceComponentsOfMultiColumn(Panel panel, List<Label> labels, Hashtable controls, int perColumn)
	{
		int num = (int)Math.Ceiling((decimal)(labels.Count / perColumn));
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = perColumn;
		}
		int num2 = 0;
		int num3 = 0;
		for (int j = 0; j < num; j++)
		{
			int num4 = num3;
			int num5 = StiScale.I8;
			int num6 = num2;
			int num7 = 0;
			for (int k = 0; k < array[j]; k++)
			{
				if (num2 >= labels.Count)
				{
					break;
				}
				Label val = labels[num2];
				((Control)panel).Controls.Add((Control)(object)val);
				if (((Control)val).Width > num7)
				{
					num7 = ((Control)val).Width;
				}
				num2++;
			}
			num7 += StiScale.I5;
			num2 = num6;
			for (int l = 0; l < array[j]; l++)
			{
				if (num2 >= labels.Count)
				{
					break;
				}
				Label val2 = labels[num2];
				object? obj = controls[val2];
				Control val3 = (Control)((obj is Control) ? obj : null);
				((Control)val2).Location = new Point(num4, num5 + (val3.Height - ((Control)val2).Height) / 2);
				val3.Location = new Point(num4 + num7, num5);
				((Control)panel).Controls.Add(val3);
				if (val3.Right > num3)
				{
					num3 = val3.Right;
				}
				num5 += val3.Height + StiScale.I4;
				num2++;
			}
			num3 += StiScale.I10;
		}
	}

	private void ButtonSubmit_Click(object sender, EventArgs e)
	{
		if (ViewerAccessor.OnSubmit() || !(ViewerAccessor.GetCurrentReport() is StiReport stiReport))
		{
			return;
		}
		StiMapDrawingCache.Clear();
		Button val = (Button)((sender is Button) ? sender : null);
		((Control)val).Enabled = false;
		variablesCurrentValues.Clear();
		CheckVariables();
		stiReport.IsReportRenderingAfterSubmit = true;
		try
		{
			stiReport.Render();
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(StiViewerControl), ex);
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
		finally
		{
			stiReport.IsReportRenderingAfterSubmit = false;
			((Control)val).Enabled = true;
		}
	}

	private void ButtonRequestFromUserButton_Click(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		StiReport stiReport = ViewerAccessor.GetCurrentReport() as StiReport;
		Button val = (Button)sender;
		if ((int)val.DialogResult == 2)
		{
			ViewerAccessor.UncheckParametersButton();
		}
		else if ((int)val.DialogResult == 1 && stiReport != null)
		{
			stiReport.IsReportRenderingAfterSubmit = true;
			try
			{
				stiReport.Render();
			}
			catch (Exception ex)
			{
				StiLogService.Write(typeof(StiViewerControl), ex);
				StiExceptionProvider.Show(ex, false, (Form)null);
			}
			finally
			{
				stiReport.IsReportRenderingAfterSubmit = false;
			}
		}
		ViewerAccessor.BlockFormClose = true;
	}

	private void ButtonReset_Click(object sender, EventArgs e)
	{
		if (!ViewerAccessor.OnReset())
		{
			StiMapDrawingCache.Clear();
			variablesCurrentValues.Clear();
			ShowDialogRequestFromUserVariables();
		}
	}

	public StiParametersAccessor(IStiViewerAccessor accessor)
	{
		ViewerAccessor = accessor;
	}
}
