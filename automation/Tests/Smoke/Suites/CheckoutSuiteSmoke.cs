using Highspring.Automation.Core;
using Highspring.Automation.Tests;
using Highspring.Automation.Tests.Smoke.Cases;
using Xunit;

namespace Highspring.Automation.Tests.Smoke.Suites;

public sealed class CheckoutSuiteSmoke : BaseCaseSuiteTest
{
    [Fact]
    [Trait("Type", "Smoke")]
    [Trait("Suite", "Checkout")]
    [Trait("Case", "TC006")]
    public async Task TC006_Checkout_Submission_Redirects_To_Confirmation()
    {
        await RunCase<TC006_Checkout_Submission_Redirects_To_Confirmation>(
            caseId: "TC006",
            createCase: (logger, runId, caseExecutionId) => new TC006_Checkout_Submission_Redirects_To_Confirmation(
                Driver,
                Wait,
                Settings.BaseUrl,
                Settings.ApiBaseUrl,
                logger,
                runId,
                caseExecutionId),
            executeFlow: async testCase =>
            {
                await testCase.PreconditionsAsync();
                testCase.Step1_PrepareCheckoutState();
                testCase.Step2_SubmitValidCheckoutForm();
                testCase.Step3_ValidateRedirectAndConfirmation();
                testCase.ValidateExpectedResults();
                testCase.ApplyPostconditions();
            });
    }
}
