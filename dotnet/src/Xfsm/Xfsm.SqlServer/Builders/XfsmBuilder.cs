using Xfsm.Core;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;

namespace Xfsm.SqlServer.Builders
{
    public static class XfsmBuilder
    {
        public static XfsmProcessor<T> BuildProcessor<T>(string connectionString, XfsmPeekMode mode, IXfsmState<T> state)
        {
            var provider = new XfsmDatabaseProvider(connectionString);
            var bag = new XfsmBag<T>(provider, mode);
            return new XfsmProcessor<T>(bag, state);
        }

        public static XfsmAppender<T> BuildAppender<T>(string connectionString, XfsmPeekMode mode)
        {
            var provider = new XfsmDatabaseProvider(connectionString);
            var bag = new XfsmBag<T>(provider, mode);
            return new XfsmAppender<T>(bag);
        }
    }
}
