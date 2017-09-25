using FluentAssertions;
using Xunit;

using static Common.Result<object>;

namespace Common.UnitTests
{
    public class OnSuccessResultExtensionsUnitTests
    {
        [Fact]
        public void on_success_returns_different_result_of_the_same_type_when_success()
        {
            var result1 = Ok(new object());
            var result2 = Ok(new object());

            var result = result1.OnSuccess(value => result2);

            result.Should().Be(result2);
        }

        [Fact]
        public void on_success_with_different_result_as_a_return_value_is_not_called_when_failure()
        {
            var result1 = Failure("Error");
            var result2 = Ok(new object());

            var onSuccessCalled = false;
            var result = result1.OnSuccess(value =>
            {
                onSuccessCalled = true;
                return result2;
            });

            onSuccessCalled.Should().BeFalse();
        }

        [Fact]
        public void on_success_returns_result_of_different_type_when_success()
        {
            var result1 = Result<int>.Ok(10);
            var result2 = Result<string>.Ok("value");

            var result = result1.OnSuccess(intValue => result2);
            result.Should().Be(result2);
        }
    }
}
