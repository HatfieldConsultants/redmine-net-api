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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Redmine.Net.Api.Types;
using Redmine.Net.Api.Extensions;

namespace Redmine.Net.Api.JSonConverters
{
    internal class IssueCustomFieldConverter : JavaScriptConverter
    {
        #region Overrides of JavaScriptConverter

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
                var customField = new IssueCustomField();

                customField.Id = dictionary.GetValue<int>(RedmineKeys.ID);
                customField.Name = dictionary.GetValue<string>(RedmineKeys.NAME);
                customField.Multiple = dictionary.GetValue<bool>(RedmineKeys.MULTIPLE);

                var val = dictionary.GetValue<object>(RedmineKeys.VALUE);

                if (val != null)
                {
                    if (customField.Values == null) customField.Values = new List<CustomFieldValue>();
                    var list = val as ArrayList;
                    if (list != null)
                    {
                        foreach (var value in list)
                        {
                            customField.Values.Add(new CustomFieldValue {Info = Convert.ToString(value)});
                        }
                    }
                    else
                    {
                        customField.Values.Add(new CustomFieldValue {Info = Convert.ToString(val)});
                    }
                }
                return customField;
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
            var entity = obj as IssueCustomField;

            var result = new Dictionary<string, object>();

            if (entity == null) return result;
            if (entity.Values == null) return null;
            var itemsCount = entity.Values.Count;

            result.Add(RedmineKeys.ID, entity.Id);
            if (itemsCount > 1)
            {
                result.Add(RedmineKeys.VALUE, entity.Values.Select(x => x.Info).ToArray());
            }
            else
            {
                result.Add(RedmineKeys.VALUE, itemsCount > 0 ? entity.Values[0].Info : null);
            }

            return result;
        }

        /// <summary>
        ///     When overridden in a derived class, gets a collection of the supported types.
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>(new[] {typeof(IssueCustomField)}); }
        }

        #endregion
    }
}
#endif