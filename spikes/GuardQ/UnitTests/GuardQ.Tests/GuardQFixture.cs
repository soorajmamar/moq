using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GuardQ.Tests
{
    [TestFixture]
    public class GuardQFixture
    {
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: x")]
        [Test]
        public void ShouldThrowIfArgumentIsNull()
        {
            object x = null;
            GuardQ.ArgumentNotNull(() => x);
        }

        class P { public string m; }
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: a.m")]
        [Test]
        public void ShouldThrowIfExpressionIsNull()
        {
            P a = new P() { m = null };
            GuardQ.ArgumentNotNull(() => a.m);
        }
    }
}
