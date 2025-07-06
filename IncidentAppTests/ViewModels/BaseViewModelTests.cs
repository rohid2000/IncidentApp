using IncidentApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IncidentAppTests.ViewModelTests
{
    public class BaseViewModelTests : BaseViewModel
    {
            private string _testProperty;
            public string TestProperty
            {
                get => _testProperty;
                set => SetProperty(ref _testProperty, value);
            }

            private int _numericProperty;
            public int NumericProperty
            {
                get => _numericProperty;
                set => SetProperty(ref _numericProperty, value, onChanged: () => CallbackAction());
            }

            public bool CallbackInvoked { get; private set; }
            private void CallbackAction() => CallbackInvoked = true;

            public bool TestSetProperty(ref int field, int value, string propertyName = null)
            {
                return SetProperty(ref field, value, propertyName);
            }

        [Fact]
        public void SetProperty_UpdatesValueAndUpdatesPropertyChanged()
        {
            //Arrange
            var vm = new BaseViewModelTests();
            var propertyUpdated = false;
            vm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TestProperty))
                {
                    propertyUpdated = true;
                }
            };

            //Act
            vm.TestProperty = "New Value";

            //Assert
            Assert.Equal("New Value", vm.TestProperty);
            Assert.True(propertyUpdated);
        }

        [Fact]
        public void SetProperty_DoesNotUpdateWhenValueUnchanged()
        {
            //Arrange
            var vm = new BaseViewModelTests();
            vm.TestProperty = "Initial Value";
            var propertyUpdated = false;
            vm.PropertyChanged += (sender, args) => propertyUpdated = true;

            //Act
            vm.TestProperty = "Initial Value";

            //Assert
            Assert.False(propertyUpdated);
        }

        [Fact]
        public void SetProperty_InvokesCallbackWhenProvided()
        {
            //Arrange
            var vm = new BaseViewModelTests();

            //Act
            vm.NumericProperty = 42;

            //Assert
            Assert.True(vm.CallbackInvoked);
        }

        [Fact]
        public void SetProperty_ReturnsTrueWhenValueChanged()
        {
            //Arrange
            var vm = new BaseViewModelTests();
            var field = 0;

            //Act
            var result = vm.TestSetProperty(ref field, 10, nameof(NumericProperty));

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void SetProperty_ReturnsFalseWhenValueUnchanged()
        {
            //Arrange
            var vm = new BaseViewModelTests();
            var field = 5; 
            var originalValue = field;

            //Act
            var result = vm.TestSetProperty(ref field, originalValue, nameof(NumericProperty));

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void OnPropertyChanged_CallsEventForCallerMemberName()
        {
            //Arrange
            var vm = new BaseViewModelTests();
            string changedProperty = null;
            vm.PropertyChanged += (sender, args) => changedProperty = args.PropertyName;

            //Act
            vm.TestProperty = "Trigger Change";

            //Assert
            Assert.Equal(nameof(TestProperty), changedProperty);
        }
    }
}
