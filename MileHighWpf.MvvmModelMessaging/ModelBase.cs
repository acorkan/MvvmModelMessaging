using CommunityToolkit.Mvvm.Messaging;
using System.Runtime.CompilerServices;

namespace MileHighWpf.MvvmModelMessaging
{
    /// <summary>
    /// Base class where you want to use a custome message class that inheriits from GenericPropertyMessage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelBase<T> where T : ModelDependentMessage, new()
    {
        /// <summary>
        /// Send update for all properties tagged ModelDependent with these property names.
        /// Override with a new message type if need be.
        /// </summary>
        /// <param name="modelPropertyName"></param>
        protected virtual void SendModelUpdate(params string[] modelPropertyNames)
        {
            WeakReferenceMessenger.Default.Send<T>(new T() { Sender = this.GetType().Name, PropertyNames = modelPropertyNames });
        } 

        /// <summary>
        /// Send update for all properties tagged ModelDependent and this property names.
        /// </summary>
        /// <param name="modelPropertyName"></param>
        protected void SendModelUpdate(string modelPropertyName)
        {
            SendModelUpdate(new string[] { modelPropertyName });
        }


        /// <summary>
        /// Send update for all properties tagged ModelDependent but no specific property name.
        /// </summary>
        protected void SendModelUpdate()
        {
            SendModelUpdate("");
        }

        /// <summary>
        /// Send update for all properties tagged ModelDependent, with or without specific property names.
        /// </summary>
        protected void SendModelUpdateAll()
        {
            SendModelUpdate(ModelDependentMessage.UpdateAllCode);
        }

        /// <summary>
        /// Send update for all properties tagged ModelDependent and this property name, but only if the property chaned value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool OnModelChangedSendUpdate<P>(ref P field, P value,
            [CallerMemberName] string propertyName = null)
        {
            if ((propertyName == null) || (EqualityComparer<P>.Default.Equals(field, value))) { return false; }
            field = value;
            SendModelUpdate(propertyName);
            return true;
        }
    }

    /// <summary>
    /// Base class that uses GenericPropertyMessage.
    /// You can let the VM filter by sender to prevent confusion between different senders.
    /// </summary>
    public class ModelBase : ModelBase<ModelDependentMessage>
    {
    }
}
