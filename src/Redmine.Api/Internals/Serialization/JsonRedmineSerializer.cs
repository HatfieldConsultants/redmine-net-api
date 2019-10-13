using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Redmine.Api;
using Redmine.Api.Exceptions;
using Redmine.Api.Extensions;

namespace Redmine.Api.Internals.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class JsonRedmineSerializer : IRedmineSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        public T Deserialize<T>(string jsonResponse) where T : new()
        {
            if (jsonResponse.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(jsonResponse), $"Could not deserialize null or empty input for type '{typeof(T).Name}'.");
            }

            var isJsonSerializable = typeof(IJsonSerializable).IsAssignableFrom(typeof(T));

            if (!isJsonSerializable)
            {
                throw new RedmineException($"Entity of type '{typeof(T)}' should implement IJsonSerializable.");
            }

            using (var stringReader = new StringReader(jsonResponse))
            {
                using (JsonReader jsonReader = new JsonTextReader(stringReader))
                {
                    var obj = Activator.CreateInstance<T>();

                    if (jsonReader.Read())
                    {
                        if (jsonReader.Read())
                        {
                            ((IJsonSerializable)obj).ReadJson(jsonReader);
                        }
                        else
                        {
                            //TODO: throw exception
                        }
                    }

                    return obj;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        public PagedResults<T> DeserializeToPagedResults<T>(string jsonResponse) where T : class, new()
        {
            if (jsonResponse.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(jsonResponse), $"Could not deserialize null or empty input for type '{typeof(T).Name}'.");
            }

            using (var sr = new StringReader(jsonResponse))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var total = 0;
                    var offset = 0;
                    var limit = 0;
                    List<T> list = null;

                    while (reader.Read())
                    {
                        if (reader.TokenType != JsonToken.PropertyName) continue;

                        switch (reader.Value)
                        {
                            case RedmineKeys.TOTAL_COUNT:
                                total = reader.ReadAsInt32().GetValueOrDefault();
                                break;
                            case RedmineKeys.OFFSET:
                                offset = reader.ReadAsInt32().GetValueOrDefault();
                                break;
                            case RedmineKeys.LIMIT:
                                limit = reader.ReadAsInt32().GetValueOrDefault();
                                break;
                            default:
                                list = reader.ReadAsCollection<T>();
                                break;
                        }
                    }

                    return new PagedResults<T>(list, total, offset, limit);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        public int Count<T>(string jsonResponse) where T : class, new()
        {
            if (jsonResponse.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(jsonResponse), $"Could not deserialize null or empty input for type '{typeof(T).Name}'.");
            }

            using (var sr = new StringReader(jsonResponse))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var total = 0;

                    while (reader.Read())
                    {
                        if (reader.TokenType != JsonToken.PropertyName)
                        {
                            continue;
                        }

                        if (reader.Value is RedmineKeys.TOTAL_COUNT)
                        {
                            total = reader.ReadAsInt32().GetValueOrDefault();
                            return total;
                        }
                    }

                    return total;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Serialize<T>(T entity) where T : class
        {
            if (entity == default(T))
            {
                throw new ArgumentNullException(nameof(entity), $"Could not serialize null of type {typeof(T).Name}");
            }

            var jsonSerializable = entity as IJsonSerializable;

            if (jsonSerializable == null)
            {
                throw new RedmineException($"Entity of type '{typeof(T)}' should implement IJsonSerializable.");
            }

            var stringBuilder = new StringBuilder();

            using (var sw = new StringWriter(stringBuilder))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.DateFormatHandling = DateFormatHandling.IsoDateFormat;

                    jsonSerializable.WriteJson(writer);

                    var json = stringBuilder.ToString();

#if NET20
                    stringBuilder = null;
#else
                    stringBuilder.Clear();
#endif
                    return json;
                }
            }
        }
    }
}
