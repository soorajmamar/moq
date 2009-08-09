using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQToolkit;
using System.Linq.Expressions;

namespace Moq.Linq
{
	internal class FluentSetupRewriter : ExpressionVisitor
	{
		Mock mock;
		Expression fluentSetup;
		Stack<Mock> fluentMocks;

		public FluentSetupRewriter(Mock mock, Expression fluentSetup)
		{
			this.mock = mock;
			this.fluentSetup = fluentSetup;
		}

		public static Expression Rewrite(Mock mock, Expression fluentSetup)
		{
			return new FluentSetupRewriter(mock, fluentSetup).Rewrite();
		}

		private Expression Rewrite()
		{
			return Visit(fluentSetup);
		}


	}
}
