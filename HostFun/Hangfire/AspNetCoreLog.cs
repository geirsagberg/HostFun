// This file is part of Hangfire.
// Copyright © 2016 Sergey Odinokov.
// 
// Hangfire is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as 
// published by the Free Software Foundation, either version 3 
// of the License, or any later version.
// 
// Hangfire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public 
// License along with Hangfire. If not, see <http://www.gnu.org/licenses/>.

using System;
using Hangfire.Annotations;
using Hangfire.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using LogLevel = Hangfire.Logging.LogLevel;

namespace Hangfire.AspNetCore
{
    internal class AspNetCoreLog : ILog
    {
        private static readonly Func<object, Exception, string> MessageFormatterFunc = MessageFormatter;
        private static readonly object[] EmptyArgs = new object[0];

        private readonly ILogger _targetLogger;

        public AspNetCoreLog([NotNull] ILogger targetLogger)
        {
            if (targetLogger == null) throw new ArgumentNullException(nameof(targetLogger));
            _targetLogger = targetLogger;
        }

        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null)
        {
            var targetLogLevel = ToTargetLogLevel(logLevel);

            // When messageFunc is null, Hangfire.Logging
            // just determines is logging enabled.
            if (messageFunc == null) {
                return _targetLogger.IsEnabled(targetLogLevel);
            }

            _targetLogger.Log(targetLogLevel, 0, CreateStateObject(messageFunc()), exception, MessageFormatterFunc);
            return true;
        }

        private static Microsoft.Extensions.Logging.LogLevel ToTargetLogLevel(LogLevel logLevel)
        {
            switch (logLevel) {
                case LogLevel.Trace:
                    return Microsoft.Extensions.Logging.LogLevel.Trace;
                case LogLevel.Debug:
                    return Microsoft.Extensions.Logging.LogLevel.Debug;
                case LogLevel.Info:
                    return Microsoft.Extensions.Logging.LogLevel.Information;
                case LogLevel.Warn:
                    return Microsoft.Extensions.Logging.LogLevel.Warning;
                case LogLevel.Error:
                    return Microsoft.Extensions.Logging.LogLevel.Error;
                case LogLevel.Fatal:
                    return Microsoft.Extensions.Logging.LogLevel.Critical;
            }

            return Microsoft.Extensions.Logging.LogLevel.None;
        }

        private static object CreateStateObject(string message)
        {
            return new FormattedLogValues(message, EmptyArgs);
        }

        private static string MessageFormatter(object state, Exception exception)
        {
            return state.ToString();
        }
    }
}