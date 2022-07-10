using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Redmine.dotNet.Api.Types;

namespace Redmine.dotNet.Api.Serialization.Xml.Converters;

public sealed class IssueConverter: IdentifiableConverter, IRedmineConverter<Issue>
{
    public Issue Payload { get; set; }
    
    public override void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString(RedmineKeys.SUBJECT, Payload.Subject);
        writer.WriteElementString(RedmineKeys.NOTES, Payload.Notes);

        if (Payload.Id != 0)
        {
            writer.WriteBoolean(RedmineKeys.PRIVATE_NOTES, Payload.PrivateNotes);
        }

        writer.WriteElementString(RedmineKeys.DESCRIPTION, Payload.Description);
        writer.WriteElementString(RedmineKeys.IS_PRIVATE, Payload.IsPrivate.ToLowerInvariant());

        writer.WriteIdIfNotNull(RedmineKeys.PROJECT_ID, Payload.Project);
        writer.WriteIdIfNotNull(RedmineKeys.PRIORITY_ID, Payload.Priority);
        writer.WriteIdIfNotNull(RedmineKeys.STATUS_ID, Payload.Status);
        writer.WriteIdIfNotNull(RedmineKeys.CATEGORY_ID, Payload.Category);
        writer.WriteIdIfNotNull(RedmineKeys.TRACKER_ID, Payload.Tracker);
        writer.WriteIdIfNotNull(RedmineKeys.ASSIGNED_TO_ID, Payload.AssignedTo);
        writer.WriteIdIfNotNull(RedmineKeys.PARENT_ISSUE_ID, Payload.ParentIssue);
        writer.WriteIdIfNotNull(RedmineKeys.FIXED_VERSION_ID, Payload.FixedVersion);

        writer.WriteValueOrEmpty(RedmineKeys.ESTIMATED_HOURS, Payload.EstimatedHours);
        writer.WriteIfNotDefaultOrNull(RedmineKeys.DONE_RATIO, Payload.DoneRatio);

        writer.WriteDateOrEmpty(RedmineKeys.START_DATE, Payload.StartDate);
        writer.WriteDateOrEmpty(RedmineKeys.DUE_DATE, Payload.DueDate);
        writer.WriteDateOrEmpty(RedmineKeys.UPDATED_ON, Payload.UpdatedOn);

        // writer.WriteArray(RedmineKeys.UPLOADS, Payload.Uploads);
        // writer.WriteArray(RedmineKeys.CUSTOM_FIELDS, Payload.CustomFields);
        //
        // writer.WriteListElements(RedmineKeys.WATCHER_USER_IDS, (IEnumerable<IValue>)Payload.Watchers);
    }
    
}