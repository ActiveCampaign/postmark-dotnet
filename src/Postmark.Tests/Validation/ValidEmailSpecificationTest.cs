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
        [TestCase("example@example.com")]
        [TestCase("John Smith <john@johnsmith.com>")]
        [TestCase("Jonh_Smith/SomeCoolDude/With%Some%Characters@someschool.EDU")]
        [TestCase("firstname.lastname@somewhere.else.in.time")]
        [TestCase("john@aol.com.nospam")]
        [TestCase("john@server.department.company.com")]
        public void IsSatisfiedBy_should_validate_emails_correctly(string email)
        {
            // Arrange
            var emailSpecification = new ValidEmailSpecification();


            // Act
            var result = emailSpecification.IsSatisfiedBy(email);

            // Assert
            Assert.AreEqual(true, result);
        }
    }
}
