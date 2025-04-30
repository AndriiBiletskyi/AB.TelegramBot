using AB.TelegramBot.Bank.Abstraction.Interfaces;
using AB.TelegramBot.Bank.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;

namespace AB.TelegramBot.Services.UnitTests
{
    [TestClass]
    public class BanksServicesTests
    {
        #region GetAccessebaleCurrencies
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetAccessebaleCurrencies_ThrowsArgumentNullExceptionWhenBankIsNullTest()
        {
            //Arrange
            var bank = new Mock<IBank>();

            var list = new List<IBank>() { bank.Object };

            var bankServices = new BanksServices(list);

            //Act
            var cur = await bankServices.GetAccessibleCurrencies(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetAccessebaleCurrencies_ThrowsArgumentExceptionWhenBankDoesNotSupportTest()
        {
            //Arrange
            var bank = new Mock<IBank>();

            var bank2 = new Mock<IBank>();
            bank2.Setup(obj => obj.Name).Returns("test");

            var list = new List<IBank>() { bank.Object };

            var bankServices = new BanksServices(list);

            //Act
            var cur = await bankServices.GetAccessibleCurrencies(bank2.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetAccessebaleCurrencies_ThrowsExceptionWhenBankReturnsNullTest()
        {
            //Arrange
            List<Currency> currencies = null;

            var bank = new Mock<IBank>();
            bank.Setup(obj => obj.GetAccessibleCurrencies()).ReturnsAsync(() => currencies);

            var list = new List<IBank>() { bank.Object };

            var bankServices = new BanksServices(list);

            //Act
            var cur = await bankServices.GetAccessibleCurrencies(bank.Object);
        }

        [TestMethod]
        public async Task GetAccessebaleCurrencies_ReturnsAllCurrenciesFromBankTest()
        {
            //Arrange
            var currencies = new List<Currency>()
            {
                new Currency(){ Name = "FirstName", CurrencyCode = "Code1" },
                new Currency(){ Name = "SecondName", CurrencyCode = "Code2" },
                new Currency(){ Name = "ThirdName", CurrencyCode = "Code3" }
            };

            var bank = new Mock<IBank>();
            bank.Setup(obj => obj.GetAccessibleCurrencies()).ReturnsAsync(() => currencies);

            var list = new List<IBank>() { bank.Object };

            var bankServices = new BanksServices(list);

            //Act
            var cur = await bankServices.GetAccessibleCurrencies(bank.Object);

            //Assert
            Assert.IsNotNull(cur);
            Assert.IsInstanceOfType(cur, typeof(IEnumerable<Currency>));
            Assert.IsTrue(cur.Count() == currencies.Count);
            for (int i = 0; i < currencies.Count; i++)
            {
                Assert.IsTrue(currencies[i].Name == cur.ElementAt(i).Name);
                Assert.IsTrue(currencies[i].CurrencyCode == cur.ElementAt(i).CurrencyCode);
            }
        }

        #endregion
        #region GetExchangeRates
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetExchangeRates_ThrowsArgumentNullExceptionWhenBanksIsNullTest()
        {
            //Arrange
            var bank = new Mock<IBank>();

            var list = new List<IBank>() { bank.Object };

            var bankServices = new BanksServices(list);

            //Act
            await foreach (var rate in bankServices.GetExchangeRates(new ExchangeQuery(), null)) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetExchangeRates_ThrowsArgumentExceptionWhenBankDoesNotSupportTest()
        {
            //Arrange
            var bank = new Mock<IBank>();

            var bank2 = new Mock<IBank>();
            bank2.Setup(obj => obj.Name).Returns("test");

            var list = new List<IBank>() { bank.Object };

            var bankServices = new BanksServices(list);

            //Act
            await foreach (var rate in bankServices.GetExchangeRates(new ExchangeQuery(), new List<IBank>() { bank.Object, bank2.Object })) { }
        }

        [TestMethod]
        public async Task GetExchangeRates_ReturnsRateWithBankCodeIfBankGetExchangeRateThrowsExceptionTest()
        {
            //Arrange
            var bank = new Mock<IBank>();
            bank.Setup(obj => obj.GetExchangeRate(It.IsAny<ExchangeQuery>())).Throws<Exception>();
            bank.Setup(obj => obj.BankCode).Returns("TestBankCode");

            var list = new List<IBank>() { bank.Object };

            var bankServices = new BanksServices(list);

            //Act
            await foreach (var rate in bankServices.GetExchangeRates(new ExchangeQuery(), new List<IBank>() { bank.Object }))
            {
                Assert.IsNotNull(rate);
                Assert.IsTrue(rate is ExchangeRate);
                Assert.AreEqual(rate.BankCode, "TestBankCode");
            }
        }

        [TestMethod]
        public async Task GetExchangeRates_ReturnsRateWithBankCodeIfBankGetExchangeRateReturnsNullTest()
        {
            //Arrange
            var bank = new Mock<IBank>();
            bank.Setup(obj => obj.GetExchangeRate(It.IsAny<ExchangeQuery>())).ReturnsAsync(() => null);
            bank.Setup(obj => obj.BankCode).Returns("TestBankCode");

            var bank2 = new Mock<IBank>();
            bank2.Setup(obj => obj.GetExchangeRate(It.IsAny<ExchangeQuery>())).ReturnsAsync(() => null);
            bank2.Setup(obj => obj.BankCode).Returns("TestBankCode");

            var list = new List<IBank>() { bank.Object, bank2.Object };

            var bankServices = new BanksServices(list);

            //Act
            await foreach (var rate in bankServices.GetExchangeRates(new ExchangeQuery(), new List<IBank>() { bank.Object, bank2.Object }))
            {
                Assert.IsNotNull(rate);
                Assert.IsTrue(rate is ExchangeRate);
                Assert.AreEqual(rate.BankCode, "TestBankCode");
            }
        }

        [TestMethod]
        public async Task GetExchangeRates_ReturnsRatesForAllBanksTest()
        {
            //Arrange
            var exchangeRate = new ExchangeRate()
            {
                Date = DateTime.Now.AddDays(-2),
                DateOfQuery = DateTime.Now,
                BankCode = "BANK1",
                //CurrencyFromCode = "USD",
                //CurrencyToCode = "UAH",
                SaleRate = 4.2f,
                PurchaseRate = 4.1f
            };
            var bank = new Mock<IBank>();
            bank.Setup(obj => obj.GetExchangeRate(It.IsAny<ExchangeQuery>())).ReturnsAsync(() => exchangeRate);

            var exchangeRate2 = new ExchangeRate()
            {
                Date = DateTime.Now.AddDays(-20),
                DateOfQuery = DateTime.Now.AddDays(-4),
                BankCode = "BANK2",
                //CurrencyFromCode = "EUR",
                //CurrencyToCode = "UAH",
                SaleRate = 4.4f,
                PurchaseRate = 4.3f
            };
            var bank2 = new Mock<IBank>();
            bank2.Setup(obj => obj.GetExchangeRate(It.IsAny<ExchangeQuery>())).ReturnsAsync(() => exchangeRate2);

            var exchangeRate3 = new ExchangeRate()
            {
                Date = DateTime.Now.AddDays(-14),
                DateOfQuery = DateTime.Now.AddDays(-10),
                BankCode = "BANK3",
                //CurrencyFromCode = "GBP",
                //CurrencyToCode = "UAH",
                SaleRate = 5.4f,
                PurchaseRate = 5.2f
            };
            var bank3 = new Mock<IBank>();
            bank3.Setup(obj => obj.GetExchangeRate(It.IsAny<ExchangeQuery>())).ReturnsAsync(() => exchangeRate3);

            var rateList = new List<ExchangeRate>()
            {
                exchangeRate, exchangeRate2, exchangeRate3
            };

            var list = new List<IBank>() { bank.Object, bank2.Object, bank3.Object };

            var bankServices = new BanksServices(list);

            //Act
            var actualRateList = new List<ExchangeRate>();
            await foreach (var rate in bankServices.GetExchangeRates(new ExchangeQuery(), new List<IBank>() { bank.Object, bank2.Object, bank3.Object }))
            {
                Assert.IsNotNull(rate);
                Assert.IsTrue(rate is ExchangeRate);
                actualRateList.Add(rate);
            }

            Assert.IsTrue(rateList.Count == actualRateList.Count);
            for (int i = 0; i < rateList.Count; i++)
            {
                Assert.AreEqual(rateList[i], actualRateList[i]);
            }
        }
        #endregion
        #region GetBanks
        [TestMethod]
        public async Task GetBanks_ReturnsListOfBankFromDITest()
        {
            //Arrange
            var bank = new Mock<IBank>();
            bank.Setup(obj => obj.Name).Returns("TestBankName1");
            bank.Setup(obj => obj.BankCode).Returns("TestBankCode1");

            var bank2 = new Mock<IBank>();
            bank2.Setup(obj => obj.Name).Returns("TestBankName2");
            bank2.Setup(obj => obj.BankCode).Returns("TestBankCode2");

            var bankList = new List<IBank>()
            {
                bank.Object, bank2.Object
            };

            var list = new List<IBank>() { bank.Object, bank2.Object };

            var bankServices = new BanksServices(list);

            //Act
            var banks = await bankServices.GetBanks();

            //Assert
            Assert.IsNotNull(banks);
            Assert.IsTrue(bankList.Count == banks.Count());

            for (int i = 0; i < bankList.Count; i++)
            {
                var item = banks.ElementAt(i);
                Assert.IsNotNull(item);
                Assert.IsTrue(item is IBank);
                Assert.AreEqual(item, bankList[i]);
            }
        }

        #endregion
    }
}