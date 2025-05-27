namespace JobPortal.Api.Tests.Unit
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_SimpleValues_ShouldReturnSum()
        {
            // Arrange
            int a = 5;
            int b = 7;

            // Act
            int result = a + b;

            // Assert
            Assert.Equal(12, result);
        }
    }
}
