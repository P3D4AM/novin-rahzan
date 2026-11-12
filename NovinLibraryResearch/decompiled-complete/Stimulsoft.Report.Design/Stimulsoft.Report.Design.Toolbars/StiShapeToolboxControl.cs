using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Design.Toolbars;

[ToolboxItem(false)]
public class StiShapeToolboxControl : Control
{
	private double scaleStep = StiScale.Step;

	private bool showBaseShapes;

	private bool showBorder;

	private MouseEventHandler mouseDown;

	private MouseEventHandler mouseMove;

	private StiDesignerControl designer;

	private bool isBuild;

	private Dictionary<string, List<StiComponent>> shapes;

	private IContainer components;

	public StiShapeToolboxControl(StiDesignerControl designer, MouseEventHandler mouseDown, MouseEventHandler mouseMove, bool showBaseShapes, bool showBorder = false)
	{
		InitializeComponent();
		this.designer = designer;
		this.mouseDown = mouseDown;
		this.mouseMove = mouseMove;
		this.showBaseShapes = showBaseShapes;
		this.showBorder = showBorder;
		shapes = StiToolboxHelper.GetShapesToolboxItems();
		((Control)this).Width = (int)Math.Ceiling(178.0 * scaleStep);
		((Control)this).Height = (showBaseShapes ? (5 * (int)Math.Ceiling(19.0 * StiScale.Y) + 7 * (int)Math.Ceiling(22.0 * scaleStep)) : (4 * (int)Math.Ceiling(19.0 * StiScale.Y) + 6 * (int)Math.Ceiling(22.0 * scaleStep)));
		if (this.showBorder)
		{
			((Control)this).Width = ((Control)this).Width + (int)(2.0 * StiScale.Step);
			((Control)this).Height = ((Control)this).Height + (int)(2.0 * StiScale.Step);
		}
	}

	public void Build()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		int num = 0;
		Panel val = new Panel
		{
			BackColor = Color.White,
			Padding = new Padding(1)
		};
		if (showBorder)
		{
			((Control)val).Left = (int)StiScale.Step;
			((Control)val).Top = (int)StiScale.Step;
			((Control)val).Width = ((Control)this).Width - (int)(2.0 * StiScale.Step);
			((Control)val).Height = ((Control)this).Height - (int)(2.0 * StiScale.Step);
		}
		else
		{
			((Control)val).Dock = (DockStyle)5;
		}
		foreach (KeyValuePair<string, List<StiComponent>> shape in shapes)
		{
			if (shape.Key.Equals("BasicShapes") && !showBaseShapes)
			{
				continue;
			}
			Label val2 = new Label
			{
				BackColor = Color.FromArgb(255, 238, 238, 238),
				Text = StiLocalization.Get("Shapes", shape.Key.ToString()),
				TextAlign = (ContentAlignment)16,
				Dock = (DockStyle)2,
				Height = (int)Math.Ceiling(19.0 * StiScale.Y)
			};
			((Control)val).Controls.Add((Control)(object)val2);
			num += ((Control)val2).Height;
			List<StiComponent> value = shape.Value;
			double num2 = Math.Ceiling((double)value.Count / Math.Floor((double)((Control)this).Width / (double)(int)Math.Ceiling(22.0 * scaleStep))) * (double)(int)Math.Ceiling(22.0 * scaleStep);
			FlowLayoutPanel val3 = new FlowLayoutPanel
			{
				Dock = (DockStyle)2,
				Height = (int)num2,
				BackColor = Color.Transparent
			};
			foreach (StiComponent item in value)
			{
				StiToolboxButton obj = new StiToolboxButton
				{
					Icon = (Image)(object)GetImageShape(item)
				};
				((Control)obj).Margin = new Padding(0);
				((Control)obj).Tag = item;
				StiToolboxButton stiToolboxButton = obj;
				if (item is StiPrimitive)
				{
					if (designer != null)
					{
						designer.SetComponentToolTip((IComponent)stiToolboxButton, item);
					}
				}
				else if (item is StiShape)
				{
					StiShape stiShape = item as StiShape;
					if (designer != null)
					{
						designer.SetToolTipOnlyHeaderEx((IComponent)stiToolboxButton, globalMeasureGraphics, stiShape.ShapeType.ServiceName, "user-manual/reports_designer_toolbox.htm");
					}
				}
				((Control)stiToolboxButton).MouseDown += mouseDown;
				((Control)stiToolboxButton).MouseMove += mouseMove;
				((Control)val3).Controls.Add((Control)(object)stiToolboxButton);
			}
			((Control)val).Controls.Add((Control)(object)val3);
			num += ((Control)val).Height;
			((Control)this).Controls.Add((Control)(object)val);
		}
		((Control)this).SetBounds(((Control)this).Location.X, ((Control)this).Location.Y, ((Control)this).Width, num);
		isBuild = true;
	}

	private Bitmap GetImageShape(StiComponent element)
	{
		string stepName = StiScale.StepName;
		Bitmap result = null;
		if (element is StiShape)
		{
			StiShape stiShape = element as StiShape;
			StiArrowShapeType stiArrowShapeType = stiShape.ShapeType as StiArrowShapeType;
			string text = stiShape.ShapeType.GetType().Name;
			if (stiArrowShapeType != null && stiArrowShapeType.Direction != 0)
			{
				text += stiArrowShapeType.Direction;
			}
			result = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + text + stepName + ".png");
		}
		else if (element is StiPrimitive)
		{
			result = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + element.GetType().Name + stepName + ".png");
		}
		return result;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		if (!isBuild)
		{
			Build();
		}
		e.Graphics.FillRectangle(Brushes.White, ((Control)this).ClientRectangle);
		Pen val = new Pen(Color.FromArgb(170, 170, 170), 1f);
		try
		{
			e.Graphics.DrawRectangle(val, new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width - 1, ((Control)this).ClientRectangle.Height - 1));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}
}
