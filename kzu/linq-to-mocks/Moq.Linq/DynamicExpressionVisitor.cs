using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace Moq.Linq
{
	/// <summary>
	/// Helper visitor class that can be constructed using lambdas for the actual 
	/// visit overrides.
	/// </summary>
	public class DynamicExpressionVisitor : IQToolkit.ExpressionVisitor
	{
		public Func<BinaryExpression, Expression> VisitBinaryHandler { get; set; }
		public Func<MemberBinding, MemberBinding> VisitBindingHandler { get; set; }
		public Func<ReadOnlyCollection<MemberBinding>, IEnumerable<MemberBinding>> VisitBindingListHandler { get; set; }
		public Func<ConditionalExpression, Expression> VisitConditionalHandler { get; set; }
		public Func<ConstantExpression, Expression> VisitConstantHandler { get; set; }
		public Func<ElementInit, ElementInit> VisitElementInitializerHandler { get; set; }
		public Func<ReadOnlyCollection<ElementInit>, IEnumerable<ElementInit>> VisitElementInitializerListHandler { get; set; }
		public Func<ReadOnlyCollection<Expression>, ReadOnlyCollection<Expression>> VisitExpressionListHandler { get; set; }
		public Func<InvocationExpression, Expression> VisitInvocationHandler { get; set; }
		public Func<LambdaExpression, Expression> VisitLambdaHandler { get; set; }
		public Func<ListInitExpression, Expression> VisitListInitHandler { get; set; }
		public Func<MemberExpression, Expression> VisitMemberAccessHandler { get; set; }
		public Func<MemberAssignment, MemberAssignment> VisitMemberAssignmentHandler { get; set; }
		public Func<MemberInitExpression, Expression> VisitMemberInitHandler { get; set; }
		public Func<MemberListBinding, MemberListBinding> VisitMemberListBindingHandler { get; set; }
		public Func<MemberMemberBinding, MemberMemberBinding> VisitMemberMemberBindingHandler { get; set; }
		public Func<MethodCallExpression, Expression> VisitMethodCallHandler { get; set; }
		public Func<NewExpression, NewExpression> VisitNewHandler { get; set; }
		public Func<NewArrayExpression, Expression> VisitNewArrayHandler { get; set; }
		public Func<ParameterExpression, Expression> VisitParameterHandler { get; set; }
		public Func<TypeBinaryExpression, Expression> VisitTypeIsHandler { get; set; }
		public Func<UnaryExpression, Expression> VisitUnaryHandler { get; set; }

		public new Expression Visit(Expression expression)
		{
			return base.Visit(expression);
		}

		protected override Expression VisitBinary(BinaryExpression expression)
		{
			if (VisitBinaryHandler != null)
				return VisitBinaryHandler(expression);
			else
				return base.VisitBinary(expression);
		}

		protected override MemberBinding VisitBinding(MemberBinding expression)
		{
			if (VisitBindingHandler != null)
				return VisitBindingHandler(expression);
			else
				return base.VisitBinding(expression);
		}

		protected override IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> expression)
		{
			if (VisitBindingListHandler != null)
				return VisitBindingListHandler(expression);
			else
				return base.VisitBindingList(expression);
		}

		protected override Expression VisitConditional(ConditionalExpression expression)
		{
			if (VisitConditionalHandler != null)
				return VisitConditionalHandler(expression);
			else
				return base.VisitConditional(expression);
		}

		protected override Expression VisitConstant(ConstantExpression expression)
		{
			if (VisitConstantHandler != null)
				return VisitConstantHandler(expression);
			else
				return base.VisitConstant(expression);
		}

		protected override ElementInit VisitElementInitializer(ElementInit expression)
		{
			if (VisitElementInitializerHandler != null)
				return VisitElementInitializerHandler(expression);
			else
				return base.VisitElementInitializer(expression);
		}

		protected override IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> expression)
		{
			if (VisitElementInitializerListHandler != null)
				return VisitElementInitializerListHandler(expression);
			else
				return base.VisitElementInitializerList(expression);
		}

		protected override ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> expression)
		{
			if (VisitExpressionListHandler != null)
				return VisitExpressionListHandler(expression);
			else
				return base.VisitExpressionList(expression);
		}

		protected override Expression VisitInvocation(InvocationExpression expression)
		{
			if (VisitInvocationHandler != null)
				return VisitInvocationHandler(expression);
			else
				return base.VisitInvocation(expression);
		}

		protected override Expression VisitLambda(LambdaExpression expression)
		{
			if (VisitLambdaHandler != null)
				return VisitLambdaHandler(expression);
			else
				return base.VisitLambda(expression);
		}

		protected override Expression VisitListInit(ListInitExpression expression)
		{
			if (VisitListInitHandler != null)
				return VisitListInitHandler(expression);
			else
				return base.VisitListInit(expression);
		}

		protected override Expression VisitMemberAccess(MemberExpression expression)
		{
			if (VisitMemberAccessHandler != null)
				return VisitMemberAccessHandler(expression);
			else
				return base.VisitMemberAccess(expression);
		}

		protected override MemberAssignment VisitMemberAssignment(MemberAssignment expression)
		{
			if (VisitMemberAssignmentHandler != null)
				return VisitMemberAssignmentHandler(expression);
			else
				return base.VisitMemberAssignment(expression);
		}

		protected override Expression VisitMemberInit(MemberInitExpression expression)
		{
			if (VisitMemberInitHandler != null)
				return VisitMemberInitHandler(expression);
			else
				return base.VisitMemberInit(expression);
		}

		protected override MemberListBinding VisitMemberListBinding(MemberListBinding expression)
		{
			if (VisitMemberListBindingHandler != null)
				return VisitMemberListBindingHandler(expression);
			else
				return base.VisitMemberListBinding(expression);
		}

		protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding expression)
		{
			if (VisitMemberMemberBindingHandler != null)
				return VisitMemberMemberBindingHandler(expression);
			else
				return base.VisitMemberMemberBinding(expression);
		}

		protected override Expression VisitMethodCall(MethodCallExpression expression)
		{
			if (VisitMethodCallHandler != null)
				return VisitMethodCallHandler(expression);
			else
				return base.VisitMethodCall(expression);
		}

		protected override NewExpression VisitNew(NewExpression expression)
		{
			if (VisitNewHandler != null)
				return VisitNewHandler(expression);
			else
				return base.VisitNew(expression);
		}

		protected override Expression VisitNewArray(NewArrayExpression expression)
		{
			if (VisitNewArrayHandler != null)
				return VisitNewArrayHandler(expression);
			else
				return base.VisitNewArray(expression);
		}

		protected override Expression VisitParameter(ParameterExpression expression)
		{
			if (VisitParameterHandler != null)
				return VisitParameterHandler(expression);
			else
				return base.VisitParameter(expression);
		}

		protected override Expression VisitTypeIs(TypeBinaryExpression expression)
		{
			if (VisitTypeIsHandler != null)
				return VisitTypeIsHandler(expression);
			else
				return base.VisitTypeIs(expression);
		}

		protected override Expression VisitUnary(UnaryExpression expression)
		{
			if (VisitUnaryHandler != null)
				return VisitUnaryHandler(expression);
			else
				return base.VisitUnary(expression);
		}
	}
}
