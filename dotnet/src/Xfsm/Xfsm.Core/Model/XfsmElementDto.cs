using System;
using Xfsm.Core.Enums;

namespace Xfsm.Core.Model
{
    internal class XfsmElementDto
    {
        public long Id { get; set; }
        public DateTimeOffset InsertedTimestamp { get; set; }
        public DateTimeOffset UpdatedTimestamp { get; set; }
        public DateTimeOffset? PeekTimestamp { get; set; }
        public int State { get; set; }
        public XfsmPeekStatus PeekStatus { get; set; }
        public string Error { get; set; }
    }
}
