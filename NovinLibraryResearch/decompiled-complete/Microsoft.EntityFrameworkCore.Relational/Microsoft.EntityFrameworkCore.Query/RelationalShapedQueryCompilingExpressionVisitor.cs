using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalShapedQueryCompilingExpressionVisitor : ShapedQueryCompilingExpressionVisitor
{
	private class CustomShaperCompilingExpressionVisitor : ExpressionVisitor
	{
		private readonly ParameterExpression _dbDataReaderParameter;

		private readonly ParameterExpression _resultCoordinatorParameter;

		private readonly bool _tracking;

		private static readonly MethodInfo _includeReferenceMethodInfo = typeof(CustomShaperCompilingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("IncludeReference");

		private static readonly MethodInfo _populateCollectionMethodInfo = typeof(CustomShaperCompilingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("PopulateCollection");

		private static readonly MethodInfo _populateIncludeCollectionMethodInfo = typeof(CustomShaperCompilingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("PopulateIncludeCollection");

		private static readonly MethodInfo _initializeIncludeCollectionMethodInfo = typeof(CustomShaperCompilingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("InitializeIncludeCollection");

		private static readonly MethodInfo _initializeCollectionMethodInfo = typeof(CustomShaperCompilingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("InitializeCollection");

		private static readonly MethodInfo _collectionAccessorAddMethodInfo = typeof(IClrCollectionAccessor).GetTypeInfo().GetDeclaredMethod("Add");

		public CustomShaperCompilingExpressionVisitor(ParameterExpression dbDataReaderParameter, ParameterExpression resultCoordinatorParameter, bool tracking)
		{
			_dbDataReaderParameter = dbDataReaderParameter;
			_resultCoordinatorParameter = resultCoordinatorParameter;
			_tracking = tracking;
		}

		private static void IncludeReference<TEntity, TIncludingEntity, TIncludedEntity>(QueryContext queryContext, TEntity entity, TIncludedEntity relatedEntity, INavigation navigation, INavigation inverseNavigation, Action<TIncludingEntity, TIncludedEntity> fixup, bool trackingQuery) where TIncludingEntity : TEntity
		{
			if (!(entity is TIncludingEntity val))
			{
				return;
			}
			if (trackingQuery && navigation.DeclaringEntityType.FindPrimaryKey() != null)
			{
				if (relatedEntity == null)
				{
					queryContext.SetNavigationIsLoaded(val, navigation);
				}
				return;
			}
			SetIsLoadedNoTracking(val, navigation);
			if (relatedEntity != null)
			{
				fixup(val, relatedEntity);
				if (inverseNavigation != null && !inverseNavigation.IsCollection())
				{
					SetIsLoadedNoTracking(relatedEntity, inverseNavigation);
				}
			}
		}

		private static void PopulateCollection<TCollection, TElement, TRelatedEntity>(int collectionId, QueryContext queryContext, DbDataReader dbDataReader, ResultCoordinator resultCoordinator, Func<QueryContext, DbDataReader, object[]> parentIdentifier, Func<QueryContext, DbDataReader, object[]> outerIdentifier, Func<QueryContext, DbDataReader, object[]> selfIdentifier, Func<QueryContext, DbDataReader, ResultContext, ResultCoordinator, TRelatedEntity> innerShaper) where TCollection : class, ICollection<TElement> where TRelatedEntity : TElement
		{
			CollectionMaterializationContext collectionMaterializationContext = resultCoordinator.Collections[collectionId];
			if (collectionMaterializationContext.Collection == null)
			{
				return;
			}
			if (resultCoordinator.HasNext == false)
			{
				GenerateCurrentElementIfPending();
				return;
			}
			if (!StructuralComparisons.StructuralEqualityComparer.Equals(outerIdentifier(queryContext, dbDataReader), collectionMaterializationContext.OuterIdentifier))
			{
				GenerateCurrentElementIfPending();
				if (!StructuralComparisons.StructuralEqualityComparer.Equals(parentIdentifier(queryContext, dbDataReader), collectionMaterializationContext.ParentIdentifier))
				{
					resultCoordinator.HasNext = true;
				}
				return;
			}
			object[] array = selfIdentifier(queryContext, dbDataReader);
			if (array.Any((object e) => e == null))
			{
				return;
			}
			if (collectionMaterializationContext.SelfIdentifier != null)
			{
				if (StructuralComparisons.StructuralEqualityComparer.Equals(array, collectionMaterializationContext.SelfIdentifier))
				{
					if (collectionMaterializationContext.ResultContext.Values != null)
					{
						ProcessCurrentElementRow();
					}
					resultCoordinator.ResultReady = false;
					return;
				}
				GenerateCurrentElementIfPending();
				resultCoordinator.HasNext = null;
				collectionMaterializationContext.UpdateSelfIdentifier(array);
			}
			else
			{
				collectionMaterializationContext.UpdateSelfIdentifier(array);
			}
			ProcessCurrentElementRow();
			resultCoordinator.ResultReady = false;
			void GenerateCurrentElementIfPending()
			{
				if (collectionMaterializationContext.ResultContext.Values != null)
				{
					resultCoordinator.HasNext = false;
					ProcessCurrentElementRow();
				}
				collectionMaterializationContext.UpdateSelfIdentifier(null);
			}
			void ProcessCurrentElementRow()
			{
				bool resultReady = resultCoordinator.ResultReady;
				resultCoordinator.ResultReady = true;
				TRelatedEntity val = innerShaper(queryContext, dbDataReader, collectionMaterializationContext.ResultContext, resultCoordinator);
				if (resultCoordinator.ResultReady)
				{
					collectionMaterializationContext.ResultContext.Values = null;
					((TCollection)collectionMaterializationContext.Collection).Add((TElement)(object)val);
				}
				resultCoordinator.ResultReady &= resultReady;
			}
		}

		private static void PopulateIncludeCollection<TIncludingEntity, TIncludedEntity>(int collectionId, QueryContext queryContext, DbDataReader dbDataReader, ResultCoordinator resultCoordinator, Func<QueryContext, DbDataReader, object[]> parentIdentifier, Func<QueryContext, DbDataReader, object[]> outerIdentifier, Func<QueryContext, DbDataReader, object[]> selfIdentifier, Func<QueryContext, DbDataReader, ResultContext, ResultCoordinator, TIncludedEntity> innerShaper, INavigation inverseNavigation, Action<TIncludingEntity, TIncludedEntity> fixup, bool trackingQuery)
		{
			CollectionMaterializationContext collectionMaterializationContext = resultCoordinator.Collections[collectionId];
			object parent = collectionMaterializationContext.Parent;
			if (!(parent is TIncludingEntity))
			{
				return;
			}
			TIncludingEntity entity = (TIncludingEntity)parent;
			if (resultCoordinator.HasNext == false)
			{
				GenerateCurrentElementIfPending();
				return;
			}
			if (!StructuralComparisons.StructuralEqualityComparer.Equals(outerIdentifier(queryContext, dbDataReader), collectionMaterializationContext.OuterIdentifier))
			{
				GenerateCurrentElementIfPending();
				if (!StructuralComparisons.StructuralEqualityComparer.Equals(parentIdentifier(queryContext, dbDataReader), collectionMaterializationContext.ParentIdentifier))
				{
					resultCoordinator.HasNext = true;
				}
				return;
			}
			object[] array = selfIdentifier(queryContext, dbDataReader);
			if (array.Any((object e) => e == null))
			{
				return;
			}
			if (collectionMaterializationContext.SelfIdentifier != null)
			{
				if (StructuralComparisons.StructuralEqualityComparer.Equals(array, collectionMaterializationContext.SelfIdentifier))
				{
					if (collectionMaterializationContext.ResultContext.Values != null)
					{
						ProcessCurrentElementRow();
					}
					resultCoordinator.ResultReady = false;
					return;
				}
				GenerateCurrentElementIfPending();
				resultCoordinator.HasNext = null;
				collectionMaterializationContext.UpdateSelfIdentifier(array);
			}
			else
			{
				collectionMaterializationContext.UpdateSelfIdentifier(array);
			}
			ProcessCurrentElementRow();
			resultCoordinator.ResultReady = false;
			void GenerateCurrentElementIfPending()
			{
				if (collectionMaterializationContext.ResultContext.Values != null)
				{
					resultCoordinator.HasNext = false;
					ProcessCurrentElementRow();
				}
				collectionMaterializationContext.UpdateSelfIdentifier(null);
			}
			void ProcessCurrentElementRow()
			{
				bool resultReady = resultCoordinator.ResultReady;
				resultCoordinator.ResultReady = true;
				TIncludedEntity val = innerShaper(queryContext, dbDataReader, collectionMaterializationContext.ResultContext, resultCoordinator);
				if (resultCoordinator.ResultReady)
				{
					collectionMaterializationContext.ResultContext.Values = null;
					if (!trackingQuery)
					{
						fixup(entity, val);
						if (inverseNavigation != null)
						{
							SetIsLoadedNoTracking(val, inverseNavigation);
						}
					}
				}
				resultCoordinator.ResultReady &= resultReady;
			}
		}

		private static void InitializeIncludeCollection<TParent, TNavigationEntity>(int collectionId, QueryContext queryContext, DbDataReader dbDataReader, ResultCoordinator resultCoordinator, TParent entity, Func<QueryContext, DbDataReader, object[]> parentIdentifier, Func<QueryContext, DbDataReader, object[]> outerIdentifier, INavigation navigation, IClrCollectionAccessor clrCollectionAccessor, bool trackingQuery) where TNavigationEntity : TParent
		{
			object collection = null;
			if (entity is TNavigationEntity)
			{
				if (trackingQuery)
				{
					queryContext.SetNavigationIsLoaded(entity, navigation);
				}
				else
				{
					SetIsLoadedNoTracking(entity, navigation);
				}
				collection = clrCollectionAccessor.GetOrCreate(entity, forMaterialization: true);
			}
			object[] parentIdentifier2 = parentIdentifier(queryContext, dbDataReader);
			object[] outerIdentifier2 = outerIdentifier(queryContext, dbDataReader);
			CollectionMaterializationContext collectionMaterializationContext = new CollectionMaterializationContext(entity, collection, parentIdentifier2, outerIdentifier2);
			resultCoordinator.SetCollectionMaterializationContext(collectionId, collectionMaterializationContext);
		}

		private static TCollection InitializeCollection<TElement, TCollection>(int collectionId, QueryContext queryContext, DbDataReader dbDataReader, ResultCoordinator resultCoordinator, Func<QueryContext, DbDataReader, object[]> parentIdentifier, Func<QueryContext, DbDataReader, object[]> outerIdentifier, IClrCollectionAccessor clrCollectionAccessor) where TCollection : class, IEnumerable<TElement>
		{
			object obj = clrCollectionAccessor?.Create() ?? new List<TElement>();
			object[] parentIdentifier2 = parentIdentifier(queryContext, dbDataReader);
			object[] outerIdentifier2 = outerIdentifier(queryContext, dbDataReader);
			CollectionMaterializationContext collectionMaterializationContext = new CollectionMaterializationContext(null, obj, parentIdentifier2, outerIdentifier2);
			resultCoordinator.SetCollectionMaterializationContext(collectionId, collectionMaterializationContext);
			return (TCollection)obj;
		}

		private static void SetIsLoadedNoTracking(object entity, INavigation navigation)
		{
			((ILazyLoader)(navigation.DeclaringEntityType.GetServiceProperties().FirstOrDefault((IServiceProperty p) => p.ClrType == typeof(ILazyLoader))?.GetGetter().GetClrValue(entity)))?.SetLoaded(entity, navigation.Name);
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (extensionExpression is IncludeExpression includeExpression)
			{
				Type type = includeExpression.EntityExpression.Type;
				Type type2 = includeExpression.Navigation.DeclaringEntityType.ClrType;
				INavigation navigation = includeExpression.Navigation.FindInverse();
				Type clrType = includeExpression.Navigation.GetTargetType().ClrType;
				if (type2 != type && type2.IsAssignableFrom(type))
				{
					type2 = type;
				}
				return Expression.Call(_includeReferenceMethodInfo.MakeGenericMethod(type, type2, clrType), QueryCompilationContext.QueryContextParameter, includeExpression.EntityExpression, includeExpression.NavigationExpression, Expression.Constant(includeExpression.Navigation), Expression.Constant(navigation, typeof(INavigation)), Expression.Constant(GenerateFixup(type2, clrType, includeExpression.Navigation, navigation).Compile()), Expression.Constant(_tracking));
			}
			if (extensionExpression is CollectionInitializingExpression collectionInitializingExpression)
			{
				if (collectionInitializingExpression.Parent != null)
				{
					Type type3 = collectionInitializingExpression.Parent.Type;
					Type type4 = collectionInitializingExpression.Navigation.DeclaringEntityType.ClrType;
					if (type4 != type3 && type4.IsAssignableFrom(type3))
					{
						type4 = type3;
					}
					return Expression.Call(_initializeIncludeCollectionMethodInfo.MakeGenericMethod(type3, type4), Expression.Constant(collectionInitializingExpression.CollectionId), QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter, _resultCoordinatorParameter, collectionInitializingExpression.Parent, Expression.Constant(Expression.Lambda(collectionInitializingExpression.ParentIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(Expression.Lambda(collectionInitializingExpression.OuterIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(collectionInitializingExpression.Navigation), Expression.Constant(collectionInitializingExpression.Navigation.GetCollectionAccessor()), Expression.Constant(_tracking));
				}
				Type type5 = collectionInitializingExpression.Type;
				Type type6 = type5.TryGetSequenceType();
				return Expression.Call(_initializeCollectionMethodInfo.MakeGenericMethod(type6, type5), Expression.Constant(collectionInitializingExpression.CollectionId), QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter, _resultCoordinatorParameter, Expression.Constant(Expression.Lambda(collectionInitializingExpression.ParentIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(Expression.Lambda(collectionInitializingExpression.OuterIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(collectionInitializingExpression.Navigation?.GetCollectionAccessor(), typeof(IClrCollectionAccessor)));
			}
			if (extensionExpression is CollectionPopulatingExpression { Parent: var parent } collectionPopulatingExpression)
			{
				Type returnType = ((LambdaExpression)parent.InnerShaper).ReturnType;
				if (collectionPopulatingExpression.IsInclude)
				{
					Type clrType2 = parent.Navigation.DeclaringEntityType.ClrType;
					INavigation navigation2 = parent.Navigation.FindInverse();
					return Expression.Call(_populateIncludeCollectionMethodInfo.MakeGenericMethod(clrType2, returnType), Expression.Constant(parent.CollectionId), QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter, _resultCoordinatorParameter, Expression.Constant(Expression.Lambda(parent.ParentIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(Expression.Lambda(parent.OuterIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(Expression.Lambda(parent.SelfIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(((LambdaExpression)Visit(parent.InnerShaper)).Compile()), Expression.Constant(navigation2, typeof(INavigation)), Expression.Constant(GenerateFixup(clrType2, returnType, parent.Navigation, navigation2).Compile()), Expression.Constant(_tracking));
				}
				Type type7 = collectionPopulatingExpression.Type;
				Type type8 = type7.TryGetSequenceType();
				return Expression.Call(_populateCollectionMethodInfo.MakeGenericMethod(type7, type8, returnType), Expression.Constant(parent.CollectionId), QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter, _resultCoordinatorParameter, Expression.Constant(Expression.Lambda(parent.ParentIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(Expression.Lambda(parent.OuterIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(Expression.Lambda(parent.SelfIdentifier, QueryCompilationContext.QueryContextParameter, _dbDataReaderParameter).Compile()), Expression.Constant(((LambdaExpression)Visit(parent.InnerShaper)).Compile()));
			}
			if (extensionExpression is GroupByShaperExpression)
			{
				throw new InvalidOperationException("Client side GroupBy is not supported.");
			}
			return base.VisitExtension(extensionExpression);
		}

		private static LambdaExpression GenerateFixup(Type entityType, Type relatedEntityType, INavigation navigation, INavigation inverseNavigation)
		{
			ParameterExpression parameterExpression = Expression.Parameter(entityType);
			ParameterExpression parameterExpression2 = Expression.Parameter(relatedEntityType);
			List<Expression> list = new List<Expression> { navigation.IsCollection() ? AddToCollectionNavigation(parameterExpression, parameterExpression2, navigation) : AssignReferenceNavigation(parameterExpression, parameterExpression2, navigation) };
			if (inverseNavigation != null)
			{
				list.Add(inverseNavigation.IsCollection() ? AddToCollectionNavigation(parameterExpression2, parameterExpression, inverseNavigation) : AssignReferenceNavigation(parameterExpression2, parameterExpression, inverseNavigation));
			}
			return Expression.Lambda(Expression.Block(typeof(void), list), parameterExpression, parameterExpression2);
		}

		private static Expression AssignReferenceNavigation(ParameterExpression entity, ParameterExpression relatedEntity, INavigation navigation)
		{
			return entity.MakeMemberAccess(navigation.GetMemberInfo(forMaterialization: true, forSet: true)).Assign(relatedEntity);
		}

		private static Expression AddToCollectionNavigation(ParameterExpression entity, ParameterExpression relatedEntity, INavigation navigation)
		{
			return Expression.Call(Expression.Constant(navigation.GetCollectionAccessor()), _collectionAccessorAddMethodInfo, entity, relatedEntity, Expression.Constant(true));
		}
	}

	private class RelationalProjectionBindingRemovingExpressionVisitor : ExpressionVisitor
	{
		private static readonly MethodInfo _isDbNullMethod = typeof(DbDataReader).GetRuntimeMethod("IsDBNull", new Type[1] { typeof(int) });

		private readonly SelectExpression _selectExpression;

		private readonly ParameterExpression _dbDataReaderParameter;

		private readonly ParameterExpression _indexMapParameter;

		private readonly IDictionary<ParameterExpression, IDictionary<IProperty, int>> _materializationContextBindings = new Dictionary<ParameterExpression, IDictionary<IProperty, int>>();

		private ReaderColumn[] ProjectionColumns { get; }

		public RelationalProjectionBindingRemovingExpressionVisitor(SelectExpression selectExpression, ParameterExpression dbDataReaderParameter, ParameterExpression indexMapParameter, bool buffer)
		{
			_selectExpression = selectExpression;
			_dbDataReaderParameter = dbDataReaderParameter;
			_indexMapParameter = indexMapParameter;
			if (buffer)
			{
				ProjectionColumns = new ReaderColumn[selectExpression.Projection.Count];
			}
		}

		public virtual Expression Visit(Expression node, out IReadOnlyList<ReaderColumn> projectionColumns)
		{
			Expression? result = Visit(node);
			projectionColumns = ProjectionColumns;
			return result;
		}

		protected override Expression VisitBinary(BinaryExpression binaryExpression)
		{
			if (binaryExpression.NodeType == ExpressionType.Assign && binaryExpression.Left is ParameterExpression parameterExpression && parameterExpression.Type == typeof(MaterializationContext))
			{
				NewExpression newExpression = (NewExpression)binaryExpression.Right;
				ProjectionBindingExpression projectionBindingExpression = (ProjectionBindingExpression)newExpression.Arguments[0];
				_materializationContextBindings[parameterExpression] = (IDictionary<IProperty, int>)GetProjectionIndex(projectionBindingExpression);
				NewExpression right = Expression.New(newExpression.Constructor, Expression.Constant(ValueBuffer.Empty), newExpression.Arguments[1]);
				return Expression.MakeBinary(ExpressionType.Assign, binaryExpression.Left, right);
			}
			if (binaryExpression.NodeType == ExpressionType.Assign && binaryExpression.Left is MemberExpression { Member: FieldInfo { IsInitOnly: not false } } memberExpression)
			{
				return memberExpression.Assign(Visit(binaryExpression.Right));
			}
			return base.VisitBinary(binaryExpression);
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericMethodDefinition() == EntityMaterializerSource.TryReadValueMethod)
			{
				IProperty property = (IProperty)((ConstantExpression)methodCallExpression.Arguments[2]).Value;
				int index = ((methodCallExpression.Arguments[0] is ProjectionBindingExpression projectionBindingExpression) ? ((IDictionary<IProperty, int>)GetProjectionIndex(projectionBindingExpression)) : _materializationContextBindings[(ParameterExpression)((MethodCallExpression)methodCallExpression.Arguments[0]).Object])[property];
				ProjectionExpression projection = _selectExpression.Projection[index];
				return CreateGetValueExpression(_dbDataReaderParameter, index, IsNullableProjection(projection), property.GetRelationalTypeMapping(), methodCallExpression.Type);
			}
			return base.VisitMethodCall(methodCallExpression);
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (extensionExpression is ProjectionBindingExpression projectionBindingExpression)
			{
				int index = (int)GetProjectionIndex(projectionBindingExpression);
				ProjectionExpression projectionExpression = _selectExpression.Projection[index];
				return CreateGetValueExpression(_dbDataReaderParameter, index, IsNullableProjection(projectionExpression), projectionExpression.Expression.TypeMapping, projectionBindingExpression.Type);
			}
			return base.VisitExtension(extensionExpression);
		}

		private object GetProjectionIndex(ProjectionBindingExpression projectionBindingExpression)
		{
			if (projectionBindingExpression.ProjectionMember == null)
			{
				if (!projectionBindingExpression.Index.HasValue)
				{
					return projectionBindingExpression.IndexMap;
				}
				return projectionBindingExpression.Index;
			}
			return ((ConstantExpression)_selectExpression.GetMappedProjection(projectionBindingExpression.ProjectionMember)).Value;
		}

		private static bool IsNullableProjection(ProjectionExpression projection)
		{
			if (projection.Expression is ColumnExpression columnExpression)
			{
				return columnExpression.IsNullable;
			}
			return true;
		}

		private Expression CreateGetValueExpression(ParameterExpression dbDataReader, int index, bool nullable, RelationalTypeMapping typeMapping, Type clrType)
		{
			MethodInfo dataReaderMethod = typeMapping.GetDataReaderMethod();
			Expression expression = Expression.Constant(index);
			if (_indexMapParameter != null)
			{
				expression = Expression.ArrayIndex(_indexMapParameter, expression);
			}
			Expression expression2 = Expression.Call((dataReaderMethod.DeclaringType != typeof(DbDataReader)) ? ((Expression)Expression.Convert(dbDataReader, dataReaderMethod.DeclaringType)) : ((Expression)dbDataReader), dataReaderMethod, expression);
			if (ProjectionColumns != null)
			{
				Type type = expression2.Type;
				if (!type.IsValueType || !BufferedDataReader.IsSupportedValueType(type))
				{
					type = typeof(object);
					expression2 = Expression.Convert(expression2, typeof(object));
				}
				if (ProjectionColumns[index] == null)
				{
					ProjectionColumns[index] = ReaderColumn.Create(type, nullable, (_indexMapParameter != null) ? ((ColumnExpression)_selectExpression.Projection[index].Expression).Name : null, Expression.Lambda(expression2, dbDataReader, _indexMapParameter ?? Expression.Parameter(typeof(int[]))).Compile());
				}
				if (dataReaderMethod.DeclaringType != typeof(DbDataReader))
				{
					expression2 = Expression.Call(dbDataReader, RelationalTypeMapping.GetDataReaderMethod(type), expression);
				}
			}
			expression2 = typeMapping.CustomizeDataReaderExpression(expression2);
			ValueConverter converter = typeMapping.Converter;
			if (converter != null)
			{
				if (expression2.Type != converter.ProviderClrType)
				{
					expression2 = Expression.Convert(expression2, converter.ProviderClrType);
				}
				expression2 = ReplacingExpressionVisitor.Replace(converter.ConvertFromProviderExpression.Parameters.Single(), expression2, converter.ConvertFromProviderExpression.Body);
			}
			if (expression2.Type != clrType)
			{
				expression2 = Expression.Convert(expression2, clrType);
			}
			if (nullable)
			{
				expression2 = Expression.Condition(Expression.Call(dbDataReader, _isDbNullMethod, expression), Expression.Default(expression2.Type), expression2);
			}
			return expression2;
		}
	}

	private class ShaperExpressionProcessingExpressionVisitor : ExpressionVisitor
	{
		private class CollectionShaperFindingExpressionVisitor : ExpressionVisitor
		{
			private bool _containsCollection;

			public bool ContainsCollectionMaterialization(Expression expression)
			{
				_containsCollection = false;
				Visit(expression);
				return _containsCollection;
			}

			public override Expression Visit(Expression expression)
			{
				if (_containsCollection)
				{
					return expression;
				}
				if (expression is RelationalCollectionShaperExpression)
				{
					_containsCollection = true;
					return expression;
				}
				return base.Visit(expression);
			}
		}

		private static readonly MemberInfo _resultContextValuesMemberInfo = typeof(ResultContext).GetTypeInfo().GetMember("Values")[0];

		private static readonly MemberInfo _resultCoordinatorResultReadyMemberInfo = typeof(ResultCoordinator).GetTypeInfo().GetMember("ResultReady")[0];

		private readonly SelectExpression _selectExpression;

		private readonly ParameterExpression _dataReaderParameter;

		private readonly ParameterExpression _resultContextParameter;

		private readonly ParameterExpression _resultCoordinatorParameter;

		private readonly ParameterExpression _indexMapParameter;

		private readonly IDictionary<Expression, Expression> _mapping = new Dictionary<Expression, Expression>();

		private readonly List<ParameterExpression> _variables = new List<ParameterExpression>();

		private readonly List<Expression> _expressions = new List<Expression>();

		private readonly List<Expression> _includeExpressions = new List<Expression>();

		private List<CollectionPopulatingExpression> _collectionPopulatingExpressions;

		private Expression _valuesArrayExpression;

		private List<Expression> _valuesArrayInitializers;

		private bool _containsCollectionMaterialization;

		public ShaperExpressionProcessingExpressionVisitor(SelectExpression selectExpression, ParameterExpression dataReaderParameter, ParameterExpression resultCoordinatorParameter, ParameterExpression indexMapParameter)
		{
			_selectExpression = selectExpression;
			_dataReaderParameter = dataReaderParameter;
			_resultContextParameter = Expression.Parameter(typeof(ResultContext), "resultContext");
			_resultCoordinatorParameter = resultCoordinatorParameter;
			_indexMapParameter = indexMapParameter;
		}

		public Expression Inject(Expression expression)
		{
			_containsCollectionMaterialization = new CollectionShaperFindingExpressionVisitor().ContainsCollectionMaterialization(expression);
			if (_containsCollectionMaterialization)
			{
				_valuesArrayExpression = Expression.MakeMemberAccess(_resultContextParameter, _resultContextValuesMemberInfo);
				_collectionPopulatingExpressions = new List<CollectionPopulatingExpression>();
				_valuesArrayInitializers = new List<Expression>();
			}
			Expression expression2 = Visit(expression);
			if (_containsCollectionMaterialization)
			{
				BinaryExpression item = Expression.Assign(_valuesArrayExpression, Expression.NewArrayInit(typeof(object), _valuesArrayInitializers));
				_expressions.Add(item);
				_expressions.AddRange(_includeExpressions);
				BlockExpression ifTrue = Expression.Block(_variables, _expressions);
				List<Expression> list = new List<Expression>();
				list.Add(Expression.IfThen(Expression.Equal(_valuesArrayExpression, Expression.Constant(null, typeof(object[]))), ifTrue));
				list.AddRange(_collectionPopulatingExpressions);
				list.Add(Expression.Condition(Expression.IsTrue(Expression.MakeMemberAccess(_resultCoordinatorParameter, _resultCoordinatorResultReadyMemberInfo)), expression2, Expression.Default(expression2.Type)));
				expression2 = Expression.Block(list);
			}
			else
			{
				_expressions.AddRange(_includeExpressions);
				_expressions.Add(expression2);
				expression2 = Expression.Block(_variables, _expressions);
			}
			return ConvertToLambda(expression2);
		}

		private LambdaExpression ConvertToLambda(Expression result)
		{
			if (_indexMapParameter == null)
			{
				return Expression.Lambda(result, QueryCompilationContext.QueryContextParameter, _dataReaderParameter, _resultContextParameter, _resultCoordinatorParameter);
			}
			return Expression.Lambda(result, QueryCompilationContext.QueryContextParameter, _dataReaderParameter, _resultContextParameter, _indexMapParameter, _resultCoordinatorParameter);
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (!(extensionExpression is EntityShaperExpression entityShaperExpression))
			{
				if (!(extensionExpression is ProjectionBindingExpression projectionBindingExpression))
				{
					if (!(extensionExpression is IncludeExpression includeExpression))
					{
						if (extensionExpression is RelationalCollectionShaperExpression relationalCollectionShaperExpression)
						{
							Expression key = GenerateKey(relationalCollectionShaperExpression);
							if (!_mapping.TryGetValue(key, out var value))
							{
								Expression innerShaper = new ShaperExpressionProcessingExpressionVisitor(_selectExpression, _dataReaderParameter, _resultCoordinatorParameter, null).Inject(relationalCollectionShaperExpression.InnerShaper);
								_collectionPopulatingExpressions.Add(new CollectionPopulatingExpression(relationalCollectionShaperExpression.Update(relationalCollectionShaperExpression.ParentIdentifier, relationalCollectionShaperExpression.OuterIdentifier, relationalCollectionShaperExpression.SelfIdentifier, innerShaper), relationalCollectionShaperExpression.Type, include: false));
								ParameterExpression parameterExpression = Expression.Parameter(relationalCollectionShaperExpression.Type);
								_variables.Add(parameterExpression);
								_expressions.Add(Expression.Assign(parameterExpression, new CollectionInitializingExpression(relationalCollectionShaperExpression.CollectionId, null, relationalCollectionShaperExpression.ParentIdentifier, relationalCollectionShaperExpression.OuterIdentifier, relationalCollectionShaperExpression.Navigation, relationalCollectionShaperExpression.Type)));
								_valuesArrayInitializers.Add(parameterExpression);
								value = Expression.Convert(Expression.ArrayIndex(_valuesArrayExpression, Expression.Constant(_valuesArrayInitializers.Count - 1)), relationalCollectionShaperExpression.Type);
								_mapping[key] = value;
							}
							return value;
						}
						return base.VisitExtension(extensionExpression);
					}
					Expression expression = Visit(includeExpression.EntityExpression);
					if (includeExpression.NavigationExpression is RelationalCollectionShaperExpression relationalCollectionShaperExpression2)
					{
						Expression innerShaper2 = new ShaperExpressionProcessingExpressionVisitor(_selectExpression, _dataReaderParameter, _resultCoordinatorParameter, null).Inject(relationalCollectionShaperExpression2.InnerShaper);
						_collectionPopulatingExpressions.Add(new CollectionPopulatingExpression(relationalCollectionShaperExpression2.Update(relationalCollectionShaperExpression2.ParentIdentifier, relationalCollectionShaperExpression2.OuterIdentifier, relationalCollectionShaperExpression2.SelfIdentifier, innerShaper2), includeExpression.Navigation.ClrType, include: true));
						_includeExpressions.Add(new CollectionInitializingExpression(relationalCollectionShaperExpression2.CollectionId, expression, relationalCollectionShaperExpression2.ParentIdentifier, relationalCollectionShaperExpression2.OuterIdentifier, includeExpression.Navigation, includeExpression.Navigation.ClrType));
					}
					else
					{
						_includeExpressions.Add(includeExpression.Update(expression, Visit(includeExpression.NavigationExpression)));
					}
					return expression;
				}
				Expression key2 = GenerateKey(projectionBindingExpression);
				if (_mapping.TryGetValue(key2, out var value2))
				{
					return value2;
				}
				ParameterExpression parameterExpression2 = Expression.Parameter(projectionBindingExpression.Type);
				_variables.Add(parameterExpression2);
				_expressions.Add(Expression.Assign(parameterExpression2, projectionBindingExpression));
				if (_containsCollectionMaterialization)
				{
					Expression expression2 = parameterExpression2;
					if (expression2.Type.IsValueType)
					{
						expression2 = Expression.Convert(expression2, typeof(object));
					}
					_valuesArrayInitializers.Add(expression2);
					value2 = Expression.Convert(Expression.ArrayIndex(_valuesArrayExpression, Expression.Constant(_valuesArrayInitializers.Count - 1)), projectionBindingExpression.Type);
				}
				else
				{
					value2 = parameterExpression2;
				}
				_mapping[key2] = value2;
				return value2;
			}
			Expression key3 = GenerateKey((ProjectionBindingExpression)entityShaperExpression.ValueBufferExpression);
			if (!_mapping.TryGetValue(key3, out var value3))
			{
				ParameterExpression parameterExpression3 = Expression.Parameter(entityShaperExpression.Type);
				_variables.Add(parameterExpression3);
				_expressions.Add(Expression.Assign(parameterExpression3, entityShaperExpression));
				if (_containsCollectionMaterialization)
				{
					_valuesArrayInitializers.Add(parameterExpression3);
					value3 = Expression.Convert(Expression.ArrayIndex(_valuesArrayExpression, Expression.Constant(_valuesArrayInitializers.Count - 1)), entityShaperExpression.Type);
				}
				else
				{
					value3 = parameterExpression3;
				}
				_mapping[key3] = value3;
			}
			return value3;
		}

		private Expression GenerateKey(Expression expression)
		{
			if (!(expression is ProjectionBindingExpression { ProjectionMember: not null } projectionBindingExpression))
			{
				return expression;
			}
			return _selectExpression.GetMappedProjection(projectionBindingExpression.ProjectionMember);
		}
	}

	private readonly Type _contextType;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;

	private readonly ISet<string> _tags;

	private readonly bool _useRelationalNulls;

	protected virtual RelationalShapedQueryCompilingExpressionVisitorDependencies RelationalDependencies { get; }

	public RelationalShapedQueryCompilingExpressionVisitor(ShapedQueryCompilingExpressionVisitorDependencies dependencies, RelationalShapedQueryCompilingExpressionVisitorDependencies relationalDependencies, QueryCompilationContext queryCompilationContext)
		: base(dependencies, queryCompilationContext)
	{
		RelationalDependencies = relationalDependencies;
		_contextType = queryCompilationContext.ContextType;
		_logger = ((AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue21016", out var isEnabled) && isEnabled) ? queryCompilationContext.Logger : null);
		_tags = queryCompilationContext.Tags;
		_useRelationalNulls = RelationalOptionsExtension.Extract(queryCompilationContext.ContextOptions).UseRelationalNulls;
	}

	protected override Expression VisitShapedQueryExpression(ShapedQueryExpression shapedQueryExpression)
	{
		SelectExpression selectExpression = (SelectExpression)shapedQueryExpression.QueryExpression;
		selectExpression.ApplyTags(_tags);
		ParameterExpression parameterExpression = Expression.Parameter(typeof(DbDataReader), "dataReader");
		ParameterExpression resultCoordinatorParameter = Expression.Parameter(typeof(ResultCoordinator), "resultCoordinator");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(int[]), "indexMap");
		Expression expression = new ShaperExpressionProcessingExpressionVisitor(selectExpression, parameterExpression, resultCoordinatorParameter, parameterExpression2).Inject(shapedQueryExpression.ShaperExpression);
		expression = InjectEntityMaterializers(expression);
		bool flag = selectExpression.IsNonComposedFromSql();
		expression = new RelationalProjectionBindingRemovingExpressionVisitor(selectExpression, parameterExpression, flag ? parameterExpression2 : null, IsBuffering).Visit(expression, out var projectionColumns);
		expression = new CustomShaperCompilingExpressionVisitor(parameterExpression, resultCoordinatorParameter, IsTracking).Visit(expression);
		IReadOnlyList<string> value = null;
		if (flag)
		{
			value = selectExpression.Projection.Select((ProjectionExpression pe) => ((ColumnExpression)pe.Expression).Name).ToList();
		}
		RelationalCommandCache value2 = new RelationalCommandCache(Dependencies.MemoryCache, RelationalDependencies.SqlExpressionFactory, RelationalDependencies.ParameterNameGeneratorFactory, RelationalDependencies.QuerySqlGeneratorFactory, _useRelationalNulls, selectExpression);
		LambdaExpression lambdaExpression = (LambdaExpression)expression;
		return Expression.New(typeof(QueryingEnumerable<>).MakeGenericType(lambdaExpression.ReturnType).GetConstructors()[0], Expression.Convert(QueryCompilationContext.QueryContextParameter, typeof(RelationalQueryContext)), Expression.Constant(value2), Expression.Constant(value, typeof(IReadOnlyList<string>)), Expression.Constant(projectionColumns, typeof(IReadOnlyList<ReaderColumn>)), Expression.Constant(lambdaExpression.Compile()), Expression.Constant(_contextType), Expression.Constant(_logger, typeof(IDiagnosticsLogger<DbLoggerCategory.Query>)));
	}
}
