using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MileHighWpf.MvvmModelMessaging;
using System.Reflection;

namespace MileHighWpf.MvvmModelMessaging
{
    /// <summary>
    /// Attribute to mark a property in a ViewModel so that when it is changed by a message from the Model, the specificed set of command Can...Execute methods are run to update the command state.
    /// This atribute is only allowed if the ViewModel property is also attributed with [ModelDependent].
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModelDependentCallCanExecuteAttribute : Attribute
    {
        public string[] RelatedCommandCanExecuteNames { get; private set; } = Array.Empty<String>();

        public ModelDependentCallCanExecuteAttribute() { }

        public ModelDependentCallCanExecuteAttribute(params string[] modelPropertyNames)
        {
            RelatedCommandCanExecuteNames = modelPropertyNames;
        }

        public ModelDependentCallCanExecuteAttribute(string propertyName) : this([propertyName]) { }

        /// <summary>
        /// Get the list of Relay commands to be upddated for a given property.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static Dictionary<string, IRelayCommand[]> BuildCommandUpdateMap(ObservableRecipient viewModel)
        {
            // For each relay command there are a list of properties that will cause it to have CanExecute updated.
            Dictionary<string, List<IRelayCommand>> commandMap = new Dictionary<string, List<IRelayCommand>>();

            // Set the default Key for no specified Model properties.
            var props = viewModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            Dictionary<string, IRelayCommand> relayCommands = GetRelayCommands(viewModel);

            foreach (var vmProp in props)
            {
                ModelDependentAttribute? dependentAttribute = vmProp.GetCustomAttribute(typeof(ModelDependentAttribute)) as ModelDependentAttribute;
                ModelDependentCallCanExecuteAttribute? canExecuteAttribute = vmProp.GetCustomAttribute(typeof(ModelDependentCallCanExecuteAttribute)) as ModelDependentCallCanExecuteAttribute;
                if (canExecuteAttribute != null)
                {
                    if (dependentAttribute == null)
                    {
                        System.Diagnostics.Trace.WriteLine($"Warning: [ModelDependendCanExecute] requires [ModelDependent] on property {viewModel.ToString()}.{vmProp.Name}.");
                    }
                    // Now record specific VM properties to update for a given Model property message.
                    if (canExecuteAttribute.RelatedCommandCanExecuteNames.Length != 0)
                    {
                        if (!commandMap.ContainsKey(vmProp.Name))
                        {
                            commandMap.Add(vmProp.Name, new List<IRelayCommand>());
                        }
                        foreach (string relayCommandName in canExecuteAttribute.RelatedCommandCanExecuteNames)
                        {
                            if (relayCommands.ContainsKey(relayCommandName))
                            {
                                commandMap[vmProp.Name].Add(relayCommands[relayCommandName]);
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine($"Warning: [ModelDependendCanExecute] command name '{relayCommandName}' on property {viewModel.ToString()}.{vmProp.Name} is not recognized.");
                            }
                        }
                    }
                }
            }
            return commandMap.ToDictionary(k => k.Key, v => v.Value.ToArray());
        }

        public static Dictionary<string, IRelayCommand> GetRelayCommands(object obj)
        {
            var commands = new Dictionary<string, IRelayCommand>();

            // Get all properties in the object
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if ((property.PropertyType == typeof(IRelayCommand)) ||
                    (property.PropertyType == typeof(IAsyncRelayCommand)))
                {
                    // Get the value of the property and add to list
                    if (property.GetValue(obj) is IRelayCommand command)
                    {
                        commands.Add(property.Name, command);
                    }
                }
            }
            return commands;
        }
    }
}
