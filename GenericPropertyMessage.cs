namespace MVVM
{
    /// <summary>
    /// Basic message for property updates.
    /// </summary>
    public class GenericPropertyMessage
    {
        /// <summary>
        /// Names of RelayCommands or properties to update in response to this message.
        /// </summary>
        public string[] PropertyNames { get; set; }
        /// <summary>
        /// Use GetType().Name here
        /// </summary>
        public string Sender { get; set; } 
        public bool UpdateAll { get; set; }
    }
}
