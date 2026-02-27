using Highspring.Automation.Core;
using Highspring.Automation.Tests;
using Highspring.Automation.Tests.E2E.Cases;
using Xunit;

namespace Highspring.Automation.Tests.E2E.Suites;

public sealed class PricingSuiteE2E : BaseCaseSuiteTest
{
    [Fact]
    [Trait("Type", "E2E")]
    [Trait("Suite", "Pricing")]
    [Trait("Case", "TC104")]
    public async Task TC104_Save10_Discount_Applies_Before_Tax_For_CA_QC()
    {
        await RunCase<TC104_Save10_Discount_Applies_Before_Tax_For_CA_QC>(
            caseId: "TC104",
            createCase: (logger, runId, caseExecutionId) => new TC104_Save10_Discount_Applies_Before_Tax_For_CA_QC(
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
                testCase.Step1_AddItemAndApplyCoupon();
                testCase.Step2_ValidatePricingValues();
                testCase.Step3_ValidateTaxBasis();
                testCase.ValidateExpectedResults();
                testCase.ApplyPostconditions();
            });
    }
}
