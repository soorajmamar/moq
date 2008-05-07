using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Moq.Tests
{
	public class ExpectationConstraintsFixture
	{
		public interface IFoo
		{
			string Bar(int a);
		}

		public class Whitness
		{
			private bool allowed;
			private bool allowedCalled;
			public bool Allowed
			{
				get
				{
					allowedCalled = true;
					return allowed;
				}
				set
				{
					allowed = value;
					allowedCalled = false;
				}
			}

			public bool AllowedCalled
			{
				get { return allowedCalled; }
			}
		}

		[Fact]
		public void ShouldUseAllowedExpectation()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict, ExpectationPolicy.NotOverride);
			var whitness = new Whitness();
			whitness.Allowed = true;
			mock.Expect(x => x.Bar(It.IsAny<int>()))
				.When(() => whitness.Allowed)
				.Returns("a");

			var r = mock.Object.Bar(4);
			Assert.Equal("a", r);
			Assert.True(whitness.AllowedCalled);
		}

		[Fact]
		public void ShouldNotUseFirstDisabledExpectation()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict, ExpectationPolicy.NotOverride);
			mock.Expect(x => x.Bar(It.IsAny<int>()))
				.When(() => false)
				.Returns("a");
			mock.Expect(x => x.Bar(It.IsAny<int>()))
				.Returns("b");

			var r = mock.Object.Bar(4);
			Assert.Equal("b", r);
		}

		[Fact]
		public void ShouldNotUseLastDisabledExpectation()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict, ExpectationPolicy.NotOverride);
			mock.Expect(x => x.Bar(It.IsAny<int>()))
				.Returns("b");
			mock.Expect(x => x.Bar(It.IsAny<int>()))
				.When(() => false)
				.Returns("a");

			var r = mock.Object.Bar(4);
			Assert.Equal("b", r);
		}
	}
}
