using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Moq.Tests
{
    [TestFixture]
    public class MockedFixture
    {
        public interface IFoo { }

        [Test]
		public void MockOfMockedInterfaceShouldReturnSame()
        {
			Mock<IFoo> mock = new Mock<IFoo>();
			IFoo mocked = mock.Object;
			Assert.AreSame(mock, Mock.Get(mocked));
        }

        public class Foo { }

        [Test]
        public void MockOfMockedClassShouldReturnSame()
        {
            Mock<Foo> mock = new Mock<Foo>();
            Foo mocked = mock.Object;
			Assert.AreSame(mock, Mock.Get(mocked));
        }

		public class FooWithCtor 
		{
			public FooWithCtor(int a) { }
		}

		[Test]
		public void MockOfMockedClassWithCtorShouldReturnSame()
		{
			Mock<FooWithCtor> mock = new Mock<FooWithCtor>(5);
			FooWithCtor mocked = mock.Object;
			Assert.AreSame(mock, Mock.Get(mocked));
		}

		public class FooMBR : MarshalByRefObject { }

		[Test]
		public void MockOfMockedClassMBRShouldReturnSame()
		{
			Mock<FooMBR> mock = new Mock<FooMBR>();
			FooMBR mocked = mock.Object;
			Assert.AreSame(mock, Mock.Get(mocked));
		}		

		[Test]
		[ExpectedException(ExceptionType = typeof(ArgumentException), ExpectedMessage = "Not instantiated from MoQ.\r\nParameter name: mocked")]
		public void GetThrowsIfObjectIsNotMocked()
		{
			Mock.Get("foo");
		}

		public class FooOverridesHash
		{
			public int Var { get; set; }
			
			public override int GetHashCode()
			{
				return Var;
			}

			public override bool Equals(object obj)
			{
				return (obj is FooOverridesHash) && ( obj as FooOverridesHash).Var == Var;
			}
		}

		[Test]
		public void MockOfMockedClassShouldWorkIfHashCodeChange()
		{
			Mock<FooOverridesHash> mock = new Mock<FooOverridesHash>();
			FooOverridesHash mocked = mock.Object;
			mocked.Var = mocked.Var + 1;
			Assert.AreSame(mock, Mock.Get(mocked));
		}
    }
}
