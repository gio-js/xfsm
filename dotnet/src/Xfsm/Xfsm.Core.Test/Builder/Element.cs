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

        private T businessElement = default(T);
        public T GetBusinessElement()
        {
            return this.businessElement;
        }

        public Element<T> SetBusinessElement(T businessElement)
        {
            this.businessElement = businessElement;
            return this;
        }

        public string GetError()
        {
            throw new NotImplementedException();
        }

        public long GetId()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetInsertedTimestamp()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastUpdateTimestamp()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset? GetPeekedTimestamp()
        {
            throw new NotImplementedException();
        }

        public XfsmPeekStatus GetPeekStatus()
        {
            throw new NotImplementedException();
        }

        public int GetState()
        {
            throw new NotImplementedException();
        }
    }
}
