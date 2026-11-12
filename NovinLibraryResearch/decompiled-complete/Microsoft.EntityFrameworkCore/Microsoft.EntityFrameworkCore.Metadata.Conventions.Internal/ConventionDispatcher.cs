using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

public class ConventionDispatcher
{
	private class ConventionBatch : IConventionBatch, IDisposable
	{
		private readonly ConventionDispatcher _dispatcher;

		private int? _runCount;

		public ConventionBatch(ConventionDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
			if (_dispatcher._scope == _dispatcher._immediateConventionScope)
			{
				_runCount = 0;
				dispatcher._scope = new ConventionScope(_dispatcher._scope);
			}
		}

		private void Run()
		{
			if (!_runCount.HasValue)
			{
				return;
			}
			while (true)
			{
				if (_runCount++ == 32767)
				{
					throw new InvalidOperationException(CoreStrings.ConventionsInfiniteLoop);
				}
				ConventionScope scope = _dispatcher._scope;
				if (scope == _dispatcher._immediateConventionScope)
				{
					break;
				}
				_dispatcher._scope = scope.Parent;
				if (scope.Children == null)
				{
					break;
				}
				scope.MakeReadonly();
				if (scope.Parent != _dispatcher._immediateConventionScope || scope.GetLeafCount() == 0)
				{
					break;
				}
				_dispatcher._scope = new ConventionScope(_dispatcher._immediateConventionScope);
				new RunVisitor(_dispatcher).VisitConventionScope(scope);
			}
		}

		public ForeignKey Run(ForeignKey foreignKey)
		{
			if (!_runCount.HasValue)
			{
				return foreignKey;
			}
			using Reference<ForeignKey> reference = _dispatcher.Tracker.Track(foreignKey);
			Run();
			return (reference.Object?.Builder == null) ? null : reference.Object;
		}

		public void Dispose()
		{
			if (_runCount == 0)
			{
				Run();
			}
		}

		IConventionForeignKey IConventionBatch.Run(IConventionForeignKey foreignKey)
		{
			return Run((ForeignKey)foreignKey);
		}

		IMetadataReference<IConventionForeignKey> IConventionBatch.Track(IConventionForeignKey foreignKey)
		{
			return _dispatcher.Tracker.Track((ForeignKey)foreignKey);
		}
	}

	private abstract class ConventionNode
	{
		public abstract ConventionNode Accept(ConventionVisitor visitor);
	}

	private class ConventionScope : ConventionNode
	{
		private List<ConventionNode> _children;

		public ConventionScope Parent
		{
			[DebuggerStepThrough]
			get;
		}

		public IReadOnlyList<ConventionNode> Children
		{
			[DebuggerStepThrough]
			get
			{
				return _children;
			}
		}

		public ConventionScope(ConventionScope parent, List<ConventionNode> children = null)
		{
			Parent = parent;
			_children = children;
		}

		public int GetLeafCount()
		{
			if (Children == null)
			{
				return 0;
			}
			Queue<ConventionScope> queue = new Queue<ConventionScope>();
			queue.Enqueue(this);
			int num = 0;
			while (queue.Count > 0)
			{
				ConventionScope conventionScope = queue.Dequeue();
				if (conventionScope.Children == null)
				{
					continue;
				}
				foreach (ConventionNode child in conventionScope.Children)
				{
					if (child is ConventionScope item)
					{
						queue.Enqueue(item);
					}
					else
					{
						num++;
					}
				}
			}
			return num;
		}

		private void Add(ConventionNode node)
		{
			if (_children == null)
			{
				_children = new List<ConventionNode>();
			}
			_children.Add(node);
		}

		public void MakeReadonly()
		{
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitConventionScope(this);
		}

		public virtual IConventionEntityTypeBuilder OnEntityTypeAdded([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder)
		{
			Add(new OnEntityTypeAddedNode(entityTypeBuilder));
			return entityTypeBuilder;
		}

		public virtual string OnEntityTypeIgnored([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] Type type)
		{
			Add(new OnEntityTypeIgnoredNode(modelBuilder, name, type));
			return name;
		}

		public virtual IConventionEntityType OnEntityTypeRemoved([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] IConventionEntityType entityType)
		{
			Add(new OnEntityTypeRemovedNode(modelBuilder, entityType));
			return entityType;
		}

		public virtual string OnEntityTypeMemberIgnored([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] string name)
		{
			Add(new OnEntityTypeMemberIgnoredNode(entityTypeBuilder, name));
			return name;
		}

		public virtual IConventionEntityType OnEntityTypeBaseTypeChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] IConventionEntityType newBaseType, [JetBrains.Annotations.CanBeNull] IConventionEntityType previousBaseType)
		{
			Add(new OnEntityTypeBaseTypeChangedNode(entityTypeBuilder, newBaseType, previousBaseType));
			return newBaseType;
		}

		public virtual IConventionAnnotation OnEntityTypeAnnotationChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
		{
			Add(new OnEntityTypeAnnotationChangedNode(entityTypeBuilder, name, annotation, oldAnnotation));
			return annotation;
		}

		public virtual IConventionAnnotation OnModelAnnotationChanged([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
		{
			Add(new OnModelAnnotationChangedNode(modelBuilder, name, annotation, oldAnnotation));
			return annotation;
		}

		public virtual IConventionRelationshipBuilder OnForeignKeyAdded([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
		{
			Add(new OnForeignKeyAddedNode(relationshipBuilder));
			return relationshipBuilder;
		}

		public virtual IConventionForeignKey OnForeignKeyRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionForeignKey foreignKey)
		{
			Add(new OnForeignKeyRemovedNode(entityTypeBuilder, foreignKey));
			return foreignKey;
		}

		public virtual IConventionAnnotation OnForeignKeyAnnotationChanged(IConventionRelationshipBuilder relationshipBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			Add(new OnForeignKeyAnnotationChangedNode(relationshipBuilder, name, annotation, oldAnnotation));
			return annotation;
		}

		public virtual IConventionKeyBuilder OnKeyAdded([JetBrains.Annotations.NotNull] IConventionKeyBuilder keyBuilder)
		{
			Add(new OnKeyAddedNode(keyBuilder));
			return keyBuilder;
		}

		public virtual IConventionKey OnKeyRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionKey key)
		{
			Add(new OnKeyRemovedNode(entityTypeBuilder, key));
			return key;
		}

		public virtual IConventionAnnotation OnKeyAnnotationChanged(IConventionKeyBuilder keyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			Add(new OnKeyAnnotationChangedNode(keyBuilder, name, annotation, oldAnnotation));
			return annotation;
		}

		public virtual IConventionKey OnEntityTypePrimaryKeyChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] IConventionKey newPrimaryKey, [JetBrains.Annotations.CanBeNull] IConventionKey previousPrimaryKey)
		{
			Add(new OnEntityTypePrimaryKeyChangedNode(entityTypeBuilder, newPrimaryKey, previousPrimaryKey));
			return newPrimaryKey;
		}

		public virtual IConventionIndexBuilder OnIndexAdded([JetBrains.Annotations.NotNull] IConventionIndexBuilder indexBuilder)
		{
			Add(new OnIndexAddedNode(indexBuilder));
			return indexBuilder;
		}

		public virtual IConventionIndex OnIndexRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionIndex index)
		{
			Add(new OnIndexRemovedNode(entityTypeBuilder, index));
			return index;
		}

		public virtual IConventionIndexBuilder OnIndexUniquenessChanged([JetBrains.Annotations.NotNull] IConventionIndexBuilder indexBuilder)
		{
			Add(new OnIndexUniquenessChangedNode(indexBuilder));
			return indexBuilder;
		}

		public virtual IConventionAnnotation OnIndexAnnotationChanged([JetBrains.Annotations.NotNull] IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
		{
			Add(new OnIndexAnnotationChangedNode(indexBuilder, name, annotation, oldAnnotation));
			return annotation;
		}

		public virtual IConventionNavigation OnNavigationAdded([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IConventionNavigation navigation)
		{
			Add(new OnNavigationAddedNode(relationshipBuilder, navigation));
			return navigation;
		}

		public virtual string OnNavigationRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder sourceEntityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder targetEntityTypeBuilder, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.CanBeNull] MemberInfo memberInfo)
		{
			Add(new OnNavigationRemovedNode(sourceEntityTypeBuilder, targetEntityTypeBuilder, navigationName, memberInfo));
			return navigationName;
		}

		public virtual IConventionRelationshipBuilder OnForeignKeyPropertiesChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> oldDependentProperties, [JetBrains.Annotations.NotNull] IConventionKey oldPrincipalKey)
		{
			Add(new OnForeignKeyPropertiesChangedNode(relationshipBuilder, oldDependentProperties, oldPrincipalKey));
			return relationshipBuilder;
		}

		public virtual IConventionRelationshipBuilder OnForeignKeyUniquenessChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
		{
			Add(new OnForeignKeyUniquenessChangedNode(relationshipBuilder));
			return relationshipBuilder;
		}

		public virtual IConventionRelationshipBuilder OnForeignKeyRequirednessChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
		{
			Add(new OnForeignKeyRequirednessChangedNode(relationshipBuilder));
			return relationshipBuilder;
		}

		public virtual IConventionRelationshipBuilder OnForeignKeyOwnershipChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
		{
			Add(new OnForeignKeyOwnershipChangedNode(relationshipBuilder));
			return relationshipBuilder;
		}

		public virtual IConventionRelationshipBuilder OnForeignKeyPrincipalEndChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
		{
			Add(new OnForeignKeyPrincipalEndChangedNode(relationshipBuilder));
			return relationshipBuilder;
		}

		public virtual IConventionPropertyBuilder OnPropertyAdded([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder)
		{
			Add(new OnPropertyAddedNode(propertyBuilder));
			return propertyBuilder;
		}

		public virtual IConventionPropertyBuilder OnPropertyNullableChanged([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder)
		{
			Add(new OnPropertyNullableChangedNode(propertyBuilder));
			return propertyBuilder;
		}

		public virtual FieldInfo OnPropertyFieldChanged([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder, FieldInfo newFieldInfo, [JetBrains.Annotations.CanBeNull] FieldInfo oldFieldInfo)
		{
			Add(new OnPropertyFieldChangedNode(propertyBuilder, newFieldInfo, oldFieldInfo));
			return newFieldInfo;
		}

		public virtual IConventionAnnotation OnPropertyAnnotationChanged([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
		{
			Add(new OnPropertyAnnotationChangedNode(propertyBuilder, name, annotation, oldAnnotation));
			return annotation;
		}
	}

	private class ImmediateConventionScope : ConventionScope
	{
		private readonly ConventionSet _conventionSet;

		private readonly ConventionDispatcher _dispatcher;

		private readonly ConventionContext<IConventionEntityTypeBuilder> _entityTypeBuilderConventionContext;

		private readonly ConventionContext<IConventionEntityType> _entityTypeConventionContext;

		private readonly ConventionContext<IConventionRelationshipBuilder> _relationshipBuilderConventionContext;

		private readonly ConventionContext<IConventionForeignKey> _foreignKeyConventionContext;

		private readonly ConventionContext<IConventionNavigation> _navigationConventionContext;

		private readonly ConventionContext<IConventionIndexBuilder> _indexBuilderConventionContext;

		private readonly ConventionContext<IConventionIndex> _indexConventionContext;

		private readonly ConventionContext<IConventionKeyBuilder> _keyBuilderConventionContext;

		private readonly ConventionContext<IConventionKey> _keyConventionContext;

		private readonly ConventionContext<IConventionPropertyBuilder> _propertyBuilderConventionContext;

		private readonly ConventionContext<IConventionModelBuilder> _modelBuilderConventionContext;

		private readonly ConventionContext<IConventionAnnotation> _annotationConventionContext;

		private readonly ConventionContext<string> _stringConventionContext;

		private readonly ConventionContext<FieldInfo> _fieldInfoConventionContext;

		public ImmediateConventionScope([JetBrains.Annotations.NotNull] ConventionSet conventionSet, ConventionDispatcher dispatcher)
			: base(null)
		{
			_conventionSet = conventionSet;
			_dispatcher = dispatcher;
			_entityTypeBuilderConventionContext = new ConventionContext<IConventionEntityTypeBuilder>(dispatcher);
			_entityTypeConventionContext = new ConventionContext<IConventionEntityType>(dispatcher);
			_relationshipBuilderConventionContext = new ConventionContext<IConventionRelationshipBuilder>(dispatcher);
			_foreignKeyConventionContext = new ConventionContext<IConventionForeignKey>(dispatcher);
			_navigationConventionContext = new ConventionContext<IConventionNavigation>(dispatcher);
			_indexBuilderConventionContext = new ConventionContext<IConventionIndexBuilder>(dispatcher);
			_indexConventionContext = new ConventionContext<IConventionIndex>(dispatcher);
			_keyBuilderConventionContext = new ConventionContext<IConventionKeyBuilder>(dispatcher);
			_keyConventionContext = new ConventionContext<IConventionKey>(dispatcher);
			_propertyBuilderConventionContext = new ConventionContext<IConventionPropertyBuilder>(dispatcher);
			_modelBuilderConventionContext = new ConventionContext<IConventionModelBuilder>(dispatcher);
			_annotationConventionContext = new ConventionContext<IConventionAnnotation>(dispatcher);
			_stringConventionContext = new ConventionContext<string>(dispatcher);
			_fieldInfoConventionContext = new ConventionContext<FieldInfo>(dispatcher);
			MakeReadonly();
		}

		public IConventionModelBuilder OnModelFinalized([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder)
		{
			_modelBuilderConventionContext.ResetState(modelBuilder);
			foreach (IModelFinalizedConvention modelFinalizedConvention in _conventionSet.ModelFinalizedConventions)
			{
				using (_dispatcher.DelayConventions())
				{
					modelFinalizedConvention.ProcessModelFinalized(modelBuilder, _modelBuilderConventionContext);
					if (_modelBuilderConventionContext.ShouldStopProcessing())
					{
						return _modelBuilderConventionContext.Result;
					}
				}
			}
			return modelBuilder;
		}

		public IConventionModelBuilder OnModelInitialized([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder)
		{
			using (_dispatcher.DelayConventions())
			{
				_modelBuilderConventionContext.ResetState(modelBuilder);
				foreach (IModelInitializedConvention modelInitializedConvention in _conventionSet.ModelInitializedConventions)
				{
					modelInitializedConvention.ProcessModelInitialized(modelBuilder, _modelBuilderConventionContext);
					if (_modelBuilderConventionContext.ShouldStopProcessing())
					{
						return _modelBuilderConventionContext.Result;
					}
				}
				return modelBuilder;
			}
		}

		public override IConventionAnnotation OnModelAnnotationChanged(IConventionModelBuilder modelBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			using (_dispatcher.DelayConventions())
			{
				_annotationConventionContext.ResetState(annotation);
				foreach (IModelAnnotationChangedConvention modelAnnotationChangedConvention in _conventionSet.ModelAnnotationChangedConventions)
				{
					modelAnnotationChangedConvention.ProcessModelAnnotationChanged(modelBuilder, name, annotation, oldAnnotation, _annotationConventionContext);
					if (_annotationConventionContext.ShouldStopProcessing())
					{
						return _annotationConventionContext.Result;
					}
				}
				return annotation;
			}
		}

		public override IConventionEntityTypeBuilder OnEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder)
		{
			using (_dispatcher.DelayConventions())
			{
				_entityTypeBuilderConventionContext.ResetState(entityTypeBuilder);
				foreach (IEntityTypeAddedConvention entityTypeAddedConvention in _conventionSet.EntityTypeAddedConventions)
				{
					if (entityTypeBuilder.Metadata.Builder == null)
					{
						return null;
					}
					entityTypeAddedConvention.ProcessEntityTypeAdded(entityTypeBuilder, _entityTypeBuilderConventionContext);
					if (_entityTypeBuilderConventionContext.ShouldStopProcessing())
					{
						return _entityTypeBuilderConventionContext.Result;
					}
				}
			}
			if (entityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return entityTypeBuilder;
		}

		public override string OnEntityTypeIgnored(IConventionModelBuilder modelBuilder, string name, Type type)
		{
			using (_dispatcher.DelayConventions())
			{
				_stringConventionContext.ResetState(name);
				foreach (IEntityTypeIgnoredConvention entityTypeIgnoredConvention in _conventionSet.EntityTypeIgnoredConventions)
				{
					if (!modelBuilder.Metadata.IsIgnored(name))
					{
						return null;
					}
					entityTypeIgnoredConvention.ProcessEntityTypeIgnored(modelBuilder, name, type, _stringConventionContext);
					if (_stringConventionContext.ShouldStopProcessing())
					{
						return _stringConventionContext.Result;
					}
				}
			}
			if (!modelBuilder.Metadata.IsIgnored(name))
			{
				return null;
			}
			return name;
		}

		public override IConventionEntityType OnEntityTypeRemoved(IConventionModelBuilder modelBuilder, IConventionEntityType entityType)
		{
			using (_dispatcher.DelayConventions())
			{
				_entityTypeConventionContext.ResetState(entityType);
				foreach (IEntityTypeRemovedConvention entityTypeRemovedConvention in _conventionSet.EntityTypeRemovedConventions)
				{
					entityTypeRemovedConvention.ProcessEntityTypeRemoved(modelBuilder, entityType, _entityTypeConventionContext);
					if (_entityTypeConventionContext.ShouldStopProcessing())
					{
						return _entityTypeConventionContext.Result;
					}
				}
				return entityType;
			}
		}

		public override string OnEntityTypeMemberIgnored(IConventionEntityTypeBuilder entityTypeBuilder, string name)
		{
			if (entityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_stringConventionContext.ResetState(name);
				foreach (IConventionEntityType item in entityTypeBuilder.Metadata.GetDerivedTypesInclusive())
				{
					foreach (IEntityTypeMemberIgnoredConvention entityTypeMemberIgnoredConvention in _conventionSet.EntityTypeMemberIgnoredConventions)
					{
						if (!entityTypeBuilder.Metadata.IsIgnored(name))
						{
							return null;
						}
						entityTypeMemberIgnoredConvention.ProcessEntityTypeMemberIgnored(item.Builder, name, _stringConventionContext);
						if (_stringConventionContext.ShouldStopProcessing())
						{
							return _stringConventionContext.Result;
						}
					}
				}
			}
			if (!entityTypeBuilder.Metadata.IsIgnored(name))
			{
				return null;
			}
			return name;
		}

		public override IConventionEntityType OnEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType previousBaseType)
		{
			using (_dispatcher.DelayConventions())
			{
				_entityTypeConventionContext.ResetState(newBaseType);
				foreach (IEntityTypeBaseTypeChangedConvention entityTypeBaseTypeChangedConvention in _conventionSet.EntityTypeBaseTypeChangedConventions)
				{
					if (entityTypeBuilder.Metadata.Builder == null)
					{
						return null;
					}
					entityTypeBaseTypeChangedConvention.ProcessEntityTypeBaseTypeChanged(entityTypeBuilder, newBaseType, previousBaseType, _entityTypeConventionContext);
					if (_entityTypeConventionContext.ShouldStopProcessing())
					{
						return _entityTypeConventionContext.Result;
					}
				}
			}
			if (entityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return newBaseType;
		}

		public override IConventionKey OnEntityTypePrimaryKeyChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey newPrimaryKey, IConventionKey previousPrimaryKey)
		{
			if (entityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_keyConventionContext.ResetState(newPrimaryKey);
				foreach (IEntityTypePrimaryKeyChangedConvention entityTypePrimaryKeyChangedConvention in _conventionSet.EntityTypePrimaryKeyChangedConventions)
				{
					entityTypePrimaryKeyChangedConvention.ProcessEntityTypePrimaryKeyChanged(entityTypeBuilder, newPrimaryKey, previousPrimaryKey, _keyConventionContext);
					if (_keyConventionContext.ShouldStopProcessing())
					{
						return _keyConventionContext.Result;
					}
				}
			}
			if (newPrimaryKey != null && newPrimaryKey.Builder == null)
			{
				return null;
			}
			return newPrimaryKey;
		}

		public override IConventionAnnotation OnEntityTypeAnnotationChanged(IConventionEntityTypeBuilder entityTypeBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			using (_dispatcher.DelayConventions())
			{
				_annotationConventionContext.ResetState(annotation);
				foreach (IEntityTypeAnnotationChangedConvention entityTypeAnnotationChangedConvention in _conventionSet.EntityTypeAnnotationChangedConventions)
				{
					if (entityTypeBuilder.Metadata.Builder == null)
					{
						return null;
					}
					entityTypeAnnotationChangedConvention.ProcessEntityTypeAnnotationChanged(entityTypeBuilder, name, annotation, oldAnnotation, _annotationConventionContext);
					if (_annotationConventionContext.ShouldStopProcessing())
					{
						return _annotationConventionContext.Result;
					}
				}
			}
			if (entityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return annotation;
		}

		public override IConventionRelationshipBuilder OnForeignKeyAdded(IConventionRelationshipBuilder relationshipBuilder)
		{
			if (relationshipBuilder.Metadata.DeclaringEntityType.Builder == null || relationshipBuilder.Metadata.PrincipalEntityType.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_relationshipBuilderConventionContext.ResetState(relationshipBuilder);
				foreach (IForeignKeyAddedConvention foreignKeyAddedConvention in _conventionSet.ForeignKeyAddedConventions)
				{
					if (relationshipBuilder.Metadata.Builder == null)
					{
						return null;
					}
					foreignKeyAddedConvention.ProcessForeignKeyAdded(relationshipBuilder, _relationshipBuilderConventionContext);
					if (_relationshipBuilderConventionContext.ShouldStopProcessing())
					{
						return _relationshipBuilderConventionContext.Result;
					}
				}
			}
			if (relationshipBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return relationshipBuilder;
		}

		public override IConventionForeignKey OnForeignKeyRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionForeignKey foreignKey)
		{
			if (entityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_foreignKeyConventionContext.ResetState(foreignKey);
				foreach (IForeignKeyRemovedConvention foreignKeyRemovedConvention in _conventionSet.ForeignKeyRemovedConventions)
				{
					foreignKeyRemovedConvention.ProcessForeignKeyRemoved(entityTypeBuilder, foreignKey, _foreignKeyConventionContext);
					if (_foreignKeyConventionContext.ShouldStopProcessing())
					{
						return _foreignKeyConventionContext.Result;
					}
				}
				return foreignKey;
			}
		}

		public override IConventionRelationshipBuilder OnForeignKeyPropertiesChanged(IConventionRelationshipBuilder relationshipBuilder, IReadOnlyList<IConventionProperty> oldDependentProperties, IConventionKey oldPrincipalKey)
		{
			using (_dispatcher.DelayConventions())
			{
				_relationshipBuilderConventionContext.ResetState(relationshipBuilder);
				foreach (IForeignKeyPropertiesChangedConvention foreignKeyPropertiesChangedConvention in _conventionSet.ForeignKeyPropertiesChangedConventions)
				{
					foreignKeyPropertiesChangedConvention.ProcessForeignKeyPropertiesChanged(relationshipBuilder, oldDependentProperties, oldPrincipalKey, _relationshipBuilderConventionContext);
					if (_relationshipBuilderConventionContext.ShouldStopProcessing())
					{
						return _relationshipBuilderConventionContext.Result;
					}
				}
			}
			if (relationshipBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return relationshipBuilder;
		}

		public override IConventionRelationshipBuilder OnForeignKeyUniquenessChanged(IConventionRelationshipBuilder relationshipBuilder)
		{
			using (_dispatcher.DelayConventions())
			{
				_relationshipBuilderConventionContext.ResetState(relationshipBuilder);
				foreach (IForeignKeyUniquenessChangedConvention foreignKeyUniquenessChangedConvention in _conventionSet.ForeignKeyUniquenessChangedConventions)
				{
					if (relationshipBuilder.Metadata.Builder == null)
					{
						return null;
					}
					foreignKeyUniquenessChangedConvention.ProcessForeignKeyUniquenessChanged(relationshipBuilder, _relationshipBuilderConventionContext);
					if (_relationshipBuilderConventionContext.ShouldStopProcessing())
					{
						return _relationshipBuilderConventionContext.Result;
					}
				}
			}
			if (relationshipBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return relationshipBuilder;
		}

		public override IConventionRelationshipBuilder OnForeignKeyRequirednessChanged(IConventionRelationshipBuilder relationshipBuilder)
		{
			using (_dispatcher.DelayConventions())
			{
				_relationshipBuilderConventionContext.ResetState(relationshipBuilder);
				foreach (IForeignKeyRequirednessChangedConvention foreignKeyRequirednessChangedConvention in _conventionSet.ForeignKeyRequirednessChangedConventions)
				{
					if (relationshipBuilder.Metadata.Builder == null)
					{
						return null;
					}
					foreignKeyRequirednessChangedConvention.ProcessForeignKeyRequirednessChanged(relationshipBuilder, _relationshipBuilderConventionContext);
					if (_relationshipBuilderConventionContext.ShouldStopProcessing())
					{
						return _relationshipBuilderConventionContext.Result;
					}
				}
			}
			if (relationshipBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return relationshipBuilder;
		}

		public override IConventionRelationshipBuilder OnForeignKeyOwnershipChanged(IConventionRelationshipBuilder relationshipBuilder)
		{
			using (_dispatcher.DelayConventions())
			{
				_relationshipBuilderConventionContext.ResetState(relationshipBuilder);
				foreach (IForeignKeyOwnershipChangedConvention foreignKeyOwnershipChangedConvention in _conventionSet.ForeignKeyOwnershipChangedConventions)
				{
					if (relationshipBuilder.Metadata.Builder == null)
					{
						return null;
					}
					foreignKeyOwnershipChangedConvention.ProcessForeignKeyOwnershipChanged(relationshipBuilder, _relationshipBuilderConventionContext);
					if (_relationshipBuilderConventionContext.ShouldStopProcessing())
					{
						return _relationshipBuilderConventionContext.Result;
					}
				}
			}
			if (relationshipBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return relationshipBuilder;
		}

		public override IConventionRelationshipBuilder OnForeignKeyPrincipalEndChanged(IConventionRelationshipBuilder relationshipBuilder)
		{
			using (_dispatcher.DelayConventions())
			{
				_relationshipBuilderConventionContext.ResetState(relationshipBuilder);
				foreach (IForeignKeyPrincipalEndChangedConvention foreignKeyPrincipalEndChangedConvention in _conventionSet.ForeignKeyPrincipalEndChangedConventions)
				{
					if (relationshipBuilder.Metadata.Builder == null)
					{
						return null;
					}
					foreignKeyPrincipalEndChangedConvention.ProcessForeignKeyPrincipalEndChanged(relationshipBuilder, _relationshipBuilderConventionContext);
					if (_relationshipBuilderConventionContext.ShouldStopProcessing())
					{
						return _relationshipBuilderConventionContext.Result;
					}
				}
			}
			if (relationshipBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return relationshipBuilder;
		}

		public override IConventionAnnotation OnForeignKeyAnnotationChanged(IConventionRelationshipBuilder relationshipBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			if (relationshipBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_annotationConventionContext.ResetState(annotation);
				foreach (IForeignKeyAnnotationChangedConvention foreignKeyAnnotationChangedConvention in _conventionSet.ForeignKeyAnnotationChangedConventions)
				{
					foreignKeyAnnotationChangedConvention.ProcessForeignKeyAnnotationChanged(relationshipBuilder, name, annotation, oldAnnotation, _annotationConventionContext);
					if (_annotationConventionContext.ShouldStopProcessing())
					{
						return _annotationConventionContext.Result;
					}
				}
				return annotation;
			}
		}

		public override IConventionNavigation OnNavigationAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation)
		{
			if (relationshipBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				if (relationshipBuilder.Metadata.GetNavigation(navigation.IsDependentToPrincipal()) != navigation)
				{
					return null;
				}
				_navigationConventionContext.ResetState(navigation);
				foreach (INavigationAddedConvention navigationAddedConvention in _conventionSet.NavigationAddedConventions)
				{
					navigationAddedConvention.ProcessNavigationAdded(relationshipBuilder, navigation, _navigationConventionContext);
					if (_navigationConventionContext.ShouldStopProcessing())
					{
						return _navigationConventionContext.Result;
					}
				}
			}
			if (relationshipBuilder.Metadata.GetNavigation(navigation.IsDependentToPrincipal()) != navigation)
			{
				return null;
			}
			return navigation;
		}

		public override string OnNavigationRemoved(IConventionEntityTypeBuilder sourceEntityTypeBuilder, IConventionEntityTypeBuilder targetEntityTypeBuilder, string navigationName, MemberInfo memberInfo)
		{
			if (sourceEntityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_stringConventionContext.ResetState(navigationName);
				foreach (INavigationRemovedConvention navigationRemovedConvention in _conventionSet.NavigationRemovedConventions)
				{
					if (sourceEntityTypeBuilder.Metadata.FindNavigation(navigationName) != null)
					{
						return null;
					}
					navigationRemovedConvention.ProcessNavigationRemoved(sourceEntityTypeBuilder, targetEntityTypeBuilder, navigationName, memberInfo, _stringConventionContext);
					if (_stringConventionContext.ShouldStopProcessing())
					{
						return _stringConventionContext.Result;
					}
				}
			}
			if (sourceEntityTypeBuilder.Metadata.FindNavigation(navigationName) != null)
			{
				return null;
			}
			return navigationName;
		}

		public override IConventionKeyBuilder OnKeyAdded(IConventionKeyBuilder keyBuilder)
		{
			if (keyBuilder.Metadata.DeclaringEntityType.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_keyBuilderConventionContext.ResetState(keyBuilder);
				foreach (IKeyAddedConvention keyAddedConvention in _conventionSet.KeyAddedConventions)
				{
					if (keyBuilder.Metadata.Builder == null)
					{
						return null;
					}
					keyAddedConvention.ProcessKeyAdded(keyBuilder, _keyBuilderConventionContext);
					if (_keyBuilderConventionContext.ShouldStopProcessing())
					{
						return _keyBuilderConventionContext.Result;
					}
				}
			}
			if (keyBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return keyBuilder;
		}

		public override IConventionKey OnKeyRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey key)
		{
			if (entityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_keyConventionContext.ResetState(key);
				foreach (IKeyRemovedConvention keyRemovedConvention in _conventionSet.KeyRemovedConventions)
				{
					keyRemovedConvention.ProcessKeyRemoved(entityTypeBuilder, key, _keyConventionContext);
					if (_keyConventionContext.ShouldStopProcessing())
					{
						return _keyConventionContext.Result;
					}
				}
				return key;
			}
		}

		public override IConventionAnnotation OnKeyAnnotationChanged(IConventionKeyBuilder keyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			if (keyBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_annotationConventionContext.ResetState(annotation);
				foreach (IKeyAnnotationChangedConvention keyAnnotationChangedConvention in _conventionSet.KeyAnnotationChangedConventions)
				{
					keyAnnotationChangedConvention.ProcessKeyAnnotationChanged(keyBuilder, name, annotation, oldAnnotation, _annotationConventionContext);
					if (_annotationConventionContext.ShouldStopProcessing())
					{
						return _annotationConventionContext.Result;
					}
				}
				return annotation;
			}
		}

		public override IConventionIndexBuilder OnIndexAdded(IConventionIndexBuilder indexBuilder)
		{
			if (indexBuilder.Metadata.DeclaringEntityType.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_indexBuilderConventionContext.ResetState(indexBuilder);
				foreach (IIndexAddedConvention indexAddedConvention in _conventionSet.IndexAddedConventions)
				{
					if (indexBuilder.Metadata.Builder == null)
					{
						return null;
					}
					indexAddedConvention.ProcessIndexAdded(indexBuilder, _indexBuilderConventionContext);
					if (_indexBuilderConventionContext.ShouldStopProcessing())
					{
						return _indexBuilderConventionContext.Result;
					}
				}
			}
			if (indexBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return indexBuilder;
		}

		public override IConventionIndex OnIndexRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionIndex index)
		{
			if (entityTypeBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_indexConventionContext.ResetState(index);
				foreach (IIndexRemovedConvention indexRemovedConvention in _conventionSet.IndexRemovedConventions)
				{
					indexRemovedConvention.ProcessIndexRemoved(entityTypeBuilder, index, _indexConventionContext);
					if (_indexConventionContext.ShouldStopProcessing())
					{
						return _indexConventionContext.Result;
					}
				}
				return index;
			}
		}

		public override IConventionIndexBuilder OnIndexUniquenessChanged(IConventionIndexBuilder indexBuilder)
		{
			using (_dispatcher.DelayConventions())
			{
				_indexBuilderConventionContext.ResetState(indexBuilder);
				foreach (IIndexUniquenessChangedConvention indexUniquenessChangedConvention in _conventionSet.IndexUniquenessChangedConventions)
				{
					if (indexBuilder.Metadata.Builder == null)
					{
						return null;
					}
					indexUniquenessChangedConvention.ProcessIndexUniquenessChanged(indexBuilder, _indexBuilderConventionContext);
					if (_indexBuilderConventionContext.ShouldStopProcessing())
					{
						return _indexBuilderConventionContext.Result;
					}
				}
			}
			if (indexBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return indexBuilder;
		}

		public override IConventionAnnotation OnIndexAnnotationChanged(IConventionIndexBuilder indexBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			if (indexBuilder.Metadata.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_annotationConventionContext.ResetState(annotation);
				foreach (IIndexAnnotationChangedConvention indexAnnotationChangedConvention in _conventionSet.IndexAnnotationChangedConventions)
				{
					indexAnnotationChangedConvention.ProcessIndexAnnotationChanged(indexBuilder, name, annotation, oldAnnotation, _annotationConventionContext);
					if (_annotationConventionContext.ShouldStopProcessing())
					{
						return _annotationConventionContext.Result;
					}
				}
				return annotation;
			}
		}

		public override IConventionPropertyBuilder OnPropertyAdded(IConventionPropertyBuilder propertyBuilder)
		{
			if (propertyBuilder.Metadata.DeclaringEntityType.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_propertyBuilderConventionContext.ResetState(propertyBuilder);
				foreach (IPropertyAddedConvention propertyAddedConvention in _conventionSet.PropertyAddedConventions)
				{
					if (propertyBuilder.Metadata.Builder == null)
					{
						return null;
					}
					propertyAddedConvention.ProcessPropertyAdded(propertyBuilder, _propertyBuilderConventionContext);
					if (_propertyBuilderConventionContext.ShouldStopProcessing())
					{
						return _propertyBuilderConventionContext.Result;
					}
				}
			}
			if (propertyBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return propertyBuilder;
		}

		public override IConventionPropertyBuilder OnPropertyNullableChanged(IConventionPropertyBuilder propertyBuilder)
		{
			if (propertyBuilder.Metadata.DeclaringEntityType.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_propertyBuilderConventionContext.ResetState(propertyBuilder);
				foreach (IPropertyNullabilityChangedConvention propertyNullabilityChangedConvention in _conventionSet.PropertyNullabilityChangedConventions)
				{
					if (propertyBuilder.Metadata.Builder == null)
					{
						return null;
					}
					propertyNullabilityChangedConvention.ProcessPropertyNullabilityChanged(propertyBuilder, _propertyBuilderConventionContext);
					if (_propertyBuilderConventionContext.ShouldStopProcessing())
					{
						return _propertyBuilderConventionContext.Result;
					}
				}
			}
			if (propertyBuilder.Metadata.Builder == null)
			{
				return null;
			}
			return propertyBuilder;
		}

		public override FieldInfo OnPropertyFieldChanged(IConventionPropertyBuilder propertyBuilder, FieldInfo newFieldInfo, FieldInfo oldFieldInfo)
		{
			if (propertyBuilder.Metadata.Builder == null || propertyBuilder.Metadata.DeclaringEntityType.Builder == null)
			{
				return null;
			}
			_fieldInfoConventionContext.ResetState(newFieldInfo);
			foreach (IPropertyFieldChangedConvention propertyFieldChangedConvention in _conventionSet.PropertyFieldChangedConventions)
			{
				propertyFieldChangedConvention.ProcessPropertyFieldChanged(propertyBuilder, newFieldInfo, oldFieldInfo, _fieldInfoConventionContext);
				if (_fieldInfoConventionContext.ShouldStopProcessing())
				{
					return _fieldInfoConventionContext.Result;
				}
			}
			return newFieldInfo;
		}

		public override IConventionAnnotation OnPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			if (propertyBuilder.Metadata.Builder == null || propertyBuilder.Metadata.DeclaringEntityType.Builder == null)
			{
				return null;
			}
			using (_dispatcher.DelayConventions())
			{
				_annotationConventionContext.ResetState(annotation);
				foreach (IPropertyAnnotationChangedConvention propertyAnnotationChangedConvention in _conventionSet.PropertyAnnotationChangedConventions)
				{
					propertyAnnotationChangedConvention.ProcessPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation, _annotationConventionContext);
					if (_annotationConventionContext.ShouldStopProcessing())
					{
						return _annotationConventionContext.Result;
					}
				}
				return annotation;
			}
		}
	}

	private class OnModelAnnotationChangedNode : ConventionNode
	{
		public IConventionModelBuilder ModelBuilder { get; }

		public string Name { get; }

		public IConventionAnnotation Annotation { get; }

		public IConventionAnnotation OldAnnotation { get; }

		public OnModelAnnotationChangedNode(IConventionModelBuilder modelBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			ModelBuilder = modelBuilder;
			Name = name;
			Annotation = annotation;
			OldAnnotation = oldAnnotation;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnModelAnnotationChanged(this);
		}
	}

	private class OnEntityTypeAddedNode : ConventionNode
	{
		public IConventionEntityTypeBuilder EntityTypeBuilder { get; }

		public OnEntityTypeAddedNode(IConventionEntityTypeBuilder entityTypeBuilder)
		{
			EntityTypeBuilder = entityTypeBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnEntityTypeAdded(this);
		}
	}

	private class OnEntityTypeIgnoredNode : ConventionNode
	{
		public IConventionModelBuilder ModelBuilder { get; }

		public string Name { get; }

		public Type Type { get; }

		public OnEntityTypeIgnoredNode(IConventionModelBuilder modelBuilder, string name, Type type)
		{
			ModelBuilder = modelBuilder;
			Name = name;
			Type = type;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnEntityTypeIgnored(this);
		}
	}

	private class OnEntityTypeRemovedNode : ConventionNode
	{
		public IConventionModelBuilder ModelBuilder { get; }

		public IConventionEntityType EntityType { get; }

		public OnEntityTypeRemovedNode(IConventionModelBuilder modelBuilder, IConventionEntityType entityType)
		{
			ModelBuilder = modelBuilder;
			EntityType = entityType;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnEntityTypeRemoved(this);
		}
	}

	private class OnEntityTypeMemberIgnoredNode : ConventionNode
	{
		public IConventionEntityTypeBuilder EntityTypeBuilder { get; }

		public string Name { get; }

		public OnEntityTypeMemberIgnoredNode(IConventionEntityTypeBuilder entityTypeBuilder, string name)
		{
			EntityTypeBuilder = entityTypeBuilder;
			Name = name;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnEntityTypeMemberIgnored(this);
		}
	}

	private class OnEntityTypeBaseTypeChangedNode : ConventionNode
	{
		public IConventionEntityTypeBuilder EntityTypeBuilder { get; }

		public IConventionEntityType NewBaseType { get; }

		public IConventionEntityType PreviousBaseType { get; }

		public OnEntityTypeBaseTypeChangedNode(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType previousBaseType)
		{
			EntityTypeBuilder = entityTypeBuilder;
			NewBaseType = newBaseType;
			PreviousBaseType = previousBaseType;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnBaseEntityTypeChanged(this);
		}
	}

	private class OnEntityTypeAnnotationChangedNode : ConventionNode
	{
		public IConventionEntityTypeBuilder EntityTypeBuilder { get; }

		public string Name { get; }

		public IConventionAnnotation Annotation { get; }

		public IConventionAnnotation OldAnnotation { get; }

		public OnEntityTypeAnnotationChangedNode(IConventionEntityTypeBuilder entityTypeBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			EntityTypeBuilder = entityTypeBuilder;
			Name = name;
			Annotation = annotation;
			OldAnnotation = oldAnnotation;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnEntityTypeAnnotationChanged(this);
		}
	}

	private class OnForeignKeyAddedNode : ConventionNode
	{
		public IConventionRelationshipBuilder RelationshipBuilder { get; }

		public OnForeignKeyAddedNode(IConventionRelationshipBuilder relationshipBuilder)
		{
			RelationshipBuilder = relationshipBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnForeignKeyAdded(this);
		}
	}

	private class OnForeignKeyRemovedNode : ConventionNode
	{
		public IConventionEntityTypeBuilder EntityTypeBuilder { get; }

		public IConventionForeignKey ForeignKey { get; }

		public OnForeignKeyRemovedNode(IConventionEntityTypeBuilder entityTypeBuilder, IConventionForeignKey foreignKey)
		{
			EntityTypeBuilder = entityTypeBuilder;
			ForeignKey = foreignKey;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnForeignKeyRemoved(this);
		}
	}

	private class OnForeignKeyAnnotationChangedNode : ConventionNode
	{
		public IConventionRelationshipBuilder RelationshipBuilder { get; }

		public string Name { get; }

		public IConventionAnnotation Annotation { get; }

		public IConventionAnnotation OldAnnotation { get; }

		public OnForeignKeyAnnotationChangedNode(IConventionRelationshipBuilder relationshipBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			RelationshipBuilder = relationshipBuilder;
			Name = name;
			Annotation = annotation;
			OldAnnotation = oldAnnotation;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnForeignKeyAnnotationChanged(this);
		}
	}

	private class OnForeignKeyPropertiesChangedNode : ConventionNode
	{
		public IConventionRelationshipBuilder RelationshipBuilder { get; }

		public IReadOnlyList<IConventionProperty> OldDependentProperties { get; }

		public IConventionKey OldPrincipalKey { get; }

		public OnForeignKeyPropertiesChangedNode(IConventionRelationshipBuilder relationshipBuilder, IReadOnlyList<IConventionProperty> oldDependentProperties, IConventionKey oldPrincipalKey)
		{
			RelationshipBuilder = relationshipBuilder;
			OldDependentProperties = oldDependentProperties;
			OldPrincipalKey = oldPrincipalKey;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnForeignKeyPropertiesChanged(this);
		}
	}

	private class OnForeignKeyUniquenessChangedNode : ConventionNode
	{
		public IConventionRelationshipBuilder RelationshipBuilder { get; }

		public OnForeignKeyUniquenessChangedNode(IConventionRelationshipBuilder relationshipBuilder)
		{
			RelationshipBuilder = relationshipBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnForeignKeyUniquenessChanged(this);
		}
	}

	private class OnForeignKeyRequirednessChangedNode : ConventionNode
	{
		public IConventionRelationshipBuilder RelationshipBuilder { get; }

		public OnForeignKeyRequirednessChangedNode(IConventionRelationshipBuilder relationshipBuilder)
		{
			RelationshipBuilder = relationshipBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnForeignKeyRequirednessChanged(this);
		}
	}

	private class OnForeignKeyOwnershipChangedNode : ConventionNode
	{
		public IConventionRelationshipBuilder RelationshipBuilder { get; }

		public OnForeignKeyOwnershipChangedNode(IConventionRelationshipBuilder relationshipBuilder)
		{
			RelationshipBuilder = relationshipBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnForeignKeyOwnershipChanged(this);
		}
	}

	private class OnForeignKeyPrincipalEndChangedNode : ConventionNode
	{
		public IConventionRelationshipBuilder RelationshipBuilder { get; }

		public OnForeignKeyPrincipalEndChangedNode(IConventionRelationshipBuilder relationshipBuilder)
		{
			RelationshipBuilder = relationshipBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnForeignKeyPrincipalEndChanged(this);
		}
	}

	private class OnNavigationAddedNode : ConventionNode
	{
		public IConventionRelationshipBuilder RelationshipBuilder { get; }

		public IConventionNavigation Navigation { get; }

		public OnNavigationAddedNode(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation)
		{
			RelationshipBuilder = relationshipBuilder;
			Navigation = navigation;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnNavigationAdded(this);
		}
	}

	private class OnNavigationRemovedNode : ConventionNode
	{
		public IConventionEntityTypeBuilder SourceEntityTypeBuilder { get; }

		public IConventionEntityTypeBuilder TargetEntityTypeBuilder { get; }

		public string NavigationName { get; }

		public MemberInfo MemberInfo { get; }

		public OnNavigationRemovedNode(IConventionEntityTypeBuilder sourceEntityTypeBuilder, IConventionEntityTypeBuilder targetEntityTypeBuilder, string navigationName, MemberInfo memberInfo)
		{
			SourceEntityTypeBuilder = sourceEntityTypeBuilder;
			TargetEntityTypeBuilder = targetEntityTypeBuilder;
			NavigationName = navigationName;
			MemberInfo = memberInfo;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnNavigationRemoved(this);
		}
	}

	private class OnKeyAddedNode : ConventionNode
	{
		public IConventionKeyBuilder KeyBuilder { get; }

		public OnKeyAddedNode(IConventionKeyBuilder keyBuilder)
		{
			KeyBuilder = keyBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnKeyAdded(this);
		}
	}

	private class OnKeyRemovedNode : ConventionNode
	{
		public IConventionEntityTypeBuilder EntityTypeBuilder { get; }

		public IConventionKey Key { get; }

		public OnKeyRemovedNode(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey key)
		{
			EntityTypeBuilder = entityTypeBuilder;
			Key = key;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnKeyRemoved(this);
		}
	}

	private class OnKeyAnnotationChangedNode : ConventionNode
	{
		public IConventionKeyBuilder KeyBuilder { get; }

		public string Name { get; }

		public IConventionAnnotation Annotation { get; }

		public IConventionAnnotation OldAnnotation { get; }

		public OnKeyAnnotationChangedNode(IConventionKeyBuilder keyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			KeyBuilder = keyBuilder;
			Name = name;
			Annotation = annotation;
			OldAnnotation = oldAnnotation;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnKeyAnnotationChanged(this);
		}
	}

	private class OnEntityTypePrimaryKeyChangedNode : ConventionNode
	{
		public IConventionEntityTypeBuilder EntityTypeBuilder { get; }

		public IConventionKey NewPrimaryKey { get; }

		public IConventionKey PreviousPrimaryKey { get; }

		public OnEntityTypePrimaryKeyChangedNode(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey newPrimaryKey, IConventionKey previousPrimaryKey)
		{
			EntityTypeBuilder = entityTypeBuilder;
			NewPrimaryKey = newPrimaryKey;
			PreviousPrimaryKey = previousPrimaryKey;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnPrimaryKeyChanged(this);
		}
	}

	private class OnIndexAddedNode : ConventionNode
	{
		public IConventionIndexBuilder IndexBuilder { get; }

		public OnIndexAddedNode(IConventionIndexBuilder indexBuilder)
		{
			IndexBuilder = indexBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnIndexAdded(this);
		}
	}

	private class OnIndexRemovedNode : ConventionNode
	{
		public IConventionEntityTypeBuilder EntityTypeBuilder { get; }

		public IConventionIndex Index { get; }

		public OnIndexRemovedNode(IConventionEntityTypeBuilder entityTypeBuilder, IConventionIndex index)
		{
			EntityTypeBuilder = entityTypeBuilder;
			Index = index;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnIndexRemoved(this);
		}
	}

	private class OnIndexUniquenessChangedNode : ConventionNode
	{
		public IConventionIndexBuilder IndexBuilder { get; }

		public OnIndexUniquenessChangedNode(IConventionIndexBuilder indexBuilder)
		{
			IndexBuilder = indexBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnIndexUniquenessChanged(this);
		}
	}

	private class OnIndexAnnotationChangedNode : ConventionNode
	{
		public IConventionIndexBuilder IndexBuilder { get; }

		public string Name { get; }

		public IConventionAnnotation Annotation { get; }

		public IConventionAnnotation OldAnnotation { get; }

		public OnIndexAnnotationChangedNode(IConventionIndexBuilder indexBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			IndexBuilder = indexBuilder;
			Name = name;
			Annotation = annotation;
			OldAnnotation = oldAnnotation;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnIndexAnnotationChanged(this);
		}
	}

	private class OnPropertyAddedNode : ConventionNode
	{
		public IConventionPropertyBuilder PropertyBuilder { get; }

		public OnPropertyAddedNode(IConventionPropertyBuilder propertyBuilder)
		{
			PropertyBuilder = propertyBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnPropertyAdded(this);
		}
	}

	private class OnPropertyNullableChangedNode : ConventionNode
	{
		public IConventionPropertyBuilder PropertyBuilder { get; }

		public OnPropertyNullableChangedNode(IConventionPropertyBuilder propertyBuilder)
		{
			PropertyBuilder = propertyBuilder;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnPropertyNullableChanged(this);
		}
	}

	private class OnPropertyFieldChangedNode : ConventionNode
	{
		public IConventionPropertyBuilder PropertyBuilder { get; }

		public FieldInfo NewFieldInfo { get; }

		public FieldInfo OldFieldInfo { get; }

		public OnPropertyFieldChangedNode(IConventionPropertyBuilder propertyBuilder, FieldInfo newFieldInfo, FieldInfo oldFieldInfo)
		{
			PropertyBuilder = propertyBuilder;
			NewFieldInfo = newFieldInfo;
			OldFieldInfo = oldFieldInfo;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnPropertyFieldChanged(this);
		}
	}

	private class OnPropertyAnnotationChangedNode : ConventionNode
	{
		public IConventionPropertyBuilder PropertyBuilder { get; }

		public string Name { get; }

		public IConventionAnnotation Annotation { get; }

		public IConventionAnnotation OldAnnotation { get; }

		public OnPropertyAnnotationChangedNode(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
		{
			PropertyBuilder = propertyBuilder;
			Name = name;
			Annotation = annotation;
			OldAnnotation = oldAnnotation;
		}

		public override ConventionNode Accept(ConventionVisitor visitor)
		{
			return visitor.VisitOnPropertyAnnotationChanged(this);
		}
	}

	private abstract class ConventionVisitor
	{
		public virtual ConventionNode Visit(ConventionNode node)
		{
			return node?.Accept(this);
		}

		public virtual ConventionScope VisitConventionScope(ConventionScope node)
		{
			if (node.Children == null)
			{
				return null;
			}
			List<ConventionNode> list = null;
			foreach (ConventionNode child in node.Children)
			{
				ConventionNode conventionNode = Visit(child);
				if (conventionNode != null)
				{
					if (list == null)
					{
						list = new List<ConventionNode>();
					}
					list.Add(conventionNode);
				}
			}
			if (list != null && list.Count != 0)
			{
				return new ConventionScope(node.Parent, list);
			}
			return null;
		}

		public virtual OnModelAnnotationChangedNode VisitOnModelAnnotationChanged(OnModelAnnotationChangedNode node)
		{
			return node;
		}

		public virtual OnEntityTypeAddedNode VisitOnEntityTypeAdded(OnEntityTypeAddedNode node)
		{
			return node;
		}

		public virtual OnEntityTypeIgnoredNode VisitOnEntityTypeIgnored(OnEntityTypeIgnoredNode node)
		{
			return node;
		}

		public virtual OnEntityTypeRemovedNode VisitOnEntityTypeRemoved(OnEntityTypeRemovedNode node)
		{
			return node;
		}

		public virtual OnEntityTypeMemberIgnoredNode VisitOnEntityTypeMemberIgnored(OnEntityTypeMemberIgnoredNode node)
		{
			return node;
		}

		public virtual OnEntityTypeBaseTypeChangedNode VisitOnBaseEntityTypeChanged(OnEntityTypeBaseTypeChangedNode node)
		{
			return node;
		}

		public virtual OnEntityTypeAnnotationChangedNode VisitOnEntityTypeAnnotationChanged(OnEntityTypeAnnotationChangedNode node)
		{
			return node;
		}

		public virtual OnForeignKeyAddedNode VisitOnForeignKeyAdded(OnForeignKeyAddedNode node)
		{
			return node;
		}

		public virtual OnForeignKeyRemovedNode VisitOnForeignKeyRemoved(OnForeignKeyRemovedNode node)
		{
			return node;
		}

		public virtual OnForeignKeyPropertiesChangedNode VisitOnForeignKeyPropertiesChanged(OnForeignKeyPropertiesChangedNode node)
		{
			return node;
		}

		public virtual OnForeignKeyUniquenessChangedNode VisitOnForeignKeyUniquenessChanged(OnForeignKeyUniquenessChangedNode node)
		{
			return node;
		}

		public virtual OnForeignKeyRequirednessChangedNode VisitOnForeignKeyRequirednessChanged(OnForeignKeyRequirednessChangedNode node)
		{
			return node;
		}

		public virtual OnForeignKeyOwnershipChangedNode VisitOnForeignKeyOwnershipChanged(OnForeignKeyOwnershipChangedNode node)
		{
			return node;
		}

		public virtual OnForeignKeyPrincipalEndChangedNode VisitOnForeignKeyPrincipalEndChanged(OnForeignKeyPrincipalEndChangedNode node)
		{
			return node;
		}

		public virtual OnForeignKeyAnnotationChangedNode VisitOnForeignKeyAnnotationChanged(OnForeignKeyAnnotationChangedNode node)
		{
			return node;
		}

		public virtual OnNavigationAddedNode VisitOnNavigationAdded(OnNavigationAddedNode node)
		{
			return node;
		}

		public virtual OnNavigationRemovedNode VisitOnNavigationRemoved(OnNavigationRemovedNode node)
		{
			return node;
		}

		public virtual OnKeyAddedNode VisitOnKeyAdded(OnKeyAddedNode node)
		{
			return node;
		}

		public virtual OnKeyRemovedNode VisitOnKeyRemoved(OnKeyRemovedNode node)
		{
			return node;
		}

		public virtual OnKeyAnnotationChangedNode VisitOnKeyAnnotationChanged(OnKeyAnnotationChangedNode node)
		{
			return node;
		}

		public virtual OnEntityTypePrimaryKeyChangedNode VisitOnPrimaryKeyChanged(OnEntityTypePrimaryKeyChangedNode node)
		{
			return node;
		}

		public virtual OnIndexAddedNode VisitOnIndexAdded(OnIndexAddedNode node)
		{
			return node;
		}

		public virtual OnIndexRemovedNode VisitOnIndexRemoved(OnIndexRemovedNode node)
		{
			return node;
		}

		public virtual OnIndexUniquenessChangedNode VisitOnIndexUniquenessChanged(OnIndexUniquenessChangedNode node)
		{
			return node;
		}

		public virtual OnIndexAnnotationChangedNode VisitOnIndexAnnotationChanged(OnIndexAnnotationChangedNode node)
		{
			return node;
		}

		public virtual OnPropertyAddedNode VisitOnPropertyAdded(OnPropertyAddedNode node)
		{
			return node;
		}

		public virtual OnPropertyNullableChangedNode VisitOnPropertyNullableChanged(OnPropertyNullableChangedNode node)
		{
			return node;
		}

		public virtual OnPropertyFieldChangedNode VisitOnPropertyFieldChanged(OnPropertyFieldChangedNode node)
		{
			return node;
		}

		public virtual OnPropertyAnnotationChangedNode VisitOnPropertyAnnotationChanged(OnPropertyAnnotationChangedNode node)
		{
			return node;
		}
	}

	private class RunVisitor : ConventionVisitor
	{
		private ConventionDispatcher Dispatcher { get; }

		public RunVisitor(ConventionDispatcher dispatcher)
		{
			Dispatcher = dispatcher;
		}

		public override OnModelAnnotationChangedNode VisitOnModelAnnotationChanged(OnModelAnnotationChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnModelAnnotationChanged(node.ModelBuilder, node.Name, node.Annotation, node.OldAnnotation);
			return null;
		}

		public override OnEntityTypeAddedNode VisitOnEntityTypeAdded(OnEntityTypeAddedNode node)
		{
			Dispatcher._immediateConventionScope.OnEntityTypeAdded(node.EntityTypeBuilder);
			return null;
		}

		public override OnEntityTypeIgnoredNode VisitOnEntityTypeIgnored(OnEntityTypeIgnoredNode node)
		{
			Dispatcher._immediateConventionScope.OnEntityTypeIgnored(node.ModelBuilder, node.Name, node.Type);
			return null;
		}

		public override OnEntityTypeRemovedNode VisitOnEntityTypeRemoved(OnEntityTypeRemovedNode node)
		{
			Dispatcher._immediateConventionScope.OnEntityTypeRemoved(node.ModelBuilder, node.EntityType);
			return null;
		}

		public override OnEntityTypeMemberIgnoredNode VisitOnEntityTypeMemberIgnored(OnEntityTypeMemberIgnoredNode node)
		{
			Dispatcher._immediateConventionScope.OnEntityTypeMemberIgnored(node.EntityTypeBuilder, node.Name);
			return null;
		}

		public override OnEntityTypeBaseTypeChangedNode VisitOnBaseEntityTypeChanged(OnEntityTypeBaseTypeChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnEntityTypeBaseTypeChanged(node.EntityTypeBuilder, node.NewBaseType, node.PreviousBaseType);
			return null;
		}

		public override OnEntityTypeAnnotationChangedNode VisitOnEntityTypeAnnotationChanged(OnEntityTypeAnnotationChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnEntityTypeAnnotationChanged(node.EntityTypeBuilder, node.Name, node.Annotation, node.OldAnnotation);
			return null;
		}

		public override OnForeignKeyAddedNode VisitOnForeignKeyAdded(OnForeignKeyAddedNode node)
		{
			Dispatcher._immediateConventionScope.OnForeignKeyAdded(node.RelationshipBuilder);
			return null;
		}

		public override OnForeignKeyRemovedNode VisitOnForeignKeyRemoved(OnForeignKeyRemovedNode node)
		{
			Dispatcher._immediateConventionScope.OnForeignKeyRemoved(node.EntityTypeBuilder, node.ForeignKey);
			return null;
		}

		public override OnForeignKeyPropertiesChangedNode VisitOnForeignKeyPropertiesChanged(OnForeignKeyPropertiesChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnForeignKeyPropertiesChanged(node.RelationshipBuilder, node.OldDependentProperties, node.OldPrincipalKey);
			return null;
		}

		public override OnForeignKeyUniquenessChangedNode VisitOnForeignKeyUniquenessChanged(OnForeignKeyUniquenessChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnForeignKeyUniquenessChanged(node.RelationshipBuilder);
			return null;
		}

		public override OnForeignKeyRequirednessChangedNode VisitOnForeignKeyRequirednessChanged(OnForeignKeyRequirednessChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnForeignKeyRequirednessChanged(node.RelationshipBuilder);
			return null;
		}

		public override OnForeignKeyOwnershipChangedNode VisitOnForeignKeyOwnershipChanged(OnForeignKeyOwnershipChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnForeignKeyOwnershipChanged(node.RelationshipBuilder);
			return null;
		}

		public override OnForeignKeyPrincipalEndChangedNode VisitOnForeignKeyPrincipalEndChanged(OnForeignKeyPrincipalEndChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnForeignKeyPrincipalEndChanged(node.RelationshipBuilder);
			return null;
		}

		public override OnForeignKeyAnnotationChangedNode VisitOnForeignKeyAnnotationChanged(OnForeignKeyAnnotationChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnForeignKeyAnnotationChanged(node.RelationshipBuilder, node.Name, node.Annotation, node.OldAnnotation);
			return null;
		}

		public override OnNavigationAddedNode VisitOnNavigationAdded(OnNavigationAddedNode node)
		{
			Dispatcher._immediateConventionScope.OnNavigationAdded(node.RelationshipBuilder, node.Navigation);
			return null;
		}

		public override OnNavigationRemovedNode VisitOnNavigationRemoved(OnNavigationRemovedNode node)
		{
			Dispatcher._immediateConventionScope.OnNavigationRemoved(node.SourceEntityTypeBuilder, node.TargetEntityTypeBuilder, node.NavigationName, node.MemberInfo);
			return null;
		}

		public override OnKeyAddedNode VisitOnKeyAdded(OnKeyAddedNode node)
		{
			Dispatcher._immediateConventionScope.OnKeyAdded(node.KeyBuilder);
			return null;
		}

		public override OnKeyRemovedNode VisitOnKeyRemoved(OnKeyRemovedNode node)
		{
			Dispatcher._immediateConventionScope.OnKeyRemoved(node.EntityTypeBuilder, node.Key);
			return null;
		}

		public override OnKeyAnnotationChangedNode VisitOnKeyAnnotationChanged(OnKeyAnnotationChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnKeyAnnotationChanged(node.KeyBuilder, node.Name, node.Annotation, node.OldAnnotation);
			return null;
		}

		public override OnEntityTypePrimaryKeyChangedNode VisitOnPrimaryKeyChanged(OnEntityTypePrimaryKeyChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnEntityTypePrimaryKeyChanged(node.EntityTypeBuilder, node.NewPrimaryKey, node.PreviousPrimaryKey);
			return null;
		}

		public override OnIndexAddedNode VisitOnIndexAdded(OnIndexAddedNode node)
		{
			Dispatcher._immediateConventionScope.OnIndexAdded(node.IndexBuilder);
			return null;
		}

		public override OnIndexRemovedNode VisitOnIndexRemoved(OnIndexRemovedNode node)
		{
			Dispatcher._immediateConventionScope.OnIndexRemoved(node.EntityTypeBuilder, node.Index);
			return null;
		}

		public override OnIndexUniquenessChangedNode VisitOnIndexUniquenessChanged(OnIndexUniquenessChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnIndexUniquenessChanged(node.IndexBuilder);
			return null;
		}

		public override OnIndexAnnotationChangedNode VisitOnIndexAnnotationChanged(OnIndexAnnotationChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnIndexAnnotationChanged(node.IndexBuilder, node.Name, node.Annotation, node.OldAnnotation);
			return null;
		}

		public override OnPropertyAddedNode VisitOnPropertyAdded(OnPropertyAddedNode node)
		{
			Dispatcher._immediateConventionScope.OnPropertyAdded(node.PropertyBuilder);
			return null;
		}

		public override OnPropertyNullableChangedNode VisitOnPropertyNullableChanged(OnPropertyNullableChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnPropertyNullableChanged(node.PropertyBuilder);
			return null;
		}

		public override OnPropertyFieldChangedNode VisitOnPropertyFieldChanged(OnPropertyFieldChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnPropertyFieldChanged(node.PropertyBuilder, node.NewFieldInfo, node.OldFieldInfo);
			return null;
		}

		public override OnPropertyAnnotationChangedNode VisitOnPropertyAnnotationChanged(OnPropertyAnnotationChangedNode node)
		{
			Dispatcher._immediateConventionScope.OnPropertyAnnotationChanged(node.PropertyBuilder, node.Name, node.Annotation, node.OldAnnotation);
			return null;
		}
	}

	private ConventionScope _scope;

	private readonly ImmediateConventionScope _immediateConventionScope;

	public virtual MetadataTracker Tracker
	{
		[DebuggerStepThrough]
		get;
	}

	public ConventionDispatcher([JetBrains.Annotations.NotNull] ConventionSet conventionSet)
	{
		_immediateConventionScope = new ImmediateConventionScope(conventionSet, this);
		_scope = _immediateConventionScope;
		Tracker = new MetadataTracker();
	}

	public virtual IConventionModelBuilder OnModelFinalized([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder)
	{
		return _immediateConventionScope.OnModelFinalized(modelBuilder);
	}

	public virtual IConventionModelBuilder OnModelInitialized([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder)
	{
		return _immediateConventionScope.OnModelInitialized(modelBuilder);
	}

	public virtual IConventionAnnotation OnModelAnnotationChanged([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
	{
		if (CoreAnnotationNames.AllNames.Contains(name))
		{
			return annotation;
		}
		return _scope.OnModelAnnotationChanged(modelBuilder, name, annotation, oldAnnotation);
	}

	public virtual IConventionEntityTypeBuilder OnEntityTypeAdded([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder)
	{
		return _scope.OnEntityTypeAdded(entityTypeBuilder);
	}

	public virtual string OnEntityTypeIgnored([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] Type type)
	{
		return _scope.OnEntityTypeIgnored(modelBuilder, name, type);
	}

	public virtual IConventionEntityType OnEntityTypeRemoved([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] IConventionEntityType type)
	{
		return _scope.OnEntityTypeRemoved(modelBuilder, type);
	}

	public virtual string OnEntityTypeMemberIgnored([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] string name)
	{
		return _scope.OnEntityTypeMemberIgnored(entityTypeBuilder, name);
	}

	public virtual IConventionEntityType OnEntityTypeBaseTypeChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] IConventionEntityType newBaseType, [JetBrains.Annotations.CanBeNull] IConventionEntityType previousBaseType)
	{
		return _scope.OnEntityTypeBaseTypeChanged(entityTypeBuilder, newBaseType, previousBaseType);
	}

	public virtual IConventionAnnotation OnEntityTypeAnnotationChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
	{
		if (CoreAnnotationNames.AllNames.Contains(name))
		{
			return annotation;
		}
		return _scope.OnEntityTypeAnnotationChanged(entityTypeBuilder, name, annotation, oldAnnotation);
	}

	public virtual IConventionRelationshipBuilder OnForeignKeyAdded([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
	{
		return _scope.OnForeignKeyAdded(relationshipBuilder);
	}

	public virtual IConventionForeignKey OnForeignKeyRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionForeignKey foreignKey)
	{
		return _scope.OnForeignKeyRemoved(entityTypeBuilder, foreignKey);
	}

	public virtual void OnForeignKeyPropertiesChanged([JetBrains.Annotations.NotNull] InternalRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<Property> oldDependentProperties, [JetBrains.Annotations.NotNull] Key oldPrincipalKey)
	{
		_scope.OnForeignKeyPropertiesChanged(relationshipBuilder, oldDependentProperties, oldPrincipalKey);
	}

	public virtual IConventionRelationshipBuilder OnForeignKeyUniquenessChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
	{
		return _scope.OnForeignKeyUniquenessChanged(relationshipBuilder);
	}

	public virtual IConventionRelationshipBuilder OnForeignKeyRequirednessChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
	{
		return _scope.OnForeignKeyRequirednessChanged(relationshipBuilder);
	}

	public virtual IConventionRelationshipBuilder OnForeignKeyOwnershipChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
	{
		return _scope.OnForeignKeyOwnershipChanged(relationshipBuilder);
	}

	public virtual IConventionRelationshipBuilder OnForeignKeyPrincipalEndChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder)
	{
		return _scope.OnForeignKeyPrincipalEndChanged(relationshipBuilder);
	}

	public virtual IConventionAnnotation OnForeignKeyAnnotationChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
	{
		if (CoreAnnotationNames.AllNames.Contains(name))
		{
			return annotation;
		}
		return _scope.OnForeignKeyAnnotationChanged(relationshipBuilder, name, annotation, oldAnnotation);
	}

	public virtual IConventionNavigation OnNavigationAdded([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IConventionNavigation navigation)
	{
		return _scope.OnNavigationAdded(relationshipBuilder, navigation);
	}

	public virtual string OnNavigationRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder sourceEntityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder targetEntityTypeBuilder, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.CanBeNull] MemberInfo memberInfo)
	{
		return _scope.OnNavigationRemoved(sourceEntityTypeBuilder, targetEntityTypeBuilder, navigationName, memberInfo);
	}

	public virtual IConventionKeyBuilder OnKeyAdded([JetBrains.Annotations.NotNull] IConventionKeyBuilder keyBuilder)
	{
		return _scope.OnKeyAdded(keyBuilder);
	}

	public virtual IConventionKey OnKeyRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionKey key)
	{
		return _scope.OnKeyRemoved(entityTypeBuilder, key);
	}

	public virtual IConventionAnnotation OnKeyAnnotationChanged([JetBrains.Annotations.NotNull] IConventionKeyBuilder keyBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
	{
		if (CoreAnnotationNames.AllNames.Contains(name))
		{
			return annotation;
		}
		return _scope.OnKeyAnnotationChanged(keyBuilder, name, annotation, oldAnnotation);
	}

	public virtual IConventionKey OnPrimaryKeyChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] IConventionKey newPrimaryKey, [JetBrains.Annotations.CanBeNull] IConventionKey previousPrimaryKey)
	{
		return _scope.OnEntityTypePrimaryKeyChanged(entityTypeBuilder, newPrimaryKey, previousPrimaryKey);
	}

	public virtual IConventionIndexBuilder OnIndexAdded([JetBrains.Annotations.NotNull] IConventionIndexBuilder indexBuilder)
	{
		return _scope.OnIndexAdded(indexBuilder);
	}

	public virtual void OnIndexRemoved([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] Microsoft.EntityFrameworkCore.Metadata.Internal.Index index)
	{
		_scope.OnIndexRemoved(entityTypeBuilder, index);
	}

	public virtual IConventionIndexBuilder OnIndexUniquenessChanged([JetBrains.Annotations.NotNull] IConventionIndexBuilder indexBuilder)
	{
		return _scope.OnIndexUniquenessChanged(indexBuilder);
	}

	public virtual IConventionAnnotation OnIndexAnnotationChanged([JetBrains.Annotations.NotNull] IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
	{
		if (CoreAnnotationNames.AllNames.Contains(name))
		{
			return annotation;
		}
		return _scope.OnIndexAnnotationChanged(indexBuilder, name, annotation, oldAnnotation);
	}

	public virtual IConventionPropertyBuilder OnPropertyAdded([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder)
	{
		return _scope.OnPropertyAdded(propertyBuilder);
	}

	public virtual IConventionPropertyBuilder OnPropertyNullableChanged([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder)
	{
		return _scope.OnPropertyNullableChanged(propertyBuilder);
	}

	public virtual FieldInfo OnPropertyFieldChanged([JetBrains.Annotations.NotNull] InternalPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] FieldInfo newFieldInfo, [JetBrains.Annotations.CanBeNull] FieldInfo oldFieldInfo)
	{
		return _scope.OnPropertyFieldChanged(propertyBuilder, newFieldInfo, oldFieldInfo);
	}

	public virtual IConventionAnnotation OnPropertyAnnotationChanged([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
	{
		if (CoreAnnotationNames.AllNames.Contains(name))
		{
			return annotation;
		}
		return _scope.OnPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation);
	}

	public virtual IConventionBatch DelayConventions()
	{
		return new ConventionBatch(this);
	}

	public virtual T Run<T>(Func<T> func, ref ForeignKey foreignKey)
	{
		IConventionBatch conventionBatch = DelayConventions();
		using Reference<ForeignKey> reference = Tracker.Track(foreignKey);
		T result = func();
		conventionBatch.Dispose();
		foreignKey = ((reference.Object?.Builder == null) ? null : reference.Object);
		return result;
	}
}
