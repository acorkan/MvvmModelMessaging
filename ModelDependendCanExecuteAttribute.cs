using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;

namespace MVVM
{
    [AttributeUsage(AttributeTargets.Property)]

    public sealed class ModelDependendCanExecuteAttribute : Attribute
    {
        public string[] PropertyNames { get; private set; } = Array.Empty<String>();

        public ModelDependendCanExecuteAttribute() { }

        public ModelDependendCanExecuteAttribute(params string[] modelPropertyNames)
        {
            PropertyNames = modelPropertyNames;
        }

        public ModelDependendCanExecuteAttribute(string propertyName) : this([propertyName]) { }

        /// <summary>
        /// Get the list of Relay commands to be upddated for a given property.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static Dictionary<string, CommunityToolkit.Mvvm.Input.IRelayCommand[]> BuildCommandUpdateMap(ObservableRecipient viewModel)
        {
            //Dictionary<string, List<string>> propertyMap = new Dictionary<string, List<string>>();
            // For each relay command there are a list of properties that will cause it to have CanExecute updated.
            Dictionary<string, List<CommunityToolkit.Mvvm.Input.IRelayCommand>> commandMap = new Dictionary<string, List<CommunityToolkit.Mvvm.Input.IRelayCommand>>();

            // Set the default Key for no specified Model properties.
            //var props = viewModel.GetType().GetProperties();
            var props = viewModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            Dictionary<string, CommunityToolkit.Mvvm.Input.IRelayCommand> relayCommands = GetRelayCommands(viewModel);

            foreach (var vmProp in props)
            {
                ModelDependentAttribute? dependentAttribute = vmProp.GetCustomAttribute(typeof(ModelDependentAttribute)) as ModelDependentAttribute;
                ModelDependendCanExecuteAttribute? canExecuteAttribute = vmProp.GetCustomAttribute(typeof(ModelDependendCanExecuteAttribute)) as ModelDependendCanExecuteAttribute;
                if (canExecuteAttribute != null)
                {
                    if(dependentAttribute == null)
                    {
                        System.Diagnostics.Trace.WriteLine($"Warning: [ModelDependendCanExecute] requires [ModelDependent] on property {viewModel.ToString()}.{vmProp.Name}");
                    }
                    // Now record specific VM properties to update for a given Model property message.
                    if (canExecuteAttribute.PropertyNames.Length != 0)
                    {
                        if (!commandMap.ContainsKey(vmProp.Name))
                        {
                            commandMap.Add(vmProp.Name, new List<CommunityToolkit.Mvvm.Input.IRelayCommand>());
                        }
                        foreach (string relayCommandName in canExecuteAttribute.PropertyNames)
                        {
                            if(relayCommands.ContainsKey(relayCommandName))
                            {
                                commandMap[vmProp.Name].Add(relayCommands[relayCommandName]);
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine($"Warning: [ModelDependendCanExecute] command name '{relayCommandName}' on property {viewModel.ToString()}.{vmProp.Name} is not recognized");
                            }
                        }
                    }
                }
            }
            return commandMap.ToDictionary(k => k.Key, v => v.Value.ToArray());
        }

        public static Dictionary<string, CommunityToolkit.Mvvm.Input.IRelayCommand> GetRelayCommands(object obj)
        {
            var commands = new Dictionary<string, CommunityToolkit.Mvvm.Input.IRelayCommand>();

            // Get all properties in the object
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(CommunityToolkit.Mvvm.Input.IRelayCommand))
                {
                    // Get the value of the property and add to list
                    if (property.GetValue(obj) is CommunityToolkit.Mvvm.Input.IRelayCommand command)
                    {
                        commands.Add(property.Name, command);
                    }
                }
            }

            return commands;
        }
    }
}
