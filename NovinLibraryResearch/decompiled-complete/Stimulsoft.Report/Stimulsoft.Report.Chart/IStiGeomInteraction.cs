namespace Stimulsoft.Report.Chart;

public interface IStiGeomInteraction
{
	void InvokeClick(StiInteractionOptions options);

	void InvokeMouseEnter(StiInteractionOptions options);

	void InvokeMouseLeave(StiInteractionOptions options);

	void InvokeMouseDown(StiInteractionOptions options);

	void InvokeMouseUp(StiInteractionOptions options);

	void InvokeDrag(StiInteractionOptions options);
}
