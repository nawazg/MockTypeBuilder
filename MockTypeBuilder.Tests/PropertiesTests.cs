using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockTypeBuilder.Tests.Models;
using FluentAssertions;

namespace MockTypeBuilder.Tests
{
    [TestClass]
    public class PropertiesTests
    {
        [TestMethod]
        public void CanUpdateSingleObjectProperty()
        {
            var builder = new TypeBuilder<Person>();
            builder.CreateSingle()
                .WithProperty("Name", "Test name")
                .WithProperty("Id", 45);

            Person person = builder.BuildSingle();

            person.Id.Should().Be(45);
            person.Name.Should().Be("Test name");
        }

        [TestMethod]
        public void CanUpdateMultipleObjectProperties()
        {
            var builder = new TypeBuilder<Person>();
            builder.CreateMultiple(2)
                .WithProperty("Name", "Test name")
                .WithProperty("Id", 45);

            List<Person> persons = builder.BuildList();

            foreach (Person person in persons)
            {
                person.Id.Should().Be(45);
                person.Name.Should().Be("Test name");
            }
        }

        [TestMethod]
        public void CanUpdateMultipleObjectPropertiesWithSpecificIndexes()
        {
            var builder = new TypeBuilder<Person>();
            builder.CreateMultiple(5)
                .WithProperty("Id", 456345223, new List<int> { 1, 2, 4 });

            List<Person> persons = builder.BuildList();

            for (var i = 0; i < persons.Count; i++)
            {
                if (i == 1 || i == 2 || i == 4)
                {
                    persons[i].Id.Should().Be(456345223);
                }
                else
                {
                    persons[i].Id.Should().NotBe(456345223);
                }
            }
        }

        [TestMethod]
        public void CanUpdateDifferentValueTypes()
        {
            const int integerItem = 123;
            const string stringItem = "xyz";
            DateTime dateTimeItem = new DateTime().AddDays(4d);

            var builder = new TypeBuilder<Person>();
            builder.CreateSingle()
                .WithProperty("Name", stringItem)
                .WithProperty("Id", integerItem)
                .WithProperty("JoinedDate", dateTimeItem);

            Person person = builder.BuildSingle();

            person.Name.Should().Be(stringItem);
            person.Id.Should().Be(integerItem);
            person.JoinedDate.Should().Be(dateTimeItem);
        }
    }
}
