using Highspring.Automation.Core;
using Highspring.Automation.Tests;
using Highspring.Automation.Tests.Smoke.Cases;
using Xunit;

namespace Highspring.Automation.Tests.Smoke.Suites;

public sealed class CartSuiteSmoke : BaseCaseSuiteTest
{
    [Fact]
    [Trait("Type", "Smoke")]
    [Trait("Suite", "Cart")]
    [Trait("Case", "TC004")]
    public async Task TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total()
    {
        await RunCase<TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total>(
            caseId: "TC004",
            createCase: (logger, runId, caseExecutionId) => new TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total(
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
                testCase.Step1_SeedCartState();
                testCase.Step2_UpdateQuantity();
                testCase.Step3_ValidateTotals();
                testCase.ValidateExpectedResults();
                testCase.ApplyPostconditions();
            });
    }
}
