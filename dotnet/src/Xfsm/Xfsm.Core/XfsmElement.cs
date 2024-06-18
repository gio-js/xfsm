using Newtonsoft.Json;
using System;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.Core.Model;

namespace Xfsm.Core
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    internal class XfsmElement<T> : IXfsmElement<T>
    {
        private readonly XfsmElementDto element;
        private readonly XfsmBusinessElementDto businessElementDto;
        private T businessElement;

        public XfsmElement(XfsmElementDto element, XfsmBusinessElementDto businessElementDto)
        {
            this.element = element;
            this.businessElementDto = businessElementDto;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public T GetBusinessElement()
        {
            if (businessElement == null && !string.IsNullOrEmpty(businessElementDto.JsonData))
            {
                businessElement = JsonConvert.DeserializeObject<T>(businessElementDto.JsonData);
            }

            return businessElement;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string GetError()
        {
            return element.Error;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public long GetId()
        {
            return element.Id;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTimeOffset GetInsertedTimestamp()
        {
            return element.InsertedTimestamp;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTimeOffset GetLastUpdateTimestamp()
        {
            return element.UpdatedTimestamp;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTimeOffset? GetPeekedTimestamp()
        {
            return element.PeekTimestamp;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public XfsmPeekStatus GetPeekStatus()
        {
            return element.PeekStatus;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int GetState()
        {
            return element.State;
        }
    }
}
