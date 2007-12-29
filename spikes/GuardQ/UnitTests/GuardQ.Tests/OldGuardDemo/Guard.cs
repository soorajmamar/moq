using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuardQ.Tests.OldGuardDemo
{
    public sealed class Guard
    {
        public static void ArgumentNotNull(object val, string argName)
        {
            if (val == null)
                throw new ArgumentNullException(argName);
        }
    }
}
