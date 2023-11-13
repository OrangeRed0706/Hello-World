using FluentAssertions;

namespace HelloWorld.Test
{
    public class Tests
    {
        [Fact]
        public void TestAddMethod()
        {
            var calculator = new Calculator();
            var result = calculator.Add(2, 3);
            Assert.Equal(5, result);
        }

        [Theory]
        [InlineData(3, 5, 8)]
        [InlineData(2, 2, 4)]
        [InlineData(0, 0, 0)]
        public void TestAddMethodWithMultipleData(int a, int b, int expected)
        {
            var calculator = new Calculator();
            var result = calculator.Add(a, b);
            result.Should().Be(expected);
        }

        [Fact]
        public void TestForException()
        {
            var calculator = new Calculator();
            Assert.Throws<ArgumentException>(() => calculator.Divide(10, 0));
        }
    }

    public class Calculator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        public int Divide(int x, int y)
        {
            if (y == 0)
            {
                throw new ArgumentException("Cannot divide by zero");
            }

            return x / y;
        }
    }
}
