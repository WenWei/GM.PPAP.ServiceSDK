using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace GM.PPAP.ServiceSDK
{
    public class ConfigHelper
    {
        private readonly IConfigurationRoot _configuration;

        public ConfigHelper(string filename = "ServiceConfig.json")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(filename);

            _configuration = builder.Build();
        }

        public TResult GetValue<TResult>(string key, Func<TResult> getDefaultValue = null,
            bool canThrowException = false)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var result = default(TResult);
            var value = _configuration[key];

            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    if (getDefaultValue != null) result = getDefaultValue();

                    //  檢查 result == default(TResult)
                    if (canThrowException && EqualityComparer<TResult>.Default.Equals(result, default(TResult)))
                        throw new ValueNullExcpetion(key);
                }
                else
                {
                    result = ConvertValue<TResult>(value);
                }
            }
            catch (ValueNullExcpetion)
            {
                // no need to check the 'canThrowException'
                throw;
            }
            catch (ValueConvertExcpetion ex)
            {
                // no need to check the 'canThrowException'
                ex.Key = key;
                throw;
            }
            catch (Exception ex)
            {
                if (canThrowException) throw new GetValueExcpetion(key, ex);
            }

            return result;
        }

        private static TResult ConvertValue<TResult>(string value, bool canThrowException = false)
        {
            try
            {
                var result = (TResult)Convert.ChangeType(value, typeof(TResult));
                return result;
            }
            catch (Exception ex)
            {
                if (canThrowException) throw new ValueConvertExcpetion(value, typeof(TResult), ex);
                return default(TResult);
            }
        }

        public class ValueNullExcpetion : Exception
        {
            internal ValueNullExcpetion(string key)
            {
                Key = key;
            }

            public string Key { get; }

            public override string Message =>
                $"Failed to get a value for the key \"{Key}\" in the App.config file.";
        }

        public class ValueConvertExcpetion : Exception
        {
            internal ValueConvertExcpetion(string value, Type targetType, Exception innerException)
                : base(null, innerException)
            {
                Value = value;
                TargetType = targetType;
            }

            public string Key { get; internal set; }

            public string Value { get; }

            public Type TargetType { get; }

            public override string Message =>
                $"Failed to convert {Value} to the type \"{TargetType.Name}\". " +
                $"See inner exception for details.";
        }

        public class GetValueExcpetion : Exception
        {
            internal GetValueExcpetion(string key, Exception innerException) : base(null, innerException)
            {
            }

            public string Key { get; private set; }

            public override string Message =>
                $"An unexpected exception occured while getting a value for the key \"{Key}\". " +
                "See inner exception for details.";
        }
    }
}
