﻿// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.SourceGenerators
{
    public class PropertyInvalidateAttributeTests : ViewModelGeneratorTestsBase
    {
        [InlineData(true, false, "[PropertyInvalidate(nameof(FirstName))]\n[PropertyInvalidate(nameof(FirstName))]")]
        [InlineData(true, true, "[PropertyInvalidate(nameof(FirstName), nameof(LastName))]")]
        [InlineData(true, true, "[PropertyInvalidate(nameof(FirstName), new string[]{nameof(LastName)})]")]
        [InlineData(true, true, "[PropertyInvalidate(nameof(FirstName), new[]{nameof(LastName)})]")]
        [InlineData(true, true, "[PropertyInvalidate(\"FirstName\", \"LastName\")]")]
        [InlineData(true, true, "[PropertyInvalidate(\"FirstName\", new string[]{\"LastName\"})]")]
        [InlineData(true, true, "[PropertyInvalidate(\"FirstName\", new[]{\"LastName\"})]")]
        [InlineData(true, true, "[PropertyInvalidate(nameof(FirstName))]\n[PropertyInvalidate(nameof(LastName))]")]
        [InlineData(true, true, "[PropertyInvalidate(\"FirstName\")]\n[PropertyInvalidate(\"LastName\")]")]
        [InlineData(true, false, "[PropertyInvalidate(\"FirstName\")]")]
        [InlineData(true, false, "[PropertyInvalidate(nameof(FirstName))]")]
        [InlineData(false, false, "")]
        [Theory]
        public void CallOnPropertyChangedInSettersOfOtherProperty(bool expectedInFirstName, bool expectedInLastName, string propertyInvalidateAttributes)
        {
            var onPropertyChangedCall = @"                    OnPropertyChanged(""FullName"");
";
            // Note: Do line-breaks in the string above with @"".
            // If you use \r\n, the test will work on Windows, but fail on GitHub.
            // If you use \n, the test will fail on Windows, but work on GitHub. :-)

            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
  [ViewModel]
  public partial class EmployeeViewModel
  {{
    [Property] string _firstName;
    [Property] string _lastName;
    
    {propertyInvalidateAttributes}
    public string FullName => $""{{FirstName}} {{LastName}}"";
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel()
        {{
            this.OnInitialize();
        }}

        partial void OnInitialize();

        public string FirstName
        {{
            get => _firstName;
            set
            {{
                if (_firstName != value)
                {{
                    _firstName = value;
                    OnPropertyChanged(""FirstName"");
{(expectedInFirstName ? onPropertyChangedCall : "")}                }}
            }}
        }}

        public string LastName
        {{
            get => _lastName;
            set
            {{
                if (_lastName != value)
                {{
                    _lastName = value;
                    OnPropertyChanged(""LastName"");
{(expectedInLastName ? onPropertyChangedCall : "")}                }}
            }}
        }}
    }}
}}
");
        }

        [InlineData(true, false, "[PropertyInvalidate(nameof(FirstName))]\n[PropertyInvalidate(nameof(FirstName))]")]
        [InlineData(true, true, "[PropertyInvalidate(nameof(FirstName), nameof(LastName))]")]
        [InlineData(true, true, "[PropertyInvalidate(nameof(FirstName), new string[]{nameof(LastName)})]")]
        [InlineData(true, true, "[PropertyInvalidate(nameof(FirstName), new[]{nameof(LastName)})]")]
        [InlineData(true, true, "[PropertyInvalidate(\"FirstName\", \"LastName\")]")]
        [InlineData(true, true, "[PropertyInvalidate(\"FirstName\", new string[]{\"LastName\"})]")]
        [InlineData(true, true, "[PropertyInvalidate(\"FirstName\", new[]{\"LastName\"})]")]
        [InlineData(true, true, "[PropertyInvalidate(nameof(FirstName))]\n[PropertyInvalidate(nameof(LastName))]")]
        [InlineData(true, true, "[PropertyInvalidate(\"FirstName\")]\n[PropertyInvalidate(\"LastName\")]")]
        [InlineData(true, false, "[PropertyInvalidate(\"FirstName\")]")]
        [InlineData(true, false, "[PropertyInvalidate(nameof(FirstName))]")]
        [InlineData(false, false, "")]
        [Theory]
        public void CallOnPropertyChangedInSettersOfGeneratedModelProperty(bool expectedInFirstName, bool expectedInLastName, string propertyInvalidateAttributes)
        {
            var onPropertyChangedCall = @"                    OnPropertyChanged(""FullName"");
";
            // Note: Do line-breaks in the string above with @"".
            // If you use \r\n, the test will work on Windows, but fail on GitHub.
            // If you use \n, the test will fail on Windows, but work on GitHub. :-)

            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
  public class Employee
  {{
    public string? FirstName {{ get; set; }}
    public string? LastName {{ get; set; }}
  }}

  [ViewModel(typeof(Employee))]
  public partial class EmployeeViewModel
  {{
    {propertyInvalidateAttributes}
    public string FullName => $""{{FirstName}} {{LastName}}"";
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel()
        {{
            this.OnInitialize();
        }}

        partial void OnInitialize();

        public string? FirstName
        {{
            get => Model.FirstName;
            set
            {{
                if (Model.FirstName != value)
                {{
                    Model.FirstName = value;
                    OnPropertyChanged(""FirstName"");
{(expectedInFirstName ? onPropertyChangedCall : "")}                }}
            }}
        }}

        public string? LastName
        {{
            get => Model.LastName;
            set
            {{
                if (Model.LastName != value)
                {{
                    Model.LastName = value;
                    OnPropertyChanged(""LastName"");
{(expectedInLastName ? onPropertyChangedCall : "")}                }}
            }}
        }}

        protected MyCode.Employee Model {{ get; set; }}
    }}
}}
");
        }
    }
}
