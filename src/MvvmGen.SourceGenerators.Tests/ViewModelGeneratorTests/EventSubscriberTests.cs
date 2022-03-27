﻿// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.SourceGenerators
{
    public class EventSubscriberTests : ViewModelGeneratorTestsBase
    {
        [Fact]
        public void ShouldAddEventAggregatorConstructorParameterAndSubscribe()
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;
using MvvmGen.Events;

namespace MyCode
{{
  [ViewModel]
  public partial class EmployeeViewModel : IEventSubscriber<string,int>
  {{
      public void OnEvent(string x) {{ }}

      public void OnEvent(int x) {{ }}
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel(MvvmGen.Events.IEventAggregator eventAggregator)
        {{
            eventAggregator.RegisterSubscriber(this);
            this.OnInitialize();
        }}

        partial void OnInitialize();
    }}
}}
");
        }

        [Fact]
        public void ShouldSubscribeWithInjectedEventAggregator()
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;
using MvvmGen.Events;

namespace MyCode
{{
  [Inject(typeof(IEventAggregator))]
  [ViewModel]
  public partial class EmployeeViewModel : IEventSubscriber<string,int>
  {{
      public void OnEvent(string x) {{ }}

      public void OnEvent(int x) {{ }}
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel(MvvmGen.Events.IEventAggregator eventAggregator)
        {{
            this.EventAggregator = eventAggregator;
            this.EventAggregator.RegisterSubscriber(this);
            this.OnInitialize();
        }}

        partial void OnInitialize();

        protected MvvmGen.Events.IEventAggregator EventAggregator {{ get; private set; }}
    }}
}}
");
        }

        [Fact]
        public void ShouldSubscribeWithInjectedEventAggregatorWithCustomName()
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;
using MvvmGen.Events;

namespace MyCode
{{
  [Inject(typeof(IEventAggregator),""AwesomeEventAggregator"")]
  [ViewModel]
  public partial class EmployeeViewModel : IEventSubscriber<string,int>
  {{
      public void OnEvent(string x) {{ }}

      public void OnEvent(int x) {{ }}
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel(MvvmGen.Events.IEventAggregator awesomeEventAggregator)
        {{
            this.AwesomeEventAggregator = awesomeEventAggregator;
            this.AwesomeEventAggregator.RegisterSubscriber(this);
            this.OnInitialize();
        }}

        partial void OnInitialize();

        protected MvvmGen.Events.IEventAggregator AwesomeEventAggregator {{ get; private set; }}
    }}
}}
");
        }
    }
}
