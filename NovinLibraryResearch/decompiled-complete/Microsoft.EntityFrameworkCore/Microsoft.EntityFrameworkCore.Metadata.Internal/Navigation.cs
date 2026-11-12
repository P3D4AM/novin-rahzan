using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Navigation : PropertyBase, IMutableNavigation, INavigation, IPropertyBase, IAnnotatable, IMutablePropertyBase, IMutableAnnotatable, IConventionNavigation, IConventionPropertyBase, IConventionAnnotatable
{
	private IClrCollectionAccessor _collectionAccessor;

	public override Type ClrType => this.GetIdentifyingMemberInfo()?.GetMemberType() ?? typeof(object);

	public virtual ForeignKey ForeignKey { get; }

	public virtual InternalNavigationBuilder Builder
	{
		[DebuggerStepThrough]
		get;
		[DebuggerStepThrough]
		[param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual EntityType DeclaringEntityType
	{
		get
		{
			if (!this.IsDependentToPrincipal())
			{
				return ForeignKey.PrincipalEntityType;
			}
			return ForeignKey.DeclaringEntityType;
		}
	}

	public override TypeBase DeclaringType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	public virtual IClrCollectionAccessor CollectionAccessor => NonCapturingLazyInitializer.EnsureInitialized(ref _collectionAccessor, this, (Navigation n) => (n.IsCollection() && !n.IsShadowProperty()) ? new ClrCollectionAccessorFactory().Create(n) : null);

	public virtual DebugView<Navigation> DebugView => new DebugView<Navigation>(this, (Navigation m) => m.ToDebugString(singleLine: false));

	IForeignKey INavigation.ForeignKey
	{
		[DebuggerStepThrough]
		get
		{
			return ForeignKey;
		}
	}

	IMutableForeignKey IMutableNavigation.ForeignKey
	{
		[DebuggerStepThrough]
		get
		{
			return ForeignKey;
		}
	}

	IEntityType INavigation.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IMutableEntityType IMutableNavigation.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IConventionEntityType IConventionNavigation.DeclaringEntityType => DeclaringEntityType;

	IConventionForeignKey IConventionNavigation.ForeignKey => ForeignKey;

	public Navigation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] PropertyInfo propertyInfo, [JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, [JetBrains.Annotations.NotNull] ForeignKey foreignKey)
		: base(name, propertyInfo, fieldInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		ForeignKey = foreignKey;
		Builder = new InternalNavigationBuilder(this, foreignKey.DeclaringEntityType.Model.Builder);
	}

	public virtual void SetIsEagerLoaded(bool? eagerLoaded, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("EagerLoaded", eagerLoaded, configurationSource);
	}

	public static bool IsCompatible([JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.CanBeNull] MemberInfo navigationProperty, [JetBrains.Annotations.NotNull] EntityType sourceType, [JetBrains.Annotations.NotNull] EntityType targetType, bool? shouldBeCollection, bool shouldThrow)
	{
		Type clrType = targetType.ClrType;
		if (clrType == null)
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.NavigationToShadowEntity(navigationName, sourceType.DisplayName(), targetType.DisplayName()));
			}
			return false;
		}
		if (!(navigationProperty == null))
		{
			return IsCompatible(navigationProperty, sourceType.ClrType, clrType, shouldBeCollection, shouldThrow);
		}
		return true;
	}

	public static bool IsCompatible([JetBrains.Annotations.NotNull] MemberInfo navigationProperty, [JetBrains.Annotations.NotNull] Type sourceClrType, [JetBrains.Annotations.NotNull] Type targetClrType, bool? shouldBeCollection, bool shouldThrow)
	{
		if (!navigationProperty.DeclaringType.GetTypeInfo().IsAssignableFrom(sourceClrType.GetTypeInfo()))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.NoClrNavigation(navigationProperty.Name, sourceClrType.ShortDisplayName()));
			}
			return false;
		}
		Type type = navigationProperty.GetMemberType().TryGetSequenceType();
		bool valueOrDefault = shouldBeCollection.GetValueOrDefault();
		if (!shouldBeCollection.HasValue)
		{
			valueOrDefault = type != null && navigationProperty.GetMemberType() != targetClrType;
			shouldBeCollection = valueOrDefault;
		}
		if (shouldBeCollection.Value && ((object)type == null || !type.GetTypeInfo().IsAssignableFrom(targetClrType.GetTypeInfo())))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.NavigationCollectionWrongClrType(navigationProperty.Name, sourceClrType.ShortDisplayName(), navigationProperty.GetMemberType().ShortDisplayName(), targetClrType.ShortDisplayName()));
			}
			return false;
		}
		if (!shouldBeCollection.Value && !navigationProperty.GetMemberType().GetTypeInfo().IsAssignableFrom(targetClrType.GetTypeInfo()))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.NavigationSingleWrongClrType(navigationProperty.Name, sourceClrType.ShortDisplayName(), navigationProperty.GetMemberType().ShortDisplayName(), targetClrType.ShortDisplayName()));
			}
			return false;
		}
		return true;
	}

	[DebuggerStepThrough]
	public virtual Navigation FindInverse()
	{
		return (Navigation)Microsoft.EntityFrameworkCore.NavigationExtensions.FindInverse(this);
	}

	[DebuggerStepThrough]
	public virtual EntityType GetTargetType()
	{
		return (EntityType)Microsoft.EntityFrameworkCore.NavigationExtensions.GetTargetType(this);
	}

	public override string ToString()
	{
		return this.ToDebugString();
	}
}
