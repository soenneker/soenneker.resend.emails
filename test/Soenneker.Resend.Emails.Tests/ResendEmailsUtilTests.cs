using Soenneker.Resend.Emails.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Resend.Emails.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class ResendEmailsUtilTests : HostedUnitTest
{
    private readonly IResendEmailsUtil _util;

    public ResendEmailsUtilTests(Host host) : base(host)
    {
        _util = Resolve<IResendEmailsUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
