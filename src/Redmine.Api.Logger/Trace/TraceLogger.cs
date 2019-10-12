﻿/*
   Copyright 2011 - 2019 Adrian Popescu.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;

namespace Redmine.Api.Logger.Trace
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TraceLogger : ILogger
    {
        private static readonly object locker = new object();
        /// <summary>
        /// Logs the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void Log(LogEntry entry)
        {
            lock (locker)
            {
                switch (entry.Severity)
                {
                    case LoggingEventType.Debug:
                        System.Diagnostics.Trace.WriteLine(entry.Message, "Debug");
                        break;
                    case LoggingEventType.Information:
                        System.Diagnostics.Trace.TraceInformation(entry.Message);
                        break;
                    case LoggingEventType.Warning:
                        System.Diagnostics.Trace.TraceWarning(entry.Message);
                        break;
                    case LoggingEventType.Error:
                        System.Diagnostics.Trace.TraceError(entry.Message);
                        break;
                    case LoggingEventType.Fatal:
                        System.Diagnostics.Trace.WriteLine(entry.Message, "Fatal");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}