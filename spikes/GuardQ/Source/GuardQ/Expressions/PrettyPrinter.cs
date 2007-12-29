using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GuardQ.Expressions
{
    public class PrettyPrinter 
    {
        public string Print(Expression exp)
        {
            return Print(new StringBuilder(), exp);
        }

        public string Print(StringBuilder buffer, Expression exp)
        {
            return new PrettyPrinterVisitor(buffer, exp).Output;
        }
    }
        
    internal class PrettyPrinterVisitor : ExpressionVisitor
    {
        StringBuilder buffer;

        public PrettyPrinterVisitor(StringBuilder buffer, Expression exp)
        {
            this.buffer = buffer;
            Visit(exp);
        }

        public string Output { get { return buffer.ToString(); } }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            var r = base.VisitMemberAccess(m);
            if (m.Expression != null && !IsContextExpression(m.Expression))
                buffer.Append(".");
            buffer.Append(m.Member.Name);
            return r;
        }

        private bool IsContextExpression(Expression expression)
        {
            return
                expression.NodeType == ExpressionType.Constant &&
                expression.Type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length > 0;
        }
    }
}
