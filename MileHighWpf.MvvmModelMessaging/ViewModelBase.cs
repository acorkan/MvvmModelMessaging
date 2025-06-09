using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace MileHighWpf.MvvmModelMessaging
{
    public class ViewModelBase<T> : ObservableRecipient where T : ModelDependentMessage, new()
    {
        private static int _TrackingIndexCounter;
        private readonly int _hashCode;
        private readonly DateTime _creationTick = DateTime.Now;
        public int TrackingIndex { get; } = Interlocked.Increment(ref _TrackingIndexCounter);

        /// <summary>
        /// Set to true to see the map of messages.
        /// If you plan to use this feature then this must be set before any ViewModels are created.
        /// </summary>
        public static bool TraceMessagesOn { get; set; }

        protected readonly Dictionary<string, string[]> _propertyUpdateMap;
        private readonly Dictionary<string, IRelayCommand[]> _commandUpdateMap;
        /// <summary>
        /// Use this to filter unwanted senders.
        /// If this is empty then all senders of the registered message type will trigger an update.
        /// If this contans any sender namers then ONLY messages from thoes senders will be processed.
        /// </summary>
        protected List<string> AllowedMessageSenders { get; } = new List<string>();

        public override int GetHashCode() => _hashCode;

        public override string ToString()
        {
            return $"{GetType().Name}<{typeof(T).Name}>, {TrackingIndex}, {_hashCode}";
        }

        protected ViewModelBase()
        {
            int hash = 17;
            hash = (hash * 7) + _creationTick.GetHashCode();
            hash = (hash * 7) + TrackingIndex.GetHashCode();
            _hashCode = hash;

            _propertyUpdateMap = ModelDependentAttribute.BuildPropertyUpdateMap(this);
            _commandUpdateMap = ModelDependentCallCanExecuteAttribute.BuildCommandUpdateMap(this);

            RegisterMessage();
            if (TraceMessagesOn)
            {
                System.Diagnostics.Trace.WriteLine($"VM {this} model message mapping...");
                foreach (KeyValuePair<string, string[]> kvp in _propertyUpdateMap)
                {
                    if (kvp.Key == "")
                    {
                        System.Diagnostics.Trace.WriteLine($"   Any model property change message will update VM properties...");
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine($"   Model property '{kvp.Key}' change message will update VM properties...");
                    }
                    foreach (string vmProp in kvp.Value)
                    {
                        System.Diagnostics.Trace.WriteLine($"      " + vmProp);
                    }
                }
                foreach (KeyValuePair<string, IRelayCommand[]> kvp in _commandUpdateMap)
                {
                    System.Diagnostics.Trace.WriteLine($"   Model property '{kvp.Key}' change message will update VM properties...");
                    foreach (IRelayCommand command in kvp.Value)
                    {
                        System.Diagnostics.Trace.WriteLine($"      " + command.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Override this to customize the message type, otherwise GenericPropertyMessage will be used.
        /// Be sure to use the same message type that the Model will be using!
        /// </summary>
        protected virtual void RegisterMessage()
        {
            Messenger.Register<ViewModelBase<T>, T>(this, (recipient, message) =>
            {
                OnModelUpdated(message); // Update the Name property with message content
            });
        }

        /// <summary>
        /// This will process the message and update accordingly.
        /// If any sender names are in AllowedMessageSenders then only thoes senders are processed, 
        /// but if empty then all senders are processed.
        /// If the update flag is set then all VM properties are updated.
        /// Otherwise only the non-specific properties and the ones tagged by the properties in the menu are updated.
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnModelUpdated(ModelDependentMessage message)
        {
            // If anything is in AllowedMessageSenders then only those things can be processed.
            if ((AllowedMessageSenders.Count != 0) && !AllowedMessageSenders.Contains(message.ModelSenderName))
            {
                Trace.WriteLineIf(TraceMessagesOn, $"{this} model message mapping is ignorring sender '{message.ModelSenderName}' because it is restricted to senders(s): " +
                    string.Join(", ", AllowedMessageSenders));
                return;
            }
            // Start with a list of the non-specific properties.
            List<string> vmPropsToUpdate = _propertyUpdateMap[""].ToList();
            // If flag is set then we update all properties.
            if ((message.ModelPropertyNames != null) &&
                (message.ModelPropertyNames.Length == 1) &&
                (message.ModelPropertyNames[0] == ModelDependentMessage.UpdateAllCode))
            {
                foreach (KeyValuePair<string, string[]> kvp in _propertyUpdateMap)
                {
                    vmPropsToUpdate.AddRange(kvp.Value);
                }
                Trace.WriteLineIf(TraceMessagesOn, $"M {this} model message sender '{message.ModelSenderName}' notified All property updates:");
            }
            else if ((message.ModelPropertyNames != null) &&
                (message.ModelPropertyNames.Length != 0))
            {
                foreach (string modelPropertyName in message.ModelPropertyNames)
                {
                    if (_propertyUpdateMap.ContainsKey(modelPropertyName))
                    {
                        vmPropsToUpdate.AddRange(_propertyUpdateMap[modelPropertyName]);
                    }
                }
                Trace.WriteLineIf(TraceMessagesOn, $"{this} model message sender '{message.ModelSenderName}' notified property updates: " +
                    string.Join(", ", message.ModelPropertyNames));
            }
            else
            {
                Trace.WriteLineIf(TraceMessagesOn, $"{this} model message sender '{message.ModelSenderName}' notified non-specific property updates:");
            }
            // Call update once on each mapped VM property.
            if (vmPropsToUpdate.Count != 0)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (string vmPropName in vmPropsToUpdate.Distinct())
                        {
                            Trace.WriteLineIf(TraceMessagesOn, "   " + vmPropName);
                            OnPropertyChanged(vmPropName);
                            if (_commandUpdateMap.ContainsKey(vmPropName))
                            {
                                foreach (IRelayCommand command in _commandUpdateMap[vmPropName])
                                {
                                    command.NotifyCanExecuteChanged();
                                }
                            }
                        }
                    });
                }
                catch(Exception ex)
                {

                }
            }
        }
    }

    public class ViewModelBase : ViewModelBase<ModelDependentMessage> { }
}
