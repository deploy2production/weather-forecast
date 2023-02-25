using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace DeployToProduction.WeatherForecast.App.UITests
{
    [TestClass]
    public class WeatherForecastAppTests
    {
        [TestMethod]
        public async Task Generate_RandomWeather()
        {
            var driver = new EdgeDriver();
            try
            {
                driver.Navigate().GoToUrl("http://localhost:5225/");

                driver.FindElement(By.Id("location")).SendKeys("Moscow");
                driver.FindElement(By.Id("submit_btn")).Click();

                var forecastBlock = driver.FindElement(By.Id("forecast_block"));

                Assert.IsNotNull(forecastBlock);
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}