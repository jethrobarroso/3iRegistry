namespace _3iRegistry.WPF.Messages
{
    public class ModifySubItemMessage
    {
        public ModifySubItemMessage() { }

        public ModifySubItemMessage(object subObject, MemberOperation operation)
        {
            SubObject = subObject;
            Operation = operation;
        }

        public object SubObject { get; set; }
        public MemberOperation Operation { get; set; }
    }
}
