using System.Linq.Expressions;
using System.Reflection;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Test.Builder
{
    public class Element<T> : IXfsmElement<T>
    {
        public static Element<T> Build()
        {
            return new Element<T>();
        }

        public Element<T> With<TOut>(Expression<Func<Element<T>, TOut>> expr, object value)
        {
            var member = expr.Body as MemberExpression;
            var prop = member?.Member as PropertyInfo;

            if (prop != null)
            {
                prop.SetValue(this, value);
            }

            return this;
        }

        public T BusinessElement { get; set; }
        public T GetBusinessElement()
        {
            return this.BusinessElement;
        }

        public string Error { get; set; }
        public string GetError()
        {
            return Error;
        }

        public long Id { get; set; }
        public long GetId()
        {
            return Id;
        }

        public DateTimeOffset InsertedTimestamp { get; set; }
        public DateTimeOffset GetInsertedTimestamp()
        {
            return InsertedTimestamp;
        }

        public DateTimeOffset LastUpdateTimestamp { get; set; }
        public DateTimeOffset GetLastUpdateTimestamp()
        {
            return LastUpdateTimestamp;
        }

        public DateTimeOffset? PeekedTimestamp { get; set; }
        public DateTimeOffset? GetPeekedTimestamp()
        {
            return PeekedTimestamp;
        }

        public XfsmPeekStatus PeekStatus { get; set; }
        public XfsmPeekStatus GetPeekStatus()
        {
            return PeekStatus;
        }

        public int State { get; set; }
        public int GetState()
        {
            return State;
        }
    }
}
