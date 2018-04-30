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
    public partial class User : IEquatable<User>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        [JsonConstructor]
        protected User() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        public User(UserInfo UserInfo = default, string OpenId = default)
        {
            // to ensure "UserInfo" is required (not null)
            this.UserInfo = UserInfo ?? throw new InvalidDataException("UserInfo is a required property for User and cannot be null");
            // to ensure "OpenId" is required (not null)
            this.OpenId = OpenId ?? throw new InvalidDataException("UserInfo is a required property for User and cannot be null");
        }

        /// <summary>
        /// 用户昵称
        /// </summary>
        /// <value>用户昵称</value>
        [DataMember(Name = "userInfo", EmitDefaultValue = false)]
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 已通过OAuth授权的用户账号ID
        /// </summary>
        /// <value>已通过OAuth授权的用户账号ID</value>
        [DataMember(Name = "openId", EmitDefaultValue = false)]
        public string OpenId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserInfo {\n");
            sb.Append("  UserInfo: ").Append(UserInfo).Append("\n");
            sb.Append("  OpenId: ").Append(OpenId).Append("\n");
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
            return this.Equals(input as User);
        }

        /// <summary>
        /// Returns true if OperationResult instances are equal
        /// </summary>
        /// <param name="input">Instance of OperationResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(User input)
        {
            if (input == null)
                return false;

            return
                (
                    this.UserInfo == input.UserInfo ||
                    (this.UserInfo != null &&
                    this.UserInfo.Equals(input.UserInfo))
                ) &&
                (
                    this.OpenId == input.OpenId ||
                    (this.OpenId != null &&
                    this.OpenId.Equals(input.OpenId))
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
                if (this.UserInfo != null)
                    hashCode = hashCode * 59 + this.UserInfo.GetHashCode();
                if (this.OpenId != null)
                    hashCode = hashCode * 59 + this.OpenId.GetHashCode();
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