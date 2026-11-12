using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.Shape;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiShapeElementTypeServiceEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	internal void OnDoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override void PaintValue(PaintValueEventArgs e)
	{
		if (e.Value is StiShapeTypeService stiShapeTypeService)
		{
			Rectangle rectangle = new Rectangle(e.Bounds.X + StiScale.I2, e.Bounds.Y + StiScale.I2, e.Bounds.Width - StiScale.I5, e.Bounds.Height - StiScale.I5);
			StiShapeElement stiShapeElement = (StiShapeElement)e.Context.Instance;
			StiShape shape = new StiShapeElementBuilder().Render(stiShapeElement);
			stiShapeTypeService.Paint(e.Graphics, shape, rectangle, 1f / (float)(StiScale.Factor * (double)stiShapeElement.Size));
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		StiShapeTypeService stiShapeTypeService = (StiShapeTypeService)value;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		if (StiOptions.Services.Shapes.Any((StiShapeTypeService s) => s.ServiceEnabled))
		{
			ImageList val = new ImageList
			{
				ColorDepth = (ColorDepth)32,
				ImageSize = new Size(16, 16),
				TransparentColor = Color.Magenta
			};
			StiListBox stiListBox = new StiListBox();
			((ListBox)stiListBox).BorderStyle = (BorderStyle)0;
			((ListBox)stiListBox).ItemHeight = StiScale.I(24);
			((Control)stiListBox).Width = StiScale.I(200);
			stiListBox.ImageList = val;
			StiListBox stiListBox2 = stiListBox;
			((Control)stiListBox2).Height = ((ListBox)stiListBox2).ItemHeight * StiOptions.Services.Shapes.Count((StiShapeTypeService s) => s.ServiceEnabled) + StiScale.I(20);
			if (((Control)stiListBox2).Height > StiScale.I(300))
			{
				((Control)stiListBox2).Height = StiScale.I(300);
			}
			int selectedIndex = 0;
			int num = 0;
			foreach (StiShapeTypeService item in StiOptions.Services.Shapes.Where((StiShapeTypeService s) => s.ServiceEnabled))
			{
				val.Images.Add((Image)(object)StiServiceBitmapAttribute.GetImage(item.GetType()));
				((ListBox)stiListBox2).Items.Add((object)item);
				if (item.GetType() == stiShapeTypeService.GetType())
				{
					selectedIndex = num;
				}
				num++;
			}
			((ListControl)stiListBox2).SelectedIndex = selectedIndex;
			((Control)stiListBox2).DoubleClick += OnDoubleClick;
			((ListBox)stiListBox2).Click += OnDoubleClick;
			edSvc.DropDownControl((Control)(object)stiListBox2);
			if (stiShapeTypeService.GetType() != ((ListBox)stiListBox2).Items[((ListControl)stiListBox2).SelectedIndex].GetType())
			{
				return StiActivator.CreateObject(((ListBox)stiListBox2).Items[((ListControl)stiListBox2).SelectedIndex].GetType());
			}
			return stiShapeTypeService;
		}
		return value;
	}
}
