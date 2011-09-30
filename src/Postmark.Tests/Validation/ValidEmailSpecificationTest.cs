// -----------------------------------------------------------------------
// <copyright file="ValidEmailSpecificationTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Postmark.Tests.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using PostmarkDotNet.Validation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class ValidEmailSpecificationTest
    {
        [TestCase("example@example.com", true)]
        [TestCase("John Smith <john@johnsmith.com>", true)]
        [TestCase("Jonh_Smith/SomeCoolDude/With%Some%Characters@someschool.EDU", true)]
        [TestCase("firstname.lastname@somewhere.else.in.time", true)]
        [TestCase("john@aol.com.nospam", true)]
        [TestCase("john@server.department.company.com", true)]
        [TestCase("test@test.com", true)]
        [TestCase("test+one@test.com", true)]
        [TestCase("test @test.com", false)]
        [TestCase("testtest.com", false)]
        [TestCase(" test@test.com", false)]
        [TestCase("test@test.com ", false)]
        [TestCase(" test@test.com ", false)]
        [TestCase("test@test.com/", false)]
        [TestCase("test@test.com\\", false)]
        [TestCase("test@test.com'", false)]
        [TestCase("test.test@test", false)]
        [TestCase("test.test@test.com'", false)]
        [TestCase("test.test@te\\st.com'", false)]
        public void IsSatisfiedBy_should_validate_emails_correctly(string email, bool expectedResult)
        {
            // Arrange
            var emailSpecification = new ValidEmailSpecification();


            // Act
            var result = emailSpecification.IsSatisfiedBy(email);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
