@echo off
:: website root directory
set "home=E:\WebProduct"
:: the folder which stores the config files for test environment
set "backupPath=%home%\Backup\DF.Finance.Tasks\bin\Release"
:: the folder which stores the compiled files
set "srcPath=%workspace%\DF.Finance.Tasks\bin\Release"
:: the physical path for IIS site
set "releasePath=%home%\Release\DF_Finance_Tasks"
:: current date
set "curDate=%date:~0,4%%date:~5,2%%date:~8,2%"
:: ------------------------------------------------
:: ---------------------------------------------------

echo stop process "DF.Finance.Tasks.exe"
taskkill /im DF.Finance.Tasks.exe /f

echo.
echo Deleting old files in %releasePath% ...
if exist %releasePath% rd /s/q %releasePath%\

echo.
echo Copying new files from %srcPath% to %releasePath% ...
if not exist %releasePath%\ Md %releasePath%\
xcopy /e/y %srcPath% %releasePath%\ >nul


echo start process "DF.Finance.Tasks.exe"
start "%releasePath%\DF.Finance.Tasks.exe"
