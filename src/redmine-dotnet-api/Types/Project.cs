/*
   Copyright 2011 - 2021 Adrian Popescu.

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
using System.Collections.Generic;
using Redmine.dotNet.Api.Serialization.Xml.Converters;

namespace Redmine.dotNet.Api.Types
{
    public sealed class Project : IdentifiableName, IEquatable<Project>
    {
        public bool Equals(Project other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Identifier, other.Identifier, StringComparison.InvariantCultureIgnoreCase) && string.Equals(Description, other.Description, StringComparison.InvariantCultureIgnoreCase) && Equals(Parent, other.Parent) && string.Equals(HomePage, other.HomePage, StringComparison.InvariantCultureIgnoreCase) && Nullable.Equals(CreatedOn, other.CreatedOn) && Nullable.Equals(UpdatedOn, other.UpdatedOn) && IsPublic == other.IsPublic && InheritMembers == other.InheritMembers && Equals(Trackers, other.Trackers) && Equals(EnabledModules, other.EnabledModules) && Equals(DefaultVersion, other.DefaultVersion) && Equals(DefaultAssignee, other.DefaultAssignee);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Project other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Identifier, StringComparer.InvariantCultureIgnoreCase);
            hashCode.Add(Description, StringComparer.InvariantCultureIgnoreCase);
            hashCode.Add(Parent);
            hashCode.Add(HomePage, StringComparer.InvariantCultureIgnoreCase);
            hashCode.Add(CreatedOn);
            hashCode.Add(UpdatedOn);
            hashCode.Add(IsPublic);
            hashCode.Add(InheritMembers);
            hashCode.Add(Trackers);
            hashCode.Add(EnabledModules);
            hashCode.Add(DefaultVersion);
            hashCode.Add(DefaultAssignee);
            return hashCode.ToHashCode();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <remarks>Required for create</remarks>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public IdentifiableName Parent { get; set; }

        /// <summary>
        /// Gets or sets the home page.
        /// </summary>
        /// <value>The home page.</value>
        public string HomePage { get; set; }

        /// <summary>
        /// Gets the created on.
        /// </summary>
        /// <value>The created on.</value>
        public DateTime? CreatedOn { get; internal set; }

        /// <summary>
        /// Gets the updated on.
        /// </summary>
        /// <value>The updated on.</value>
        public DateTime? UpdatedOn { get; internal set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
//        public ProjectStatus Status { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether this project is public.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this project is public; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>Available in Redmine starting with 2.6.0 version.</remarks>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [inherit members].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [inherit members]; otherwise, <c>false</c>.
        /// </value>
        public bool InheritMembers { get; set; }

        /// <summary>
        /// Gets or sets the trackers.
        /// </summary>
        /// <value>
        /// The trackers.
        /// </value>
        /// <remarks>Available in Redmine starting with 2.6.0 version.</remarks>
        public IList<ProjectTracker> Trackers { get; set; }

        /// <summary>
        /// Gets or sets the enabled modules.
        /// </summary>
        /// <value>
        /// The enabled modules.
        /// </value>
        /// <remarks>Available in Redmine starting with 2.6.0 version.</remarks>
        public IList<ProjectEnabledModule> EnabledModules { get; set; }

        /// <summary>
        /// Gets or sets the custom fields.
        /// </summary>
        /// <value>
        /// The custom fields.
        /// </value>
//        public IList<IssueCustomField> CustomFields { get; set; }

        /// <summary>
        /// Gets the issue categories.
        /// </summary>
        /// <value>
        /// The issue categories.
        /// </value>
        /// <remarks>Available in Redmine starting with 2.6.0 version.</remarks>
//        public IList<ProjectIssueCategory> IssueCategories { get; internal set; }

        /// <summary>
        /// Gets the time entry activities.
        /// </summary>
        /// <remarks>Available in Redmine starting with 3.4.0 version.</remarks>
//        public IList<ProjectTimeEntryActivity> TimeEntryActivities { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public IdentifiableName DefaultVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IdentifiableName DefaultAssignee { get; set; }
        #endregion

    
    }
}
