using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiLinearMarker : StiMarkerBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearMarker;

	public override string LocalizeName => "LinearMarker";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Value() };
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.Offset(),
			propertiesHelper.Placement(),
			propertiesHelper.RelativeHeight(),
			propertiesHelper.RelativeWidth(),
			propertiesHelper.Skin()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Indicator, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.Font(),
			propertiesHelper.Format(),
			propertiesHelper.ShowValue(),
			propertiesHelper.TextBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.BorderWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public override void ApplyStyle(IStiGaugeStyle style)
	{
		if (AllowApplyStyle)
		{
			Brush = style.Core.MarkerBrush;
			BorderBrush = style.Core.MarkerBorderBrush;
			BorderWidth = style.Core.MarkerBorderWidth;
		}
		if (!StiGaugeV2InitHelper.AllowOldEditor)
		{
			BorderBrush = style.Core.LinearMarkerBorder;
		}
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiLinearMarker();
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		RectangleF rectangle = GetRectangle(base.ValueObj);
		RectangleF rectangle2 = GetRectangle(0f);
		if (context.Gauge.IsAnimation)
		{
			StiTranslationAnimation stiTranslationAnimation = new StiTranslationAnimation(new PointF(rectangle2.X, rectangle2.Y), new PointF(rectangle.X, rectangle.Y), StiGaugeHelper.GlobalDurationElement, StiGaugeHelper.GlobalBeginTimeElement);
			stiTranslationAnimation.Id = "linearMarker_" + base.Scale.Items.IndexOf(this);
			stiTranslationAnimation.ApplyPreviousAnimation(context.Gauge.PreviousAnimations);
			base.Animation = stiTranslationAnimation;
		}
		StiGaugeElementSkin actualSkin = GetActualSkin();
		actualSkin.Draw(context, this, rectangle);
	}

	private RectangleF GetRectangle(float value)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		StiLinearScale stiLinearScale = base.Scale as StiLinearScale;
		SizeF size = stiLinearScale.barGeometry.Size;
		RectangleF rectGeometry = stiLinearScale.barGeometry.RectGeometry;
		float barPosition = GetBarPosition(stiLinearScale.Orientation, value);
		float num = size.Width * base.RelativeWidth;
		float num2 = size.Height * base.RelativeHeight;
		float num3 = num2 / 2f;
		float num4 = 0f;
		float num5 = 0f;
		if ((int)stiLinearScale.Orientation == 0)
		{
			if (Placement != StiPlacement.Overlay)
			{
				float restToLenght = base.Scale.barGeometry.GetRestToLenght();
				float position = base.Scale.GetPosition(value);
				float num6 = ((base.Scale.StartWidth < base.Scale.EndWidth) ? ((1f - position) * restToLenght) : (restToLenght * position));
				num5 = ((Placement == StiPlacement.Outside) ? (rectGeometry.Top - num2 + num6) : (rectGeometry.Bottom - num6));
			}
			else
			{
				num5 = StiRectangleHelper.CenterY(base.Scale.barGeometry.RectGeometry) - num3 + base.Offset * size.Height;
			}
			if (base.Scale.IsReversed)
			{
				barPosition += num / 2f;
				num4 = rectGeometry.Right - barPosition;
			}
			else
			{
				barPosition -= num / 2f;
				num4 = rectGeometry.Left + barPosition;
			}
		}
		else
		{
			if (Placement != StiPlacement.Overlay)
			{
				float restToLenght2 = base.Scale.barGeometry.GetRestToLenght();
				float position2 = base.Scale.GetPosition(value);
				float num7 = ((base.Scale.StartWidth < base.Scale.EndWidth) ? ((1f - position2) * restToLenght2) : (restToLenght2 * position2));
				num4 = ((Placement == StiPlacement.Outside) ? (rectGeometry.Left - num + num7) : (rectGeometry.Right - num7));
			}
			else
			{
				num4 = StiRectangleHelper.CenterX(base.Scale.barGeometry.RectGeometry) - num / 2f + base.Offset * size.Width;
			}
			if (base.Scale.IsReversed)
			{
				barPosition -= num3;
				num5 = rectGeometry.Top + barPosition;
			}
			else
			{
				barPosition += num3;
				num5 = rectGeometry.Bottom - barPosition;
			}
		}
		return new RectangleF(num4, num5, num, num2);
	}

	protected internal override void InteractiveClick(RectangleF rect, Point p)
	{
	}

	private float GetBarPosition(Orientation orientation, float value)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		if (float.IsNaN(value))
		{
			value = actualMinimum;
		}
		else if (value < actualMinimum)
		{
			value = actualMinimum;
		}
		else if (value > actualMaximum)
		{
			value = actualMaximum;
		}
		float num = StiMathHelper.Length(actualMinimum, value);
		float num2 = (((int)orientation == 0) ? base.Scale.barGeometry.RectGeometry.Width : base.Scale.barGeometry.RectGeometry.Height);
		return num / base.Scale.ScaleHelper.TotalLength * num2;
	}
}
