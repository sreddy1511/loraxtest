using NUnit.Framework;
using CalculatorApp;

namespace CalculatorTests
{
    [TestFixture]
    public class CalculatorTests
    {
        private Calculator _calculator;

        [SetUp]
        public void Setup()
        {
            _calculator = new Calculator();
        }

        #region Addition Tests
        [Test]
        public void Add_WithPositiveNumbers_ReturnsCorrectSum()
        {
            double result = _calculator.Add(3, 5);
            Assert.AreEqual(8, result, "3 + 5 should equal 8");
        }

        [Test]
        public void Add_WithNegativeNumbers_ReturnsCorrectSum()
        {
            double result = _calculator.Add(-2, -7);
            Assert.AreEqual(-9, result, "-2 + -7 should equal -9");
        }

        [Test]
        public void Add_WithPositiveAndNegative_ReturnsCorrectSum()
        {
            double result = _calculator.Add(10, -4);
            Assert.AreEqual(6, result, "10 + -4 should equal 6");
        }

        [Test]
        public void Add_WithZero_ReturnsOriginalNumber()
        {
            double result = _calculator.Add(5, 0);
            Assert.AreEqual(5, result, "5 + 0 should equal 5");
        }
        #endregion

        #region Subtraction Tests
        [Test]
        public void Subtract_WithPositiveNumbers_ReturnsCorrectDifference()
        {
            double result = _calculator.Subtract(10, 4);
            Assert.AreEqual(6, result, "10 - 4 should equal 6");
        }

        [Test]
        public void Subtract_WithNegativeNumbers_ReturnsCorrectDifference()
        {
            double result = _calculator.Subtract(-5, -2);
            Assert.AreEqual(-3, result, "-5 - (-2) should equal -3");
        }

        [Test]
        public void Subtract_WithPositiveFromNegative_ReturnsCorrectDifference()
        {
            double result = _calculator.Subtract(-8, 3);
            Assert.AreEqual(-11, result, "-8 - 3 should equal -11");
        }

        [Test]
        public void Subtract_WithZero_ReturnsOriginalNumber()
        {
            double result = _calculator.Subtract(7, 0);
            Assert.AreEqual(7, result, "7 - 0 should equal 7");
        }
        #endregion

        #region Multiplication Tests
        [Test]
        public void Multiply_WithPositiveNumbers_ReturnsCorrectProduct()
        {
            double result = _calculator.Multiply(4, 3);
            Assert.AreEqual(12, result, "4 * 3 should equal 12");
        }

        [Test]
        public void Multiply_WithNegativeNumbers_ReturnsCorrectProduct()
        {
            double result = _calculator.Multiply(-2, -5);
            Assert.AreEqual(10, result, "-2 * -5 should equal 10");
        }

        [Test]
        public void Multiply_WithPositiveAndNegative_ReturnsCorrectProduct()
        {
            double result = _calculator.Multiply(6, -3);
            Assert.AreEqual(-18, result, "6 * -3 should equal -18");
        }

        [Test]
        public void Multiply_WithZero_ReturnsZero()
        {
            double result = _calculator.Multiply(9, 0);
            Assert.AreEqual(0, result, "9 * 0 should equal 0");
        }
        #endregion

        #region Division Tests
        [Test]
        public void Divide_WithPositiveNumbers_ReturnsCorrectQuotient()
        {
            double result = _calculator.Divide(15, 3);
            Assert.AreEqual(5, result, "15 / 3 should equal 5");
        }

        [Test]
        public void Divide_WithNegativeNumbers_ReturnsCorrectQuotient()
        {
            double result = _calculator.Divide(-10, -2);
            Assert.AreEqual(5, result, "-10 / -2 should equal 5");
        }

        [Test]
        public void Divide_WithPositiveByNegative_ReturnsCorrectQuotient()
        {
            double result = _calculator.Divide(12, -4);
            Assert.AreEqual(-3, result, "12 / -4 should equal -3");
        }

        [Test]
        public void Divide_WithZeroNumerator_ReturnsZero()
        {
            double result = _calculator.Divide(0, 5);
            Assert.AreEqual(0, result, "0 / 5 should equal 0");
        }

        [Test]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            Assert.Throws<DivideByZeroException>(
                () => _calculator.Divide(8, 0),
                "Division by zero should throw an exception");
        }

        [Test]
        public void Divide_WithSmallNumberByLargeNumber_ReturnsCorrectFraction()
        {
            double result = _calculator.Divide(1, 4);
            Assert.AreEqual(0.25, result, 0.0001, "1 / 4 should equal 0.25");
        }
        #endregion
    }
}
