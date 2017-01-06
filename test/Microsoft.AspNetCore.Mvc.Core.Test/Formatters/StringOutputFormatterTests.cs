// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.Formatters
{
    public class StringOutputFormatterTests
    {
        public static IEnumerable<object[]> CanWriteResultForStringTypesData
        {
            get
            {
                // object value, bool useDeclaredTypeAsString
                yield return new object[] { "declared and runtime type are same", true };
                yield return new object[] { "declared and runtime type are different", false };
                yield return new object[] { null, true };
            }
        }

        public static IEnumerable<object[]> CannotWriteResultForNonStringTypesData
        {
            get
            {
                // object value, bool useDeclaredTypeAsString
                yield return new object[] { null, false };
                yield return new object[] { new object(), false };
            }
        }

        [Fact]
        public void CannotWriteResult_ForNonTextPlainOrNonBrowserMediaTypes()
        {
            // Arrange
            var formatter = new StringOutputFormatter();
            var expectedContentType = new StringSegment("application/json");

            var context = new OutputFormatterWriteContext(
                new DefaultHttpContext(),
                new TestHttpResponseStreamWriterFactory().CreateWriter,
                typeof(string),
                "Thisisastring");
            context.ContentType = expectedContentType;

            // Act
            var result = formatter.CanWriteResult(context);

            // Assert
            Assert.False(result);
            Assert.Equal(expectedContentType, context.ContentType);
        }

        [Fact]
        public void CanWriteResult_DefaultContentType()
        {
            // Arrange
            var formatter = new StringOutputFormatter();
            var context = new OutputFormatterWriteContext(
                new DefaultHttpContext(),
                new TestHttpResponseStreamWriterFactory().CreateWriter,
                typeof(string),
                "Thisisastring");

            // For example, this can happen when a request is received without any Accept header OR a request
            // is from a browser (in which case this ContentType is set to null by the infrastructure when
            // RespectBrowserAcceptHeader is set to false)
            context.ContentType = new StringSegment();

            // Act
            var result = formatter.CanWriteResult(context);

            // Assert
            Assert.True(result);
            Assert.Equal(new StringSegment("text/plain; charset=utf-8"), context.ContentType);
        }

        [Theory]
        [MemberData(nameof(CanWriteResultForStringTypesData))]
        public void CanWriteResult_ForStringTypes(
            object value,
            bool useDeclaredTypeAsString)
        {
            // Arrange
            var expectedContentType = new StringSegment("text/plain; charset=utf-8");

            var formatter = new StringOutputFormatter();
            var type = useDeclaredTypeAsString ? typeof(string) : typeof(object);

            var context = new OutputFormatterWriteContext(
                new DefaultHttpContext(),
                new TestHttpResponseStreamWriterFactory().CreateWriter,
                type,
                value);
            context.ContentType = new StringSegment("text/plain");

            // Act
            var result = formatter.CanWriteResult(context);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedContentType, context.ContentType);
        }

        [Theory]
        [MemberData(nameof(CannotWriteResultForNonStringTypesData))]
        public void CannotWriteResult_ForNonStringTypes(
            object value,
            bool useDeclaredTypeAsString)
        {
            // Arrange
            var expectedContentType = new StringSegment("text/plain; charset=utf-8");

            var formatter = new StringOutputFormatter();
            var type = useDeclaredTypeAsString ? typeof(string) : typeof(object);

            var context = new OutputFormatterWriteContext(
                new DefaultHttpContext(),
                new TestHttpResponseStreamWriterFactory().CreateWriter,
                type,
                value);
            context.ContentType = new StringSegment("text/plain");

            // Act
            var result = formatter.CanWriteResult(context);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task WriteAsync_DoesNotWriteNullStrings()
        {
            // Arrange
            Encoding encoding = Encoding.UTF8;
            var memoryStream = new MemoryStream();
            var response = new Mock<HttpResponse>();
            response.SetupProperty(o => o.ContentLength);
            response.SetupGet(r => r.Body).Returns(memoryStream);
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(o => o.Response).Returns(response.Object);

            var formatter = new StringOutputFormatter();
            var context = new OutputFormatterWriteContext(
                httpContext.Object,
                new TestHttpResponseStreamWriterFactory().CreateWriter,
                typeof(string),
                @object: null);

            // Act
            await formatter.WriteResponseBodyAsync(context, encoding);

            // Assert
            Assert.Equal(0, memoryStream.Length);
            response.VerifySet(r => r.ContentLength = It.IsAny<long?>(), Times.Never());
        }
    }
}
