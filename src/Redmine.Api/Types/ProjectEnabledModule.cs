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
using System.Diagnostics;
using System.Xml.Serialization;
using Redmine.Api.Extensions;

namespace Redmine.Api.Types
{
    /// <summary>
    /// the module name: boards, calendar, documents, files, gant, issue_tracking, news, repository, time_tracking, wiki.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [XmlRoot(RedmineKeys.ENABLED_MODULE)]
    public sealed class ProjectEnabledModule : IdentifiableName, IValue
    {
        #region Ctors
        /// <summary>
        /// 
        /// </summary>
        public ProjectEnabledModule() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName">boards, calendar, documents, files, gant, issue_tracking, news, repository, time_tracking, wiki.</param>
        public ProjectEnabledModule(string moduleName)
        {
            if (moduleName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(nameof(moduleName));
            }

            Name = moduleName;
        }

        #endregion

        #region Implementation of IValue
        /// <summary>
        /// 
        /// </summary>
        public string Value => Name;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string DebuggerDisplay => $"[{nameof(ProjectEnabledModule)}: {ToString()}]";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule Boards()
        {
            return new ProjectEnabledModule("boards");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule Calendar()
        {
            return new ProjectEnabledModule("calendar");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule Documents()
        {
            return new ProjectEnabledModule("documents");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule Files()
        {
            return new ProjectEnabledModule("files");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule Gant()
        {
            return new ProjectEnabledModule("gant");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule IssueTracking()
        {
            return new ProjectEnabledModule("issue_tracking");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule News()
        {
            return new ProjectEnabledModule("news");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule Repository()
        {
            return new ProjectEnabledModule("repository");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule TimeTracking()
        {
            return new ProjectEnabledModule("time_tracking");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProjectEnabledModule Wiki()
        {
            return new ProjectEnabledModule("wiki");
        }

    }
}