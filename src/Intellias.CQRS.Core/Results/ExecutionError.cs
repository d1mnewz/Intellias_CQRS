﻿using Intellias.CQRS.Core.Results.Errors;
using Newtonsoft.Json;

namespace Intellias.CQRS.Core.Results
{
    /// <summary>
    /// ExecutionError.
    /// </summary>
    public class ExecutionError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionError"/> class.
        /// </summary>
        /// <param name="errorCodeInfo">Error Code Info.</param>
        public ExecutionError(ErrorCodeInfo errorCodeInfo)
        {
            CodeInfo = errorCodeInfo;
            Code = errorCodeInfo.Code;
            Message = errorCodeInfo.Message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionError"/> class.
        /// </summary>
        /// <param name="errorCodeInfo">Error Code Info.</param>
        /// <param name="source">Source.</param>
        public ExecutionError(ErrorCodeInfo errorCodeInfo, string source)
        {
            CodeInfo = errorCodeInfo;
            Source = source;
            Code = errorCodeInfo.Code;
            Message = errorCodeInfo.Message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionError"/> class.
        /// </summary>
        /// <param name="errorCodeInfo">Error Code Info.</param>
        /// <param name="source">Source.</param>
        /// <param name="customMessage">Custom message.</param>
        public ExecutionError(ErrorCodeInfo errorCodeInfo, string source, string customMessage)
        {
            CodeInfo = errorCodeInfo;
            Source = source;
            Code = errorCodeInfo.Code;
            Message = customMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionError"/> class.
        /// </summary>
        [JsonConstructor]
        protected ExecutionError()
        {
        }

        /// <summary>
        /// Error code.
        /// </summary>
        [JsonProperty]
        public ErrorCodeInfo CodeInfo { get; protected set; }

        /// <summary>
        /// Error code.
        /// </summary>
        [JsonProperty]
        public string Code { get; protected set; } = string.Empty;

        /// <summary>
        /// Error Source, optional.
        /// </summary>
        [JsonProperty]
        public string Source { get; protected set; } = string.Empty;

        /// <summary>
        /// Reason of failure.
        /// </summary>
        [JsonProperty]
        public string Message { get; protected set; } = string.Empty;

        /// <summary>
        /// ErrorMessage.
        /// </summary>
        /// <returns>Converted String.</returns>
        public override string ToString()
        {
            return $"{Code}: '{Message}'";
        }
    }
}
