namespace Xfsm.Core.Enums
{
    /// <summary>
    /// The xfsm element in a specific state has an internal workflow related to extraction (peek) and elaboration.
    /// </summary>
    public enum XfsmPeekStatus : byte
    {
        /// <summary>
        /// Element inserted and ready to be elaborated
        /// </summary>
        Todo = 0,

        /// <summary>
        /// Element extracted and under elaboration
        /// </summary>
        Progress = 1,

        /// <summary>
        /// Element successfully elaborated
        /// </summary>
        Done = 2,

        /// <summary>
        /// Element elaborated with errors
        /// </summary>
        Error = 3
    }
}
