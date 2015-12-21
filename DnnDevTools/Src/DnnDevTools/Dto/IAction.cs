using System;

namespace weweave.DnnDevTools.Dto
{
    public interface IAction
    {
        string Type { get; }

        string Id { get; }

        /// <summary>
        /// Time stamp when the notification was created
        /// </summary>
        DateTime TimeStamp { get; }
    }
}