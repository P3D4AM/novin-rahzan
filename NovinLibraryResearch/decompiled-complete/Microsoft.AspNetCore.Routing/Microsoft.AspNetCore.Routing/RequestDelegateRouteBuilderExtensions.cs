using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Routing;

public static class RequestDelegateRouteBuilderExtensions
{
	public static IRouteBuilder MapRoute(this IRouteBuilder builder, string template, RequestDelegate handler)
	{
		Route item = new Route(new RouteHandler(handler), template, null, null, null, GetConstraintResolver(builder));
		builder.Routes.Add(item);
		return builder;
	}

	public static IRouteBuilder MapMiddlewareRoute(this IRouteBuilder builder, string template, Action<IApplicationBuilder> action)
	{
		IApplicationBuilder applicationBuilder = builder.ApplicationBuilder.New();
		action(applicationBuilder);
		return builder.MapRoute(template, applicationBuilder.Build());
	}

	public static IRouteBuilder MapDelete(this IRouteBuilder builder, string template, RequestDelegate handler)
	{
		return builder.MapVerb("DELETE", template, handler);
	}

	public static IRouteBuilder MapMiddlewareDelete(this IRouteBuilder builder, string template, Action<IApplicationBuilder> action)
	{
		return builder.MapMiddlewareVerb("DELETE", template, action);
	}

	public static IRouteBuilder MapDelete(this IRouteBuilder builder, string template, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
	{
		return builder.MapVerb("DELETE", template, handler);
	}

	public static IRouteBuilder MapGet(this IRouteBuilder builder, string template, RequestDelegate handler)
	{
		return builder.MapVerb("GET", template, handler);
	}

	public static IRouteBuilder MapMiddlewareGet(this IRouteBuilder builder, string template, Action<IApplicationBuilder> action)
	{
		return builder.MapMiddlewareVerb("GET", template, action);
	}

	public static IRouteBuilder MapGet(this IRouteBuilder builder, string template, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
	{
		return builder.MapVerb("GET", template, handler);
	}

	public static IRouteBuilder MapPost(this IRouteBuilder builder, string template, RequestDelegate handler)
	{
		return builder.MapVerb("POST", template, handler);
	}

	public static IRouteBuilder MapMiddlewarePost(this IRouteBuilder builder, string template, Action<IApplicationBuilder> action)
	{
		return builder.MapMiddlewareVerb("POST", template, action);
	}

	public static IRouteBuilder MapPost(this IRouteBuilder builder, string template, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
	{
		return builder.MapVerb("POST", template, handler);
	}

	public static IRouteBuilder MapPut(this IRouteBuilder builder, string template, RequestDelegate handler)
	{
		return builder.MapVerb("PUT", template, handler);
	}

	public static IRouteBuilder MapMiddlewarePut(this IRouteBuilder builder, string template, Action<IApplicationBuilder> action)
	{
		return builder.MapMiddlewareVerb("PUT", template, action);
	}

	public static IRouteBuilder MapPut(this IRouteBuilder builder, string template, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
	{
		return builder.MapVerb("PUT", template, handler);
	}

	public static IRouteBuilder MapVerb(this IRouteBuilder builder, string verb, string template, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
	{
		RequestDelegate handler2 = (HttpContext httpContext) => handler(httpContext.Request, httpContext.Response, httpContext.GetRouteData());
		return builder.MapVerb(verb, template, handler2);
	}

	public static IRouteBuilder MapVerb(this IRouteBuilder builder, string verb, string template, RequestDelegate handler)
	{
		Route item = new Route(new RouteHandler(handler), template, null, new RouteValueDictionary(new
		{
			httpMethod = new HttpMethodRouteConstraint(verb)
		}), null, GetConstraintResolver(builder));
		builder.Routes.Add(item);
		return builder;
	}

	public static IRouteBuilder MapMiddlewareVerb(this IRouteBuilder builder, string verb, string template, Action<IApplicationBuilder> action)
	{
		IApplicationBuilder applicationBuilder = builder.ApplicationBuilder.New();
		action(applicationBuilder);
		return builder.MapVerb(verb, template, applicationBuilder.Build());
	}

	private static IInlineConstraintResolver GetConstraintResolver(IRouteBuilder builder)
	{
		return builder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>();
	}
}
