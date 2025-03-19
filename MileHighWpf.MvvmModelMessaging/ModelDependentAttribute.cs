using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;

namespace MileHighWpf.MvvmModelMessaging
{
    /// <summary>
    /// Attribute to mark a property in a ViewModel as being bound (by messaging) to a set of properties in a Model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModelDependentAttribute : Attribute
    {
        /// <summary>
        /// The names of model properties that will trigger an update of a viewmodel property.
        /// </summary>
        public string[] ModelPropertyNames { get; private set; } = Array.Empty<String>();

        public ModelDependentAttribute() { }

        public ModelDependentAttribute(params string[] modelPropertyNames)
        {
            ModelPropertyNames = modelPropertyNames;
        }

        public ModelDependentAttribute(string propertyName) : this([propertyName]) { }

        /// <summary>
        /// Use this to pre-build a map to look up which ViewModel properties should be updated for a given Model property update.
        /// Model property is the Key, and the Value is the list of VM properties to call OnPropertyChanged() on.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static Dictionary<string, string[]> BuildPropertyUpdateMap(ObservableRecipient viewModel)
        {
            Dictionary<string, List<string>> propertyMap = new Dictionary<string, List<string>>();
            Dictionary<RelayCommand, List<string>> commandMap = new Dictionary<RelayCommand, List<string>>();

            // Set the default Key for no specified Model properties.
            propertyMap[""] = new List<string>();
            var props = viewModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var vmProp in props)
            {
                ModelDependentAttribute? attribute = vmProp.GetCustomAttribute(typeof(ModelDependentAttribute)) as ModelDependentAttribute;
                if (attribute != null)
                {
                    // Now record specific VM properties to update for a given Model property message.
                    if (attribute.ModelPropertyNames.Length != 0)
                    {
                        foreach (string modelPropName in attribute.ModelPropertyNames)
                        {
                            if (!propertyMap.ContainsKey(modelPropName))
                            {
                                propertyMap.Add(modelPropName, new List<string>());
                            }
                            propertyMap[modelPropName].Add(vmProp.Name);
                        }
                    }
                    else // Add to the non-specific assignment.
                    {
                        propertyMap[""].Add(vmProp.Name);
                    }
                }
            }
            return propertyMap.ToDictionary(k => k.Key, v => v.Value.ToArray());
        }
    }
}
