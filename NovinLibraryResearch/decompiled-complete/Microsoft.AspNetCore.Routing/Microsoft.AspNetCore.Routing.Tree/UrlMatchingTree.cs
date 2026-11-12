namespace Microsoft.AspNetCore.Routing.Tree;

public class UrlMatchingTree
{
	public int Order { get; }

	public UrlMatchingNode Root { get; } = new UrlMatchingNode(0);


	public UrlMatchingTree(int order)
	{
		Order = order;
	}
}
