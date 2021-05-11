﻿// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using Xunit;

namespace MvvmGen.SourceGenerators
{
    public class CommandAttributeTests : ViewModelGeneratorTestsBase
    {
        [Fact]
        public void GenerateCommandProperty()
        {
            ShouldGenerateExpectedCode(
      @"using MvvmGen;

namespace MyCode
{   
  [ViewModel]
  public partial class EmployeeViewModel
  {
    [Command]public void SaveAll() { }
  }
}",
      $@"{AutoGeneratedComment}
{AutoGeneratedUsings}

namespace MyCode
{{
    partial class EmployeeViewModel : ViewModelBase
    {{
        public EmployeeViewModel()
        {{
            this.InitializeCommands();
            this.OnInitialize();
        }}

        partial void OnInitialize();

        private void InitializeCommands()
        {{
            SaveAllCommand = new(_ => SaveAll());
        }}

        public DelegateCommand SaveAllCommand {{ get; private set; }}
    }}
}}
");
        }

        [InlineData("CanExecuteMethod=\"CanSaveAll\"")]
        [InlineData("CanExecuteMethod=nameof(CanSaveAll)")]
        [InlineData("nameof(CanSaveAll)")]
        [InlineData("\"CanSaveAll\"")]
        [Theory]
        public void GenerateCommandPropertyWithCanExecuteMethod(string attributeArgument)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{   
    [ViewModel]
    public partial class EmployeeViewModel
    {{
        [Command({attributeArgument})]
        public void SaveAll() {{ }}

        public bool CanSaveAll() => true;
    }}
}}",
      $@"{AutoGeneratedComment}
{AutoGeneratedUsings}

namespace MyCode
{{
    partial class EmployeeViewModel : ViewModelBase
    {{
        public EmployeeViewModel()
        {{
            this.InitializeCommands();
            this.OnInitialize();
        }}

        partial void OnInitialize();

        private void InitializeCommands()
        {{
            SaveAllCommand = new(_ => SaveAll(), _ => CanSaveAll());
        }}

        public DelegateCommand SaveAllCommand {{ get; private set; }}
    }}
}}
");
        }

        [InlineData("CanExecuteMethod=nameof(CanSaveAll)")]
        [InlineData("CanExecuteMethod=\"CanSaveAll\"")]
        [InlineData("nameof(CanSaveAll)")]
        [InlineData("\"CanSaveAll\"")]
        [Theory]
        public void GenerateCommandPropertyWithCanExecuteMethodAndCommandNameUsingNamedArgument(string canExecuteParameter)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{   
  [ViewModel]
  public partial class EmployeeViewModel
  {{
    [Command({canExecuteParameter}, CommandName=""SuperCommand"")]
    public void SaveAll() {{ }}

    public bool CanSaveAll() => true;
  }}
}}",
      $@"{AutoGeneratedComment}
{AutoGeneratedUsings}

namespace MyCode
{{
    partial class EmployeeViewModel : ViewModelBase
    {{
        public EmployeeViewModel()
        {{
            this.InitializeCommands();
            this.OnInitialize();
        }}

        partial void OnInitialize();

        private void InitializeCommands()
        {{
            SuperCommand = new(_ => SaveAll(), _ => CanSaveAll());
        }}

        public DelegateCommand SuperCommand {{ get; private set; }}
    }}
}}
");
        }

        [InlineData("_ => Save(), _ => CanSave()", "void Save()", "bool CanSave()")]
        [InlineData("Save, _ => CanSave()", "void Save(object o)", "bool CanSave()")]
        [InlineData("Save, CanSave", "void Save(object o)", "bool CanSave(object o)")]
        [InlineData("_ => Save(), CanSave", "void Save()", "bool CanSave(object o)")]
        [Theory]
        public void GenerateCommandWithParametersOnMethods(string expectedConstructorArguments,
            string executeMethod, string canExecuteMethod)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{   
    [ViewModel]
    public partial class EmployeeViewModel
    {{
        [Command(CanExecuteMethod=nameof(CanSave))]
        public {executeMethod} {{ }}

        public {canExecuteMethod} => true;
    }}
}}",
      $@"{AutoGeneratedComment}
{AutoGeneratedUsings}

namespace MyCode
{{
    partial class EmployeeViewModel : ViewModelBase
    {{
        public EmployeeViewModel()
        {{
            this.InitializeCommands();
            this.OnInitialize();
        }}

        partial void OnInitialize();

        private void InitializeCommands()
        {{
            SaveCommand = new({expectedConstructorArguments});
        }}

        public DelegateCommand SaveCommand {{ get; private set; }}
    }}
}}
");
        }
    }
}
