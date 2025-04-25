# MileHighWpf.MvvmModelMessaging Package

This was inspired by the recent release of CommunityToolkit.Mvvm and its use of Attributes to auto-generate code for View-to-ViewModel bindings and greatly reduce the boilerplate code common in WFP/MVVM applications in the past. This provides base classes to implement messaging as a way to update the ViewModel from changes in the Modle in such a way that the View is automatically updated in response to the ViewModel changes. Basically a Model message to the ViewModel causes an OnPropertyChanged() call for targeted ViewModel properties, and thus the Model is triggering View updates without violating the principles of MVVM.

By using the base classes for Model and ViewModel the required Model-to-ViewModel messaging can be implemented just using Attributes [ModelDependent] and [ModelDependentCallCanExecute] on ViewModel properties. 
For efficiency sake the messages from the Model can contain multiple names of Model properties so there is not just a one-to-one mapping of message to ViewModel updates, but rather it can be many-to-many depending on how the ViewModel properties are attributed.

## Prerequisites

This package targets Windows, is built on .Net 8.0, and requires CommunityToolkit.Mvvm 8.4.0.

## Getting started

You can see an example of implementation in a small demo application at the github link below but here are the basic steps.

* Install MileHighWpf.MvvmModelMessaging package.
* Implement a new Model class using ModelBase or ModelBase<>.
    * If using the default message type then use ModelBase for the base class and then let the ViewModel decide to filter messages of that type.
    * If using a new message class it must extend ModelDependentMessage and then that class can be used at the template parameter for ModelBase<>. The ViewModel intended to receive the message must also implement that new message class in its base class.
* Implement a new ViewModel class using ViewModelBase or ViewModelBase<>.
    * If using the default message type then use ViewModelBase for the base class. Because the ViewModel is receiving default messages it needs to add the full type name of the model it is tied to the AllowedMessageSenders List.
    * If using a new message class it must extend ViewModelDependentMessage with the same message class that the corresponding ModelBase<> wil be sending. In this case there is no need to update AllowedMessageSenders.
* In the derived ViewModel class you can assign an instance of the [ModelDependent] and optional parameters to automate updating both properties.
    * Properties that should be updated for any change in the model should be annotated with just [ModelDependent]. This means that any message from the model will trigger update of this ViewModel property to the corresponding View binding.
    * Properties that should be updated only for a specific change in the model should be annotated with [ModelDependent(name of model property, name of another model property, ...)]. This means that any message from the model that containas a name that appears in the attribute parasmeter list will will trigger an update of this ViewModel property to the corresponding View binding.
* If a ViewModel property has been attributed as [ModelDependent] then ic can also use the [ModelDependentCallCanExecute(..)] attribute. This attribute takes a list of methods that update the 'Can Execute' state of the ViewModel's relay commands. If the ViewModel property is updated in response to any message from the Model then any of the methods listed will be called using the NotifyCanExecuteChanged() method of the corresponding commands.
* To better understand the mappings that you put in place it may be helpful to set the global property TraceMessagesOn before any ViewModels in your application are created, and this will send mapping and messaging information to the System.Diagnostics.Trace output.

## Feedback and Sample Usage

https://github.com/acorkan/MvvmModelMessaging
