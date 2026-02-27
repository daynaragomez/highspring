using Highspring.Automation.Core;
using Highspring.Automation.Tests;
using Highspring.Automation.Tests.Smoke.Cases;
using Xunit;

namespace Highspring.Automation.Tests.Smoke.Suites;

public sealed class ProductsSuiteSmoke : BaseCaseSuiteTest
{
    [Fact]
    [Trait("Type", "Smoke")]
    [Trait("Suite", "Products")]
    [Trait("Case", "TC003")]
    public async Task TC003_Add_To_Cart_From_Products_Creates_Cart_Line()
    {
        await RunCase<TC003_Add_To_Cart_From_Products_Creates_Cart_Line>(
            caseId: "TC003",
            createCase: (logger, runId, caseExecutionId) => new TC003_Add_To_Cart_From_Products_Creates_Cart_Line(
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
                testCase.Step1_OpenProductsPage();
                testCase.Step2_AddProductToCart();
                testCase.Step3_ValidateCartLineAndTotals();
                testCase.ValidateExpectedResults();
                testCase.ApplyPostconditions();
            });
    }
}
