using System.Collections.Generic;
using System.Dynamic;
using flx.SILQ.Errors;
using flx.SILQ.Models;

namespace flx.SILQ.Tests.Models
{
    [TestClass]
    public class SelectObjectTests
    {
        [TestMethod]
        public void Constructor_WithDictionary_SetsProperties()
        {
            // Arrange
            var dict = new Dictionary<string, object> { { "Name", "Alice" }, { "Age", 30 } };

            // Act
            dynamic obj = new SelectObject(dict);

            // Assert
            Assert.AreEqual("Alice", obj.Name);
            Assert.AreEqual(30, obj.Age);
        }

        [TestMethod]
        public void Constructor_WithSingleProperty_SetsProperty()
        {
            // Arrange
            var name = "City";
            var value = "London";

            // Act
            dynamic obj = new SelectObject(name, value);

            // Assert
            Assert.AreEqual("London", obj.City);
        }

        [TestMethod]
        public void TrySetMember_AddsOrUpdatesProperty()
        {
            // Arrange
            var name = "Country";
            var value = "UK";

            // Act
            dynamic obj = new SelectObject(name, value);
            obj.Country = "Germany";

            // Assert
            Assert.AreEqual("Germany", obj.Country);
        }

        [TestMethod]
        public void TryGetMember_ReturnsNullIfNotFound()
        {
            // Arrange
            dynamic obj = new SelectObject("Foo", 123);
            object value = null;
            var binder = new TestGetMemberBinder("Bar");

            // Act
            bool found = ((DynamicObject)obj).TryGetMember(binder, out value);

            // Assert
            Assert.IsFalse(found);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void TryGetMember_ReturnsValueIfFound()
        {
            // Arrange
            var dict = new Dictionary<string, object> { { "Name", "Alice" }, { "Age", 30 } };
            dynamic obj = new SelectObject(dict);
            object value = null;
            var binder = new TestGetMemberBinder("Name");

            // Act
            bool found = ((DynamicObject)obj).TryGetMember(binder, out value);

            // Assert
            Assert.IsTrue(found);
            Assert.AreEqual("Alice", value);
        }

                [TestMethod]
        public void GetMemeber_ReturnsValue_WhenPropertyExists()
        {
            // Arrange
            var dict = new Dictionary<string, object> { { "Foo", 42 } };
            var obj = new SelectObject(dict);

            // Act
            var value = obj.GetMemeber("Foo");

            // Assert
            Assert.AreEqual(42, value);
        }

        [TestMethod]
        public void GetMemeber_Throws_WhenNameIsNullOrEmptyOrWhitespace()
        {
            // Arrange
            var obj = new SelectObject("Bar", 123);

            // Act & Assert
            Assert.ThrowsException<RuntimeError>(() => obj.GetMemeber(null));
            Assert.ThrowsException<RuntimeError>(() => obj.GetMemeber(""));
            Assert.ThrowsException<RuntimeError>(() => obj.GetMemeber("   "));
        }

        [TestMethod]
        public void GetMemeber_Throws_WhenNameContainsDot()
        {
            // Arrange
            var obj = new SelectObject("Baz", 1);

            // Act & Assert
            Assert.ThrowsException<RuntimeError>(() => obj.GetMemeber("Baz.Qux"));
        }

        [TestMethod]
        public void GetMemeber_Throws_WhenPropertyNotFound()
        {
            // Arrange
            var obj = new SelectObject("X", 99);

            // Act & Assert
            Assert.ThrowsException<RuntimeError>(() => obj.GetMemeber("Y"));
        }

        // Helper binder for testing TryGetMember
        private class TestGetMemberBinder : GetMemberBinder
        {
            public TestGetMemberBinder(string name) : base(name, false) { }
            public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion) => null;
        }
    }
}
