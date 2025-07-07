using CommunityToolkit.Mvvm.Messaging;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MileHighWpf.MvvmModelMessaging
{
    /// <summary>
    /// Base class where you want to use a GenericPropertyMessage message class, or one that inherits from it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelBase<T> where T : ModelDependentMessage, new()
    {
        private static int _TrackingIndexCounter;
        private readonly int _hashCode;
        private readonly DateTime _creationTick = DateTime.Now;

        [JsonIgnore]
        public int TrackingIndex { get; } = Interlocked.Increment(ref _TrackingIndexCounter);

        public ModelBase()
        {
            int hash = 17;
            hash = (hash * 7) + _creationTick.GetHashCode();
            hash = (hash * 7) + TrackingIndex.GetHashCode();
            _hashCode = hash;
        }
        public override int GetHashCode() => _hashCode;

        public override string ToString()
        {
            return $"ModelBase<{typeof(T).Name}>, {TrackingIndex}, {_hashCode}";
        }

        /// <summary>
        /// Send update for all properties tagged as ModelDependent and with these property names.
        /// Override for a new message type if need be.
        /// </summary>
        /// <param name="modelPropertyName"></param>
        protected virtual void SendModelUpdate(params string[] modelPropertyNames)
        {
            WeakReferenceMessenger.Default.Send<T>(new T() 
            { 
                ModelSenderName = GetType().FullName, 
                ModelPropertyNames = modelPropertyNames,
                ModelHashCode = GetHashCode()
            });
        }

        /// <summary>
        /// Send update for all properties tagged ModelDependent and with these property names.
        /// </summary>
        /// <param name="modelPropertyName"></param>
        protected void SendModelUpdate(string modelPropertyName)
        {
            SendModelUpdate(new string[] { modelPropertyName });
        }


        /// <summary>
        /// Send update for all properties tagged ModelDependent but no property name was specified.
        /// </summary>
        protected void SendModelUpdate()
        {
            SendModelUpdate("");
        }

        /// <summary>
        /// Send update for all properties tagged ModelDependent regardless if they have any property names assigned or not.
        /// </summary>
        protected void SendModelUpdateAll()
        {
            SendModelUpdate(ModelDependentMessage.UpdateAllCode);
        }

        /// <summary>
        /// Method to both update a property backing field and send an update for all properties 
        /// tagged ModelDependent and this property name, but only if the property chaned value.
        /// Use the return value to decide if you need to take further action.
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

        public Type GetMessageType()
        {
            return typeof(T);
        }
    }

    /// <summary>
    /// Base class that uses GenericPropertyMessage.
    /// You can let the receiving ViewModel filter by the message sender to prevent confusion between different models senders.
    /// </summary>
    public class ModelBase : ModelBase<ModelDependentMessage>
    {
    }
}
