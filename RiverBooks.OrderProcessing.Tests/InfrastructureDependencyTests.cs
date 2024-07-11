using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using Xunit.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace RiverBooks.OrderProcessing.Tests;

public class InfrastructureDependencyTests(ITestOutputHelper outputHelper)
{
  private static readonly Architecture Architecture = new ArchLoader()
                                                                      .LoadAssemblies(typeof(OrderProcessing.AssemblyInfo).Assembly)
                                                                      .Build();

  [Fact]
  public void DomainTypesShouldNotReferenceInfrastructure()
  {
    var domainTypes = Types().That().ResideInNamespace("RiverBooks.OrderProcessing.Domain.*", useRegularExpressions: true).As("OrderProcessing Domain Types");

    var infrastructureTypes = Types().That().ResideInNamespace("RiverBooks.OrderProcessing.Infrastructure.*", useRegularExpressions: true).As("OrderProcessing Infrastructure Types");

    var rule = domainTypes.Should().NotDependOnAny(infrastructureTypes);
    PrintTypes(domainTypes, infrastructureTypes);
    rule.Check(Architecture);
  }
  /// <summary>
  /// Used for debugging puproses
  /// </summary>
  /// <param name="domainTypes"></param>
  /// <param name="infrastructureTypes"></param>
  private void PrintTypes(GivenTypesConjunctionWithDescription domainTypes, GivenTypesConjunctionWithDescription infrastructureTypes)
  {
    // Debugging - Inspect classes and their dependencies
    foreach (var domainClass in domainTypes.GetObjects(Architecture))
    {
      outputHelper.WriteLine($"Domain Types: {domainClass.FullName}");
      foreach (var dependency in domainClass.Dependencies)
      {
        var targetType = dependency.Target;
        if (infrastructureTypes.GetObjects(Architecture).Any(infraClass => infraClass.FullName == targetType.FullName))
        {
          outputHelper.WriteLine($"  Depends on: {targetType.FullName}");
        }
      }
    }

    foreach (var infraClass in infrastructureTypes.GetObjects(Architecture))
    {
      outputHelper.WriteLine($"Infrastructure Types: {infraClass.FullName}");
    }
  }
}
