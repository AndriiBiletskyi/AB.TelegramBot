namespace AB.TelegramBot.Banks.UnitTests
{
    [TestClass]
    public class BankTests
    {
        //    private Dictionary<DateTime, IEnumerable<Currency>> _currencies;
        //    private Mock<IDateTimeProvider> _mockDateTimeProvider = new();
        //    private DateTime _defaultDateTime = new DateTime(2023, 11, 28);
        //    private IEnumerable<IBank> _banks;

        //    [TestInitialize]
        //    public void Initialize()
        //    {
        //        _currencies = new()
        //        {
        //            { _defaultDateTime,
        //                new List<Currency>()
        //                {
        //                    new Currency(){ CurrencyCode = "USD", Name = "долар США" },
        //                    new Currency(){ CurrencyCode = "EUR", Name = "євро" },
        //                    new Currency(){ CurrencyCode = "CHF", Name = "швейцарський франк" },
        //                    new Currency(){ CurrencyCode = "GBP", Name = "британський фунт" },
        //                    new Currency(){ CurrencyCode = "PLN", Name = "польський злотий" },
        //                    new Currency(){ CurrencyCode = "SEK", Name = "шведська крона" },
        //                    new Currency(){ CurrencyCode = "XAU", Name = "золото" },
        //                    new Currency(){ CurrencyCode = "CAD", Name = "канадський долар" },
        //                    new Currency(){ CurrencyCode = "UAH", Name = "українська гривня" }
        //                }
        //            }
        //        };

        //        _banks = new List<IBank>()
        //        {
        //            new PrivatBank(_mockPrivatBankRates.Object, _mockDateTimeProvider.Object),
        //            new NBU(_mockNBURates.Object, _mockDateTimeProvider.Object)
        //        };
        //    }

        //    #region GetAccessebaleCurrencies
        //    [TestMethod]
        //    public async Task GetAccessebaleCurrencies_FillCurrenciesToNextDayAfterCreateTest()
        //    {
        //        //Arrange
        //        _mockPrivatBankRates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);

        //        _mockNBURates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);

        //        int q = 0;
        //        _mockDateTimeProvider.Setup(obj => obj.GetCurrentTime()).Returns(() => _defaultDateTime.AddDays(q++));

        //        //Act
        //        IEnumerable<Currency> actualCurrencies = new List<Currency>();
        //        foreach (var bank in _banks)
        //        {
        //            actualCurrencies = await bank.GetAccessibleCurrencies();
        //            actualCurrencies = await bank.GetAccessibleCurrencies();

        //            //Assert
        //            Assert.IsNotNull(actualCurrencies);
        //            Assert.IsTrue(actualCurrencies is IEnumerable<Currency>);
        //            Assert.IsTrue(actualCurrencies.Count() == _currencies.First().Value.Count());
        //            for (int i = 0; i < actualCurrencies.Count(); i++)
        //            {
        //                Assert.AreEqual(_currencies.First().Value.ElementAt(i), actualCurrencies.ElementAt(i));
        //            }
        //        }
        //        _mockPrivatBankRates.Verify(service => service.GetCurrencies(It.IsAny<string>()), Times.Exactly(2));
        //        _mockNBURates.Verify(service => service.GetCurrencies(It.IsAny<string>()), Times.Exactly(2));
        //    }

        //    [TestMethod]
        //    public async Task GetAccessebaleCurrencies_DoesNotFillCurrenciesBeforNextDayTest()
        //    {
        //        //Arrange
        //        _mockPrivatBankRates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);

        //        _mockNBURates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);

        //        _mockDateTimeProvider.Setup(obj => obj.GetCurrentTime()).Returns(_defaultDateTime);

        //        //Act
        //        IEnumerable<Currency> actualCurrencies = new List<Currency>();
        //        foreach (var bank in _banks)
        //        {
        //            actualCurrencies = await bank.GetAccessibleCurrencies();
        //            actualCurrencies = await bank.GetAccessibleCurrencies();

        //            //Assert
        //            Assert.IsNotNull(actualCurrencies);
        //            Assert.IsTrue(actualCurrencies is IEnumerable<Currency>);
        //            Assert.IsTrue(actualCurrencies.Count() == _currencies.First().Value.Count());
        //            for (int i = 0; i < actualCurrencies.Count(); i++)
        //            {
        //                Assert.AreEqual(_currencies.First().Value.ElementAt(i), actualCurrencies.ElementAt(i));
        //            }
        //        }

        //        _mockPrivatBankRates.Verify(service => service.GetCurrencies(It.IsAny<string>()), Times.Once());
        //        _mockNBURates.Verify(service => service.GetCurrencies(It.IsAny<string>()), Times.Once());

        //    }
        //    #endregion
        //    #region GetExchangeRate
        //    [TestMethod]
        //    public async Task GetExchangeRate_ThrowArgumentNullExceptionWhenExchangeQueryIsNullTest()
        //    {
        //        foreach (var bank in _banks)
        //        {
        //            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await bank.GetExchangeRate(null));
        //        }
        //    }

        //    [TestMethod]
        //    public async Task GetExchangeRate_ThrowArgumentNullExceptionWhenExchangeQueryIsNotValidTest()
        //    {
        //        //Act
        //        foreach (var bank in _banks)
        //        {
        //            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await bank.GetExchangeRate(new ExchangeQuery() { CurrencyFrom = null }));
        //            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await bank.GetExchangeRate(new ExchangeQuery() { CurrencyTo = null }));
        //            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await bank.GetExchangeRate(new ExchangeQuery()));
        //        }
        //    }

        //    [TestMethod]
        //    public async Task GetExchangeRate_ThrowsArgumentExceptionIfCurrencyFromCodeIsNotSupportedTest()
        //    {
        //        //Arrange
        //        _mockPrivatBankRates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);

        //        _mockNBURates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);

        //        _mockDateTimeProvider.Setup(obj => obj.GetCurrentTime()).Returns(_defaultDateTime);

        //        var exchangeQuery = new ExchangeQuery()
        //        {
        //            CurrencyFrom = new Currency()
        //            {
        //                CurrencyCode = "TEST"
        //            },
        //            CurrencyTo = new Currency()
        //            {
        //                CurrencyCode = "UAH"
        //            }
        //        };

        //        //Assert
        //        foreach (var bank in _banks)
        //        {
        //            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await bank.GetExchangeRate(exchangeQuery));
        //        }
        //    }

        //    [TestMethod]
        //    public async Task GetExchangeRate_ThrowsArgumentOutOfRangeExceptionIfDateIsTommorowTest()
        //    {
        //        //Arrange
        //        _mockPrivatBankRates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);

        //        _mockNBURates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);

        //        _mockDateTimeProvider.Setup(obj => obj.GetCurrentTime()).Returns(_defaultDateTime);

        //        var exchangeQuery = new ExchangeQuery()
        //        {
        //            Date = _mockDateTimeProvider.Object.GetCurrentTime().Date.AddDays(1),
        //            CurrencyFrom = new Currency()
        //            {
        //                CurrencyCode = "USD"
        //            },
        //            CurrencyTo = new Currency()
        //            {
        //                CurrencyCode = "UAH"
        //            }
        //        };

        //        //Assert
        //        foreach (var bank in _banks)
        //        {
        //            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await bank.GetExchangeRate(exchangeQuery));
        //        }
        //    }

        //    [TestMethod]
        //    public async Task GetExchangeRate_ThrowsExceptionIfCantReturnRateTest()
        //    {
        //        //Arrange
        //        _mockPrivatBankRates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);
        //        _mockPrivatBankRates.Setup(obj => obj.GetRates(It.IsAny<string>())).ReturnsAsync(() => null);

        //        _mockNBURates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);
        //        _mockNBURates.Setup(obj => obj.GetRates(It.IsAny<string>())).ReturnsAsync(() => null);

        //        _mockDateTimeProvider.Setup(obj => obj.GetCurrentTime()).Returns(_defaultDateTime);

        //        var exchangeQuery = new ExchangeQuery()
        //        {
        //            Date = _mockDateTimeProvider.Object.GetCurrentTime().Date,
        //            CurrencyFrom = new Currency()
        //            {
        //                CurrencyCode = "USD"
        //            },
        //            CurrencyTo = new Currency()
        //            {
        //                CurrencyCode = "UAH"
        //            }
        //        };

        //        //Assert
        //        foreach (var bank in _banks)
        //        {
        //            await Assert.ThrowsExceptionAsync<Exception>(async () => await bank.GetExchangeRate(exchangeQuery));
        //        }
        //    }

        //    [TestMethod]
        //    public async Task GetExchangeRate_ReturnsCorrectRateTest()
        //    {
        //        //Arrange
        //        var rates = new List<ExchangeRate>()
        //        {
        //            new ExchangeRate()
        //            {
        //                Date = _defaultDateTime.Date.AddDays(-1),
        //                DateOfQuery = _defaultDateTime,
        //                BankCode = "PB",
        //                CurrencyFrom = new Currency() { CurrencyCode = "USD" },
        //                CurrencyTo = new Currency() { CurrencyCode = "UAH" },
        //                SaleRate = 4.4f,
        //                PurchaseRate = 4.5f
        //            }
        //        };

        //        _mockPrivatBankRates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);
        //        _mockPrivatBankRates.Setup(obj => obj.GetRates(It.IsAny<string>())).ReturnsAsync(() => rates);

        //        _mockNBURates.Setup(obj => obj.GetCurrencies(It.IsAny<string>())).ReturnsAsync(() => _currencies.First().Value);
        //        _mockNBURates.Setup(obj => obj.GetRates(It.IsAny<string>())).ReturnsAsync(() => rates);

        //        _mockDateTimeProvider.Setup(obj => obj.GetCurrentTime()).Returns(_defaultDateTime);

        //        var exchangeQuery = new ExchangeQuery()
        //        {
        //            Date = _mockDateTimeProvider.Object.GetCurrentTime().Date.AddDays(-1),
        //            CurrencyFrom = new Currency()
        //            {
        //                CurrencyCode = "USD"
        //            },
        //            CurrencyTo = new Currency()
        //            {
        //                CurrencyCode = "UAH"
        //            }
        //        };

        //        foreach (var bank in _banks)
        //        {
        //            var actualExchangeRate = await bank.GetExchangeRate(exchangeQuery);
        //            //Assert
        //            Assert.IsNotNull(actualExchangeRate);
        //            Assert.IsTrue(actualExchangeRate is ExchangeRate);
        //            Assert.AreEqual(actualExchangeRate, rates[0]);
        //        }
        //    }
        //    #endregion
    }
}
