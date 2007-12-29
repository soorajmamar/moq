using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GuardQ.Tests.OldGuardDemo
{
    [TestFixture]
    public class Demo
    {
        public class Account { }

        public class Bank
        {
            public void Transfer(Account source, Account destination, decimal amount)
            {
                Guard.ArgumentNotNull(source, "source");
                Guard.ArgumentNotNull(destination, "destination");
                // TODO ... actual founds transfer
            }
        }

        [ExpectedException(
            typeof(ArgumentNullException),
            ExpectedMessage = "Value cannot be null.\r\nParameter name: source")
        ]
        [Test]
        public void ShouldThrowIfSourceIsNull()
        {
            new Bank().Transfer(null, new Account(), 10m);
        }

        [ExpectedException(
            typeof(ArgumentNullException),
            ExpectedMessage = "Value cannot be null.\r\nParameter name: destination")
        ]
        [Test]
        public void ShouldThrowIfDestinationIsNull()
        {
            new Bank().Transfer(new Account(), null, 100m);
        }
    }
}
