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

    [Fact]
    [Trait("Type", "Smoke")]
    [Trait("Suite", "Checkout")]
    [Trait("Case", "TC007")]
    public async Task TC007_Confirmation_Displays_Order_Id_And_Key_Totals()
    {
        await RunCase<TC007_Confirmation_Displays_Order_Id_And_Key_Totals>(
            caseId: "TC007",
            createCase: (logger, runId, caseExecutionId) => new TC007_Confirmation_Displays_Order_Id_And_Key_Totals(
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
                testCase.Step1_OpenConfirmationState();
                testCase.Step2_ValidateOrderId();
                testCase.Step3_ValidateKeyTotals();
                testCase.ValidateExpectedResults();
                testCase.ApplyPostconditions();
            });
    }

    [Fact]
    [Trait("Type", "Smoke")]
    [Trait("Suite", "Checkout")]
    [Trait("Case", "TC010")]
    public async Task TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation()
    {
        await RunCase<TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation>(
            caseId: "TC010",
            createCase: (logger, runId, caseExecutionId) => new TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation(
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
                testCase.Step1_OpenCheckoutState();
                testCase.Step2_SubmitInvalidDetails();
                testCase.Step3_ValidateErrorFeedbackAndRoute();
                testCase.ValidateExpectedResults();
                testCase.ApplyPostconditions();
            });
    }
}
