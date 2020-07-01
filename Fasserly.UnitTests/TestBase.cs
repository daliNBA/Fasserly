using Fasserly.Database.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Fasserly.UnitTests
{
    public abstract class TestsBase
    {
        protected readonly IConfiguration configuration;
        protected readonly ILoggerFactory loggerFactory;
        private readonly Random random = new Random();

        /// <summary>Constructor.</summary>
        public TestsBase()
        {
            var configData = new Dictionary<string, string>
            {
                { "WebServiceEnvironment", "E0" }
            };
            configuration = new ConfigurationBuilder().AddInMemoryCollection(configData).Build();
            loggerFactory = new LoggerFactory();
        }

        protected string RandomString(int length = 10, bool mustBeUnique = false)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[length];
            bool tryAgain;
            string result;
            do
            {
                for (var i = 0; i < stringChars.Length; i++)
                    stringChars[i] = chars[random.Next(chars.Length)];
                result = new string(stringChars);
                if (mustBeUnique && UnicityChecker.AlreadySeen(result.ToUpper()))
                    tryAgain = true;
                else
                    tryAgain = false;
            } while (tryAgain);
            return result;
        }

        protected decimal RandomDecimal()
        {
            return (decimal)Math.Round(random.NextDouble(), 6);
        }

        protected decimal RandomDecimal(decimal min, decimal max)
        {
            decimal range = max - min;
            decimal r = (decimal)Math.Round(random.NextDouble(), 6);
            return min + range * r;
        }

        protected long RandomLong(long? max = null)
        {
            var buffer = new byte[8];
            random.NextBytes(buffer);
            var l = BitConverter.ToInt64(buffer, 0);
            return max.HasValue ? Math.Abs(l % max.Value) : l;
        }

        protected int RandomInteger()
        {
            return random.Next();
        }

        protected int RandomInteger(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        protected short RandomShort()
        {
            return (short)random.NextDouble();
        }

        protected byte RandomByte()
        {
            return (byte)random.NextDouble();
        }

        protected bool RandomBoolean()
        {
            return random.NextDouble() >= 0.5;
        }

        protected bool? RandomNullabeBoolean()
        {
            return RandomBoolean() ? (bool?)null : RandomBoolean();
        }

        private readonly string[] timeZones = new string[] {
            "Arab Standard Time",
            "Arabian Standard Time",
            "Atlantic Standard Time",
            "AUS Eastern Standard Time",
            "Central America Standard Time",
            "Central Europe Standard Time",
            "Central European Standard Time",
            "Central Pacific Standard Time",
            "China Standard Time",
            "E. Africa Standard Time",
            "Eastern Standard Time",
            "Egypt Standard Time",
            "GMT Standard Time",
            "Greenwich Standard Time",
            "GTB Standard Time",
            "India Standard Time",
            "Korea Standard Time",
            "Mauritius Standard Time",
            "Morocco Standard Time",
            "Namibia Standard Time",
            "Pacific SA Standard Time",
            "Romance Standard Time",
            "SA Pacific Standard Time",
            "SA Western Standard Time",
            "SE Asia Standard Time",
            "Singapore Standard Time",
            "South Africa Standard Time",
            "Turkey Standard Time",
            "UTC",
            "W. Central Africa Standard Time",
            "W. Europe Standard Time" };

        protected string RandomTimeZone()
        {
            return timeZones[random.Next(timeZones.Length)];
        }

        protected string RandomEmail()
        {
            return RandomString(8) + "@colas.com";
        }

        protected DateTime RandomDateTime(bool zeroMilliseconds = false)
        {
            var minDate = new DateTime(1999, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            var maxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc);
            var ticks = (maxDate - minDate).Ticks;
            var d = minDate.AddTicks(RandomLong(ticks));
            if (zeroMilliseconds)
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, 0, DateTimeKind.Utc);
            else
                return d;
        }

        protected CultureInfo RandomCulture()
        {
            return CultureInfo.CurrentCulture;
        }

        protected Training RandomTraining(bool? isValid = null)
        {
            return new Training
            {
                TrainingId = Guid.NewGuid(),
                Price = RandomDecimal(),
                Title = RandomString(),
                IsActive = isValid ?? RandomBoolean(),
                DateOfCreation = DateTime.Now,
            };
        }

        protected Category RandomCategory()
        {
            return new Category
            {
                CategoryId = Guid.NewGuid(),
                Label = RandomString(),
            };
        }

        private static class UnicityChecker
        {
            private static readonly object myLock = new object();
            private static readonly SortedSet<String> uniqueStrings = new SortedSet<string>();

            /// <summary>
            /// Check if a string has already been seen.
            /// Remember the string for next queries.
            /// </summary>
            /// <param name="aString">The string to check and remember</param>
            /// <returns>True if the string is unique, false if it has already been seen</returns>
            public static bool AlreadySeen(string aString)
            {
                bool exists;
                lock (myLock)
                {
                    if (!(exists = uniqueStrings.Contains(aString)))
                    {
                        uniqueStrings.Add(aString);
                    }
                }
                return exists;
            }
        }
    }
}
