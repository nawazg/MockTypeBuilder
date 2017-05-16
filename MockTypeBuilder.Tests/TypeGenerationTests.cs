using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockTypeBuilder.Tests.Models;
using FluentAssertions;
namespace MockTypeBuilder.Tests
{
    [TestClass]
    public class TypeGenerationTests
    {
        [TestMethod]
        public void CanCreateDefaultSingleItem()
        {
            var builder = new TypeBuilder<Person>();

            Person person = builder.BuildSingle();

            person.Should().NotBeNull();
        }

        [TestMethod]
        public void CanCreateDefaultList()
        {
            var builder = new TypeBuilder<Person>();

            List<Person> persons= builder.BuildList();

            persons.Should().NotBeNull();
            persons.Should().NotBeEmpty();
        }

        [TestMethod]
        public void CanCreateSingleItem()
        {
            var builder = new TypeBuilder<Person>();

            builder.CreateSingle();
            Person person = builder.BuildSingle();

            person.Should().NotBeNull();
        }

        [TestMethod]
        public void CanChainCreateSingleToCreateAList()
        {
            var builder = new TypeBuilder<Person>();

            builder.CreateSingle();
            builder.CreateSingle();
            builder.CreateSingle();

            List<Person> persons = builder.BuildList();

            persons.Should().NotBeNull();
            persons.Count.Should().Be(3);
        }

        [TestMethod]
        public void CanCreateListOfItemsInBulk()
        {
            var builder = new TypeBuilder<Person>();

            builder.CreateMultiple(10);
            List<Person> persons = builder.BuildList();

            persons.Should().NotBeNull();
            persons.Count.Should().Be(10);
        }
    }
}
