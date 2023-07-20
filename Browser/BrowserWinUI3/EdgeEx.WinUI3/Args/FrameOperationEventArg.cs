using EdgeEx.WinUI3.Enums;

namespace EdgeEx.WinUI3.Args
{
    public class FrameOperationEventArg
    {
        public string PersistenceId { get; set; }
        public string TabItemName { get; set; }
        public FrameOperation Operation { get; set; }
        public object Source { get; set; }
        public FrameOperationEventArg(string persistenceId, string tabItemName, FrameOperation operation, object source)
        {
            Operation = operation;
            TabItemName = tabItemName;
            PersistenceId = persistenceId;
            Source = source;
        }
    }
}
