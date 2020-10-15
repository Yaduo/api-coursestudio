Tools:

Problem: "donet-ef" commend not found.
Solution:
https://stackoverflow.com/questions/40850156/visual-studio-mac-preview-entity-framework-sqlite-add-migration

Visual Studio for Mac 2017 currently (April 2017) does not support adding a reference to Microsoft.EntityFrameworkCore.Tools.DotNet and returns an error:
Package 'Microsoft.EntityFrameworkCore.Tools.DotNet 1.0.0' has a package type 'DotnetCliTool' that is not supported by project 'MacMvc'.

You can edit the file manually and add the reference directly to the csproj file, as documented. Add this to your csproj file:

""""
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.All" Version="2.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="2.1.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="2.1.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="2.1.1" />
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.3" />
  </ItemGroup>

""""
Then run dotnet restore to install the package. After that, you will be able to use dotnet ef migrations add NameOfMigration and dotnet ef database update scripts as per documentation.

N.B.: you must be in the project directory when executing commands.

Also see suggestion feeedback for VS 2017 for Mac:

https://visualstudio.uservoice.com/forums/563332-visual-studio-for-mac/suggestions/17169425-add-sql-server-integration
https://visualstudio.uservoice.com/forums/563332-visual-studio-for-mac/suggestions/17138506-terminal-window


More answers:
https://github.com/aspnet/EntityFrameworkCore/issues/10457