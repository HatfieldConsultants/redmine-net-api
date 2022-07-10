using Redmine.dotNet.Api;
using Redmine.dotNet.Api.Serialization.Xml.Converters;
using Redmine.dotNet.Api.Types;

namespace redmine_dotnet_api.tests;

public sealed class IssueTests
{
    [Fact]
    public void Serialize()
    {
        IRedmineSerializationFactory redmineSerializationFactory = RedmineSerializationFactory.Create();
        IRedmineSerialization serialization = redmineSerializationFactory.Get(RedmineSerializationType.Xml);

        const bool NEW_ISSUE_IS_PRIVATE = true;

        const int NEW_ISSUE_PROJECT_ID = 1;
        const int NEW_ISSUE_TRACKER_ID = 1;
        const int NEW_ISSUE_STATUS_ID = 1;
        const int NEW_ISSUE_PRIORITY_ID = 9;
        const int NEW_ISSUE_CATEGORY_ID = 18;
        const int NEW_ISSUE_FIXED_VERSION_ID = 9;
        const int NEW_ISSUE_ASSIGNED_TO_ID = 8;
        const int NEW_ISSUE_PARENT_ISSUE_ID = 96;
        const int NEW_ISSUE_CUSTOM_FIELD_ID = 13;
        const int NEW_ISSUE_ESTIMATED_HOURS = 12;
        const int NEW_ISSUE_FIRST_WATCHER_ID = 2;
        const int NEW_ISSUE_SECOND_WATCHER_ID = 8;

        const string NEW_ISSUE_CUSTOM_FIELD_VALUE = "Issue custom field completed";
        const string NEW_ISSUE_SUBJECT = "Issue created using Rest API";
        const string NEW_ISSUE_DESCRIPTION = "Issue description...";

        var newIssueStartDate = DateTime.Now;
        var newIssueDueDate = DateTime.Now.AddDays(10);
        
        var converter = new IssueConverter()
        {
            Payload = new Issue()
            {
                Project = IdentifiableName.Create<IdentifiableName>(NEW_ISSUE_PROJECT_ID),
                Tracker = IdentifiableName.Create<IdentifiableName>(NEW_ISSUE_TRACKER_ID),
                Status = IdentifiableName.Create<IdentifiableName>(NEW_ISSUE_STATUS_ID),
                Priority = IdentifiableName.Create<IdentifiableName>(NEW_ISSUE_PRIORITY_ID),
                Subject = NEW_ISSUE_SUBJECT,
                Description = NEW_ISSUE_DESCRIPTION,
                Category = IdentifiableName.Create<IdentifiableName>(NEW_ISSUE_CATEGORY_ID),
                FixedVersion = IdentifiableName.Create<IdentifiableName>(NEW_ISSUE_FIXED_VERSION_ID),
                AssignedTo = IdentifiableName.Create<IdentifiableName>(NEW_ISSUE_ASSIGNED_TO_ID),
                ParentIssue = IdentifiableName.Create<IdentifiableName>(NEW_ISSUE_PARENT_ISSUE_ID),

             //   CustomFields = new List<IssueCustomField> {icf},
                IsPrivate = NEW_ISSUE_IS_PRIVATE,
                EstimatedHours = NEW_ISSUE_ESTIMATED_HOURS,
                StartDate = newIssueStartDate,
                DueDate = newIssueDueDate,
                // Watchers = new List<Watcher>
                // {
                //     IdentifiableName.Create<Watcher>(NEW_ISSUE_FIRST_WATCHER_ID),
                //     IdentifiableName.Create<Watcher>(NEW_ISSUE_SECOND_WATCHER_ID)
                // }
            }
        };

        
        var result = serialization.Serialize(converter);
    }

}