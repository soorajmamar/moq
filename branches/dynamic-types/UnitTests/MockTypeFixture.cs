using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;

namespace Moq.Tests
{
    [TestFixture]
    public class MockTypeFixture
    {
        [Test]
        public void ShouldBeAssignable()
        {
            var mock = new Mock<IBar>();
            Assert.IsTrue(typeof(IBar).IsAssignableFrom(mock.Object.GetType()));
        }

        [Test]
        public void ShouldBeClassType()
        {
            var mock = new Mock<IBar>();
            Assert.IsTrue(mock.Object.GetType().IsClass);
            Debug.WriteLine(mock.Object.GetType().AssemblyQualifiedName);
            Debug.WriteLine(mock.Object.GetType());
        }

        [Test]
        public void ManyMocksShouldBeOfSameType()
        {
            var mock1 = new Mock<IBar>();
            var mock2 = new Mock<IBar>();
            Assert.AreEqual(mock1.Object.GetType(), mock2.Object.GetType());
        }

        interface IInnerBar { }
        class C { public interface IInnerBar { } }
        
        [Test]
        public void ShouldAssignDifferentMockTypesToDifferentInterfaces()
        {
            var mock1 = new Mock<IInnerBar>();
            Debug.WriteLine(mock1.Object.GetType());
            
            var mock2 = new Mock<C.IInnerBar>();
            Debug.WriteLine(mock2.Object.GetType());

            Assert.AreNotEqual(mock2.Object.GetType(),mock1.Object.GetType());
        }

        interface GenericService<T> { }

        [Test]
        public void ShouldMockGenericInterfaces()
        {
            var mock = new Mock<GenericService<string>>();
            var type = mock.Object.GetType();
            Debug.WriteLine(type.FullName);
        }

        [Test]
        public void ShouldMockManyGenericInterfaces()
        {
            var mock1 = new Mock<GenericService<string>>();
            var type1 = mock1.Object.GetType();
            Debug.WriteLine(type1.FullName);

            var mock2 = new Mock<GenericService<int>>();
            var type2 = mock2.Object.GetType();
            Debug.WriteLine(type2.FullName);
        }
    }

    interface IBar { };
}
