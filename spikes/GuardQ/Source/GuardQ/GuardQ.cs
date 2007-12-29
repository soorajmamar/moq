using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuardQ.Expressions;
using System.Linq.Expressions;

namespace GuardQ
{
    public sealed class GuardQ
    {
        public static void ArgumentNotNull<TResult>(Expression<Func<TResult>> expression)
        {
            if (expression.Compile().Invoke() == null)
            {
                PrettyPrinter printer = new PrettyPrinter();
                string argName = printer.Print(expression.Body);
                throw new ArgumentNullException(argName);
            }
        }
    }
}
