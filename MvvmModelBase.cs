using CommunityToolkit.Mvvm.Messaging;

namespace MVVM
{
    /// <summary>
    /// Base class where you want to use a custome message class that inheriits from GenericPropertyMessage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelBase<T> where T : GenericPropertyMessage, new()
    {
        /// <summary>
        /// Override with a new message type if need be.
        /// </summary>
        /// <param name="modelPropertyName"></param>
        public virtual void OnModelChanged(params string[] modelPropertyNames)
        {
            WeakReferenceMessenger.Default.Send<T>(new T() { Sender = this.GetType().Name, PropertyNames = modelPropertyNames });
            //List<string> propertyNames = new List<string>(moreModelPropertyName);
            //OnModelChanged(propertyNames.ToArray());
        }

        public virtual void OnModelChanged(string modelPropertyName)
        {
            OnModelChanged(new string[] { modelPropertyName });
            //WeakReferenceMessenger.Default.Send<T>(new T() { Sender = this.GetType().Name, PropertyNames = modelPropertyNames });
        }

        public virtual void OnModelChanged()
        {
            OnModelChanged("");
        }

        public virtual void OnModelChangedAll()
        {
            WeakReferenceMessenger.Default.Send<T>(new T() { Sender = this.GetType().Name, UpdateAll = true, PropertyNames = Array.Empty<string>() });
        }
    }

    /// <summary>
    /// Base class that uses GenericPropertyMessage.
    /// You can let the VM filter by sender to prevent confusion between different senders.
    /// </summary>
    public class ModelBase : ModelBase<GenericPropertyMessage>
    {
    }
}
