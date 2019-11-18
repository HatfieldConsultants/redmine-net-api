using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Redmine.Api.Exceptions;
using Redmine.Api.Extensions.Async;
using Redmine.Api.Tests.Infrastructure;
using Redmine.Api.Types;
using Xunit;
using Xunit.Abstractions;

namespace Redmine.Api.Tests.Tests
{
    [Trait("Redmine-api", "Credentials")]
    [Collection("RedmineCollection")]
    [Order(1)]
    public class RedmineTest : IClassFixture<RedmineFixture>
    {
        private readonly RedmineFixture fixture;
        private readonly ITestOutputHelper testOutput;

        public RedmineTest(RedmineFixture fixture, ITestOutputHelper testOutput)
        {
            this.fixture = fixture;
            this.testOutput = testOutput;
        }

        [Fact(Skip = "it sets host null and the other tests will fail.")]
        public void Should_Throw_InvalidCastException_When_No_Host_Set()
        {
            fixture.ConnectionSettings.Host = null;
            Assert.Throws<RedmineException>(() => fixture.RedmineManager.GetCurrentUser());
        }

        [Fact]
        public void Should_Throw_Redmine_Exception_When_No_Authentication_Set()
        {
            fixture.ConnectionSettings.UseApiKey = false;
            fixture.ConnectionSettings.Credentials = null;
            Assert.Throws<UnauthorizedException>(() => fixture.RedmineManager.GetCurrentUser());
        }

        [Fact]
        public void Should_Connect_With_Username_And_Password()
        {
            var expectedUser = fixture.RedmineManager.GetCurrentUser();
            Assert.NotNull(expectedUser);
            Assert.Equal(expectedUser.Login, fixture.ConnectionSettings.UserName);
        }

        [Fact]
        public void Should_Connect_With_Api_Key()
        {
            var @params = new NameValueCollection { { "include", "groups, memberships, customfields" } };

            var expectedUser = fixture.RedmineManager.GetCurrentUser(@params);
            Assert.NotNull(expectedUser);
            Assert.Equal(expectedUser.ApiKey, fixture.ConnectionSettings.ApiKey);
        }

#if !(NET20 || NET40)
        [Fact(Skip = "it sets host null and the other tests will fail.")]
        public async Task Should_Throw_Redmine_Exception_When_No_Host_Set_Async()
        {
            fixture.ConnectionSettings.Host = null;
            await Assert.ThrowsAsync<RedmineException>(() => fixture.RedmineManager.GetCurrentUserAsync());
        }


        [Fact]
        public async Task Should_Throw_Redmine_Exception_When_No_Authentication_Set_Async()
        {
            fixture.ConnectionSettings.UseApiKey = false;
            fixture.ConnectionSettings.Credentials = null;
            await Assert.ThrowsAsync<UnauthorizedException>(() => fixture.RedmineManager.GetCurrentUserAsync());
        }

        [Fact]
        public async Task Should_Connect_With_Username_And_Password_Async()
        {
            fixture.ConnectionSettings.UseApiKey = false;
            var expectedUser = await fixture.RedmineManager.GetCurrentUserAsync();
            Assert.NotNull(expectedUser);
            Assert.Equal(expectedUser.Login, fixture.ConnectionSettings.UserName);
        }

        [Fact]
        public async Task Should_Connect_With_Api_Key_Async()
        {
            var @params = new NameValueCollection { { "include", "groups, memberships, customfields" } };

            var expectedUser = await fixture.RedmineManager.GetCurrentUserAsync(@params);
            Assert.NotNull(expectedUser);
            Assert.Equal(expectedUser.ApiKey, fixture.ConnectionSettings.ApiKey);
        }
#endif

        #region Project

        [Fact]
        public void Should_Create_Project()
        {
            Project project = new Project("Redmine API dotnet project")
            {
                Description = "Redmine dotnet api",
                HomePage = "www.github.com",
                Identifier = "redmine-dotnet-api",
                InheritMembers = true,
                IsPublic = true,
                Trackers = new List<ProjectTracker>() { new ProjectTracker(1) },
                EnabledModules = new List<ProjectEnabledModule>() { ProjectEnabledModule.Wiki(), ProjectEnabledModule.Boards(), ProjectEnabledModule.Calendar() },
                CustomFields = new List<IssueCustomField>() {new IssueCustomField() {Multiple = true, Values = new List<CustomFieldValue>() {new CustomFieldValue("1.1.0.1"), new CustomFieldValue("3.0.1")}}}
            };

            var actual = fixture.RedmineManager.CreateObject(project);
            Assert.True(actual.Id > 0);

        }

        [Fact]
        public void Should_Create_Project_With_Custom_Fields()
        {
            Project project = new Project("Redmine API dotnet project - custom fields")
            {
                Description = "Redmine dotnet api - cf",
                HomePage = "www.github.com/redmine",
                Identifier = "redmine-dotnet-api-cf",
                InheritMembers = true,
                IsPublic = true,
                CustomFields = new List<IssueCustomField>() {new IssueCustomField(1,"Project Custom field") {Multiple = true, Values = new List<CustomFieldValue>() {new CustomFieldValue("1.1.0.1"), new CustomFieldValue("3.0.1")}}}
            };

            var actual = fixture.RedmineManager.CreateObject(project);
            Assert.True(actual.Id > 0);
        }

        [Fact]
        public void Should_Update_Project_Name()
        {
            Project project = new Project(2, "Changed Redmine API dotnet project");

            fixture.RedmineManager.UpdateObject(project.Id.ToString(), project);
        }

        [Fact]
        public void Should_Get_Projects()
        {
            var list = fixture.RedmineManager.GetObjects<Project>("trackers,enabled_modules,custom_fields");

            Assert.NotNull(list);
            Assert.True(list[0].EnabledModules.Count == 10);
            Assert.True(list[0].Trackers.Count == 3);
        }

        [Fact]
        public void Should_Delete_Project()
        {
            fixture.RedmineManager.DeleteObject<Project>(2.ToString(CultureInfo.InvariantCulture));
        }


        [Fact(DisplayName = "Should throw error when creating a project with incorrect values.")]
        public void Should_Throw_RedmineException_When_Create_Project()
        {
            Project project = new Project();

            Assert.Throws<RedmineException>(()=>fixture.RedmineManager.CreateObject(project));
        }
        #endregion
    }
}