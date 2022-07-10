/*
   Copyright 2011 - 2022 Adrian Popescu.

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


namespace Redmine.dotNet.Api.Types
{
    public class IdentifiableName : Identifiable<IdentifiableName>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Create<T>(int id) where T: IdentifiableName, new()
        { 
            var t = new T {Id = id};
            return t;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableName"/> class.
        /// </summary>
        public IdentifiableName() { }

        public IdentifiableName(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes the class by using the given Id and Name.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="name">The Name.</param>
        internal IdentifiableName(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #region Properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public virtual string Name { get; protected internal set; }
        #endregion
    }
}