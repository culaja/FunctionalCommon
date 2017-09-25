using FluentAssertions;
using System;
using Xunit;

using static Common.Result<object>;

namespace Common.UnitTests
{
    public class ResultUnitTests
    {
        [Fact]
        public void is_success_set_to_true_on_ok()
        {
            var result = Ok(new object());
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void value_is_the_same_on_success()
        {
            var value = new object();
            var result = Ok(value);

            result.Value.Should().Be(value);
        }

        [Fact]
        public void exception_is_thrown_on_success_with_null_value()
        {
            Assert.Throws<ArgumentNullException>(() => Ok(null));
        }

        [Fact]
        public void exception_is_thrown_on_failure_with_null_error()
        {
            Assert.Throws<ArgumentNullException>(() => Failure(default(Error)));
        }

        [Fact]
        public void access_to_error_throws_exception_on_success()
        {
            var result = Ok(new object());
            Assert.Throws<InvalidOperationException>(() => result.Error);
        }

        [Fact]
        public void access_to_value_throws_exception_on_failure()
        {
            var result = Failure("Error");
            Assert.Throws<InvalidOperationException>(() => result.Value);
        }

        [Fact]
        public void failure_throws_exception_if_empty_string_is_passed()
        {
            Assert.Throws<ArgumentNullException>(() => Failure(string.Empty));
        }

        [Fact]
        public void error_message_is_the_same_on_failure()
        {
            var errorMessage = "Error message";
            var result = Failure(errorMessage);
            result.Error.ToString().Should().Be(errorMessage);
        }

        [Fact]
        public void on_failure_called_with_same_error_when_failure()
        {
            var result = Failure("Error");

            var onFailureError = string.Empty;
            result.OnFailure((error) => onFailureError = error);

            onFailureError.Should().Be("Error");
        }

        [Fact]
        public void on_failure_not_called_when_success()
        {
            var result = Ok(new object());

            var onFailureCalled = false;
            result.OnFailure(error => onFailureCalled = true);

            onFailureCalled.Should().BeFalse();
        }

        class TestError : Error { public TestError() : base("Test Error") { } }

        [Fact]
        public void on_failure_for_specific_error_called_on_same_failure()
        {
            var expectedError = new TestError();
            var result = Failure(expectedError);

            var actualError = default(TestError);
            result.OnFailure<TestError>(error => actualError = error);

            actualError.Should().Be(expectedError);
        }

        [Fact]
        public void on_failure_for_specific_error_not_called_on_generic_error()
        {
            var result = Failure("Error");

            var onFailureCalled = false;
            result.OnFailure<TestError>(error => onFailureCalled = true);

            onFailureCalled.Should().BeFalse();
        }

        [Fact]
        public void on_failure_for_specific_error_not_called_when_success()
        {
            var result = Ok(new object());

            var onFailureCalled = false;
            result.OnFailure<TestError>(error => onFailureCalled = true);

            onFailureCalled.Should().BeFalse();
        }

        [Fact]
        public void on_success_called_with_the_same_value_when_success()
        {
            var expectedObject = new object();
            var result = Ok(expectedObject);

            var actualObject = default(object);
            result.OnSuccess(value => actualObject = value);

            actualObject.Should().Be(expectedObject);
        }

        [Fact]
        public void on_success_not_called_when_failure()
        {
            var result = Failure("Error");

            var onSuccessCalled = false;
            result.OnSuccess(value => onSuccessCalled = true);

            onSuccessCalled.Should().BeFalse();
        }

        [Fact]
        public void init_result_state_when_default_struct_constructor_is_called()
        {
            var result = new Result<int>();
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Message.Should().Be("Default constructor should not be used.");
        }
    }
}
