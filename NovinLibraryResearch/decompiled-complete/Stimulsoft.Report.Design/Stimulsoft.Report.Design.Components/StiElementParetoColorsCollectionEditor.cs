using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design.Components;

public class StiElementParetoColorsCollectionEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private IStiParetoSeriesColors component;

	private int colorCellSize = StiScale.XXI(20.0);

	private int colorCellStep = StiScale.I4;

	private string LocFromStyle => Loc.Get("FormStyleDesigner", "FromStyle");

	private string LocPredefined => Loc.Get("FormStyleDesigner", "Predefined");

	private string LocCustom => Loc.Get("FormColorBoxPopup", "Custom");

	private string LocNotSpecified => Loc.Get("FormStyleDesigner", "NotSpecified");

	private string LocEditColors => Loc.Get("FormStyleDesigner", "EditColors");

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		object obj = val.Items[e.Index];
		Graphics graphics = e.Graphics;
		string text = obj as string;
		bool flag = (e.State & 1) > 0;
		Color color = (flag ? SystemColors.MenuHighlight : Color.White);
		Color color2 = (flag ? SystemColors.HighlightText : SystemColors.ControlText);
		if (text == LocPredefined || text == LocCustom)
		{
			color = StiColor.Get("f2f2f2");
			color2 = Color.DimGray;
		}
		StiDrawing.FillRectangle(graphics, color, bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		if (text != null && !(obj is Color[]))
		{
			Font val2 = new Font("Arial", 8f);
			try
			{
				bounds.Inflate(-StiScale.I4, 0);
				TextFormatFlags val3 = (TextFormatFlags)4;
				TextRenderer.DrawText((IDeviceContext)(object)graphics, text, val2, bounds, color2, val3);
				return;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Color[] array = obj as Color[];
		if (array == null)
		{
			return;
		}
		int num = 0;
		bool flag2 = array.Length == 0;
		int num2 = colorCellStep;
		if (flag2)
		{
			array = new Color[1] { Color.FromArgb(153, 255, 255, 255) };
		}
		Color[] array2 = array;
		foreach (Color color3 in array2)
		{
			Rectangle rectangle = new Rectangle(bounds.X + num2, bounds.Y + colorCellStep, colorCellSize, colorCellSize);
			SolidBrush val4 = new SolidBrush(color3);
			try
			{
				graphics.FillRectangle((Brush)(object)val4, rectangle);
				if (flag2)
				{
					graphics.DrawRectangle(Pens.DimGray, rectangle);
					Font val5 = new Font("Arial", 8f);
					try
					{
						bounds.X += rectangle.Width + StiScale.I6;
						bounds.Width -= rectangle.Width - StiScale.I6;
						TextFormatFlags val6 = (TextFormatFlags)4;
						TextRenderer.DrawText((IDeviceContext)(object)graphics, LocNotSpecified, val5, bounds, color2, val6);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				num2 += colorCellSize + colorCellStep;
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			num++;
			if (num == 6)
			{
				break;
			}
		}
	}

	private void ListBox_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		if (e.Index != -1)
		{
			e.ItemWidth = GetTotalWidth();
			e.ItemHeight = colorCellSize + colorCellStep * 2;
		}
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)3;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Invalid comparison between Unknown and I4
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = false;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		component = context.Instance as IStiParetoSeriesColors;
		Array array = context.Instance as Array;
		if (component == null && array != null)
		{
			component = array.GetValue(0) as IStiParetoSeriesColors;
		}
		if (component == null)
		{
			return null;
		}
		ListBox val = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			DrawMode = (DrawMode)2
		};
		int selectedIndex = -1;
		val.Items.Add((object)LocFromStyle);
		if (component.ParetoSeriesColors == null || component.ParetoSeriesColors.Length == 0)
		{
			selectedIndex = val.Items.Count - 1;
		}
		val.Items.Add((object)LocCustom);
		val.Items.Add((object)((component.ParetoSeriesColors != null) ? component.ParetoSeriesColors : new Color[0]));
		if (component.ParetoSeriesColors != null && component.ParetoSeriesColors.Length != 0)
		{
			selectedIndex = val.Items.Count - 1;
		}
		val.Items.Add((object)LocEditColors);
		val.Items.Add((object)LocPredefined);
		foreach (Color[] negativeSet in StiPredefinedColors.NegativeSets)
		{
			val.Items.Add((object)negativeSet);
			if (IsEqual(negativeSet, component.ParetoSeriesColors))
			{
				selectedIndex = val.Items.Count - 1;
			}
		}
		((Control)val).Width = GetTotalWidth();
		((Control)val).Height = GetTotalHeight(val);
		((ListControl)val).SelectedIndex = selectedIndex;
		((Control)val).DoubleClick += ListBox_DoubleClick;
		val.Click += ListBox_DoubleClick;
		val.DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		val.MeasureItem += new MeasureItemEventHandler(ListBox_MeasureItem);
		edSvc.DropDownControl((Control)(object)val);
		if (cancel)
		{
			return value;
		}
		string text = val.SelectedItem?.ToString();
		Color[] array2 = val.SelectedItem as Color[];
		if (text == LocFromStyle)
		{
			return new Color[0];
		}
		if (text == LocEditColors || (array2 != null && array2.Length == 0))
		{
			StiColorsEditorForm stiColorsEditorForm = new StiColorsEditorForm(component.ParetoSeriesColors);
			try
			{
				Form val2 = ((IEnumerable)Application.OpenForms).Cast<Form>().Last();
				if (val2 != null)
				{
					val2.AddOwnedForm((Form)(object)stiColorsEditorForm);
				}
				if ((int)((Form)stiColorsEditorForm).ShowDialog() == 1)
				{
					return stiColorsEditorForm.Colors.Clone();
				}
			}
			finally
			{
				((IDisposable)stiColorsEditorForm)?.Dispose();
			}
		}
		else
		{
			if (text == LocPredefined || text == LocCustom)
			{
				return value;
			}
			if (array2 != null)
			{
				return array2.Clone();
			}
		}
		return null;
	}

	private int GetTotalWidth()
	{
		return (colorCellSize + colorCellStep) * 6 + colorCellStep;
	}

	private int GetTotalHeight(ListBox listBox)
	{
		return listBox.Items.Count * (colorCellSize + colorCellStep * 2);
	}

	private bool IsEqual(Color[] colors1, Color[] colors2)
	{
		if (colors1 != null && colors2 != null)
		{
			return colors1.SequenceEqual(colors2);
		}
		return false;
	}
}
