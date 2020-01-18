# NETStandardLibrary.Email

NETStandardLibrary.Email is a helper library for building and sending e-mails using embedded Razor templates.

## Installation

* Include `NETStandardLibrary.Email` in your project.
* Extend the `NETStandardLibrary.Email.EmailService<T>` since you'll need a concrete EmailService implementation. `T` should be any class in the assembly that contains your Email classes and embedded Razor templates.
* Call the `EmailService.Initialize` method to allow rendering and sending of e-mails to work.

## Usage

* Modify your `.csproj` file to embed your resources.  For a .NET Core project, here's an example:

```xml
  <ItemGroup>
    <Content Remove="Emails/**/*.cshtml" />
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Include="Emails/**/*.cshtml" />
  </ItemGroup>
```

* Modify your `.csproj` file to preserve the compilation context.  For a .NET Core project, here's an example

```xml
<PropertyGroup>
  <PreserveCompilationContext>true</PreserveCompilationContext>
</PropertyGroup>
```

* Extend the `NETStandardLibrary.Email.Email` object. This object will be the model for your e-mail template. The base object only contains the necessary properties for a functional e-mail.
* Create a `.cshtml` with the same name as the *class* name of the C# object (not the file name, though it will probably be the same).  e.g. `SignupEmail.cs` and `SignupEmail.cshtml`.
* Begin your `.cshtml` file with the following as this will give you Intellisense support:

```html
@using RazorLight
@inherits TemplatePage<MyLib.Email.SignupEmail>
```
