using System;
using NUnit.Framework;

namespace Moq.Tests
{
	[TestFixture]
	public class MockEventsFixture
	{
		[Test]
		public void ShouldExpectAddHandler()
		{
			var mock = new Mock<IFoo>();

			var handler = mock.CreateHandler<FooArgs>();

			mock.Object.DoingFoo += handler;

			var driver = new Driver(mock.Object);
			bool fired = true;

			driver.Fired += (sender, args) =>
				{
					fired = true;
					Assert.IsTrue(args is FooArgs);
					Assert.AreEqual("foo", ((FooArgs)args).Value);
				};

			handler.Raise(new FooArgs { Value = "foo" });
		}

		public class Driver
		{
			public event EventHandler Fired;

			public Driver(IFoo foo)
			{
				foo.DoingFoo += (sender, args) => Fired(sender, args);
			}
		}

		public class FooArgs : EventArgs
		{
			public string Value { get; set; }
		}

		public interface IFoo
		{
			event EventHandler<FooArgs> DoingFoo;
		}
	}
}
