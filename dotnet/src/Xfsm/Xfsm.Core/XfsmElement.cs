using Newtonsoft.Json;
using System;
using Xfsm.Core.Enums;
using Xfsm.Core.Model;

namespace Xfsm.Core.Interfaces
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
            if (this.businessElement == null && !string.IsNullOrEmpty(this.businessElementDto.JsonData))
            {
                this.businessElement = JsonConvert.DeserializeObject<T>(this.businessElementDto.JsonData);
            }

            return this.businessElement;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string GetError()
        {
            return this.element.Error;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public long GetId()
        {
            return this.element.Id;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTimeOffset GetInsertedTimestamp()
        {
            return this.element.InsertedTimestamp;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTimeOffset GetLastUpdateTimestamp()
        {
            return this.element.UpdatedTimestamp;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTimeOffset? GetPeekedTimestamp()
        {
            return this.element.PeekTimestamp;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public XfsmPeekStatus GetPeekStatus()
        {
            return this.element.PeekStatus;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int GetState()
        {
            return this.element.State;
        }
    }
}
