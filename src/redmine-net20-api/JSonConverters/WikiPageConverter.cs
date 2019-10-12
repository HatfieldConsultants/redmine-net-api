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

#if !NET20
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Redmine.Api.Extensions;
using Redmine.Api.Types;

namespace Redmine.Api.JSonConverters
{
    internal class WikiPageConverter : JavaScriptConverter
    {
        /// <summary>
        ///     When overridden in a derived class, gets a collection of the supported types.
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>(new[] { typeof(WikiPage) }); }
        }

        /// <summary>
        ///     When overridden in a derived class, converts the provided dictionary into an object of the specified type.
        /// </summary>
        /// <param name="dictionary">
        ///     An <see cref="T:System.Collections.Generic.IDictionary`2" /> instance of property data stored
        ///     as name/value pairs.
        /// </param>
        /// <param name="type">The type of the resulting object.</param>
        /// <param name="serializer">The <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer" /> instance.</param>
        /// <returns>
        ///     The deserialized object.
        /// </returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
            JavaScriptSerializer serializer)
        {
            if (dictionary != null)
            {
                var tracker = new WikiPage();

                tracker.Id = dictionary.GetValue<int>(RedmineKeys.ID);
                tracker.Author = dictionary.GetValueAsIdentifiableName(RedmineKeys.AUTHOR);
                tracker.Comments = dictionary.GetValue<string>(RedmineKeys.COMMENTS);
                tracker.CreatedOn = dictionary.GetValue<DateTime?>(RedmineKeys.CREATED_ON);
                tracker.Text = dictionary.GetValue<string>(RedmineKeys.TEXT);
                tracker.Title = dictionary.GetValue<string>(RedmineKeys.TITLE);
                tracker.UpdatedOn = dictionary.GetValue<DateTime?>(RedmineKeys.UPDATED_ON);
                tracker.Version = dictionary.GetValue<int>(RedmineKeys.VERSION);
                tracker.Attachments = dictionary.GetValueAsCollection<Attachment>(RedmineKeys.ATTACHMENTS);

                return tracker;
            }

            return null;
        }

        /// <summary>
        ///     When overridden in a derived class, builds a dictionary of name/value pairs.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="serializer">The object that is responsible for the serialization.</param>
        /// <returns>
        ///     An object that contains key/value pairs that represent the object�s data.
        /// </returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var entity = obj as WikiPage;
            var result = new Dictionary<string, object>();

            if (entity != null)
            {
                result.Add(RedmineKeys.TEXT, entity.Text);
                result.Add(RedmineKeys.COMMENTS, entity.Comments);
                result.WriteValueOrEmpty<int>(entity.Version, RedmineKeys.VERSION);
                result.WriteArray(RedmineKeys.UPLOADS, entity.Uploads, new UploadConverter(), serializer);

                var root = new Dictionary<string, object>();
                root[RedmineKeys.WIKI_PAGE] = result;
                return root;
            }

            return result;
        }
    }
}
#endif