/*
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

namespace Redmine.Api
{
    /// <summary>
    /// Struct to report Http Transfer Progress (Upload and Download)
    /// </summary>
    public struct HttpProgress
    {
        /// <summary>
        /// A value between 0-100 indicating percentage complete
        /// </summary>
        public double Progress { get; private set; }

        /// <summary>
        /// A value representing the current Transfer Speed in Bytes per seconds
        /// </summary>
        public double TransferSpeed { get; private set; }

        /// <summary>
        /// A value representing the calculated 'Estimated time of arrival'
        /// </summary>
        public TimeSpan Eta { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="transferSpeed"></param>
        /// <param name="remainingTime"></param>
        public HttpProgress(double progress, double transferSpeed, TimeSpan remainingTime)
        {
            Progress = progress;
            TransferSpeed = transferSpeed;
            Eta = remainingTime;
        }

        /// <summary>
        /// Convert Transfer Speed (bytes per second) in human readable format
        /// </summary>
        public string TransferSpeedToString()
        {
            var num = TransferSpeed > 0.0 ? TransferSpeed / 1024.0 : 0.0;
            return num < 1024.0 ? $"{Math.Round(num, 2)} KB/s" : $"{Math.Round(num / 1024.0, 2)} MB/s";
        }
    }
}