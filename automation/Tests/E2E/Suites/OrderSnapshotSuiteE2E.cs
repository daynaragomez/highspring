using Highspring.Automation.Core;
using Highspring.Automation.Tests;
using Highspring.Automation.Tests.E2E.Cases;
using Xunit;

namespace Highspring.Automation.Tests.E2E.Suites;

public sealed class OrderSnapshotSuiteE2E : BaseCaseSuiteTest
{
    [Fact]
    [Trait("Type", "E2E")]
    [Trait("Suite", "OrderSnapshot")]
    [Trait("Case", "TC112")]
    public async Task TC112_Order_Snapshot_Matches_Confirmation_Totals()
    {
        await RunCase<TC112_Order_Snapshot_Matches_Confirmation_Totals>(
            caseId: "TC112",
            createCase: (logger, runId, caseExecutionId) => new TC112_Order_Snapshot_Matches_Confirmation_Totals(
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
                testCase.Step1_CompleteCheckoutAndCaptureOrderId();
                await testCase.Step2_QueryOrderSnapshotApiAsync();
                await testCase.Step3_CompareTotalsAsync();
                await testCase.ValidateExpectedResultsAsync();
                testCase.ApplyPostconditions();
            });
    }
}
