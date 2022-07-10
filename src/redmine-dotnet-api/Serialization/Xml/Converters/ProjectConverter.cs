using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Redmine.dotNet.Api.Types;

namespace Redmine.dotNet.Api.Serialization.Xml.Converters;

[XmlRoot(RedmineKeys.PROJECT)]
public sealed class ProjectConverter: IdentifiableConverter, IRedmineConverter<Project>
{
    public Project Payload { get; set; }
    
    #region Implementation of IXmlSerializer
    /// <summary>
    /// Generates an object from its XML representation.
    /// </summary>
    /// <param name="reader">The <see cref="System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
    public override void ReadXml(XmlReader reader)
    {
        reader.Read();
        while (!reader.EOF)
        {
            if (reader.IsEmptyElement && !reader.HasAttributes)
            {
                reader.Read();
                continue;
            }

            switch (reader.Name)
            {
                case RedmineKeys.ID: Payload.Id = reader.ReadElementContentAsInt(); break;
                case RedmineKeys.CREATED_ON: Payload.CreatedOn = reader.ReadElementContentAsNullableDateTime(); break;
                //    case RedmineKeys.CUSTOM_FIELDS: Payload.CustomFields = reader.ReadElementContentAsCollection<IssueCustomField>(); break;
                case RedmineKeys.DESCRIPTION: Payload.Description = reader.ReadElementContentAsString(); break;
                //    case RedmineKeys.ENABLED_MODULES: Payload.EnabledModules = reader.ReadElementContentAsCollection<ProjectEnabledModule>(); break;
                case RedmineKeys.HOMEPAGE: Payload.HomePage = reader.ReadElementContentAsString(); break;
                case RedmineKeys.IDENTIFIER: Payload.Identifier = reader.ReadElementContentAsString(); break;
                case RedmineKeys.INHERIT_MEMBERS: Payload.InheritMembers = reader.ReadElementContentAsBoolean(); break;
                case RedmineKeys.IS_PUBLIC: Payload.IsPublic = reader.ReadElementContentAsBoolean(); break;
                //    case RedmineKeys.ISSUE_CATEGORIES: Payload.IssueCategories = reader.ReadElementContentAsCollection<ProjectIssueCategory>(); break;
                case RedmineKeys.NAME: Payload.Name = reader.ReadElementContentAsString(); break;
                //    case RedmineKeys.PARENT: Payload.Parent = new IdentifiableName(reader); break;
                //   case RedmineKeys.STATUS: Payload.Status = (ProjectStatus)reader.ReadElementContentAsInt(); break;
                //   case RedmineKeys.TIME_ENTRY_ACTIVITIES: Payload.TimeEntryActivities = reader.ReadElementContentAsCollection<ProjectTimeEntryActivity>(); break;
                //    case RedmineKeys.TRACKERS: Payload.Trackers = reader.ReadElementContentAsCollection<ProjectTracker>(); break;
                case RedmineKeys.UPDATED_ON: Payload.UpdatedOn = reader.ReadElementContentAsNullableDateTime(); break;
                //    case RedmineKeys.DEFAULT_ASSIGNEE: Payload.DefaultAssignee = new IdentifiableName(reader); break;
                //    case RedmineKeys.DEFAULT_VERSION: Payload.DefaultVersion = new IdentifiableName(reader); break;
                default: reader.Read(); break;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="writer"></param>
    public override void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString(RedmineKeys.NAME, Payload.Name);
        writer.WriteElementString(RedmineKeys.IDENTIFIER, Payload.Identifier);
            
        writer.WriteIfNotDefaultOrNull(RedmineKeys.DESCRIPTION, Payload.Description);
        writer.WriteBoolean(RedmineKeys.INHERIT_MEMBERS, Payload.InheritMembers);
        writer.WriteBoolean(RedmineKeys.IS_PUBLIC, Payload.IsPublic);
        writer.WriteIfNotDefaultOrNull(RedmineKeys.HOMEPAGE, Payload.HomePage);
            
        writer.WriteIdIfNotNull(RedmineKeys.PARENT_ID, Payload.Parent);
            
        writer.WriteRepeatableElement(RedmineKeys.TRACKER_IDS, Payload.Trackers);
        writer.WriteRepeatableElement(RedmineKeys.ENABLED_MODULE_NAMES, Payload.EnabledModules);
            
        if (Payload.Id == 0)
        {
           // writer.WriteRepeatableElement(RedmineKeys.ISSUE_CUSTOM_FIELD_IDS, (IEnumerable<IValue>)Payload.CustomFields);
            return;
        }
            
        // writer.WriteArray(RedmineKeys.CUSTOM_FIELDS, Payload.CustomFields);
    }
    #endregion
    
}