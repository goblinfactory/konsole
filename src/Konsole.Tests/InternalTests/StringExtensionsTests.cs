using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Konsole.Internal;
using FluentAssertions;
using System.Linq;

namespace Konsole.Tests.InternalTests
{
    public class StringExtensionsTests
    {
        public class SplitByCrLfOrNullShould
        {
            [Test]
            public void split_text_with_crlf_into_lines()
            {
                var source = "this\nis a test \rwith a lot \n\r of\n\rdifferent  \r\ntypes of splits.";
                var expected = new[]
                {
                    "this",
                    "is a test ",
                    "with a lot ",
                    " of",
                    "different  ",
                    "types of splits."
                };
                source.SplitByCrLfOrNull().Should().BeEquivalentTo(expected);
            }

            [Test]
            public void return_null_when_splits()
            {
                var source = "this is a test with a lot of different types of splits.";
                source.SplitByCrLfOrNull().Should().BeNull();
            }

            [Test]
            public void splits_should_contain_no_further_encoded_crlfs()
            {
                var source = "this\nis a test \rwith a lot \n\r of\n\rdifferent  \r\ntypes of splits.";
                var splits = source.SplitByCrLfOrNull();
                Assert.True(splits.Length == 6);
                Assert.False(splits.Any(s => s.ContainsEncodedCrLf()));
            }

        }
    }
}
