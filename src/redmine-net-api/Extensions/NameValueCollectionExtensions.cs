/*
   Copyright 2011 - 2017 Adrian Popescu

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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Redmine.Net.Api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public static string GetParameterValue(this NameValueCollection parameters, string parameterName)
        {
            if (parameters == null)
            {
                return null;
            }

            var value = parameters.Get(parameterName);
            
            return value.IsNullOrWhiteSpace() ? null : value;
        }

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public static string GetValue(this NameValueCollection parameters, string parameterName)
        {
            if (parameters == null)
            {
                return null;
            }

            var value = parameters.Get(parameterName);

            return value.IsNullOrWhiteSpace() ? null : value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection requestParameters)
        {
            if (requestParameters == null || requestParameters.Count == 0)
            {
                return null;
            }

            // var stringBuilder = new StringBuilder("?");
            //
            // for (var index = 0; index < requestParameters.Count; ++index)
            // {
            //     stringBuilder
            //         .Append(requestParameters.AllKeys[index])
            //         .Append('=')
            //         .Append(Uri.EscapeDataString(requestParameters[index]))
            //         .Append('&');
            // }
            //
            // stringBuilder.Length -= 1;
            //
            // var queryString = stringBuilder.ToString();
            //
            // #if !(NET20)
            // stringBuilder.Clear();
            // #endif
            // stringBuilder = null;

            var queryString = string.Join("&", requestParameters.AllKeys.Select(k=> $"{k}={Uri.EscapeDataString(requestParameters[k])}"));
            
            return queryString;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this NameValueCollection nameValueCollection)
        {
            if (nameValueCollection != null)
            {
                var dict = new Dictionary<string, string>();
                foreach (var k in nameValueCollection.AllKeys)
                {
                    dict.Add(k, nameValueCollection[k]);
                }

                return dict;
            }

            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                return null;
            }
            
            var dict = new Dictionary<TKey, TValue>();
            var keyConverter = TypeDescriptor.GetConverter(typeof(TKey));
            var valueConverter = TypeDescriptor.GetConverter(typeof(TValue));

            foreach(string name in nameValueCollection)
            {
                TKey key = (TKey)keyConverter.ConvertFromString(name);
                TValue value = (TValue)valueConverter.ConvertFromString(nameValueCollection[name]);
                if (key != null)
                {
                    dict.Add(key, value);
                }
            }

            return dict;
        }
        
        internal static NameValueCollection SetInclude(this string[] arr)
        {
            if (arr is not {Length: > 0})
            {
                return null;
            }
            
            var nvc = new NameValueCollection();
                
            nvc.Set(RedmineKeys.INCLUDE, string.Join(",", arr));

            return nvc;
        }
        
         internal static NameValueCollection SetInclude(this NameValueCollection nvc, string[] arr)
        {
            if (arr is not {Length: > 0})
            {
                return nvc;
            }
            
            nvc ??= new NameValueCollection();
                
            nvc.Set(RedmineKeys.INCLUDE, string.Join(",", arr));

            return nvc;
        }
        
        internal static NameValueCollection SetOffset(this NameValueCollection nv, int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Offset should be greater or equal than 0 (zero)",nameof(offset));
            }
            
            nv ??= new NameValueCollection();

            nv.Set(RedmineKeys.OFFSET, offset.ToString(CultureInfo.InvariantCulture));

            return nv;
        }
        
        internal static NameValueCollection SetLimit(this NameValueCollection nv, int limit)
        {
            if (limit <= 0)
            {
                throw new ArgumentException("Limit should be greater than 0 (zero)", nameof(limit));
            }
            
            nv ??= new NameValueCollection();

            nv.Set(RedmineKeys.LIMIT, limit.ToString(CultureInfo.InvariantCulture));

            return nv;
        }

        internal static int GetLimitOrDefault(this NameValueCollection nvc, int defaultValue)
        {
            if (nvc == null)
            {
                return 25;
            }
            
            if (!int.TryParse(nvc[RedmineKeys.LIMIT], out var limit))
            {
                return 25;
            }
            
            if (limit <= 0 || defaultValue <= 0)
            {
                return 25;
            }
       
            return limit;
        }
        
        internal static int GetOffsetOrDefault(this NameValueCollection nvc)
        {
            if (nvc == null)
            {
                return 0;
            }

            if (!int.TryParse(nvc[RedmineKeys.OFFSET], out var offset))
            {
                return offset;
            }
            
            return offset < 0 ? default : offset;
        }
    }
}