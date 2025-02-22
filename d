[33mcommit 78bb4b9b747237eae06f2e003570a5ca6d941e45[m[33m ([m[1;31morigin/main[m[33m, [m[1;31morigin/LABOURLIN-16[m[33m, [m[1;31morigin/LABOURLIN-13[m[33m, [m[1;31morigin/HEAD[m[33m, [m[1;32mmain[m[33m, [m[1;32mLABOURLIN-16[m[33m, [m[1;32mLABOURLIN-13[m[33m)[m
Author: SuhailMenakuth <suhailmenakuth@gmail.com>
Date:   Mon Feb 10 16:14:33 2025 +0530

    feat : added functionalities for registration and login

M	AdminService/AdminService.csproj
M	AdminService/Controllers/MuncipalityController.cs
M	AdminService/Controllers/SkillController.cs
M	AdminService/LogInformation.txt
M	AdminService/Program.cs
A	AuthenticationService/AuthenticationService.csproj
A	AuthenticationService/AuthenticationService.http
A	AuthenticationService/Controllers/AuthController.cs
A	AuthenticationService/Data/AuthenticationDbContext.cs
A	AuthenticationService/Dtos/AuthenticationDtos/EmployerRegistrationDto.cs
A	AuthenticationService/Dtos/AuthenticationDtos/LabourProfilePhotoDto.cs
A	AuthenticationService/Dtos/AuthenticationDtos/LabourRegistrationDto.cs
A	AuthenticationService/Dtos/AuthenticationDtos/LoginDto.cs
A	AuthenticationService/Enums/LabourPreferedTime.cs
A	AuthenticationService/Enums/UserType.cs
A	AuthenticationService/Helpers/CloudinaryHelper/CloudinaryHelper.cs
A	AuthenticationService/Helpers/CloudinaryHelper/ICloudinaryHelper.cs
A	AuthenticationService/Helpers/JwtHelper/IJwtHelper.cs
A	AuthenticationService/Helpers/JwtHelper/JwtHelper.cs
A	AuthenticationService/Mapper/MapperProfile.cs
A	AuthenticationService/Models/Employer.cs
A	AuthenticationService/Models/Labour.cs
A	AuthenticationService/Models/RefreshToken.cs
A	AuthenticationService/Models/User.cs
A	AuthenticationService/Program.cs
A	AuthenticationService/Properties/launchSettings.json
A	AuthenticationService/Repositories/AuthRepository.cs
A	AuthenticationService/Repositories/IAuthRepository.cs
A	AuthenticationService/Sevices/AuthSerrvice/AuthService.cs
A	AuthenticationService/Sevices/AuthSerrvice/IAuthService.cs
A	AuthenticationService/appsettings.Development.json
A	AuthenticationService/appsettings.json
M	LabourLinkAPIGateway/Program.cs
M	LabourLinkApi.sln
