namespace JobPortal.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IWebHostEnvironment> webHostEnvironmentMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            this.userService = new UserService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                webHostEnvironment: this.webHostEnvironmentMock.Object);
        }

        private static User CreateRandomUser() =>
            CreateUserFiller(GetRandomDateTime()).Create();

        private static DateTime GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime(2000, 1, 1)).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 9).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static SqlException GetSqlError() =>
            CreateSqlException();

        private IQueryable<User> CreateRandomUsers()
        {
            return CreateUserFiller(GetRandomDateTime())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static T GetInvalidEnum<T>()
        {
            int randomNumber = GetRandomNumber();

            while (Enum.IsDefined(typeof(T), randomNumber))
            {
                randomNumber = GetRandomNumber();
            }

            return (T)(object)randomNumber;
        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<User> CreateUserFiller(DateTime date)
        {
            var filler = new Filler<User>();

            filler.Setup()
                .OnType<DateTime>().Use(date);

            filler.Setup()
                .OnProperty(user => user.Id).Use(new IntRange(1, 1000));

            filler.Setup()
                .OnProperty(user => user.FirstName).Use(GetRandomString);

            filler.Setup()
                .OnProperty(user => user.LastName).Use(GetRandomString);

            filler.Setup()
                .OnProperty(user => user.Email).Use(() => $"{GetRandomString()}@mail.com");

            filler.Setup()
                .OnProperty(user => user.PasswordHash).Use(GetRandomString);

            filler.Setup()
                .OnProperty(user => user.Role).Use(() => Role.User);

            filler.Setup()
                .OnProperty(user => user.Status).Use(() => UserStatus.Active);

            filler.Setup()
                .OnProperty(user => user.AvatarUrl).Use(() => $"https://avatars.com/{GetRandomString()}.png");

            filler.Setup()
                .OnProperty(user => user.RefreshToken).Use(GetRandomString);

            filler.Setup()
                .OnProperty(user => user.RefreshTokenExpiryTime).Use(() => date.AddDays(7));

            filler.Setup()
                .OnProperty(user => user.PasswordResetToken).Use(GetRandomString);

            filler.Setup()
                .OnProperty(user => user.PasswordResetTokenExpiry).Use(() => date.AddHours(1));

            filler.Setup()
                .OnProperty(user => user.Profile).IgnoreIt(); 

            return filler;
        }

        private static SqlException CreateSqlException()
        {
            var sqlError = (SqlError)typeof(SqlError)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0]
                .Invoke(new object[] {
            1,
            (byte)1, 
            (byte)1, 
            "server", 
            "Simulated SQL exception", 
            "procedure", 
            42 
                });

            var errorCollection = (SqlErrorCollection)typeof(SqlErrorCollection)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0]
                .Invoke(null);

            typeof(SqlErrorCollection)
                .GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(errorCollection, new object[] { sqlError });

            var sqlException = (SqlException)typeof(SqlException)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0]
                .Invoke(new object[] {
            "Simulated SQL exception", 
            errorCollection, 
            null!, 
            Guid.NewGuid() 
                });

            return sqlException;
        }
    }
}
