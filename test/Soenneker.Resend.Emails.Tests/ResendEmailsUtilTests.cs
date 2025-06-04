using Soenneker.Resend.Emails.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Resend.Emails.Tests;

[Collection("Collection")]
public sealed class ResendEmailsUtilTests : FixturedUnitTest
{
    private readonly IResendEmailsUtil _util;

    public ResendEmailsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IResendEmailsUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
