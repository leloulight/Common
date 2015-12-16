// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Microsoft.Extensions.Primitives
{
    public class StringValuesTests
    {
        public static TheoryData<StringValues> DefaultOrNullStringValues
        {
            get
            {
                return new TheoryData<StringValues>
                {
                    new StringValues((string)null),
                    new StringValues((string[])null),
                    (string)null,
                    (string[])null
                };
            }
        }

        public static TheoryData<StringValues> EmptyStringValues
        {
            get
            {
                return new TheoryData<StringValues>
                {
                    StringValues.Empty,
                    new StringValues(new string[0]),
                    new string[0]
                };
            }
        }

        public static TheoryData<StringValues> FilledStringValues
        {
            get
            {
                return new TheoryData<StringValues>
                {
                    new StringValues("abc"),
                    new StringValues(new[] { "abc" }),
                    new StringValues(new[] { "abc", "bcd" }),
                    new StringValues(new[] { "abc", "bcd", "foo" }),
                    "abc",
                    new[] { "abc" },
                    new[] { "abc", "bcd" },
                    new[] { "abc", "bcd", "foo" }
                };
            }
        }

        public static TheoryData<StringValues, string> FilledStringValuesWithExpectedStrings
        {
            get
            {
                return new TheoryData<StringValues, string>
                {
                    { StringValues.Empty, (string)null },
                    { new StringValues(new string[] { }), (string)null },
                    { new StringValues(string.Empty), string.Empty },
                    { new StringValues(new string[] { string.Empty }), string.Empty },
                    { new StringValues("abc"), "abc" }
                };
            }
        }
        
        public static TheoryData<StringValues, object> FilledStringValuesWithExpectedObjects
        {
            get
            {
                return new TheoryData<StringValues, object>
                {
                    { StringValues.Empty, (object)null },
                    { new StringValues(new string[] { }), (object)null },
                    { new StringValues("abc"), (object)"abc" },
                    { new StringValues("abc"), (object)new[] { "abc" } },
                    { new StringValues(new[] { "abc" }), (object)new[] { "abc" } },
                    { new StringValues(new[] { "abc", "bcd" }), (object)new[] { "abc", "bcd" } }
                };
            }
        }

        public static TheoryData<StringValues, string[]> FilledStringValuesWithExpected
        {
            get
            {
                return new TheoryData<StringValues, string[]>
                {
                    { StringValues.Empty, new string[0] },
                    { new StringValues(string.Empty), new[] { string.Empty } },
                    { new StringValues("abc"), new[] { "abc" } },
                    { new StringValues(new[] { "abc" }), new[] { "abc" } },
                    { new StringValues(new[] { "abc", "bcd" }), new[] { "abc", "bcd" } },
                    { new StringValues(new[] { "abc", "bcd", "foo" }), new[] { "abc", "bcd", "foo" } },
                    { string.Empty, new[] { string.Empty } },
                    { "abc", new[] { "abc" } },
                    { new[] { "abc" }, new[] { "abc" } },
                    { new[] { "abc", "bcd" }, new[] { "abc", "bcd" } },
                    { new[] { "abc", "bcd", "foo" }, new[] { "abc", "bcd", "foo" } }
                };
            }
        }

        [Theory]
        [MemberData(nameof(DefaultOrNullStringValues))]
        [MemberData(nameof(EmptyStringValues))]
        [MemberData(nameof(FilledStringValues))]
        public void IsReadOnly_True(StringValues stringValues)
        {
            Assert.True(((IList<string>)stringValues).IsReadOnly);
            Assert.Throws<NotSupportedException>(() => ((IList<string>)stringValues)[0] = string.Empty);
            Assert.Throws<NotSupportedException>(() => ((ICollection<string>)stringValues).Add(string.Empty));
            Assert.Throws<NotSupportedException>(() => ((IList<string>)stringValues).Insert(0, string.Empty));
            Assert.Throws<NotSupportedException>(() => ((ICollection<string>)stringValues).Remove(string.Empty));
            Assert.Throws<NotSupportedException>(() => ((IList<string>)stringValues).RemoveAt(0));
            Assert.Throws<NotSupportedException>(() => ((ICollection<string>)stringValues).Clear());
        }

        [Theory]
        [MemberData(nameof(DefaultOrNullStringValues))]
        public void DefaultOrNull_ExpectedValues(StringValues stringValues)
        {
            Assert.Null((string[])stringValues);
        }

        [Theory]
        [MemberData(nameof(DefaultOrNullStringValues))]
        [MemberData(nameof(EmptyStringValues))]
        public void DefaultNullOrEmpty_ExpectedValues(StringValues stringValues)
        {
            Assert.Equal(0, stringValues.Count);
            Assert.Null((string)stringValues);
            Assert.Equal((string)null, stringValues);
            Assert.Equal(string.Empty, stringValues.ToString());
            Assert.Equal(new string[0], stringValues.ToArray());

            Assert.True(StringValues.IsNullOrEmpty(stringValues));
            Assert.Throws<IndexOutOfRangeException>(() => stringValues[0]);
            Assert.Throws<IndexOutOfRangeException>(() => ((IList<string>)stringValues)[0]);
            Assert.Equal(string.Empty, stringValues.ToString());
            Assert.Equal(-1, ((IList<string>)stringValues).IndexOf(null));
            Assert.Equal(-1, ((IList<string>)stringValues).IndexOf(string.Empty));
            Assert.Equal(-1, ((IList<string>)stringValues).IndexOf("not there"));
            Assert.False(((ICollection<string>)stringValues).Contains(null));
            Assert.False(((ICollection<string>)stringValues).Contains(string.Empty));
            Assert.False(((ICollection<string>)stringValues).Contains("not there"));
            Assert.Equal(0, stringValues.Count());
        }

        [Fact]
        public void ImplicitStringConverter_Works()
        {
            string nullString = null;
            StringValues stringValues = nullString;
            Assert.Equal(0, stringValues.Count);
            Assert.Null((string)stringValues);
            Assert.Null((string[])stringValues);

            string aString = "abc";
            stringValues = aString;
            Assert.Equal(1, stringValues.Count);
            Assert.Equal(aString, stringValues);
            Assert.Equal(aString, stringValues[0]);
            Assert.Equal(aString, ((IList<string>)stringValues)[0]);
            Assert.Equal<string[]>(new string[] { aString }, stringValues);
        }

        [Fact]
        public void ImplicitStringArrayConverter_Works()
        {
            string[] nullStringArray = null;
            StringValues stringValues = nullStringArray;
            Assert.Equal(0, stringValues.Count);
            Assert.Null((string)stringValues);
            Assert.Null((string[])stringValues);

            string aString = "abc";
            string[] aStringArray = new[] { aString };
            stringValues = aStringArray;
            Assert.Equal(1, stringValues.Count);
            Assert.Equal(aString, stringValues);
            Assert.Equal(aString, stringValues[0]);
            Assert.Equal(aString, ((IList<string>)stringValues)[0]);
            Assert.Equal<string[]>(aStringArray, stringValues);

            aString = "abc";
            string bString = "bcd";
            aStringArray = new[] { aString, bString };
            stringValues = aStringArray;
            Assert.Equal(2, stringValues.Count);
            Assert.Equal("abc,bcd", stringValues);
            Assert.Equal<string[]>(aStringArray, stringValues);
        }

        [Theory]
        [MemberData(nameof(DefaultOrNullStringValues))]
        [MemberData(nameof(EmptyStringValues))]
        public void DefaultNullOrEmpty_Enumerator(StringValues stringValues)
        {
            var e = stringValues.GetEnumerator();
            Assert.Null(e.Current);
            Assert.False(e.MoveNext());
            Assert.Null(e.Current);
            Assert.False(e.MoveNext());
            Assert.False(e.MoveNext());
            Assert.False(e.MoveNext());

            var e1 = ((IEnumerable<string>)stringValues).GetEnumerator();
            Assert.Null(e1.Current);
            Assert.False(e1.MoveNext());
            Assert.Null(e1.Current);
            Assert.False(e1.MoveNext());
            Assert.False(e1.MoveNext());
            Assert.False(e1.MoveNext());

            var e2 = ((IEnumerable)stringValues).GetEnumerator();
            Assert.Null(e2.Current);
            Assert.False(e2.MoveNext());
            Assert.Null(e2.Current);
            Assert.False(e2.MoveNext());
            Assert.False(e2.MoveNext());
            Assert.False(e2.MoveNext());
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpected))]
        public void Enumerator(StringValues stringValues, string[] expected)
        {
            var e = stringValues.GetEnumerator();
            Assert.Null(e.Current);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(e.MoveNext());
                Assert.Equal(expected[i], e.Current);
            }
            Assert.False(e.MoveNext());
            Assert.False(e.MoveNext());
            Assert.False(e.MoveNext());

            var e1 = ((IEnumerable<string>)stringValues).GetEnumerator();
            Assert.Null(e1.Current);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(e1.MoveNext());
                Assert.Equal(expected[i], e1.Current);
            }
            Assert.False(e1.MoveNext());
            Assert.False(e1.MoveNext());
            Assert.False(e1.MoveNext());

            var e2 = ((IEnumerable)stringValues).GetEnumerator();
            Assert.Null(e2.Current);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(e2.MoveNext());
                Assert.Equal(expected[i], e2.Current);
            }
            Assert.False(e2.MoveNext());
            Assert.False(e2.MoveNext());
            Assert.False(e2.MoveNext());
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpected))]
        public void IndexOf(StringValues stringValues, string[] expected)
        {
            IList<string> list = stringValues;
            Assert.Equal(-1, list.IndexOf("not there"));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(i, list.IndexOf(expected[i]));
            }
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpected))]
        public void Contains(StringValues stringValues, string[] expected)
        {
            ICollection<string> collection = stringValues;
            Assert.False(collection.Contains("not there"));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(collection.Contains(expected[i]));
            }
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpected))]
        public void CopyTo_TooSmall(StringValues stringValues, string[] expected)
        {
            ICollection<string> collection = stringValues;
            string[] tooSmall = new string[0];

            if (collection.Count > 0)
            {
                Assert.Throws<ArgumentException>(() => collection.CopyTo(tooSmall, 0));
            }
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpected))]
        public void CopyTo_CorrectSize(StringValues stringValues, string[] expected)
        {
            ICollection<string> collection = stringValues;
            string[] actual = new string[expected.Length];

            if (collection.Count > 0)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(actual, -1));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(actual, actual.Length + 1));
            }
            collection.CopyTo(actual, 0);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(DefaultOrNullStringValues))]
        [MemberData(nameof(EmptyStringValues))]
        public void DefaultNullOrEmpty_Concat(StringValues stringValues)
        {
            string[] expected = new[] { "abc", "bcd", "foo" };
            Assert.Equal(expected, StringValues.Concat(stringValues, new StringValues(expected)));
            Assert.Equal(expected, StringValues.Concat(new StringValues(expected), stringValues));

            string[] empty = new string[0];
            Assert.Equal(empty, StringValues.Concat(stringValues, StringValues.Empty));
            Assert.Equal(empty, StringValues.Concat(StringValues.Empty, stringValues));
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpected))]
        public void Concat(StringValues stringValues, string[] array)
        {
            string[] filled = new[] { "abc", "bcd", "foo" };

            string[] expectedPrepended = array.Concat(filled).ToArray();
            Assert.Equal(expectedPrepended, StringValues.Concat(stringValues, new StringValues(filled)));

            string[] expectedAppended = filled.Concat(array).ToArray();
            Assert.Equal(expectedAppended, StringValues.Concat(new StringValues(filled), stringValues));
        }

        [Fact]
        public void Equals_OperatorEqual()
        {
            var equalString = "abc";

            var equalStringArray = new string[] { equalString };
            var equalStringValues = new StringValues(equalString);
            var stringArray = new string[] { equalString, equalString };
            var stringValuesArray = new StringValues(stringArray);

            Assert.True(equalStringValues == equalStringValues);

            Assert.True(equalStringValues == equalString);
            Assert.True(equalString == equalStringValues);

            Assert.True(equalStringValues == equalStringArray);
            Assert.True(equalStringArray == equalStringValues);

            Assert.True(stringArray == stringValuesArray);
            Assert.True(stringValuesArray == stringArray);

            Assert.False(stringValuesArray == equalString);
            Assert.False(stringValuesArray == equalStringArray);
            Assert.False(stringValuesArray == equalStringValues);
        }

        [Fact]
        public void Equals_OperatorNotEqual()
        {
            var equalString = "abc";

            var equalStringArray = new string[] { equalString };
            var equalStringValues = new StringValues(equalString);
            var stringArray = new string[] { equalString, equalString };
            var stringValuesArray = new StringValues(stringArray);

            Assert.False(equalStringValues != equalStringValues);

            Assert.False(equalStringValues != equalString);
            Assert.False(equalString != equalStringValues);

            Assert.False(equalStringValues != equalStringArray);
            Assert.False(equalStringArray != equalStringValues);

            Assert.False(stringArray != stringValuesArray);
            Assert.False(stringValuesArray != stringArray);

            Assert.True(stringValuesArray != equalString);
            Assert.True(stringValuesArray != equalStringArray);
            Assert.True(stringValuesArray != equalStringValues);
        }

        [Fact]
        public void Equals_Instance()
        {
            var equalString = "abc";

            var equalStringArray = new string[] { equalString };
            var equalStringValues = new StringValues(equalString);
            var stringArray = new string[] { equalString, equalString };
            var stringValuesArray = new StringValues(stringArray);

            Assert.True(equalStringValues.Equals(equalStringValues));
            Assert.True(equalStringValues.Equals(equalString));
            Assert.True(equalStringValues.Equals(equalStringArray));
            Assert.True(stringValuesArray.Equals(stringArray));
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpectedObjects))]
        public void Equals_ObjectEquals(StringValues stringValues, object obj)
        {
            Assert.True(stringValues == obj);
            Assert.True(obj == stringValues);
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpectedObjects))]
        public void Equals_ObjectNotEquals(StringValues stringValues, object obj)
        {
            Assert.False(stringValues != obj);
            Assert.False(obj != stringValues);
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpectedStrings))]
        public void Equals_String(StringValues stringValues, string expected)
        {
            var notEqual = new StringValues("bcd");

            Assert.True(StringValues.Equals(stringValues, expected));
            Assert.False(StringValues.Equals(stringValues, notEqual));
        }

        [Theory]
        [MemberData(nameof(FilledStringValuesWithExpected))]
        public void Equals_StringArray(StringValues stringValues, string[] expected)
        {
            var notEqual = new StringValues(new[] { "bcd", "abc" });

            Assert.True(StringValues.Equals(stringValues, expected));
            Assert.False(StringValues.Equals(stringValues, notEqual));
        }

        [Theory]
        [MemberData(nameof(DefaultOrNullStringValues))]
        [MemberData(nameof(EmptyStringValues))]
        [MemberData(nameof(FilledStringValues))]
        public void TryGetConstant_NullDefaultEncodingAndEncodedBytes(StringValues stringValues)
        {
            Encoding encoding;
            byte[] bytes;

            Assert.False(stringValues.TryGetPreEncoded(out bytes, out encoding));
            Assert.Null(encoding);
            Assert.Null(bytes);
        }

        [Theory]
        [MemberData(nameof(DefaultOrNullStringValues))]
        [MemberData(nameof(EmptyStringValues))]
        public void CreateConstant_ThrowsForNullOrEmpty(StringValues stringValues)
        {
            Assert.Throws<ArgumentNullException>(() => (StringValues.CreatePreEncoded(stringValues, Encoding.ASCII)));
        }

        [Fact]
        public void CreateConstant_EncodesEmptyString()
        {
            var stringValueConstant = StringValues.CreatePreEncoded("", Encoding.ASCII);
            byte[] bytes;
            Encoding encoding;

            stringValueConstant.TryGetPreEncoded(out bytes, out encoding);

            Assert.NotNull(bytes);
            Assert.Empty(bytes);
        }

        [Fact]
        public void CreateConstant_EncodesUsingSpecifiedEncoding()
        {
            var testString = "foo bar";
            var expectedBytes = Encoding.ASCII.GetBytes(testString);
            byte[] outBytes;
            Encoding outEncoding;

            var constantStringValues = StringValues.CreatePreEncoded(testString, Encoding.ASCII);
            constantStringValues.TryGetPreEncoded(out outBytes, out outEncoding);

            Assert.Equal(expectedBytes, outBytes);
            Assert.Equal(Encoding.ASCII, outEncoding);
        }

        [Fact]
        public void TryGetConstant_ReturnsIdenticalBytesAndEncoding()
        {
            var encoding = Encoding.GetEncoding(20127); // US-ASCII
            var constantStringValues = StringValues.CreatePreEncoded("foo bar", encoding);

            byte[] bytes1, bytes2;
            Encoding encoding1, encoding2;
            constantStringValues.TryGetPreEncoded(out bytes1, out encoding1);
            constantStringValues.TryGetPreEncoded(out bytes2, out encoding2);

            Assert.Same(bytes1, bytes2);
            Assert.Same(encoding, encoding1);
            Assert.Same(encoding, encoding2);
        }
    }
}
