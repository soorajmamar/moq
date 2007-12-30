using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

public static class Reflector<TTarget>
{
	public static MethodInfo GetMethod(Expression<Action<TTarget>> method)
	{
		return GetMethodInfo(method);
	}

	public static MethodInfo GetMethod<T1>(Expression<Action<TTarget, T1>> method)
	{
		return GetMethodInfo(method);
	}
	
	public static MethodInfo GetMethod<T1, T2>(Expression<Action<TTarget, T1, T2>> method)
	{
		return GetMethodInfo(method);
	}
	
	public static MethodInfo GetMethod<T1, T2, T3>(Expression<Action<TTarget, T1, T2, T3>> method)
	{
		return GetMethodInfo(method);
	}

	private static MethodInfo GetMethodInfo(Expression method)
	{
		LambdaExpression lambda = method as LambdaExpression;
		if (lambda == null) throw new ArgumentNullException("Not a lambda expression", "method");
		if (lambda.Body.NodeType != ExpressionType.Call) throw new ArgumentException("Not a method call", "method");

		return ((MethodCallExpression)lambda.Body).Method;
	}

	public static PropertyInfo GetProperty(Expression<Func<TTarget, object>> property)
	{
		PropertyInfo info = GetMemberInfo(property) as PropertyInfo;
		if (info == null) throw new ArgumentException("Member is not a property");

		return info;
	}

	public static FieldInfo GetField(Expression<Func<TTarget, object>> field)
	{
		FieldInfo info = GetMemberInfo(field) as FieldInfo;
		if (info == null) throw new ArgumentException("Member is not a field");

		return info;
	}

	private static MemberInfo GetMemberInfo(Expression member)
	{
		LambdaExpression lambda = member as LambdaExpression;
		if (lambda == null) throw new ArgumentNullException("member");

		MemberExpression memberExpr = null;

		// The Func<TTarget, object> we use returns an object, so first statement can be either 
		// a cast (if the field/property does not return an object) or the direct member access.
		if (lambda.Body.NodeType == ExpressionType.Convert)
		{
			// The cast is an unary expression, where the operand is the 
			// actual member access expression.
			memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
		}
		else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
		{
			memberExpr = lambda.Body as MemberExpression;
		}

		if (memberExpr == null) throw new ArgumentException("Not a member access", "member");

		return memberExpr.Member;
	}
}