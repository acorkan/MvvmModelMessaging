namespace MileHighWpf.MvvmModelMessaging
{
    /// <summary>
    /// Basic message for notifying of model property updates.
    /// The sender is only identified by a string and multiple property notifications can be sent at once.
    /// </summary>
    public class ModelDependentMessage
    {
        /// <summary>
        /// Code to trigger an update of all model dependent ViewModel properties.
        /// </summary>
        public static readonly string UpdateAllCode = "@ChangeAll";
        /// <summary>
        /// Names of properties in the model that have changed and now need to be updated in the ViewModel.
        /// </summary>
        public string[] PropertyNames { get; set; }
        /// <summary>
        /// Name of the sending object, which can be used to filter the message.
        /// Typically use GetType().Name for this.
        /// </summary>
        public string Sender { get; set; }
    }
}
