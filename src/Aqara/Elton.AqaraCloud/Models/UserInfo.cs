using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Elton.AqaraCloud.Models
{
    [DataContract]
    public partial class UserInfo : IEquatable<UserInfo>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        [JsonConstructor]
        protected UserInfo() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        public UserInfo(string NickName = default)
        {
            // to ensure "Success" is required (not null)
            this.NickName = NickName ?? throw new InvalidDataException("NickName is a required property for UserInfo and cannot be null");
        }

        /// <summary>
        /// 用户昵称
        /// </summary>
        /// <value>用户昵称</value>
        [DataMember(Name = "nickName", EmitDefaultValue = false)]
        public string NickName { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserInfo {\n");
            sb.Append("  NickName: ").Append(NickName).Append("\n");
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
            return this.Equals(input as UserInfo);
        }

        /// <summary>
        /// Returns true if OperationResult instances are equal
        /// </summary>
        /// <param name="input">Instance of OperationResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserInfo input)
        {
            if (input == null)
                return false;

            return
                (
                    this.NickName == input.NickName ||
                    (this.NickName != null &&
                    this.NickName.Equals(input.NickName))
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
                if (this.NickName != null)
                    hashCode = hashCode * 59 + this.NickName.GetHashCode();
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