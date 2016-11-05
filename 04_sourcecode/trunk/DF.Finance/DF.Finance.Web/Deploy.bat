@echo off
:: Keith Mo
:: 2015-12-25

:: define variables
:: ---------------------------------------------------
:: website root directory
set "home=E:\WebProduct"
:: the folder which stores the config files for test environment
set "backupPath=%home%\Backup\DF_Finance_Web"
:: the folder which stores the compiled files
set "srcPath=%home%\Build\DF_Finance_Web"
:: the physical path for IIS site
set "releasePath=%home%\Release\DF_Finance_Web"
:: current date
set "curDate=%date:~0,4%%date:~5,2%%date:~8,2%"
:: ------------------------------------------------
:: ---------------------------------------------------



echo.
echo Stoping IIS site "DF_Finance_Web" ...
appcmd stop site "DF_Finance_Web"

:echo.
:echo delete "*.config"
:del /f /s /q %srcPath%\*.config

echo.
echo Copying backup config file(s) from %releasePath% to %backupPath%   ...
if not exist %backupPath%\%curDate% Md %backupPath%\%curDate%
xcopy /e/y  %srcPath% %backupPath%\%curDate%

if not exist "%backupPath%\%curDate%\wb\" Md "%backupPath%\%curDate%\wb\"
xcopy /y  %releasePath%\*.config "%backupPath%\%curDate%\wb\"

echo.
echo Deleting old files in %releasePath% ...
if exist %releasePath% rd /s/q %releasePath%\

echo.
echo Copying new files from %srcPath% to %releasePath% ...
if not exist "%releasePath%\" Md "%releasePath%\"
xcopy /e/y "%srcPath%" "%releasePath%\" >nul
xcopy /y  %backupPath%\%curDate%\wb\*.config "%releasePath%\" >nul


if exist %backupPath%\%curDate%\wb\ rd /s/q %backupPath%\%curDate%\wb\


echo.
echo Checking if all files are correctly copied ...
echo Checking if %releasePath% exists ...
if not exist %releasePath% (
	echo [WARNING] Can not create %releasePath%. Please copy all files in %srcPath% to %releasePath% manually.
)

echo.
echo Checking file ^& folder count ...
for /f "delims=: tokens=1" %%i in ('dir /s/b %srcPath% ^| findstr /n .*') do set "filesInSrcPath=%%i"
for /f "delims=: tokens=1" %%i in ('dir /s/b %releasePath% ^| findstr /n .*') do set "filesInReleasePath=%%i"

echo.
echo The number of files and folders in %srcPath% is:  %filesInSrcPath%
echo The number of files and folders in %releasePath% is:  %filesInReleasePath%

echo.
if %filesInSrcPath% neq %filesInReleasePath% (
	echo [WARNING] NOT all the files are copied. Please copy all files in %srcPath% to %releasePath% manually.
)

echo.
echo Recycling IIS app pool "DF_Finance_Web" ...
appcmd recycle apppool "DF_Finance_Web"

echo.
echo. Starting IIS site "DF_Finance_Web "...
appcmd start site "DF_Finance_Web"
