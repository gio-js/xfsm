rm -r -fo ..\Xfsm.Core.Test\TestResults
rm -r -fo ..\Xfsm.SqlServer.Test\TestResults
rm -r -fo ..\coverageresults

cd ..\Xfsm.Core.Test
dotnet test --collect:"XPlat Code Coverage"

cd ..\Xfsm.SqlServer.Test
dotnet test --collect:"XPlat Code Coverage"

cd ..\
reportgenerator -reports:".\Xfsm.Core.Test\TestResults\**\*.xml;.\Xfsm.SqlServer.Test\TestResults\**\*.xml" -targetdir:"coverageresults" -reporttypes:HTML