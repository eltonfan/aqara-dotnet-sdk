#region License

//   Copyright 2014 Elton FAN (eltonfan@live.cn, http://elton.io)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 

#endregion

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Elton.Phantom.Models.Version1
{
    /// <summary>
    /// 关闭某个门锁
    /// </summary>
    [DataContract]
    public partial class OperationResult :  IEquatable<OperationResult>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        [JsonConstructor]
        protected OperationResult() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        /// <param name="Success">操作成功与否 (required).</param>
        /// <param name="Reason">若操作失败，返回失败原因.</param>
        public OperationResult(bool? Success = default, string Reason = default)
        {
            // to ensure "Success" is required (not null)
            this.Success = Success ?? throw new InvalidDataException("Success is a required property for OperationResult and cannot be null");
            this.Reason = Reason;
        }
        
        /// <summary>
        /// 操作成功与否
        /// </summary>
        /// <value>操作成功与否</value>
        [DataMember(Name="success", EmitDefaultValue=false)]
        public bool Success { get; set; }

        /// <summary>
        /// 若操作失败，返回失败原因
        /// </summary>
        /// <value>若操作失败，返回失败原因</value>
        [DataMember(Name="reason", EmitDefaultValue=false)]
        public string Reason { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class OperationResult {\n");
            sb.Append("  Success: ").Append(Success).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as OperationResult);
        }

        /// <summary>
        /// Returns true if OperationResult instances are equal
        /// </summary>
        /// <param name="input">Instance of OperationResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(OperationResult input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Success == input.Success ||
                    (this.Success != null &&
                    this.Success.Equals(input.Success))
                ) && 
                (
                    this.Reason == input.Reason ||
                    (this.Reason != null &&
                    this.Reason.Equals(input.Reason))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Success != null)
                    hashCode = hashCode * 59 + this.Success.GetHashCode();
                if (this.Reason != null)
                    hashCode = hashCode * 59 + this.Reason.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
